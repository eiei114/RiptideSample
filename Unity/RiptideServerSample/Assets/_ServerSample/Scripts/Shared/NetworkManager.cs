#if !UNITY_EDITOR
using System;
#endif
using Riptide;
using Riptide.Utils;
using UnityEngine;
using UnityEngine.Events;

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
        private Server _server;

        private readonly UnityEvent<ushort> _onConnected = new();
        private readonly UnityEvent<ushort> _onDisconnected = new();
        private readonly UnityEvent<ushort,string> _onSpawned = new();
        private readonly UnityEvent<ushort> _onAddScored = new();

        public UnityEvent<ushort> OnConnected => _onConnected;
        public UnityEvent<ushort> OnDisconnected => _onDisconnected;
        public UnityEvent<ushort,string> OnSpawned => _onSpawned;
        public UnityEvent<ushort> OnAddScored => _onAddScored;

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
         
            _onConnected.Invoke(e.Client.Id);
        }

        private void ClientDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            _onDisconnected.Invoke(e.Client.Id);
            
            Debug.Log($"Client disconnected: {e.Client}");
        }

        private void OnApplicationQuit()
        {
            _server.Stop();

            //サーバー接続時、切断時のイベントを解除
            _server.ClientConnected -= ClientConnected;
            _server.ClientDisconnected -= ClientDisconnected;
        }

        #region ToServerMessage

        [MessageHandler((ushort)ClientToServerId.AddScore)]
        private void AddScore(ushort fromClientId)
        {
            _onAddScored.Invoke(fromClientId);
        }
        
        [MessageHandler((ushort)ClientToServerId.SpawnFromClient)]
        private void SpawnFromClient(ushort fromClientId, Message message)
        {
            var username = message.GetString();
            
            _onSpawned.Invoke(fromClientId, username);
        }

        #endregion

        #region ToClientMessage

        public void SendSpawn(ushort toClient, string username)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer);
            message.AddUShort(toClient);
            message.AddString(username);
            _server.Send(message,toClient);
        }
        
        public void SendToAllSpawn(ushort toClient, string username)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer);
            message.AddUShort(toClient);
            message.AddString(username);
            _server.SendToAll(message);
        }
        
        public void SendAddScore(ushort toClient,int score)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerId.AddScore);
            message.AddUShort(toClient);
            message.AddInt(score);
            _server.SendToAll(message);
        }

        #endregion
    }
}