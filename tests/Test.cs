
using FluentAssertions;
using Xunit;

namespace AlarmClock.tests
{
    public class Test
    {
        [Fact]
        public void Tst() => Add(1, 2).Should().Be(1 + 2, "Math");

        [Fact]
        public void Fail() => Add(1, 5).Should().NotBe(1 - 5, "Math!");

        public int Add(int a, int b) => a + b;
    }
}
