using System;
using _ClientSample.Scripts.Core;
using Riptide;
using Riptide.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace _ClientSample.Scripts.Shared
{
    public enum ServerToClientId : ushort
    {
        SpawnPlayer = 1,
        PlayerMovement,
    }

    public enum ClientToServerId : ushort
    {
        PlayerName = 1,
        PlayerInput,
    }

    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private string ip = "localhost";
        [SerializeField] private ushort port = 7777;

        private string _hogeName;
        private Client _client;
        private PlayerRegistry _playerRegistry;
        private readonly UnityEvent _viewChangeEvent = new();
        
        public UnityEvent ViewChangeEvent => _viewChangeEvent;

        private void Awake()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            _client = new Client();
            _client.Connected += DidConnect;
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

            _client.Connected -= DidConnect;
            _client.ConnectionFailed -= FailedToConnect;
            _client.ClientDisconnected -= PlayerLeft;
            _client.Disconnected -= DidDisconnect;
        }

        public void Connect(string hogeName)
        {
            _hogeName = hogeName;
            _client.Connect($"{ip}:{port}");
        }
        
        public void Disconnect()
        {
            _client.Disconnect();
        }

        private void DidConnect(object sender, EventArgs e)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlayerName);
            message.AddString(_hogeName);
            _client.Send(message);
        }

        private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            //Todo:抜けたプレイヤーのViewを削除
            _playerRegistry.RemovePlayer(e.Id);
        }

        private void DidDisconnect(object sender, DisconnectedEventArgs e)
        {
            _playerRegistry.RemoveAllPlayers();
            _viewChangeEvent.Invoke();
        }

        private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
        {
            //Startシーンに遷移
            _viewChangeEvent.Invoke();
        }
        
        public void SendAddScoreMessage()
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlayerInput);
            _client.Send(message);
        }
    }
}