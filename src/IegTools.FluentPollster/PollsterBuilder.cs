namespace IegTools.FluentPollster;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides methods to build a polling service.
/// </summary>
public class PollsterBuilder : IPollsterBuilder
{
    /// <inheritdoc />
    public PollsterConfiguration Configuration { get; init; } = new();


    /// <summary>
    /// Creates a new Pollster-Builder for configuration in .NET 6 style.
    /// This is good for short crispy configs.
    /// </summary>
    public static IPollsterBuilder Create() =>
        new PollsterBuilder();
    
    /// <inheritdoc />
    public IPollster Build() =>
        Build<Pollster>();

    /// <inheritdoc />
    public IPollster Build<TPollster>() where TPollster : IPollster, new()
    {
        return new TPollster().SetConfiguration(Configuration);
    }


    /// <inheritdoc />
    public IPollsterBuilder AddJob(Action action, TimeSpan pollInterval, string jobName = "") =>
        AddJob(action, pollInterval, () => true, jobName);


    /// <inheritdoc />
    public IPollsterBuilder AddJob(Action action, TimeSpan pollInterval, Func<bool> condition, string jobName = "")
    {
        Configuration.Jobs.Add(new Job(Configuration, action, pollInterval, condition)
        {
            JobName = jobName,
        });
        return this;
    }

    /// <inheritdoc />
    [Obsolete("Use new AddPollInterval method instead.")]
    public IPollsterBuilder AddJob(Action action, IList<(TimeSpan pollInterval, Func<bool> condition)> intervals, string jobName = "")
    {
        Configuration.Jobs.Add(new Job(Configuration, action, intervals)
        {
            JobName = jobName,
        });
        return this;
    }


    /// <inheritdoc />
    public IPollsterBuilder SetLogger(ILogger logger)
    {
        Configuration.Logger = logger;
        return this;
    }

    /// <inheritdoc />
    public IPollsterBuilder SetMaxJobsPerPoll(int maxJobsPerPoll)
    {
        Configuration.MaxJobsPerPoll = maxJobsPerPoll;
        return this;
    }
}