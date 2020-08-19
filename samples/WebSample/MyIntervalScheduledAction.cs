using Microsoft.Extensions.Logging;
using Punctual;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebSample
{
    [ScheduledActionAlias("MyInterval")]
    public class MyIntervalScheduledAction : IScheduledAction
    {
        private readonly ILogger _logger;

        public MyIntervalScheduledAction(ILogger<MyIntervalScheduledAction> logger)
        {
            _logger = logger;
        }

        public Task Execute(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Interval action triggered at {DateTimeOffset.Now}.");

            return Task.CompletedTask;
        }
    }
}
