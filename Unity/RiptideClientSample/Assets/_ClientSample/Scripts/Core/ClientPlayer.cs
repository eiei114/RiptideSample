using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _ClientSample.Scripts.Core
{
    public class ClientPlayer : MonoBehaviour
    {
        private ushort _id;
        private string _name;
        private readonly AsyncReactiveProperty<int> _score = new(0);
        
        public ushort Id => _id;
        public string Name => _name;
        public IReadOnlyAsyncReactiveProperty<int> Score => _score;
        
        public int SetScore
        {
            set => _score.Value = value;
        }
        
        public void InitPlayer(ushort id, string name,int score)
        {
            _id = id;
            _name = name;
            _score.Value = score;
        }
    }
}