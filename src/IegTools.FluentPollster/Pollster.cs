namespace IegTools.FluentPollster;

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

/// <summary>
/// Service to execute any action like polls and so on.
/// </summary>
public class Pollster : IPollster
{
    private readonly CancellationTokenSource _cts = new();
    private readonly object _automaticPollingLock = new();
    private PollsterConfiguration _configuration  = new();

    private Timer _timer;
    
    /// <summary>
    /// Stops the execution of the jobs and disposes all used resources
    /// </summary>
    public void Dispose()
    {
        Stop();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }


    /// <inheritdoc />
    public IPollster SetConfiguration(PollsterConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    /// <inheritdoc />
    public virtual IPollster Execute()
    {
        lock (_automaticPollingLock)
        {
            _configuration.Jobs
                .Where(x => x.PollIntervalExpired() && x.JobConditionValidated())
                .OrderBy(x => x.LastPolledTime)
                .Take(_configuration.MaxJobsPerPoll).ToList()
                .ForEach(job => job.ExecuteJobAction());
        }
        
        return this;
    }

    /// <inheritdoc />
    public virtual async Task<IPollster> ExecuteAsync() =>
        await Task.Run(Execute, _cts.Token);
    
    /// <inheritdoc />
    public void ExecuteAsBackgroundTaskEvery(TimeSpan pollInterval) =>
        this.ExecuteAsBackgroundTaskEvery(pollInterval, TimeSpan.Zero);

    /// <inheritdoc />
    public void ExecuteAsBackgroundTaskEvery(TimeSpan pollInterval, TimeSpan startDelay)
    {
        _configuration.AutomaticPollInterval = pollInterval;
        _configuration.StartDelay = startDelay;

        _timer = new Timer(TimerExecute, null, _configuration.StartDelay, _configuration.AutomaticPollInterval);
    }

    /// <inheritdoc />
    public void Stop()
    {
        _configuration.Logger?.LogTrace("Automatic polling stop initiated");
        _cts?.Cancel();
        _timer?.Dispose();
        _timer = null;
    }

    private void TimerExecute(object obj)
    {
        try
        {
            if (_timer != null && !_cts.Token.IsCancellationRequested)
            {
                Execute();
                _configuration.Logger?.LogTrace("Automatic polling executed");
            }
        }
        catch (OperationCanceledException e)
        {
            _configuration.Logger?.LogTrace(e, "Automatic polling stopped (operation canceled)");
        }
    }
}