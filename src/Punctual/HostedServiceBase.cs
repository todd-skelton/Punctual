using System;
using System.Threading;
using System.Threading.Tasks;

namespace Punctual
{
    /// <summary>
    /// Base class for creating hosted services that perform an action
    /// </summary>
    /// <typeparam name="TScheduledAction"></typeparam>
    public abstract class HostedServiceBase<TScheduledAction> : IHostedService<TScheduledAction>
    where TScheduledAction : class, IScheduledAction
    {
        /// <summary>
        /// Cancellation token source for the scheduled action
        /// </summary>
        protected CancellationTokenSource _scheduledActionTokenSource;
        /// <summary>
        /// Cancellation token source for the hosted service
        /// </summary>
        protected CancellationTokenSource _HostedServiceTokenSource;
        /// <summary>
        /// Task to track the scheduled action
        /// </summary>
        protected Task _scheduledActionTask;
        /// <summary>
        /// Scheduled action that will be performed
        /// </summary>
        protected readonly TScheduledAction _scheduledAction;
        /// <summary>
        /// Whether or not the service is started
        /// </summary>
        protected bool _isStarted;

        /// <summary>
        /// Initializes a hosted service with the action to perform
        /// </summary>
        /// <param name="scheduledAction"></param>
        protected HostedServiceBase(TScheduledAction scheduledAction)
        {
            _scheduledAction = scheduledAction ?? throw new ArgumentNullException(nameof(scheduledAction));
            _isStarted = false;
        }

        /// <summary>
        /// Starts the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Stops the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Runs the action the hosted service is configured to perform
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task RunAction(CancellationToken cancellationToken)
        {
            await _scheduledAction.Action(cancellationToken);
        }

        /// <summary>
        /// Implementation for executing the action the hosted service handles
        /// </summary>
        /// <param name="HostedServiceCancellationToken"></param>
        /// <param name="scheduledActionCancellationToken"></param>
        /// <returns></returns>
        protected abstract Task ExecuteScheduledActionAsync(CancellationToken HostedServiceCancellationToken, CancellationToken scheduledActionCancellationToken);
    }
}