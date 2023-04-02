namespace IntegrationTests.FluentPollster;

using FluentAssertions;
using Microsoft.Extensions.Logging;

public class BuilderTests
{
    [Fact]
    public void Test_add_simple_job()
    {
        var uut = PollsterBuilder.Create();

        uut.AddJob(() => { }, TimeSpan.FromMicroseconds(100), "Job1");

        uut.Should().NotBeNull();
    }

    [Fact]
    public void Test_add_simple_job_fluently()
    {
        var uut = PollsterBuilder.Create()
            .AddJob(() => { }, TimeSpan.FromMicroseconds(100), "Job1");

        uut.Should().NotBeNull();
    }

    [Fact]
    public void Test_SetLogger()
    {
        // or use dependency injection as usual...
        var logger = new Logger<BuilderTests>(new LoggerFactory());
        
        var uut = PollsterBuilder.Create()
            .SetLogger(logger);

        uut.Configuration.Logger.Should().NotBeNull();
    }

    [Fact]
    public void Test_SetMaxJobsPerPoll()
    {
        var uut = PollsterBuilder.Create()
            .SetMaxJobsPerPoll(10);

        uut.Configuration.MaxJobsPerPoll.Should().Be(10);
    }
}