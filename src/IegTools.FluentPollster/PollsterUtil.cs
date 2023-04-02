namespace IegTools.FluentPollster;

public static class PollsterUtil
{
    /// <summary>
    /// Returns true if the current minute and optional minute-offset is divisible by everyMinutes with remainder is 0
    /// </summary>
    /// <param name="everyMinutes">Every x minutes</param>
    /// <param name="offsetMinute">The offset minute</param>
    public static bool IsCurrentMinuteDivisibleBy(int everyMinutes, int offsetMinute = 0) =>
        (DateTime.Now.Minute + offsetMinute) % everyMinutes == 0;
}