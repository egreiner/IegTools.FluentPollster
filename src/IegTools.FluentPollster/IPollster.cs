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
    /// Executes all specified jobs once
    /// </summary>
    [Obsolete("Use Execute instead")]
    IPollster Run();

    /// <summary>
    /// Async Executes all specified jobs once
    /// </summary>
    [Obsolete("Use ExecuteAsync instead")]
    Task<IPollster> RunAsync();

    /// <summary>
    /// Executes all specified jobs automatically in an Background Task
    /// with the specified poll intervall, until it is stopped.
    /// </summary>
    /// <param name="pollInterval">The poll intervall</param>
    void RunAutomaticEvery(TimeSpan pollInterval);

    /// <summary>
    /// Stops the execution of all specified jobs
    /// </summary>
    Task StopAsync();

    /// <summary>
    /// Set the pollster-configuration
    /// </summary>
    IPollster SetConfiguration(PollsterConfiguration configuration);
}