using System.Threading;
using _sample.Scripts.Shared;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace _ServerSample.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private PlayerRegistry playerRegistry;
        [SerializeField] private PlayerSpawner playerSpawner;
        private CancellationToken _cts;

        private void Awake()
        {
            _cts = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            _networkManager.OnConnected.AddListener((id =>
            {
                var players = playerRegistry.Players;
                
                foreach (var client in players)
                {
                    if (client.Id == id) continue;
                    var username = client.Username;
                    _networkManager.SendSpawn(id, username);
                }
            }));

            _networkManager.OnDisconnected.AddListener((id =>
            {
                playerRegistry.UnregisterPlayer(id);
            }));

            _networkManager.OnSpawned.AddListener(((id, username) =>
            {
                var player = playerSpawner.Spawn(id, username);
                playerRegistry.RegisterPlayer(player);

                _networkManager.SendToAllSpawn(id, username);
            }));

            _networkManager.OnAddScored.AddListener(((id) =>
            {
                var player = playerRegistry.GetPlayer(id);
                player.AddScored();
            }));
            
            playerRegistry.OnPlayerAdded.AddListener((player =>
            {
                player.Score.Subscribe(value =>
                {
                    _networkManager.SendAddScore(player.Id,value);
                }).AddTo(_cts);
            }));
        }
    }
}