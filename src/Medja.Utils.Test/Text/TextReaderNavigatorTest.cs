using Medja.Utils.Text;
using Xunit;
using System.IO;

namespace Medja.Utils.Test.Text
{
    public class TextReaderNavigatorTest
    {
        [Fact]
        public void CanReadChar()
        {
            var reader = new TextReaderNavigator("abc");

            Assert.Equal('a', reader.ReadChar());
            Assert.Equal('b', reader.ReadChar());
            Assert.True(reader.HasMore);
            Assert.Equal('c', reader.ReadChar());

            Assert.Throws<EndOfStreamException>(() => reader.ReadChar());
            Assert.False(reader.HasMore);
        }

        [Fact]
        public void CanPeekChar()
        {
            var reader = new TextReaderNavigator("abc");

            Assert.Equal('a', reader.PeekChar());
            Assert.Equal('a', reader.PeekChar());

            reader.ReadChar();
            reader.ReadChar();

            Assert.Equal('c', reader.PeekChar());
            Assert.True(reader.HasMore);

            reader.ReadChar();

            Assert.Throws<EndOfStreamException>(() => reader.PeekChar());
        }

        [Fact]
        public void CanSkipExpected()
        {
            var reader = new TextReaderNavigator("a b c");
            Assert.True(reader.SkipExpected("a "));
            Assert.True(reader.SkipExpected("b"));

            Assert.Equal(' ', reader.ReadChar());
            Assert.False(reader.SkipExpected("d"));
            Assert.Equal('c', reader.ReadChar());
            Assert.False(reader.HasMore);
            Assert.False(reader.SkipExpected("d"));
        }

        [Fact]
        public void CanSkipLongerExpectedThanString()
        {
            var reader = new TextReaderNavigator("abc");
            Assert.False(reader.SkipExpected("abcd"));
        }

        [Fact]
        public void SkipExpectedKeepsReaderPosOnFailure()
        {
            var reader = new TextReaderNavigator("abcdef");
            Assert.False(reader.SkipExpected("aba"));
            Assert.Equal('a', reader.ReadChar());
            Assert.Equal('b', reader.ReadChar());
        }

        [Fact]
        public void CanPeekMax()
        {
            var reader = new TextReaderNavigator("abcdefg");

            Assert.Equal("abcd", reader.PeekMax(4));
            Assert.Equal('a', reader.ReadChar());
            Assert.Equal("bcdefg", reader.PeekMax(10));
            Assert.Equal('b', reader.ReadChar());
            Assert.True(reader.HasMore);
        }

        [Fact]
        public void IsAtNewLineTest()
        {
            var reader = new TextReaderNavigator("abc\ndef");
            reader.SkipExpected("abc");

            Assert.True(reader.IsAtNewLine());
            reader.ReadChar();
            Assert.False(reader.IsAtNewLine());
        }

	[Fact]
	public void SkipLineTest()
	{
	    var reader = new TextReaderNavigator("abc\ndef\r\nghi\r");

	    reader.SkipLine();
	    Assert.Equal('d', reader.PeekChar());

	    reader.SkipLine();
            Assert.Equal('g', reader.PeekChar());

            reader.SkipLine();
            Assert.False(reader.HasMore);
	}
    }
}
