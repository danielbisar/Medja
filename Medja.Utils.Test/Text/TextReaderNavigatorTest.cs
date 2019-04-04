using Medja.Utils.Text;
using Xunit;
using System.IO;

namespace Medja.Utils.Test.Text
{
    public class TextReaderNavigatorText
    {
        [Fact]
        public void CanReadChar()
        {
            var reader = new TextReaderNavigator("abc");

            Assert.Equal('a', reader.ReadChar());
            Assert.Equal('b', reader.ReadChar());
            Assert.Equal('c', reader.ReadChar());

            Assert.Throws<EndOfStreamException>(() => reader.ReadChar());
        }
    }
}