using System;
using System.Threading;
using _sample.Scripts.Core.Player;
using _sample.Scripts.Shared;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace _ServerSample.Scripts.Presenter
{
    public class PlayerPresenter : IDisposable
    {
        private readonly CancellationTokenSource _cts = new();

        public PlayerPresenter(ServerPlayer model, NetworkManager manager)
        {
            var token = _cts.Token;

            model.Score
                .Subscribe(value => { manager.SendAddScore(model.Id, value); }).AddTo(token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}