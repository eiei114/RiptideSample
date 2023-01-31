using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _sample.Scripts.Core.Player
{
    public class ServerPlayer : MonoBehaviour
    {
        private ushort _id;
        private string _username;
        private readonly AsyncReactiveProperty<int> _score = new(0);

        public ushort Id => _id;
        public string Username => _username;
        public IReadOnlyAsyncReactiveProperty<int> Score => _score;

        public void SetId(ushort id)
        {
            _id = id;
        }
        
        public void SetUsername(string username)
        {
            _username = username;
        }

        public void AddScored()
        {
            _score.Value++;
        }
    }
}