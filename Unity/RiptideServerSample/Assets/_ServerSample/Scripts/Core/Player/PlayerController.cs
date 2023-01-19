using _sample.Scripts.Shared;
using Riptide;
using UnityEngine;

namespace _sample.Scripts.Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        private ushort _id;
        private string _username;
        private int _score;
        
        public string Username
        {
            get => _username;
           private set => _username = value;
        }
        
        public void SetId(ushort id)
        {
            _id = id;
        }
        
        public void SetUsername(string username)
        {
            _username = username;
        }

        [MessageHandler((ushort)ClientToServerId.AddScore)]
        private void AddScore(ushort fromId)
        {
            if(this._id!=fromId)return;
            
            _score++;
        }
    }
}