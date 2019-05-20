using Xunit;
using Medja.Utils.Text;

namespace Medja.Utils.Test
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