using Medja.Controls;
using Medja.Theming;
using Medja.Utils;
using Xunit;

namespace Medja.Test.Controls
{
    public class TextEditorTest
    {
        private TextEditor CreateEditor()
        {
            return new ControlFactory().Create<TextEditor>();
        }

        [Fact]
        public void SetTextHandlesNewLine()
        {
            var editor = CreateEditor();

            editor.SetText("abc\ndef\r\nhij\r");
            MedjaAssert.Equal(editor.Lines, "abc", "def", "hij", "");
            
            editor.SetText("");
            MedjaAssert.Equal(editor.Lines, "");
            
            editor.SetText("bc");
            MedjaAssert.Equal(editor.Lines, "bc");
        }

        [Fact]
        public void HasOneLineAfterCreation()
        {
            var editor = CreateEditor();
            
            // ReSharper disable once xUnit2013
            Assert.Equal(1, editor.Lines.Count);
        }
    }
}