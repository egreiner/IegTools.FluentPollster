namespace IntegrationTests.FluentPollster;

using FluentAssertions;
using Tools;

public class PollsterTests
{
    [Fact]
    public void Test_Run_counter_is_1()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => counter++, TimeSpan.FromMilliseconds(100));

        var pollster = uut.Build();

        for (int i = 0; i < 3; i++)
        {
            pollster.Run();
        }

        counter.Should().Be(1);
    }

    [Fact]
    public void Test_Run_counter_is_20()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => counter++, TimeSpan.FromMicroseconds(0.01));

        var pollster = uut.Build();

        for (int i = 0; i < 20; i++)
        {
            pollster.Run();
        }

        counter.Should().Be(20);
    }
    
    [Fact]
    public void Test_Run_counter_is_2()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => counter++, TimeSpan.FromMilliseconds(10));

        var pollster = uut.Build();

        for (int i = 0; i < 20; i++)
        {
            pollster.Run();
            BlockThread.For(1).Milliseconds();
        }

        counter.Should().BeLessThan(5);
    }

    [Fact]
    public void Test_Run_with_failing_Action()
    {
        var uut = PollsterBuilder.Create();

        Action failingAction = () => throw new Exception("Test exception");
        uut.AddJob(failingAction, TimeSpan.FromMicroseconds(10));

        var pollster = uut.Build();

        var task = () => pollster.Run();
        task.Should().NotThrow();
    }

    [Fact]
    public void Test_RunAutomatic_with_failing_Action()
    {
        var uut = PollsterBuilder.Create();

        Action failingAction = () => throw new Exception("Test exception");
        uut.AddJob(failingAction, TimeSpan.FromMicroseconds(10));

        var pollster = uut.Build();

        var task = () => pollster.RunAutomaticEvery(TimeSpan.FromMilliseconds(10));
        task.Should().NotThrow();

        pollster.StopAsync().Wait();
    }


    // this timer is not usable for polling intervalls less than 10ms
    [Fact]
    public void Test_RunAutomatic()
    {
        var counter = 0;
        var pollster = PollsterBuilder.Create()
            .AddJob(() => counter++, TimeSpan.FromMicroseconds(10)).Build();

        pollster.RunAutomaticEvery(TimeSpan.FromMilliseconds(10));

        BlockThread.For(100).Milliseconds();

        pollster.StopAsync().Wait();

        counter.Should().BeGreaterOrEqualTo(5);
        counter.Should().BeLessOrEqualTo(10);
    }
}