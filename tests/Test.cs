
using Xunit;

namespace AlarmClock.tests
{
    public class Test
    {
        [Fact]
        public void Tst()
        {
            Assert.Equal(1, 2 - 1);
        }

        [Fact]
        public void Fail()
        {
            Assert.Equal(1, 2);
        }
    }
}
