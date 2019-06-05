using System;
using System.Threading;
using System.Threading.Tasks;

namespace Punctual
{
    public abstract class HostedServiceBase<TScheduledAction> : IHostedService<TScheduledAction>
    where TScheduledAction : class, IScheduledAction
    {
        protected CancellationTokenSource _scheduledActionTokenSource;
        protected CancellationTokenSource _HostedServiceTokenSource;
        protected Task _scheduledActionTask;
        protected readonly TScheduledAction _scheduledAction;
        protected bool _isStarted;

        protected HostedServiceBase(TScheduledAction scheduledAction)
        {
            _scheduledAction = scheduledAction ?? throw new ArgumentNullException(nameof(scheduledAction));
            _isStarted = false;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _isStarted = true;

            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            _HostedServiceTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Create a cancel token just for the action so it can be reset if configuration changes
            _scheduledActionTokenSource = new CancellationTokenSource();

            // Store the task we're executing
            _scheduledActionTask = ExecuteScheduledActionAsync(_HostedServiceTokenSource.Token, _scheduledActionTokenSource.Token);

            // If the task is completed then return it, otherwise it's running
            return _scheduledActionTask.IsCompleted ? _scheduledActionTask : Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _isStarted = false;

            // Stop called without start
            if (_scheduledActionTask == null)
            {
                return;
            }

            // Signal cancellation to the executing method
            _HostedServiceTokenSource.Cancel();

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_scheduledActionTask, Task.Delay(-1, cancellationToken));

            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }

        protected virtual async Task RunAction(CancellationToken cancellationToken)
        {
            await _scheduledAction.Action(cancellationToken);
        }

        protected abstract Task ExecuteScheduledActionAsync(CancellationToken HostedServiceCancellationToken, CancellationToken scheduledActionCancellationToken);
    }
}