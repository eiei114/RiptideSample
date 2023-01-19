using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _ClientSample.Scripts.View
{
    public class StartView : MonoBehaviour
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _connectButton;
        
        private readonly UnityEvent<string> _connectEvent = new();

        public UnityEvent<string> OnConnectButtonClicked => _connectEvent;
        
        private void Awake()
        {
            _connectButton.onClick.AddListener(() => OnConnectButtonClicked?.Invoke(_inputField.text));
        }
    }
}