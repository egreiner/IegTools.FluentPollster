namespace IegTools.FluentPollster;

using Microsoft.Extensions.Logging;

public class PollsterConfiguration
{
    public ILogger? Logger { get; set; }

    public TimeSpan AutomaticPollIntervall { get; set; } = TimeSpan.Zero;

    public IList<Job> Jobs    { get; set; } = new List<Job>();
    public int MaxJobsPerPoll { get; set; } = int.MaxValue;
}