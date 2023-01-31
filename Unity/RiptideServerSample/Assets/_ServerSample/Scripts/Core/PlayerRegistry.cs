using System.Collections.Generic;
using _sample.Scripts.Core.Player;
using UnityEngine;
using UnityEngine.Events;

namespace _sample.Scripts.Shared
{
    public class PlayerRegistry : MonoBehaviour
    {
        private readonly List<ServerPlayer> _players = new();
        private readonly UnityEvent<ServerPlayer> _onPlayerAdded = new();

        public List<ServerPlayer> Players => _players;
        public UnityEvent<ServerPlayer> OnPlayerAdded => _onPlayerAdded;
        
        public void RegisterPlayer(ServerPlayer serverPlayer)
        {
            _players.Add(serverPlayer);
            _onPlayerAdded.Invoke(serverPlayer);
        }
        
        public void UnregisterPlayer(ushort id)
        {
            _players.RemoveAll(player => player.Id == id);
        }
        
        public ServerPlayer GetPlayer(ushort id)
        {
            return _players.Find(player => player.Id == id);
        }
    }
}