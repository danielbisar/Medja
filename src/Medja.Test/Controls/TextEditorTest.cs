using Medja.Controls;
using Medja.Theming;
using Medja.Utils;
using Xunit;
using System;

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
            Assert.Equal(1, (int)editor.Lines.Count);
        }

        [Fact]
        public void LengthOfListAfterLineBreak()
        {
            var editor = CreateEditor();

            editor.SetText("abcdefghijklm");
            Assert.Equal(1, (int)editor.Lines.Count);

            editor.GetType().GetProperty("CaretX").SetValue(editor, 7, null);
            // editor.InputState.NotifyKeyPressed();

            editor.SetText("abcdefg\nhijklm");
            Assert.Equal(2, (int)editor.Lines.Count);
        }

        [Fact]
        public void TestSetCaretPosition()
        {
            var editor = CreateEditor();

            editor.SetText("Test test test.");
            editor.SetCaretPosition(0, 3);
            Assert.Equal(0, editor.CaretY);
            Assert.Equal(3, editor.CaretX);

            Assert.Throws<ArgumentOutOfRangeException>(() => editor.SetCaretPosition(1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => editor.SetCaretPosition(-1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => editor.SetCaretPosition(99, -1));

            editor.SetText("Test\nTest\nTest");
            editor.SetCaretPosition(2, 3);
            Assert.Equal(2, editor.CaretY);
            Assert.Equal(3, editor.CaretX);

        }
    }
}