namespace IegTools.FluentPollster;

[Flags]
public enum FixedToMinutes
{
    NotSpecified        = 0,
    EveryMinute         = 1,
    EveryTwoMinutes     = 2,
    EveryFiveMinutes    = 4,
    EveryTenMinutes     = 8,
    EveryFifteenMinutes = 16,
    EveryTwentyMinutes  = 32,
    EveryThirtyMinutes  = 64,
    EveryHour           = 128
}