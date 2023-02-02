using System;
using Riptide;
using Riptide.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace _ClientSample.Scripts.Shared
{
    public enum ServerToClientId : ushort
    {
        SpawnPlayer = 1,
        AddScore,
    }

    public enum ClientToServerId : ushort
    {
        SetPlayerName = 1,
        AddScore,
    }

    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private string ip = "127.0.0.1";
        [SerializeField] private ushort port = 7777;

        private Client _client;
        private ushort _id;
        private readonly UnityEvent _onMyConnect = new();
        private readonly UnityEvent<ushort> _onOtherConnect = new();
        private readonly UnityEvent<ushort> _onPlayerLeft = new();
        private readonly UnityEvent _didDisconnected = new();
        private readonly UnityEvent _onFailedToConnect = new();
        private static readonly UnityEvent<ushort, string, int> _onSpawned = new();
        private static readonly UnityEvent<ushort, int> _onAddScored = new();

        public ushort Id => _id;
        public UnityEvent OnMyConnect => _onMyConnect;
        public UnityEvent<ushort> OnOtherConnect => _onOtherConnect;
        public UnityEvent<ushort> OnPlayerLeft => _onPlayerLeft;
        public UnityEvent DidDisconnected => _didDisconnected;
        public UnityEvent OnFailedToConnect => _onFailedToConnect;
        public UnityEvent<ushort, string, int> OnSpawned => _onSpawned;
        public UnityEvent<ushort, int> OnAddScored => _onAddScored;

        private void Awake()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            _client = new Client();
            _client.Connected += MyClientConnect;
            _client.ClientConnected += OtherClientConnect;
            _client.ConnectionFailed += FailedToConnect;
            _client.ClientDisconnected += PlayerLeft;
            _client.Disconnected += DidDisconnect;
        }


        private void FixedUpdate()
        {
            _client.Update();
        }

        private void OnApplicationQuit()
        {
            _client.Disconnect();

            _client.Connected -= MyClientConnect;
            _client.ConnectionFailed -= FailedToConnect;
            _client.ClientDisconnected -= PlayerLeft;
            _client.Disconnected -= DidDisconnect;
        }

        public void Connect()
        {
            _client.Connect($"{ip}:{port}");
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }

        private void MyClientConnect(object sender, EventArgs e)
        {
            _id = _client.Id;
            _onMyConnect.Invoke();
        }

        private void OtherClientConnect(object sender, ClientConnectedEventArgs e)
        {
            _onOtherConnect.Invoke(e.Id);
        }

        private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            _onPlayerLeft.Invoke(e.Id);
        }

        private void DidDisconnect(object sender, DisconnectedEventArgs e)
        {
            _didDisconnected.Invoke();
        }

        private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
        {
            _onFailedToConnect.Invoke();
        }

        public void SendAddScoreMessage()
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerId.AddScore);
            _client.Send(message);
        }

        public void SendSetPlayerNameMessage(string username)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerId.SetPlayerName);
            message.AddString(username);
            _client.Send(message);
        }

        [MessageHandler((ushort)ServerToClientId.SpawnPlayer)]
        private static void SpawnPlayer(Message message)
        {
            var id = message.GetUShort();
            var username = message.GetString();
            var userScore = message.GetInt();

            _onSpawned.Invoke(id, username, userScore);
        }

        [MessageHandler((ushort)ServerToClientId.AddScore)]
        private static void AddScore(Message message)
        {
            var id = message.GetUShort();
            var score = message.GetInt();

            _onAddScored?.Invoke(id, score);
        }
    }
}