using System;
using _ClientSample.Scripts.Shared;
using _ClientSample.Scripts.View;
using UnityEngine;

namespace _ClientSample.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private StartView _startView;
        [SerializeField] private MainView _mainView;

        private void Awake()
        {
            InitStartView();
            InitMainView();
        }

        private void Start()
        {
            _networkManager.ViewChangeEvent.AddListener(SwitchView);
        }

        private void InitStartView()
        {
            _startView.OnConnectButtonClicked.AddListener((hogeName) =>
            {
                _networkManager.Connect(hogeName);
                SwitchView();
            });
        }
        
        private void InitMainView()
        {
            _mainView.DisconnectEvent.AddListener(() => _networkManager.Disconnect());
            _mainView.AddScoreEvent.AddListener((() => _networkManager.SendAddScoreMessage()));
        }

        //StartViewとMainViewの切り替え
        private void SwitchView()
        {
            _startView.gameObject.SetActive(!_startView.gameObject.activeSelf);
            _mainView.gameObject.SetActive(!_mainView.gameObject.activeSelf);
        }
    }
}