using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _ClientSample.Scripts.View
{
    public class StartView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _connectButton;
        
        private readonly UnityEvent _connectEvent = new();

        public UnityEvent OnConnectButtonClicked => _connectEvent;
        public string InputName => _inputField.text;
        
        private void Awake()
        {
            _connectButton.onClick.AddListener(() => OnConnectButtonClicked?.Invoke());
        }
    }
}