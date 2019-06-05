[![](https://img.shields.io/nuget/v/Punctual.svg)](https://www.nuget.org/packages/Punctual) [![](https://img.shields.io/nuget/vpre/Punctual.svg)](https://www.nuget.org/packages/Punctual)

# Punctual
Punctual is an implemenation of .NET's Hosted Service that allows you to easily create and configure actions to be performed on a schedule or interval.

## Installation
### Package Manager
`Install-Package Punctual`

### .NET CLI
`dotnet add package Punctual`

### IScheduledAction
Use the `IScheduledAction` interface to create the action you want to perform on a schedule.

```csharp
public class MyAction : IScheduledAction
{
    public Task Action(CancellationToken cancellationToken)
    {
        // perform some awesome action here
    }
}
```

#### Example

```csharp
public class SendReport : IScheduledAction
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _services;

    public SendReport(IServiceProvider services, ILogger<MyAction> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task Action(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Report is being generated.");

        using (var scope = _services.CreateScope())
        {
            var db = scope.GetService<MyDbContext>();
            var reportGenerator = scope.GetService<IReportGenerator>();
            var emailService = scope.GetServive<IEmailService>();

            var reportItems = await db.ItemsToReport.ToListAsync();

            var report = reportGenerator.Generate(reportItems);

            emailService.SendReport(report);
        }

        _logger.LogInformation("Report has finished generating.");
    }
}
```

### Configuration
In your `Startup` class, add `using Punctual` and use the `services.AddPunctual()` method to setup your scheduled action.

You have two options `AddIntervally` and `AddWeekly`. As the names implies, you can run the action on a set interval, or have a weekly schedule. You can chain as many actions as you want to run.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // other services added here
    // ...

    services.AddPunctual(configure => configure
        .AddConfiguration(Configuration)
        .AddIntervally<MyAction>()
        .AddWeekly<MyAction>()
    );
}
```

### Intervally
You can configure an action scheduled at intervals using your configuration (like `appsettings.json`) or manually.

#### Using `appsettings.json`
Add a section to your settings called Punctual. Then, add another section for each action you are configuring with the name of the action as the key.

```json
{
  "Punctual": {
    "MyAction": {
      "Frequency": "Seconds",
      "Period": 10,
      "RunOnStart": true
    }
  }
}
```

`Frequency` can be set to: `Milliseconds`, `Seconds`, `Minutes`, `Hours`, `Days`, `Weeks`.

`Period` is an integer.

`RunOnStart` is `true` or `false`.

#### Using Options
If you don't want to use `appsettings.json` or another form of `IConfiguration`, you can configure it directly in `Startup`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // other services added here
    // ...

    services.AddPunctual(configure => configure
        .AddConfiguration(Configuration)
        .AddIntervally<MyAction>(options =>
        {
            options.Frequency = Frequency.Seconds;
            options.Period = 30;
            options.RunOnStart = true;
        })
    );
}
```

### Weekly
The weekly schduler will allow you to configure the action to run on a weekly schedule on the days and times you setup. 

#### Using `appsettings.json`
Add a section to your settings called Punctual. Then, add another section for each action you are configuring with the name of the action as the key.

```json
{
  "Punctual": {
    "MyAction": {
      "Schedule": [
        {
          "Days": "Weekdays",
          "Times": [ "23:59" ]
        },
        {
          "Days": "Weekend",
          "Times": [ "20:00" ]
        }
      ]
    }
  }
}
```

The weekly sceduler takes a key called `Schedule` which is an array of days and times you want to run.

`Days` can be set to: `Sunday`, `Monday`, `Tuesday`, `Wednesday`, `Thursday`, `Friday`, `Saturday`, `Weekdays`, `Weekend`, `All`.

Days are flags, so you can combine them (e.g. `Monday,Wednesday,Friday`)

`Times` is an array of times to run written in 24 hour format. (e.g 5:00 PM = `"17:00"`)

#### Using Options
If you don't want to use `appsettings.json` or another form of `IConfiguration`, you can configure it directly in `Startup`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // other services added here
    // ...

    services.AddPunctual(configure => configure
        .AddConfiguration(Configuration)
        .AddWeekly<MyWeeklyScheduledAction>(options =>
        {
            options.Schedule.AddRange(new[]
            {
                new DailyScheduleOptions()
                {
                    Days = DaysToRun.Monday | DaysToRun.Wednesday | DaysToRun.Friday,
                    Times = new List<TimeSpan>
                    {
                        TimeSpan.FromHours(0),  // midnight
                        TimeSpan.FromHours(12)  // noon
                    }
                },
                new DailyScheduleOptions()
                {
                    Days = DaysToRun.Weekend,
                    Times = new List<TimeSpan>
                    {
                        TimeSpan.FromHours(0)  // midnight
                    }
                }
            });
        })
    );
}
```

`Schedule` takes a collection of `DailyScheduleOptions` which is a set of `Days` and `Times` to run. Time is just a timespan from 0-24 hours for the time you want to run.

### Further help
Please check out the two samples I've provided for addional help on setting up actions.