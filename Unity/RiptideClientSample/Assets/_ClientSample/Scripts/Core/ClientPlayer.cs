using UnityEngine;

namespace _ClientSample.Scripts.Core
{
    public class ClientPlayer : MonoBehaviour
    {
        private ushort _id;
        private string _name;
        private int _score;
        
        public ushort Id => _id;
        public string Name => _name;
        public int Score => _score;
        
        public void InitPlayer(ushort id, string name)
        {
            _id = id;
            _name = name;
            _score = 0;
        }
    }
}