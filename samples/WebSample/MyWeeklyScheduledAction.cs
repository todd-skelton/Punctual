using Microsoft.Extensions.Logging;
using Punctual;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebSample
{
    [ScheduledActionAlias("MyWeekly")]
    public class MyWeeklyScheduledAction : IScheduledAction
    {
        private readonly ILogger _logger;

        public MyWeeklyScheduledAction(ILogger<MyWeeklyScheduledAction> logger)
        {
            _logger = logger;
        }

        public Task Execute(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Weekly scheduled action triggered at {DateTimeOffset.Now}.");

            return Task.CompletedTask;
        }
    }
}
