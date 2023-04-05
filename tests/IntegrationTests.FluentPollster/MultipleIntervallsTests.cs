namespace IntegrationTests.FluentPollster;

using FluentAssertions;
using Tools;

public class MultipleIntervallsTests
{
    [Theory]
    [InlineData("Cond10", 9, 102)]
    [InlineData("Cond20", 19, 52)]
    public void Test_Run_multiple_times_counter_is_2(string condition, int microseconds, int expected)
    {
        var counter = 0;

        var intervalls = new List<(TimeSpan, Func<bool>)>
        {
            (TimeSpan.FromMicroseconds(microseconds), () => condition == "Cond10"),
            (TimeSpan.FromMicroseconds(microseconds), () => condition == "Cond20"),
        };

        var uut = PollsterBuilder.Create()
            .AddJob(() => counter++, intervalls);

        var pollster = uut.Build();

        for (int i = 0; i < 100; i++)
        {
            pollster.Run();
            BlockThread.For(10).Microseconds();
        }

        counter.Should().BeInRange(9, expected);
    }
}