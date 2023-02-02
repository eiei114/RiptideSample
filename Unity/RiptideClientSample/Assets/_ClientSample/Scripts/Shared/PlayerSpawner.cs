using _ClientSample.Scripts.Core;
using UnityEngine;

namespace _ClientSample.Scripts.Shared
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private ClientPlayer clientPlayer;
        
        public ClientPlayer Spawn(ushort id,string name,int score)
        {
            var player = Instantiate(clientPlayer);
            player.InitPlayer(id,name,score);
            
            return player;
        }
    }
}