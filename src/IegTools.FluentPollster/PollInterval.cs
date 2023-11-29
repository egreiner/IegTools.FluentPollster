namespace IegTools.FluentPollster;

public sealed class PollInterval
{
    public PollInterval(TimeSpan pollInterval, Func<bool> condition)
    {
        IntervalTime = pollInterval;
        Condition    = condition;
    }

    public TimeSpan   IntervalTime { get; }
    public Func<bool> Condition    { get; }

    public FixedToMinutes FixedToMinutes { get; } = FixedToMinutes.NotSpecified;


    public bool HasReachedMinute(int minuteOffset = 0) =>
        HasReachedMinute(DateTime.Now, minuteOffset);

    public bool HasReachedMinute(DateTime dateTime, int minuteOffset = 0)
    {
        // if (FixedToMinutes is FixedToMinutes.NotSpecified or FixedToMinutes is FixedToMinutes.EveryMinute)
        if (FixedToMinutes is FixedToMinutes.NotSpecified)
            return true;

        if (FixedToMinutes.HasFlag(FixedToMinutes.EveryMinute))
            return true;

        var result = false;

        result |= hasFlag(FixedToMinutes.EveryMinute, 1);
        result |= hasFlag(FixedToMinutes.EveryTwoMinutes, 2);
        result |= hasFlag(FixedToMinutes.EveryFiveMinutes, 5);
        result |= hasFlag(FixedToMinutes.EveryTenMinutes, 10);
        result |= hasFlag(FixedToMinutes.EveryFifteenMinutes, 15);
        result |= hasFlag(FixedToMinutes.EveryTwentyMinutes, 20);
        result |= hasFlag(FixedToMinutes.EveryThirtyMinutes, 30);
        result |= hasFlag(FixedToMinutes.EveryHour, 60);

        return result;

        bool hasFlag(FixedToMinutes fixedMinutes, int moduloMinute) =>
            this.FixedToMinutes.HasFlag(fixedMinutes) && (dateTime.Minute - minuteOffset) % moduloMinute == 0;
    }
}