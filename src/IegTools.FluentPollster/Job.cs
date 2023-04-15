namespace IegTools.FluentPollster;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides all infos that are needed to execute a poll task
/// </summary>
public class Job
{
    private readonly PollsterConfiguration _configuration;

    /// <summary>
    /// Define a poll task with a single interval
    /// </summary>
    /// <param name="configuration">The pollster configuration</param>
    /// <param name="action">The action that should be executed when it's time to poll</param>
    /// <param name="pollInterval">The poll interval</param>
    /// <param name="condition">The condition when the poll is enabled</param>
    public Job(PollsterConfiguration configuration, Action action, TimeSpan pollInterval, Func<bool> condition)
    {
        _configuration = configuration;
        JobAction      = action;
        Intervals     = new[] { (pollInterval, condition) };
    }

    /// <summary>
    /// Define a pool task with multiple intervals
    /// </summary>
    /// <param name="configuration">The pollster configuration</param>
    /// <param name="action">The action that should be executed when it's time to poll</param>
    /// <param name="intervals">The poll intervals with there conditions</param>
    public Job(PollsterConfiguration configuration, Action action, IList<(TimeSpan pollInterval, Func<bool> condition)> intervals)
    {
        _configuration = configuration;
        JobAction      = action;
        Intervals     = intervals;
    }


    /// <summary>
    /// The name of the job
    /// </summary>
    public string JobName   { get; init; } = string.Empty;

    /// <summary>
    /// The action that should be executed when it's time to poll
    /// </summary>
    public Action JobAction { get; }

    /// <summary>
    /// The poll intervals with there conditions
    /// </summary>
    public IList<(TimeSpan pollInterval, Func<bool> condition)> Intervals { get; }

    /// <summary>
    /// Last time the job was executed
    /// </summary>
    public DateTime   LastPolledTime   { get; set; } = DateTime.MinValue;
    
    /// <summary>
    /// Last poll was successful
    /// </summary>
    public bool       LastPollResult   { get; set; }

    /// <summary>
    /// Last poll error
    /// </summary>
    public Exception? LastPollError    { get; set; }
    
    /// <summary>
    /// Last poll duration
    /// </summary>
    public TimeSpan   LastPollDuration { get; set; }

    /// <summary>
    /// The number of polls that have been executed
    /// </summary>
    public int PollCount { get; set; }


    /// <summary>
    /// Returns true if any job conditions are met
    /// </summary>
    public bool JobConditionValidated() =>
        Intervals.Any(x => x.condition.Invoke());

    /// <summary>
    /// Returns true if the poll-interval is expired
    /// </summary>
    public bool PollIntervalExpired() =>
        DateTime.Now >= LastPolledTime + GetActivePollInterval();

    /// <summary>
    /// Returns the first poll-interval that is valid
    /// </summary>
    public TimeSpan GetActivePollInterval() => 
        Intervals.OrderBy(x => x.pollInterval)
            .Where(x => x.condition.Invoke())
            .Select(x => x.pollInterval).FirstOrDefault();

    /// <summary>
    /// Job Action will be executed if job-conditions are met and the poll-interval is expired
    /// </summary>
    public void ExecuteJobAction()
    {
        ExecuteJobAction(JobConditionValidated() && PollIntervalExpired());
    }

    /// <summary>
    /// Job Action will be executed if enableExecution is true
    /// </summary>
    public void ExecuteJobAction(bool enableExecution)
    {
        if (!enableExecution) return;

        try
        {
            var lastPolledTime = DateTime.Now;

            JobAction.Invoke();

            // set the LastPolledTime after the job has executed, as the job may fail
            LastPolledTime = lastPolledTime;
            LastPollResult = true;
            LastPollDuration = DateTime.Now - lastPolledTime;
            PollCount++;
        }
        catch (Exception e)
        {
            LastPollError = e;
            LastPollResult = false;
            _configuration.Logger?.LogError(e, $"Error during execution of Job '{JobName}'");
        }
        finally
        {
            _configuration.Logger?.LogTrace($"Execution of Job '{JobName}' took {LastPollDuration}");
        }
    }
}