using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _ClientSample.Scripts.View
{
    public class MainView : MonoBehaviour
    {
        [SerializeField] private Button _addScoreButton;
        [SerializeField] private Button _disconnectButton;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _playerViewParent;
        
        private readonly UnityEvent _addScoreEvent = new();
        private readonly UnityEvent _disconnectEvent = new();
        
        public UnityEvent AddScoreEvent => _addScoreEvent;
        public UnityEvent DisconnectEvent => _disconnectEvent;

        private void Awake()
        {
            _addScoreButton.onClick.AddListener(() => _addScoreEvent.Invoke());
            _disconnectButton.onClick.AddListener(() => _disconnectEvent.Invoke());
        }

        public PlayerView CreatePlayerView()
        {
            var playerView = Instantiate(_playerView, _playerViewParent);
            return playerView;
        }
        
        
    }
}