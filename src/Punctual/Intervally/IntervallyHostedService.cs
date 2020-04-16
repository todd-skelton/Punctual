using Microsoft.Extensions.Options;
using Punctual.Intervally;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Punctual.Intervally
{
    public class IntervallyHostedService<TScheduledAction> : HostedServiceBase<TScheduledAction>
    where TScheduledAction : class, IScheduledAction
    {
        private IntervallyHostedServiceOptions<TScheduledAction> _options;
        private IDisposable _optionsReloadToken;

        public IntervallyHostedService(IOptionsMonitor<IntervallyHostedServiceOptions<TScheduledAction>> options, TScheduledAction scheduledAction) : base(scheduledAction)
        {
            _optionsReloadToken = options.OnChange(ReloadOptions);
            ReloadOptions(options.CurrentValue);
        }

        public DateTimeOffset NextRun { get; private set; }

        private void SetNextRun()
        {
            NextRun = NextRun.Add(_options.Frequency.GetIntervally(_options.Period));
        }

        private async void ReloadOptions(IntervallyHostedServiceOptions<TScheduledAction> options)
        {
            if (_options == null || !_options.Equals(options))
            {
                _options = options;

                if (_isStarted)
                {
                    _scheduledActionTokenSource.Cancel();
                    await StartAsync(_HostedServiceTokenSource.Token);
                }
            }
        }

        protected override async Task ExecuteScheduledActionAsync(CancellationToken HostedServiceCancellationToken, CancellationToken scheduledActionCancellationToken)
        {
            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(HostedServiceCancellationToken, scheduledActionCancellationToken);

            NextRun = DateTimeOffset.Now;

            if (!_options.RunOnStart)
                SetNextRun();

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var delay = NextRun.Subtract(DateTimeOffset.Now) > TimeSpan.Zero ? NextRun.Subtract(DateTimeOffset.Now) : TimeSpan.Zero;

                await Task.Delay(delay, cancellationTokenSource.Token);

                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    SetNextRun();

                    await RunAction(cancellationTokenSource.Token);
                }
            }
        }
    }
}