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
            var player = GetPlayer(id);

            if (player == null) return;

            _players.Remove(player);
            Destroy(player.gameObject);
        }

        public void RemoveAllPlayers()
        {
            // リストの中身削除と共にClientPlayerも破壊する
            _players.ForEach(player => Destroy(player.gameObject));
            _players.Clear();
        }

        public ClientPlayer GetPlayer(ushort id)
        {
            return _players.Find(player => player.Id == id);
        }
    }
}