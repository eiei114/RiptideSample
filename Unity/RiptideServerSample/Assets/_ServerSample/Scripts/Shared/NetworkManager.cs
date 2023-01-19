#if !UNITY_EDITOR
using System;
#endif
using _sample.Scripts.Core.Player;
using Riptide;
using Riptide.Utils;
using UnityEngine;

namespace _sample.Scripts.Shared
{
    public enum ServerToClientId : ushort
    {
        SpawnPlayer = 1,
    }

    public enum ClientToServerId : ushort
    {
        SpawnFromClient = 1,
        AddScore,
    }

    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private ushort port;
        [SerializeField] private ushort maxConnections;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private PlayerRegistry playerRegistry;

        private Server _server;
        private static NetworkManager _singleton;

        public PlayerController PlayerController => playerPrefab;
        public PlayerRegistry PlayerRegistry => playerRegistry;
        public Server Server => _server;

        public static NetworkManager Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying object!");
                    Destroy(value);
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

#if UNITY_EDITOR
            //エラーログを出力する
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
            //コンソール初期化
            Console.Title = "Server";
            Console.Clear();
            //(https://docs.unity3d.com/ja/2018.4/ScriptReference/Application.SetStackTraceLogType.html)
            // スタックトレースがログに出力されません
            Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
            RiptideLogger.Initialize(Debug.Log, true);
#endif
            //サーバーの起動、初期化
            _server = new Server();
            //サーバー接続時、切断時のイベントを登録
            _server.ClientConnected += ClientConnected;
            _server.ClientDisconnected += ClientDisconnected;

            _server.Start(port, maxConnections);
        }

        private void FixedUpdate()
        {
            // サーバーが接続を受け入れてメッセージを処理できるようにする
            _server.Update();
        }

        private void ClientConnected(object sender, ServerConnectedEventArgs e)
        {
            Debug.Log($"Client connected: {e.Client}");
            var playerList = playerRegistry.Players;

            foreach (var id in playerList)
            {
                if (id.Key == e.Client.Id) continue;
                //他のクライアントが生成されたとき
                var clientId = e.Client.Id;
                var userName = playerRegistry.Players[clientId].Username;
                playerSpawner.SendSpawner(clientId, userName);
            }
        }

        private void ClientDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            Debug.Log($"Client disconnected: {e.Client}");
        }

        private void OnApplicationQuit()
        {
            _server.Stop();

            //サーバー接続時、切断時のイベントを解除
            _server.ClientConnected -= ClientConnected;
            _server.ClientDisconnected -= ClientDisconnected;
        }
    }
}