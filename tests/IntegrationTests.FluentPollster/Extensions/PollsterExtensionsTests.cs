namespace IntegrationTests.FluentPollster.Extensions;

using FluentAssertions;

public class PollsterExtensionsTests
{
    [Fact]
    public void Test_Is()
    {
        var actual = new DateTime(2023, 1, 1, 10, 00, 00).IsMinuteDivisibleBy(15);

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
        var actual = new DateTime(2023, 1, 1, 10, minute, 00).IsMinuteDivisibleBy(everyMinute);

        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    public void Test_IsCurrentMinuteDivisibleBy_false(int minute, int everyMinute)
    {
        var actual = new DateTime(2023, 1, 1, 10, minute, 00).IsMinuteDivisibleBy(everyMinute);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(15, 0, true)]
    [InlineData(16, 1, true)]
    [InlineData(14, -1, true)]
    public void Test_IsCurrentMinuteDivisibleBy_offset_false(int minute, int offset, bool expected)
    {
        var actual = new DateTime(2023, 1, 1, 10, minute, 00).IsMinuteDivisibleBy(15, offset);

        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData(00, 00, 00, 150, true)]
    [InlineData(00, 00, 09, 150, true)]
    [InlineData(00, 00, 10, 150, false)]
    [InlineData(00, 01, 10, 150, false)]
    [InlineData(01, 01, 10, 150, false)]
    [InlineData(01, 02, 20, 150, false)]
    [InlineData(01, 02, 30, 150, true)]
    [InlineData(01, 02, 40, 150, false)]
    [InlineData(01, 05, 05, 150, true)]
    [InlineData(12, 07, 39, 150, true)]
    [InlineData(00, 00, 00, 090, true)]
    [InlineData(00, 01, 30, 090, true)]
    [InlineData(00, 01, 39, 090, true)]
    [InlineData(00, 01, 40, 090, false)]
    public void Test_IsDivisibleBySeconds(int hour, int minute, int second, int divisor, bool expected)
    {
        var time = new DateTime(2023,5, 2, hour, minute, second);

        var actual = time.IsDivisibleBySeconds(divisor);

        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData(00, 00, 10, 150, true)]
    [InlineData(00, 00, 19, 150, true)]
    [InlineData(00, 00, 20, 150, false)]
    [InlineData(00, 01, 20, 150, false)]
    [InlineData(01, 01, 20, 150, false)]
    [InlineData(01, 02, 30, 150, false)]
    [InlineData(01, 02, 40, 150, true)]
    [InlineData(01, 02, 50, 150, false)]
    [InlineData(01, 05, 15, 150, true)]
    [InlineData(12, 07, 49, 150, true)]
    [InlineData(00, 00, 10, 090, true)]
    [InlineData(00, 01, 40, 090, true)]
    [InlineData(00, 01, 49, 090, true)]
    [InlineData(00, 01, 50, 090, false)]
    public void Test_IsDivisibleBySeconds_with_SecondsOffset(int hour, int minute, int second, int divisor, bool expected)
    {
        var time = new DateTime(2023,5, 2, hour, minute, second);

        var actual = time.IsDivisibleBySeconds(divisor, 10);

        actual.Should().Be(expected);
    }
}