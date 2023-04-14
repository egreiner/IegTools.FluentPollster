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

    private PeriodicTimer? _timer;
    private Task<IPollster>? _timerTask;

    
    /// <summary>
    /// Stops the execution of the jobs and disposes all used resources
    /// </summary>
    public void Dispose()
    {
        StopAsync().Wait();
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
                .Where(x => x.PollIntervallExpired() && x.JobConditionValidated())
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
    [Obsolete("Use Execute instead")]
    public virtual IPollster Run() => Execute();

    /// <inheritdoc />
    [Obsolete("Use ExecuteAsync instead")]
    public virtual async Task<IPollster> RunAsync() => await ExecuteAsync();


    /// <inheritdoc />
    public void RunAutomaticEvery(TimeSpan pollInterval)
    {
        _configuration.AutomaticPollIntervall = pollInterval;

        _timer = new PeriodicTimer(pollInterval);
        _timerTask = RunAutomaticAsync();
    }

    /// <inheritdoc />
    public async Task StopAsync()
    {
        if (_timerTask == null) return;

        _configuration.Logger?.LogTrace("Automatic polling stop initiated");
        _cts.Cancel();
        await _timerTask;
        _timer?.Dispose();
        _timerTask.Dispose();
    }

    private async Task<IPollster> RunAutomaticAsync()
    {
        try
        {
            while (_timer != null && await _timer.WaitForNextTickAsync(_cts.Token) && !_cts.Token.IsCancellationRequested)
            {
                await ExecuteAsync();
                _configuration.Logger?.LogTrace("Automatic polling executed");
            }
        }
        catch (OperationCanceledException e)
        {
            _configuration.Logger?.LogTrace(e, "Automatic polling stopped (operation canceled)");
        }
        return this;
    }
}