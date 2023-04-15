namespace IegTools.FluentPollster;

using Microsoft.Extensions.Logging;

/// <summary>
/// The configuration for the pollster
/// </summary>
public class PollsterConfiguration
{
    /// <summary>
    /// The logger that can be used for logging
    /// </summary>
    public ILogger? Logger { get; set; }

    /// <summary>
    /// The automatic poll interval
    /// </summary>
    public TimeSpan AutomaticPollInterval { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// The delay before the first automatic poll
    /// </summary>
    public TimeSpan StartDelay { get; set; }
 
    /// <summary>
    /// List of jobs that should be polled
    /// </summary>
    public IList<Job> Jobs    { get; set; } = new List<Job>();

    /// <summary>
    /// Maximum number of jobs that should be polled per poll
    /// </summary>
    public int MaxJobsPerPoll { get; set; } = int.MaxValue;
}