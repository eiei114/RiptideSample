using System;
using Riptide;
using Riptide.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void DidConnect(object sender, EventArgs e)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlayerName);
            message.AddString(_hogeName);
            _client.Send(message);
        }

        private void DidDisconnect(object sender, DisconnectedEventArgs e)
        {
            //Todo: クライアント側が持つプレイヤーのリストをクリアする
        }

        private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            //Todo: クライアント側が持つプレイヤーのリストから抜けたプレイヤーを削除する
        }

        private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
        {
            //Startシーンに遷移
            SceneManager.LoadSceneAsync("Start");
        }
    }
}