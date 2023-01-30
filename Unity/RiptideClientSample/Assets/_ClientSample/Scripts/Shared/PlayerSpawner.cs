using _ClientSample.Scripts.Core;
using Riptide;
using UnityEngine;

namespace _ClientSample.Scripts.Shared
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerRegistry playerRegistry;
        [SerializeField] private ClientPlayer clientPlayer;
        
        private void Spawn(ushort id,string name)
        {
            var player = Instantiate(clientPlayer);
            player.InitPlayer(id,name);
            
            playerRegistry.AddPlayer(player);
        }

        [MessageHandler((ushort)ServerToClientId.SpawnPlayer)]
        private void SpawnPlayer(Message message)
        {
            Spawn(message.GetUShort(),message.GetString());
        }
    }
}