using Xunit;

namespace Medja.ZeroMQ.Test
{
    public class ZeroMQSocketTest
    {
        [Theory]
        [InlineData(new byte[0], 0, 0)]
        [InlineData(new byte[] { 1 }, 0, 1)]
        [InlineData(new byte[] { 1, 2 }, 2, 0)]
        [InlineData(new byte[] { 1, 2, 3 }, 2, 1)]
        [InlineData(new byte[] { 1, 3 }, 0, 2)]
        [InlineData(new byte[] { 9, 2, 3 }, 0, 3)]
        public void RemoveMessagePrefixOnlyStripsCompleteMatches(byte[] data, int offset, int count)
        {
            var result = ZeroMQSocket.RemoveMessagePrefix(data, new byte[] { 1, 2 });

            Assert.Same(data, result.Array);
            Assert.Equal(offset, result.Offset);
            Assert.Equal(count, result.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        public void RemoveMessagePrefixWithoutPrefixPreservesMessage(byte[] prefix)
        {
            var data = new byte[] { 1, 2, 3 };

            var result = ZeroMQSocket.RemoveMessagePrefix(data, prefix);

            Assert.Same(data, result.Array);
            Assert.Equal(0, result.Offset);
            Assert.Equal(data.Length, result.Count);
        }
    }
}
