using _sample.Scripts.Core.Player;
using UnityEngine;

namespace _sample.Scripts.Shared
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private ServerPlayer serverPlayer;
        
        public ServerPlayer Spawn(ushort id, string username)
        {
            //playerPrefab生成
            var player = Instantiate(serverPlayer);
            player.SetId(id);
            player.SetUsername(username);

            return player;
        }
    }
}