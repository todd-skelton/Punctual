using System;
using System.Threading;
using Punctual;
using System.Threading.Tasks;

namespace ConsoleSample
{
    [ScheduledActionAlias("Long")]
    public class LongRunningAction : IScheduledAction
    {
        public async Task Action(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Long running action started at {DateTimeOffset.Now}.");

            await Task.WhenAny(Task.Delay(30000, cancellationToken));

            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Long running action cancelled before completion.");
            }
            else
            {
                Console.WriteLine($"Long running action completed.");
            }

            return;
        }
    }
}
