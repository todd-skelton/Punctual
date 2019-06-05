using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Punctual;
using Punctual.Intervally;
using Punctual.Weekly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = BuildProvider();

            var services = provider.GetServices<IHostedService>();

            var cancellationTokenSource = new CancellationTokenSource();

            var serviceTasks = new List<Task>();

            foreach(var service in services)
            {
                serviceTasks.Add(service.StartAsync(cancellationTokenSource.Token));
            }

            Console.WriteLine("Services started: type 'quit' to terminate; 'stop' to stop server; 'start' to start server");

            string input;

            do
            {
                input = Console.ReadLine();

                if (input == "stop")
                {
                    Console.WriteLine("Stoping server. Please wait...");
                    foreach (var service in services)
                    {
                        service.StopAsync(cancellationTokenSource.Token);
                    }
                    Console.WriteLine("Server stopped.");
                }
                else if (input == "start")
                {
                    Console.WriteLine("Starting server. Please wait...");
                    foreach (var service in services)
                    {
                        service.StartAsync(cancellationTokenSource.Token);
                    }
                    Console.WriteLine("Server running.");
                }
            } while (input != "quit");

            cancellationTokenSource.Cancel();

            Task.WhenAll(serviceTasks).GetAwaiter().GetResult();
        }

        static IServiceProvider BuildProvider()
        {
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var now = DateTimeOffset.Now;

            var listOfTimes = new List<TimeSpan>();

            for (int x = 1; x <= 100; x++)
            {
                listOfTimes.Add(now.AddSeconds(x * 10).TimeOfDay);
            }

            // Builder service provider for dependency injection
            return new ServiceCollection()
                .AddPunctual(configure => configure
                    .AddConfiguration(configuration)
                    .AddIntervally<MyIntervalScheduledAction>() // action scheduled to run at intervals configured with appsettings.json
                    .AddWeekly<MyWeeklyScheduledAction>(options =>
                    {
                        options.Schedule.Add(new DailyScheduleOptions(DaysToRun.All, listOfTimes));
                    }) // action scheduled to run at certain times during the week
                    .AddIntervally<LongRunningAction>(options =>
                    {
                        options.Frequency = Frequency.Seconds;
                        options.Period = 30;
                        options.RunOnStart = true;
                    })
                )
                .BuildServiceProvider();
        }
    }
}
