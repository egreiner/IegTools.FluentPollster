namespace IntegrationTests.FluentPollster.Tools;

using System.Diagnostics;

/// <summary>
/// Thread is blocked for defined time with doing nothing.
/// Waste the time in an while loop.
/// This is written in a kind of DSL way...
/// </summary>
public sealed class BlockThread
{
    private readonly long _startTime;
    private readonly long _blockTime;


    private BlockThread(long theBlockTime)
    {
        _startTime = Stopwatch.GetTimestamp();
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
    /// Thread is blocked for defined microseconds.
    /// </summary>
    public void Microseconds() =>
        Block(TimeSpan.FromMicroseconds(_blockTime));

    /// <summary>
    /// Thread is blocked for defined milliseconds.
    /// </summary>
    public void Milliseconds() =>
        Block(TimeSpan.FromMilliseconds(_blockTime));


    private void Block(TimeSpan timeSpan)
    {
        while (Stopwatch.GetElapsedTime(_startTime) < timeSpan)
        {
            // waste time...
        }
    }
}