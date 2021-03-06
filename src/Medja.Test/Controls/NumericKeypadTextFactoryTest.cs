using Xunit;
using Medja.Utils;
using Medja.Controls;

namespace Medja.Test.Controls
{
    public class NumericKeypadTextFactoryTest
    {
        private string Translate(char c)
        {
            return new NumericKeypadTextFactory().Translate(c);
        }

        [Fact]
        public void TranslatesClear()
        {
            var result = Translate('c');
            Assert.Equal(Globalization.Clear, result);
        }

        [Fact]
        public void TranslatesBack()
        {
            var result = Translate('b');
            Assert.Equal(Globalization.Back, result);
        }

        [Fact]
        public void Translates0to9()
        {
            for (int i = 0; i <= 9; i++)
            {
                // 48 = ascii digit 0, 49 = 1, ... 
                var result = Translate((char)(i + 48));
                Assert.Equal(i.ToString(), result);
            }
        }

        [Fact]
        public void TranslatesFile()
        {
            var layout =
@"7 8 9 c
4 5 6 b
1 2 3 -
- 0 -";

            var result = new NumericKeypadTextFactory().Translate(layout);

            MedjaAssert.Equal(result, new string[][]
            {
                new []{ "7", "8", "9", Globalization.Clear},
                new []{ "4", "5", "6", Globalization.Back},
                new []{ "1", "2", "3", ""},
                new []{ "", "0", ""}
            });
        }
    }
}

