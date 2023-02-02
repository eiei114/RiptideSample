using System.Threading;
using _sample.Scripts.Shared;
using _ServerSample.Scripts.Presenter;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _ServerSample.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private PlayerRegistry playerRegistry;
        [SerializeField] private PlayerSpawner playerSpawner;
        private CancellationToken _ct;

        private void Awake()
        {
            _ct = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            _networkManager.OnConnected.AddListener((id =>
            {
                var players = playerRegistry.Players;

                foreach (var client in players)
                {
                    Debug.Log($"Spawned {client.Id} / {client.Username}");
                    var username = client.Username;
                    var userId = client.Id;
                    var userScore = client.Score.Value;
                    _networkManager.SendSpawn(id, userId, username, userScore);
                }
            }));

            _networkManager.OnDisconnected.AddListener((id =>
            {
                var player = playerRegistry.GetPlayer(id);
                playerRegistry.UnregisterPlayer(id);
                Destroy(player.gameObject);
            }));

            _networkManager.OnSpawned.AddListener(((id, username) =>
            {
                var player = playerSpawner.Spawn(id, username);
                playerRegistry.RegisterPlayer(player);
                _networkManager.SendToAllSpawn(id, username);
                
                new PlayerPresenter(player, _networkManager).AddTo(_ct);
            }));

            _networkManager.OnAddScored.AddListener(((id) =>
            {
                var player = playerRegistry.GetPlayer(id);
                player.AddScored();
            }));
        }
    }
}