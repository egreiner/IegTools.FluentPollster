namespace IegTools.FluentPollster;

/// <summary>
/// Interface for a pollster
/// </summary>
public interface IPollster: IDisposable
{

    /// <summary>
    /// Executes all specified jobs once
    /// </summary>
    IPollster Execute();

    /// <summary>
    /// Async Executes all specified jobs once
    /// </summary>
    Task<IPollster> ExecuteAsync();
    
    /// <summary>
    /// Executes all specified jobs automatically in an Background Task
    /// with the specified poll interval, until it is stopped.
    /// </summary>
    /// <param name="pollInterval">The poll interval</param>
    void RunAutomaticEvery(TimeSpan pollInterval);

    /// <summary>
    /// Executes all specified jobs automatically in an Background Task
    /// with the specified poll interval, until it is stopped.
    /// </summary>
    /// <param name="pollInterval">The poll interval</param>
    /// <param name="delay">The delay before the poll interval starts</param>
    void RunAutomaticEvery(TimeSpan pollInterval, TimeSpan delay);

    /// <summary>
    /// Stops the execution of all specified jobs
    /// </summary>
    void Stop();

    /// <summary>
    /// Set the pollster-configuration
    /// </summary>
    IPollster SetConfiguration(PollsterConfiguration configuration);
}