namespace IntegrationTests.FluentPollster.Tools;

using System.Diagnostics;
using FluentAssertions;

/// <summary>
/// Tests are very inaccurate.
/// Take them for manually testing.
/// </summary>
public class BlockThreadTests
{
    ////// initialize BlockThread class
    ////public BlockThreadTests()
    ////{
    ////    // warmup
    ////    BlockThread.For(1).Milliseconds();
    ////    BlockThread.For(1).Microseconds();
    ////}


    [Fact]
    public void Test_Block_Microseconds()
    {
        var start = Stopwatch.GetTimestamp();

        BlockThread.For(100).Microseconds();

        var actual = Stopwatch.GetElapsedTime(start).TotalMicroseconds;

        actual.Should().BeInRange(100, 110);
    }

    [Fact]
    public void Test_Block_Milliseconds()
    {
        var start = Stopwatch.GetTimestamp();

        BlockThread.For(5).Milliseconds();

        var actual = Stopwatch.GetElapsedTime(start).TotalMilliseconds;

        actual.Should().BeInRange(5, 7);
    }
}