namespace IntegrationTests.FluentPollster.Tools;

using System.Diagnostics;

/// <summary>
/// Thread is blocked for defined time with doing nothing.
/// Waste the time in an while loop.
/// This is written in a kind of DSL way...
/// </summary>
public sealed class BlockThread
{
    private readonly long _start;
    private readonly long _blockTime;


    private BlockThread(long theBlockTime)
    {
        _start     = Stopwatch.GetTimestamp();
        _blockTime = theBlockTime;
    }


    /// <summary>
    /// Thread is blocked for defined time with doing nothing.
    /// Waste the time in an while loop.
    /// </summary>
    /// <param name="blockTime"></param>
    public static BlockThread For(long blockTime) =>
        new(blockTime);


    /// <summary>
    /// Thread is blocked for defined ticks.
    /// </summary>
    public void Ticks() =>
        BlockForTicks(_blockTime);

        
    /// <summary>
    /// Thread is blocked for defined microseconds.
    /// </summary>
    public void Microseconds() =>
        BlockForTicks(_blockTime * Stopwatch.Frequency / 1_000_000);

    /// <summary>
    /// Thread is blocked for defined milliseconds.
    /// </summary>
    public void Milliseconds() =>
        BlockForTicks(_blockTime * Stopwatch.Frequency / 1_000);


    private void BlockForTicks(long ticks)
    {
        while (Stopwatch.GetTimestamp() - _start < ticks)
        {
            // waste time...
        }
    }
}