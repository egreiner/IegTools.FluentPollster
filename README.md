# IegTools.FluentPollster

FluentPollster provides a user-friendly fluent interface for creating easy-to-read polling tasks.  


## Why another polling library?
I found that FluentScheduler didn't meet my requirements.  
I had some ideas and well it's less than 500 loc at the moment, so that's nothing.  
Hey and it's fun.


## Usage
To get into the details, explore  the Integration-Tests.  


### Simple polling job

You can add any Action as job to the PollsterBuilder,  
specify the poll-intervall and the condition,  
build the Pollster and run it.  

```csharp
private IPollster _pollster;

public void SimplePolling()
{
    var counter = 0;
    var uut = PollsterBuilder.Create()
        .AddJob(() => counter++, TimeSpan.FromMilliseconds(100), () => HasWhatSoEverCondition());

    _pollster = uut.Build();
    
    // runs only once, so when and how you call it belongs to you
    _pollster.Run();
}
```

### Execute the pollster automatically every x minutes, seconds,...

You can add Actions also without any condition,  
build the Pollster and run it in automatic-mode.  

```csharp
public void AutomaticPolling()
{
    var counter = 0;
    var uut = PollsterBuilder.Create()
        .AddJob(() => counter++, TimeSpan.FromSeconds(5));

    var pollster = uut.Build();
    
    // run automatic (like FluentScheduler does it...)
    pollster.RunAutomaticEvery(TimeSpan.FromSeconds(10));
}
```

### Multiple intervall polling job

You can add any Action with multiple intervall/condition combinations as job to the PollsterBuilder,  
build the Pollster and run it.  

```csharp
private IPollster _pollster;

public void MultipleIntervallPolling()
{
    var counter = 0;

    var intervalls = new List<(TimeSpan, Func<bool>)>
    {
        (TimeSpan.FromSeconds(60),  () => Condition1()),
        (TimeSpan.FromSeconds(120), () => Condition2()),
    };

    var uut = PollsterBuilder.Create()
        .AddJob(() => counter++, intervalls);

    var pollster = uut.Build();

    _pollster.Run();
}
```


## Additional settings

The FluentPollster needs no dependency injection but you can inject an ILogger
so you will be informed about any exceptions thrown in your poll actions.  
(all other important events are logged as 'Trace' at the moment)  

And you can set the maximum number of poll tasks that should be executed during one poll cycle.

```csharp
    var pollster = PollsterBuilder.Create()
        .SetLogger(logger)
        .SetMaxJobsPerPoll(10)
        .AddJob(() => Job1(), TimeSpan.FromSeconds(5));
        .AddJob(() => Job2(), TimeSpan.FromSeconds(10));
        .Build();

    pollster.RunAutomaticEvery(TimeSpan.FromSeconds(1));
```

## ExtensionMethods

At the moment there is only one ExtensionMethod, you can use  
  DateTime.Now.IsCurrentMinuteDivisibleBy(...) to force your polling task to run only at 'special' minutes within an hour.

Example:  
    Use DateTime.Now.IsCurrentMinuteDivisibleBy(5) as condition and your action will be called only in 
    minute 0, 5, 10, 15 and so on  
or:  
    Use DateTime.Now.IsCurrentMinuteDivisibleBy(15) as condition and your action will be called only in 
    minute 0, 15, 30 and 45  

for further information -> IntegrationsTests...

```csharp
    IsCurrentMinuteDivisibleBy(this DateTime time, int everyMinutes, int offsetMinute = 0)
```
