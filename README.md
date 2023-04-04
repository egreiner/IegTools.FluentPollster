# IegTools.FluentPollster

FluentPollster provides a user-friendly fluent interface for creating easy-to-read polling tasks.  


## Why 'yet another polling library'?
I found that FluentScheduler didn't meet my requirements.  
I had some ideas and well it's less than 500 loc at the moment, so thats nothing.  
Hey and it's fun.


## Usage
To get into the details study the IntegrationTests.  


### Simple polling job

You can add any Action as job to the PollsterBuilder,  
specify the poll-intervall and the condition,  
build the Pollster and run it.  

```csharp
public void SimplePolling()
{
    var counter = 0;
    var uut = PollsterBuilder.Create()
        .AddJob(() => counter++, TimeSpan.FromMilliseconds(100), () => IsWhatSoEverCondition());

    var pollster = uut.Build();
    
    // run only once, so the time when you call it belongs to you
    pollster.Run();
}
```

### Automatic polling jobs

You can add any Action as job to the PollsterBuilder,  
specify the poll-intervall (also without any condition),  
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

### Details

#### You can inject an ILogger  

```csharp
    PollsterBuilder.Create().SetLogger(logger);
```


#### Set the maximum poll tasks that should be executed during one poll cycle  

```csharp
	PollsterBuilder.Create().SetMaxJobsPerPoll(10);
```


to be continued...