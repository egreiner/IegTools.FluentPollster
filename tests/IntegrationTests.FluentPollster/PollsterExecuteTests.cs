namespace IntegrationTests.FluentPollster;

using FluentAssertions;
using Tools;

public class PollsterExecuteTests
{
    [Fact]
    public void Test_Execute_once()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create()
            .AddJob(() => counter++, TimeSpan.FromMilliseconds(100));

        var pollster = uut.Build();

        pollster.Execute();

        counter.Should().Be(1);
    }

    [Fact]
    public void Test_Execute_job_condition_is_false()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create()
            .AddJob(() => counter++, TimeSpan.FromMilliseconds(100), () => false);

        var pollster = uut.Build();

        pollster.Execute();

        counter.Should().Be(0);
    }

    [Fact]
    public void Test_Execute_multiple_times_counter_is_1()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => counter++, TimeSpan.FromMilliseconds(100));

        var pollster = uut.Build();

        for (int i = 0; i < 3; i++)
        {
            pollster.Execute();
        }

        counter.Should().Be(1);
    }

    [Fact]
    public void Test_Execute_multiple_times_counter_is_20()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => counter++, TimeSpan.FromMicroseconds(0.01));

        var pollster = uut.Build();

        for (int i = 0; i < 20; i++)
        {
            pollster.Execute();
        }

        counter.Should().Be(20);
    }
    
    [Fact]
    public void Test_Execute_multiple_times_counter_is_2()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => counter++, TimeSpan.FromMilliseconds(10));

        var pollster = uut.Build();

        for (int i = 0; i < 20; i++)
        {
            pollster.Execute();
            BlockThread.For(1).Milliseconds();
        }

        counter.Should().BeInRange(2, 10);
    }
    
    [Fact]
    public void Test_Execute_with_failing_Action()
    {
        var uut = PollsterBuilder.Create();

        uut.AddJob(throwException, TimeSpan.FromMicroseconds(10));

        var pollster = uut.Build();

        var task = () => pollster.Execute();
        task.Should().NotThrow();
        
        void throwException() => throw new Exception("Test exception");
    }
}