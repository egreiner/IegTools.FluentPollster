namespace IegTools.FluentPollster;

using Microsoft.Extensions.Logging;

public class Job
{
    private readonly PollsterConfiguration _configuration;

    public Job(PollsterConfiguration configuration, Action action, TimeSpan pollIntervall, Func<bool> condition)
    {
        _configuration = configuration;
        JobAction      = action;
        Intervalls     = new[] { (pollIntervall, condition) };
    }

    public Job(PollsterConfiguration configuration, Action action, IList<(TimeSpan pollIntervall, Func<bool> condition)> intervalls)
    {
        _configuration = configuration;
        JobAction      = action;
        Intervalls     = intervalls;
    }


    public string JobName   { get; init; } = string.Empty;

    public Action JobAction { get; }

    public IList<(TimeSpan pollIntervall, Func<bool> condition)> Intervalls { get; }


    public DateTime   LastPolledTime   { get; set; } = DateTime.MinValue;
    public bool       LastPollResult   { get; set; }
    public Exception? LastPollError    { get; set; }
    public TimeSpan   LastPollDuration { get; set; }

    public int PollCount { get; set; }


    public bool JobConditionValidated() =>
        Intervalls.Any(x => x.condition.Invoke());

    public bool PollIntervallExpired() =>
        DateTime.Now >= LastPolledTime + GetActivePollIntervall();

    public TimeSpan GetActivePollIntervall() => 
        Intervalls.OrderBy(x => x.pollIntervall)
            .Where(x => x.condition.Invoke())
            .Select(x => x.pollIntervall).FirstOrDefault();

    /// <summary>
    /// Job Action will be executed if job-conditions are met and the poll-intervall is expired
    /// </summary>
    public void ExecuteJobAction()
    {
        ExecuteJobAction(JobConditionValidated() && PollIntervallExpired());
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