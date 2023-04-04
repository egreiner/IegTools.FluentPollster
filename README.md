# IegTools.FluentPollster

IegTools.FluentPollster provides a fluent interface for creating easy-to-read polling jobs.  


## Why 'yet another polling library'?
FluentScheduler didn't really fit well to my needs.
I had some ideas and well it's less than 500 loc at the moment, so thats nothing.
Hey and it's fun.


## Usage
To get an overview study the IntegrationTests.


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
specify the poll-intervall without any condition,
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



// TODO to be continued


