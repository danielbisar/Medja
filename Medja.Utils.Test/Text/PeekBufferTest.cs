using System;
using System.Text;
using System.IO;
using Xunit;

using Medja.Utils.Text;

namespace Medja.Utils.Test.Text
{
    public class PeekBufferTest
    {
        [Fact]
        public void CanAdd()
        {
            var buffer = new PeekBuffer();

            Assert.False(buffer.HasMore);
            buffer.Add('c');
            Assert.True(buffer.HasMore);
        }

        [Fact]
        public void CanReadChar()
        {
            var buffer = new PeekBuffer();
            buffer.Add('c');
            buffer.Add('d');
            buffer.Add('e');

            Assert.Equal('c', buffer.ReadChar());
            Assert.True(buffer.HasMore);
            Assert.Equal('d', buffer.ReadChar());
            Assert.True(buffer.HasMore);

            buffer.Add('f');

            Assert.Equal('e', buffer.ReadChar());
            Assert.True(buffer.HasMore);
            Assert.Equal('f', buffer.ReadChar());
            Assert.False(buffer.HasMore);

            Assert.Throws<EndOfStreamException>(() => buffer.ReadChar());
        }

        [Fact]
        public void CanPeekChar()
        {
            var buffer = new PeekBuffer();
            buffer.Add("abc");

            Assert.Equal('a', buffer.PeekChar());
            Assert.Equal('a', buffer.PeekChar());
            buffer.ReadChar();
            Assert.Equal('b', buffer.PeekChar());
            buffer.ReadChar();
            Assert.Equal('c', buffer.PeekChar());
            buffer.ReadChar();
            Assert.False(buffer.HasMore);
        }

        [Fact]
        public void PeekMaxTest()
        {
            var buffer = new PeekBuffer();
            buffer.Add("abcdef");

            Assert.Equal("ab", buffer.PeekMax(2));
            Assert.Equal("ab", buffer.PeekMax(2));
            Assert.Equal('a', buffer.ReadChar());
            Assert.Equal('b', buffer.ReadChar());
            Assert.Equal("cdef", buffer.PeekMax(6));
        }
    }
}