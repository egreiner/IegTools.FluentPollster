namespace IntegrationTests.FluentPollster.Tools;

using System.Diagnostics;
using FluentAssertions;

/// <summary>
/// Tests are very inaccurate.
/// Take them for manually testing.
/// </summary>
public class BlockThreadTests
{
    // initialize BlockThread class
    public BlockThreadTests()
    {
        // warmup
        BlockThread.For(1).Milliseconds();
        BlockThread.For(1).Microseconds();
        BlockThread.For(1).Ticks();
    }


    [Fact]
    public void Test_Block_Ticks()
    {
        var start = Stopwatch.GetTimestamp();

        BlockThread.For(100).Ticks();

        var actual = Stopwatch.GetTimestamp() - start;

        actual.Should().BeInRange(100, 150);
    }

    [Fact]
    public void Test_Block_Microseconds()
    {
        var start = Stopwatch.GetTimestamp();

        BlockThread.For(100).Microseconds();

        var actual = (Stopwatch.GetTimestamp() - start) / 10;

        actual.Should().BeInRange(100, 150);
    }

    [Fact]
    public void Test_Block_Milliseconds()
    {
        var start = Stopwatch.GetTimestamp();

        BlockThread.For(5).Milliseconds();

        var actual = (Stopwatch.GetTimestamp() - start) / 10_000;

        actual.Should().BeInRange(5, 10);
    }
}