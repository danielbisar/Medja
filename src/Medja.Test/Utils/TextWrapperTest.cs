using Medja.Primitives;
using Medja.Utils;
using Xunit;

namespace Medja.Test.Utils
{
    public class TextWrapperTest
    {
        [Fact]
        public void AutoWrapContainsLastEmptyLine()
        {
            var wrapper = new TextWrapper();
            wrapper.GetWidth = p => p.Length;
            wrapper.TextWrapping = TextWrapping.Auto;

            var lines = wrapper.Wrap("ABC\n", 4);

            Assert.Equal("ABC", lines[0]);
            Assert.Equal(2, lines.Length);
            Assert.Equal("", lines[1]);
        }

        [Fact]
        public void WrapEllipsesContainsLastEmptyLine()
        {
            var wrapper = new TextWrapper();
            wrapper.GetWidth = p => p.Length;
            wrapper.TextWrapping = TextWrapping.Ellipses;

            var lines = wrapper.Wrap("ABC\n", 4);

            Assert.Equal("ABC", lines[0]);
            Assert.Equal(2, lines.Length);
            Assert.Equal("", lines[1]);
        }

        [Fact]
        public void WrapWithEllipses()
        {
            var wrapper = new TextWrapper();
            wrapper.GetWidth = p => p.Length;
            wrapper.TextWrapping = TextWrapping.Ellipses;

            var lines = wrapper.Wrap("0123456789", 5);

            Assert.Single(lines);
            Assert.Equal("01...", lines[0]);
        }

        [Fact]
        public void AutoWrap()
        {
            var wrapper = new TextWrapper();
            wrapper.GetWidth = p => p.Length;
            wrapper.TextWrapping = TextWrapping.Auto;

            var lines = wrapper.Wrap("0123456789", 5);

            Assert.Equal(2, lines.Length);
            Assert.Equal("01234", lines[0]);
            Assert.Equal("56789", lines[1]);
        }

        [Fact]
        public void AutoWrapLongLine()
        {
            var wrapper = new TextWrapper();
            wrapper.GetWidth = p => p.Length;
            wrapper.TextWrapping = TextWrapping.Auto;

            var lines = wrapper.Wrap("012345678901234", 5);

            Assert.Equal(3, lines.Length);
            Assert.Equal("01234", lines[0]);
            Assert.Equal("56789", lines[1]);
            Assert.Equal("01234", lines[2]);
        }
    }
}