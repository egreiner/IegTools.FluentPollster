// ReSharper disable once CheckNamespace
namespace IegTools.FluentPollster;

/// <summary>
/// DateTime extension methods
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns true if the minute and optional minute-offset of the specified date-time
    /// is divisible by everyMinutes with remainder is 0
    /// </summary>
    /// <param name="time">The date-time</param>
    /// <param name="everyMinutes">Every x minutes</param>
    /// <param name="offsetMinute">The offset minute</param>
    public static bool IsDivisibleByMinutes(this DateTime time, int everyMinutes, int offsetMinute = 0) =>
        everyMinutes > 0 && (time.Minute - offsetMinute) % everyMinutes == 0;

    /// <summary>
    /// Returns true if the combined seconds
    /// (derived from the minute and second values of the specified date-time, adjusted by an optional seconds-offset)
    /// are divisible by the provided <see cref="secondsDivisor"/> (with a remainder of 0) within a span of 10 seconds.
    /// </summary>
    /// <param name="time">The DateTime</param>
    /// <param name="secondsDivisor">The second divisor</param>
    /// <param name="offsetSeconds">The offset minute</param>
    public static bool IsDivisibleBySeconds(this DateTime time, int secondsDivisor, int offsetSeconds = 0)
    {
        var seconds = (time.Minute * 60 + time.Second - offsetSeconds) / 10;
        return seconds % (secondsDivisor / 10) == 0;
    }
}