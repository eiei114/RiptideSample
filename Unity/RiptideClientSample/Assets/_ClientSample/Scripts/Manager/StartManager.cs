using _ClientSample.Scripts.Shared;
using _ClientSample.Scripts.View;
using UnityEngine;

namespace _ClientSample.Scripts.Manager
{
    public class StartManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private StartView _startView;

        private void Awake()
        {
            _startView.OnConnectButtonClicked.AddListener(Connecting);
        }
        
        private void Connecting(string hogeName)
        {
            _networkManager.Connect(hogeName);
        }
    }
}