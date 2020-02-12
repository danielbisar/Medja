using Medja.Controls;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class TextControlTest
    {
        private TextControl CreateTextControl()
        {
            return new ControlFactory().Create<TextControl>();
        }
        
        [Fact]
        public void GetLongestLineWidthTest()
        {
            var control = CreateTextControl();

            Assert.Equal(0f, control.GetLongestLineWidth());
            
            control.Text = "ABC\ndef\nKKKK\n";

            Assert.Equal(-1.0f, control.GetLongestLineWidth());

            control.Font.GetWidth = p => p.Length; // no actual calculation of rendered length

            Assert.Equal(4f, control.GetLongestLineWidth());

            control.Text = "ABC\ndef\nKKKK";
            Assert.Equal(4f, control.GetLongestLineWidth());
            
            control.Text = "ABC";
            Assert.Equal(3f, control.GetLongestLineWidth());
        }
    }
}