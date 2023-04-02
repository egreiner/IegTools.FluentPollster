namespace IntegrationTests.FluentPollster.Tools;

using System.Diagnostics;

/// <summary>
/// Thread is blocked for defined time with doing nothing.
/// Waste the time in an while loop.
/// This is written in a kind of DSL way...
/// </summary>
public sealed class BlockThread
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private readonly long _blockTime;


    private BlockThread(long theBlockTime) => _blockTime = theBlockTime;


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
    public void Ticks() => BlockForTicks(_blockTime);


    /// <summary>
    /// Thread is blocked for defined microseconds.
    /// </summary>
    public void Microseconds()
    {
        var ticks = _blockTime * Stopwatch.Frequency / 1000_000;
        BlockForTicks(ticks);
    }

    /// <summary>
    /// Thread is blocked for defined microseconds.
    /// </summary>
    public void Milliseconds()
    {
        var ticks = _blockTime * Stopwatch.Frequency / 1000;
        BlockForTicks(ticks);
    }


    private void BlockForTicks(long ticks)
    {
        while (_stopwatch.ElapsedTicks < ticks)
        {
            // waste time with doing nothing
        }
    }
}