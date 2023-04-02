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
    }


    /// <inheritdoc />
    public IPollster SetConfiguration(PollsterConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }


    /// <inheritdoc />
    public virtual IPollster Run()
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
    public virtual async Task<IPollster> RunAsync() =>
        await Task.Run(Run, _cts.Token);


    /// <inheritdoc />
    public void RunAutomaticEvery(TimeSpan pollIntervall)
    {
        _configuration.AutomaticPollIntervall = pollIntervall;

        _timer = new PeriodicTimer(pollIntervall);
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
                await RunAsync();
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