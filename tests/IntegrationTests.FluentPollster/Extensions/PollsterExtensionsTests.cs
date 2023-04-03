namespace IntegrationTests.FluentPollster.Extensions;

using FluentAssertions;

public class PollsterExtensionsTests
{
    [Fact]
    public void Test_Is()
    {
        var actual = new DateTime(2023, 1, 1, 10, 00, 00).IsCurrentMinuteDivisibleBy(15);

        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(3, 1)]
    [InlineData(0, 2)]
    [InlineData(2, 2)]
    [InlineData(0, 3)]
    [InlineData(3, 3)]
    [InlineData(6, 3)]
    public void Test_IsCurrentMinuteDivisibleBy_true(int minute, int everyMinute)
    {
        var actual = new DateTime(2023, 1, 1, 10, minute, 00).IsCurrentMinuteDivisibleBy(everyMinute);

        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    public void Test_IsCurrentMinuteDivisibleBy_false(int minute, int everyMinute)
    {
        var actual = new DateTime(2023, 1, 1, 10, minute, 00).IsCurrentMinuteDivisibleBy(everyMinute);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(15, 0, true)]
    [InlineData(14, 1, true)]
    [InlineData(16, -1, true)]
    public void Test_IsCurrentMinuteDivisibleBy_offset_false(int minute, int offset, bool expected)
    {
        var actual = new DateTime(2023, 1, 1, 10, minute, 00).IsCurrentMinuteDivisibleBy(15, offset);

        actual.Should().Be(expected);
    }
}