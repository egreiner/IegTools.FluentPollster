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
    public static bool IsCurrentMinuteDivisibleBy(this DateTime time, int everyMinutes, int offsetMinute = 0) => 
        everyMinutes <= 0 
        ? false
        : (time.Minute + offsetMinute) % everyMinutes == 0;
}