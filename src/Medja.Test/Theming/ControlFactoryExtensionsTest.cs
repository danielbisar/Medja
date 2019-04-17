using Medja.Theming;
using Xunit;

namespace Medja.Test.Theming
{
    public class ControlFactoryExtensionsTest
    {
        [Fact]
        public void CreateLabelAddsColon()
        {
            var text = "my";
            var result = new ControlFactory().CreateLabel(text);

            Assert.Equal("my: ", result.Text);
        }

        [Fact]
        public void CreateTextBlock()
        {
            var text = "my";
            var result = new ControlFactory().CreateTextBlock(text);

            Assert.Equal(text, result.Text);
        }
    }
}