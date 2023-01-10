using System.Collections.Generic;
using _sample.Scripts.Core.Player;
using UnityEngine;

namespace _sample.Scripts.Shared
{
    public class PlayerRegistry : MonoBehaviour
    {
        private Dictionary<ushort,PlayerController> _players = new();
        
        public Dictionary<ushort,PlayerController> Players => _players;
        
        public void RegisterPlayer(ushort id, PlayerController player)
        {
            _players.Add(id, player);
        }
        
        public void UnregisterPlayer(ushort id)
        {
            _players.Remove(id);
        }
    }
}