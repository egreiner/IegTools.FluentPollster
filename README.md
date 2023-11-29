# IegTools.FluentPollster

FluentPollster provides a user-friendly fluent interface for creating easy-to-read polling tasks.  
The library is written in C# 11.0 and targets .NET Standard 2.0 (.NET and .NET Framework).  

## Topics
[Why another polling library?](#why-another-polling-library?)    
[Installation](#installation)  
[Usage](#usage)  
[Additional settings](#additional-settings)   
[ExtensionMethods](#extensionmethods)  
[Breaking Changes](#breaking-changes)  


## Why another polling library?
I need polling for two scenarios
- get data from an third party Api
- get data from 1-Wire Bus DS18B20 Temperature-sensors

I found that FluentScheduler didn't meet my requirements.  
I had some ideas and well it's less than 500 loc at the moment, so that's nothing.  
Hey and it's fun.


## Installation
The library is available as a [NuGet package](https://www.nuget.org/packages/IegTools.FluentPollster/).


## Usage
To get into the details, explore the Integration-Tests.  


### Simple polling job

You can add any Action as job to the PollsterBuilder,  
specify the poll-interval and the condition,  
build the Pollster and run it.  

```csharp
private IPollster _pollster;

public void SimplePolling()
{
    var counter = 0;
    var uut = PollsterBuilder.Create()
        .AddJob(() => counter++, TimeSpan.FromMilliseconds(100), () => HasWhatSoEverCondition());

    _pollster = uut.Build();
    
    // executes only once, so when and how you call it belongs to you
    _pollster.Execute();
}
```

### Execute the pollster automatically every x minutes, seconds,...

You can add Actions also without any condition,  
build the Pollster and run it in automatic-mode as background task.  

```csharp
private IPollster _pollster;

public void AutomaticPolling()
{
    var counter = 0;
    var uut = PollsterBuilder.Create()
        .AddJob(() => counter++, TimeSpan.FromSeconds(5));

    _pollster = uut.Build();
    
    // runs automatic as background task (like FluentScheduler does it...)
    _pollster.ExecuteAsBackgroundTaskEvery(TimeSpan.FromSeconds(10));
}
```

### Multiple interval polling job

You can add any Action with multiple interval/condition combinations as job to the PollsterBuilder,  
build the Pollster and run it.  

```csharp
private IPollster _pollster;

public void MultipleIntervalPolling()
{
    var counter = 0;

    var intervals = new List<(TimeSpan, Func<bool>)>
    {
        (TimeSpan.FromSeconds(60),  () => Condition1()),
        (TimeSpan.FromSeconds(120), () => Condition2()),
    };

    var uut = PollsterBuilder.Create()
        .AddJob(() => counter++, intervals);

    var pollster = uut.Build();

    _pollster.Execute();
}
```


## Additional settings

### Logger
The FluentPollster does not require dependency injection,
however, it provides the option to inject an ILogger so that you can receive notifications about
any exceptions that may occur during your poll actions.   
Additionally, all other significant events are currently being logged as 'Trace'.

### Maximum tasks per poll cycle
And you can set the maximum number of poll tasks that should be executed during one poll cycle.

```csharp
    _pollster = PollsterBuilder.Create()
        .SetLogger(logger)
        .SetMaxJobsPerPoll(10)
        .AddJob(() => Job1(), TimeSpan.FromSeconds(5));
        .AddJob(() => Job2(), TimeSpan.FromSeconds(10));
        .Build();

    _pollster.RunAutomaticEvery(TimeSpan.FromSeconds(1));
```

## ExtensionMethods

### IsMinuteDivisibleBy

```csharp
    // Returns true if the minute and optional minute-offset of the specified date-time  
    // is divisible by everyMinutes with remainder is 0  
    public static bool IsMinuteDivisibleBy(this DateTime time, int everyMinutes, int offsetMinute = 0)
```

Examples:  
- Use `DateTime.Now.IsMinuteDivisibleBy(5)` as condition and your action will be called only in 
minute 0, 5, 10, 15...  

- Use `DateTime.Now.IsMinuteDivisibleBy(15)` as condition and your action will be called only in 
minute 0, 15, 30 and 45  

- Use `DateTime.Now.IsMinuteDivisibleBy(15, 1)` as condition and your action will be called only in 
minute 1, 16, 31 and 46  


### IsDivisibleBySeconds

```csharp
    // Returns true if the combined seconds  
    // (derived from the minute and second values of the specified date-time, adjusted by an optional seconds-offset)  
    // are divisible by the provided secondsDivisor (with a remainder of 0) within a span of 10 seconds.  
    public static bool IsDivisibleBySeconds(this DateTime time, int secondsDivisor, int offsetSeconds = 0)
```

Examples:
- Use `DateTime.Now.IsDivisibleBySeconds(150)` as condition and your action will be called only in
  minute 0, 2.5, 5, 7.5...  


For additional details, please refer to IntegrationTests.


# Breaking Changes
## preparing for v2.2 to v3.0
- ExtensionMethod .IsMinuteDivisibleBy(...) is deleted use method .IsDivisibleByMinutes(...) instead
- renamed Pollster.RunAutomaticEvery(...) to Pollster.ExecuteAsBackgroundTaskEvery(...)


## v2.1 to v2.2
- ExtensionMethod .IsMinuteDivisibleBy(...) is now obsolete and will be deleted in future versions
  use method .IsDivisibleByMinutes(...) instead

## v1 to v2
- switched from net7.0 to netstandard2.0 (there is no PeriodicTimer in netstandard2.0...)
- rename method poller.Run...() to poller.Execute...()
- rename method poller.StopAsync() to poller.Stop()
- rename ExtensionMethod IsCurrentMinuteDivisibleBy(...) to IsMinuteDivisibleBy(...)
- change behaviour of ExtensionMethod IsMinuteDivisibleBy(...) offsetMinute  
  from (time.Minute + offsetMinute) to (time.Minute - offsetMinute)