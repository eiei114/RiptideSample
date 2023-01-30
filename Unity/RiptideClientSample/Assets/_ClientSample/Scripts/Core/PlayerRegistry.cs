using System.Collections.Generic;
using UnityEngine;

namespace _ClientSample.Scripts.Core
{
    public class PlayerRegistry : MonoBehaviour
    {
        private List<ClientPlayer> _players = new();

        public List<ClientPlayer> Players => _players;

        public void AddPlayer(ClientPlayer clientPlayer)
        {
            _players.Add(clientPlayer);
        }

        public void RemovePlayer(ushort id)
        {
            _players.RemoveAll(player => player.Id == id);
        }
        
        public void RemoveAllPlayers()
        {
            _players.Clear();
        }
    }
}