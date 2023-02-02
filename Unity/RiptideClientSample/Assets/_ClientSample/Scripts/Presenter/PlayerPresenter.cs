using System;
using System.Threading;
using _ClientSample.Scripts.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace _ClientSample.Scripts.Presenter
{
    public class PlayerPresenter : IDisposable
    {
        private readonly CancellationTokenSource _cts = new();

        public PlayerPresenter(ClientPlayer model, PlayerView view)
        {
            var token = _cts.Token;

            model.Score
                .Subscribe(value => view.SetScore = value).AddTo(token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}