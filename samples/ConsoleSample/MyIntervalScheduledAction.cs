using System;
using System.Threading;
using Punctual;
using System.Threading.Tasks;

namespace ConsoleSample
{
    [ScheduledActionAlias("MyInterval")]
    public class MyIntervalScheduledAction : IScheduledAction
    {
        public Task Action(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Interval action triggered at {DateTimeOffset.Now}.");

            return Task.CompletedTask;
        }
    }
}
