namespace IegTools.FluentPollster;

using Microsoft.Extensions.Logging;

/// <summary>
/// Interface for a pollster builder
/// </summary>
public interface IPollsterBuilder
{
    /// <summary>
    /// The pollster configuration
    /// </summary>
    PollsterConfiguration Configuration { get; init; }

    /// <summary>
    /// Builds a default pollster with the specified configuration
    /// </summary>
    IPollster Build();

    /// <summary>
    /// Builds a customized pollster with the specified configuration
    /// </summary>
    IPollster Build<TPollster>() where TPollster : IPollster, new();


    /// <summary>
    /// Adds a simple job to the pollster-configuration
    /// </summary>
    /// <param name="action">The action that should be executed</param>
    /// <param name="pollIntervall">The poll intervall</param>
    /// <param name="jobName">The job name</param>
    IPollsterBuilder AddJob(Action action, TimeSpan pollIntervall, string jobName = "");

    /// <summary>
    /// Adds a simple job to the pollster-configuration
    /// </summary>
    /// <param name="action">The action that should be executed</param>
    /// <param name="pollIntervall">The poll intervall</param>
    /// <param name="condition">The condition when the execution of the action is allowed</param>
    /// <param name="jobName">The job name</param>
    IPollsterBuilder AddJob(Action action, TimeSpan pollIntervall, Func<bool> condition, string jobName = "");

    /// <summary>
    /// Adds a job to the pollster-configuration
    /// with multiple
    /// </summary>
    /// <param name="action">The action that should be executed</param>
    /// <param name="intervalls">The list of poll intervalls with there conditions</param>
    /// <param name="jobName">The job name</param>
    IPollsterBuilder AddJob(Action action, IList<(TimeSpan pollIntervall, Func<bool> condition)> intervalls, string jobName = "");

    /// <summary>
    /// Injects an logger
    /// </summary>
    /// <param name="logger">The logger</param>
    IPollsterBuilder SetLogger(ILogger logger);

    /// <summary>
    /// Sets the maximum jobs that will be executed per poll.
    /// Remaining jobs will be executed at the next poll.
    /// </summary>
    /// <param name="maxJobsPerPoll">The maximum count of jobs per poll. Default is int.MaxValue</param>
    IPollsterBuilder SetMaxJobsPerPoll(int maxJobsPerPoll);
}