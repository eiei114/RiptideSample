using System.Threading;
using _ClientSample.Scripts.Core;
using _ClientSample.Scripts.Presenter;
using _ClientSample.Scripts.Shared;
using _ClientSample.Scripts.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _ClientSample.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private PlayerRegistry _playerRegistry;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private StartView _startView;
        [SerializeField] private MainView _mainView;
        private CancellationToken _ct;

        private void Awake()
        {
            _ct = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            _startView.OnConnectButtonClicked.AddListener(() =>
            {
                _networkManager.Connect();
                SwitchView();
            });

            _mainView.DisconnectEvent.AddListener(() =>
            {
                _networkManager.Disconnect();
            });

            _mainView.AddScoreEvent.AddListener(() =>
            {
                _networkManager.SendAddScoreMessage();
            });

            _networkManager.OnMyConnect.AddListener((() =>
            {
                //サーバー上のプレイヤー初期化時に必要な情報を送信 
                _networkManager.SendSetPlayerNameMessage(_startView.InputName);
            }));

            _networkManager.OnPlayerLeft.AddListener(((id) =>
            {
                _playerRegistry.RemovePlayer(id);
                var view = _mainView.PlayerViews.Find(x => x.Id == id);
                _mainView.PlayerViews.Remove(view);
                Destroy(view.gameObject);
            }));

            _networkManager.DidDisconnected.AddListener((() =>
            {
                _playerRegistry.RemoveAllPlayers();
                _mainView.PlayerViews.ForEach(view => Destroy(view.gameObject));
                _mainView.PlayerViews.Clear();
                SwitchView();
            }));

            _networkManager.OnFailedToConnect.AddListener((() => { SwitchView(); }));

            _networkManager.OnSpawned.AddListener(((id, username,score) =>
            {
                Debug.Log($"Spawned {id} / {username}");
                var playerView = _mainView.CreatePlayerView();
                playerView.InitPlayerView(id, username);
                var player = _playerSpawner.Spawn(id, username,score);
                _playerRegistry.AddPlayer(player);

                new PlayerPresenter(player, playerView).AddTo(_ct);
            }));

            _networkManager.OnAddScored.AddListener(((id, score) =>
            {
                var player = _playerRegistry.GetPlayer(id);
                if(player == null) return;
                player.SetScore = score;
            }));
        }

        private void SwitchView()
        {
            _startView.gameObject.SetActive(!_startView.gameObject.activeSelf);
            _mainView.gameObject.SetActive(!_mainView.gameObject.activeSelf);
        }
    }
}