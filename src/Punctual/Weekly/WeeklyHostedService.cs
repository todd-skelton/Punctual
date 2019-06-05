using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Punctual.Weekly
{
    public class WeeklyHostedService<TScheduledAction> : HostedServiceBase<TScheduledAction>
    where TScheduledAction : class, IScheduledAction
    {
        private WeeklyHostedServiceOptions<TScheduledAction> _options;
        private IEnumerable<WeeklySchedulePoint> _schedule;
        private IDisposable _optionsReloadToken;


        public WeeklyHostedService(IOptionsMonitor<WeeklyHostedServiceOptions<TScheduledAction>> options, TScheduledAction scheduledAction) : base(scheduledAction)
        {
            _optionsReloadToken = options.OnChange(ReloadOptions);
            ReloadOptions(options.CurrentValue);
        }

        public DateTimeOffset NextRun { get; private set; }

        private async void ReloadOptions(WeeklyHostedServiceOptions<TScheduledAction> options)
        {
            if (_options == null || !_options.Equals(options))
            {
                _options = options ?? throw new ArgumentNullException(nameof(options));
                if (!_options.Schedule.Any()) throw new InvalidOperationException($"Weekly HostedService options for {typeof(TScheduledAction).Name} must contain atleast one run time");
                _schedule = options.GetSchedule();

                if (_isStarted)
                {
                    _scheduledActionTokenSource.Cancel();
                    await StartAsync(_HostedServiceTokenSource.Token);
                }
            }
        }

        private void SetNextRun()
        {
            var orderedSchedule = _schedule.Select(e => e.NextDateTimeOffset).OrderBy(e => e);

            NextRun = orderedSchedule.First();
        }

        protected override async Task ExecuteScheduledActionAsync(CancellationToken HostedServiceCancellationToken, CancellationToken scheduledActionCancellationToken)
        {
            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(HostedServiceCancellationToken, scheduledActionCancellationToken);

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
