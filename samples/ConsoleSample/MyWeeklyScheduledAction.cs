using System;
using System.Threading;
using Punctual;
using System.Threading.Tasks;

namespace ConsoleSample
{
    [ScheduledActionAlias("MyWeekly")]
    public class MyWeeklyScheduledAction : IScheduledAction
    {
        public Task Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Weekly scheduled action triggered at {DateTimeOffset.Now}.");

            return Task.CompletedTask;
        }
    }
}
