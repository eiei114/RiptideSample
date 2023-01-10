using Riptide;
using UnityEngine;

namespace _sample.Scripts.Shared
{
    public class PlayerSpawner : MonoBehaviour
    {
        public static void Spawn(ushort id, string username)
        {
            //playerPrefab生成
            var player = Instantiate(NetworkManager.Singleton.PlayerController);
            player.SetId(id);
            player.SetUsername(username);
            NetworkManager.Singleton.PlayerRegistry.RegisterPlayer(id, player);

            SpawnPlayer(id,username);
        }
        
        public static void Despawn(ushort id)
        {
            NetworkManager.Singleton.PlayerRegistry.UnregisterPlayer(id);
        }

        /// <summary>
        /// 全てのクライアントに生成情報を送信する
        /// 自分が生成したプレイヤーの生成時
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private static void SpawnPlayer(ushort id,string username)
        {
            var message = GetSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer), id,
                username);
            
            NetworkManager
                .Singleton
                .Server
                .SendToAll(message);
        }

        /// <summary>
        /// 指定したクライアントに生成情報を送信する
        /// 自分が生成していないプレイヤーの生成時
        /// </summary>
        /// <param name="toClient"></param>
        /// <param name="username"></param>
        public void SendSpawner(ushort toClient,string username)
        {
            var message = GetSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer), toClient,
                username);
            
            NetworkManager
                .Singleton
                .Server
                .Send(message, toClient);
        }

        private static Message GetSpawnData(Message message,ushort id, string username)
        {
            message.AddUShort(id);
            message.AddString(username);
            return message;
        }

        [MessageHandler((ushort)ClientToServerId.SpawnFromClient)]
        private static void SpawnFromClient(ushort fromClientId,Message message)
        {
            Spawn(fromClientId,message.GetString());
        }
    }
}