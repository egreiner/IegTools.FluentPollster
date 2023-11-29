namespace IntegrationTests.FluentPollster;

using FluentAssertions;
using Tools;

public class PollsterRunAutomaticTests
{
    [Fact]
    public void Test_RunAutomatic_with_failing_Action()
    {
        var uut = PollsterBuilder.Create();

        Action failingAction = () => throw new Exception("Test exception");
        uut.AddJob(failingAction, TimeSpan.FromMicroseconds(10));

        var pollster = uut.Build();

        var task = () => pollster.ExecuteAsBackgroundTaskEvery(TimeSpan.FromMilliseconds(10));
        task.Should().NotThrow();

        pollster.Dispose();
    }


    // the used Timer is not usable for polling intervals less than 10ms
    [Fact]
    public void Test_RunAutomatic()
    {
        var counter = 0;
        var pollster = PollsterBuilder.Create()
            .AddJob(() => counter++, TimeSpan.FromMilliseconds(9)).Build();

        pollster.ExecuteAsBackgroundTaskEvery(TimeSpan.FromMilliseconds(10));

        BlockThread.For(50).Milliseconds();

        pollster.Dispose();

        counter.Should().BeInRange(1, 7);
    }
    
    
    [Fact]
    public void Test_RunAutomatic_stop_execution()
    {
        var counter = 0;
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => counter++, TimeSpan.FromMilliseconds(9));

        var pollster = uut.Build();

        pollster.ExecuteAsBackgroundTaskEvery(TimeSpan.FromMilliseconds(10));

        for (int i = 0; i < 20; i++)
        {
            if (i > 5)
            {
                pollster.Stop();
                break;
            }

            BlockThread.For(1).Milliseconds();
        }

        counter.Should().BeInRange(1, 10);
    }
}