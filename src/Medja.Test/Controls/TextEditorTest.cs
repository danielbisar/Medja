using Medja.Controls;
using Medja.Theming;
using Medja.Utils;
using Medja.Input;
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
        public void ReturnKeyHandlingTest()
        {
            var editor = CreateEditor();

            editor.SetText("abcdefghijklm");
            Assert.Equal(1, (int)editor.Lines.Count);

            editor.SetCaretPosition(3, 0);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Return, ModifierKeys.None));

            MedjaAssert.Equal(editor.Lines, "abc", "defghijklm");
            Assert.Equal(0, editor.CaretX);
            Assert.Equal(1, editor.CaretY);

            editor.SetCaretPosition(0, 0);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Return, ModifierKeys.None));
            MedjaAssert.Equal(editor.Lines, "", "abc", "defghijklm");
            Assert.Equal(0, editor.CaretX);
            Assert.Equal(1, editor.CaretY);

            editor.SetCaretPosition(10, 2);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Return, ModifierKeys.None));
            MedjaAssert.Equal(editor.Lines, "", "abc", "defghijklm", "");
            Assert.Equal(0, editor.CaretX);
            Assert.Equal(3, editor.CaretY);
        }

        [Fact]
        public void CanSetCaretPosition()
        {
            var editor = CreateEditor();

            editor.SetText("Test test test.");
            editor.SetCaretPosition(3, 0);
            Assert.Equal(3, editor.CaretX);
            Assert.Equal(0, editor.CaretY);

            editor.SetText("Test\nTest\nTest");
            editor.SetCaretPosition(3, 2);
            Assert.Equal(3, editor.CaretX);
            Assert.Equal(2, editor.CaretY);
        }

        [Fact]
        public void SetCaretPositionWithInvalidArguments()
        {
            var editor = CreateEditor();
            editor.SetText("Test test test.");

            Assert.Throws<ArgumentOutOfRangeException>(() => editor.SetCaretPosition(1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => editor.SetCaretPosition(-1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => editor.SetCaretPosition(99, -1));
        }
    }
}