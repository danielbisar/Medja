using Medja.Primitives;
using Xunit;

namespace Medja.Test.Primitives
{
    public class RectTest
    {
        [Fact]
        public void SetFromSetsAllValues()
        {
            var r1 = new Rect();
            var r2 = new Rect(1, 1, 2, 2);

            r1.SetFrom(r2);

            Assert.Equal(r2, r1);
            Assert.Equal(r2.X, r1.X);
            Assert.Equal(r2.Y, r1.Y);
            Assert.Equal(r2.Width, r1.Width);
            Assert.Equal(r2.Height, r1.Height);
        }

        [Fact]
        public void EqualsTest()
        {
            var r1 = new Rect();
            var r2 = new Rect(1, 2, 3, 4);
            var r3 = new Rect(1, 2, 3, 4);

            Assert.NotEqual(r2, r1);
            Assert.Equal(r3, r2);
        }
    }
}