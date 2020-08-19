using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Punctual;
using Punctual.Periodically;
using Punctual.Weekly;
using System;
using System.Collections.Generic;

namespace WebSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var now = DateTimeOffset.Now;

            var listOfTimes = new List<TimeSpan>();

            for (int x = 1; x <= 100; x++)
            {
                listOfTimes.Add(now.AddSeconds(x * 10).TimeOfDay);
            }

            services.AddPunctual(configure => configure
                .AddConfiguration(Configuration)
                .AddPeriodically<MyIntervalScheduledAction>() // action scheduled to run at intervals configured with appsettings.json
                .AddWeekly<MyWeeklyScheduledAction>(options =>
                {
                    options.Schedule.Add(new DailyScheduleOptions(DaysToRun.All, listOfTimes));
                }) // action scheduled to run at certain times during the week
                .AddPeriodically<LongRunningAction>(options =>
                {
                    options.Frequency = Frequency.Seconds;
                    options.Period = 30;
                    options.RunOnStart = true;
                })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
