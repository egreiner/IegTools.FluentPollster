namespace IegTools.FluentPollster;

using Microsoft.Extensions.Logging;

public class PollsterConfiguration
{
    public ILogger? Logger { get; set; }

    public TimeSpan AutomaticPollIntervall { get; set; } = TimeSpan.Zero;
    public TimeSpan AutomaticStartDelay    { get; set; } = TimeSpan.FromSeconds(2);

    public IList<Job> Jobs    { get; set; } = new List<Job>();
    public int MaxJobsPerPoll { get; set; } = int.MaxValue;
}