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

        var task = () => pollster.RunAutomaticEvery(TimeSpan.FromMilliseconds(10));
        task.Should().NotThrow();

        pollster.Dispose();
    }


    // the used PeriodicTimer is not usable for polling intervalls less than 10ms
    [Fact]
    public void Test_RunAutomatic()
    {
        var counter = 0;
        var pollster = PollsterBuilder.Create()
            .AddJob(() => counter++, TimeSpan.FromMilliseconds(9)).Build();

        pollster.RunAutomaticEvery(TimeSpan.FromMilliseconds(10));

        BlockThread.For(50).Milliseconds();

        pollster.Dispose();

        counter.Should().BeInRange(1, 7);
    }
}