using Medja.Controls.TextEditor;
using Xunit;

namespace Medja.Test.Controls.TextEditor
{
    public class CaretTest
    {
        [Fact]
        public void GreaterThanOperator()
        {
            var a = new Caret(0, 1);
            var b = new Caret(1,0);
            var c = new Caret(2,1);

            Assert.True(a > b);
            Assert.True(c > b);
            Assert.True(c > a);

            Assert.False(b > a);
            Assert.False(b > c);
            Assert.False(a > c);

            Assert.True(a > null);
            Assert.False(null > a);
        }

        [Fact]
        public void LessThanOperator()
        {
            var a = new Caret(0, 1);
            var b = new Caret(1,0);
            var c = new Caret(2,1);

            Assert.False(a < b);
            Assert.False(c < b);
            Assert.False(c < a);

            Assert.True(b < a);
            Assert.True(b < c);
            Assert.True(a < c);

            Assert.False(a < null);
            Assert.True(null < a);
        }

        [Fact]
        public void EqualsTest()
        {
            var a = new Caret(5,3);
            var b = new Caret(5,3);
            var c = new Caret(5,4);
            var d = new Caret(4,3);

            Assert.Equal(a, b);
            Assert.Equal(b, a);
            Assert.NotEqual(a, c);
            Assert.NotEqual(a, d);

            Assert.True(a == b);
            Assert.True(b == a);
            Assert.False(a == c);
            Assert.False(a == d);
        }
    }
}
