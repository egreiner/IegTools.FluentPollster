namespace IegTools.FluentPollster;

/// <summary>
/// Interface for a pollster
/// </summary>
public interface IPollster: IDisposable
{
    /// <summary>
    /// Executes all specified jobs once
    /// </summary>
    IPollster Run();

    /// <summary>
    /// Executes async all specified jobs once
    /// </summary>
    Task<IPollster> RunAsync();

    /// <summary>
    /// Executes all specified jobs automatically in an Background Task
    /// with the specified poll intervall, until it is stopped.
    /// </summary>
    /// <param name="pollIntervall">The poll intervall</param>
    void RunAutomaticEvery(TimeSpan pollIntervall);

    /// <summary>
    /// Stops the execution of all specified jobs
    /// </summary>
    Task StopAsync();

    /// <summary>
    /// Set the pollster-configuration
    /// </summary>
    IPollster SetConfiguration(PollsterConfiguration configuration);
}