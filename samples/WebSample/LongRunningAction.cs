using Microsoft.Extensions.Logging;
using Punctual;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebSample
{
    [ScheduledActionAlias("Long")]
    public class LongRunningAction : IScheduledAction
    {
        private readonly ILogger _logger;

        public LongRunningAction(ILogger<LongRunningAction> logger)
        {
            _logger = logger;
        }

        public async Task Action(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Long running action started at {DateTimeOffset.Now}.");

            await Task.WhenAny(Task.Delay(30000, cancellationToken));

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Long running action cancelled before completion.");
            }
            else
            {
                _logger.LogInformation($"Long running action completed.");
            }

            return;
        }
    }
}
