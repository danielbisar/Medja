using Medja.Utils.Text;
using Xunit;

namespace Medja.Utils.Test.Text
{
    public class StringExtensionsTest
    {
        [Fact]
        public void IndexOfOutsideQuotesTest()
        {
            Assert.Equal(-1, "bla bla bla".IndexOfOutsideQuotes('='));
            Assert.Equal(-1, "bla '=' bla".IndexOfOutsideQuotes('='));
            Assert.Equal(-1, "bla \"=\" bla".IndexOfOutsideQuotes('='));
            Assert.Equal(-1, "bla \"==== bla".IndexOfOutsideQuotes('='));

            Assert.Equal(0, "=bla \"==== bla".IndexOfOutsideQuotes('='));
            Assert.Equal(9, "bla \"==\" = bla".IndexOfOutsideQuotes('='));
            Assert.Equal(9, "bla '==' = bla".IndexOfOutsideQuotes('='));
        }
    }
}
