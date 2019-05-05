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

        [Fact]
        public void CanInsertTab()
        {
            var editor = CreateEditor();
            editor.SetText("abcdef");
            editor.SetCaretPosition(3, 0);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Tab, ModifierKeys.None));

            MedjaAssert.Equal(editor.Lines, "abc    def");
        }

        [Fact]
        public void CanMoveCaretForward()
        {
            var editor = CreateEditor();
            editor.SetText("abc\ndef");
            editor.MoveCaretForward(false);
            editor.MoveCaretForward(false);
            editor.MoveCaretForward(false);
            editor.MoveCaretForward(false);
            
            Assert.Equal(0, editor.CaretX);
            Assert.Equal(1, editor.CaretY);
            
            editor.MoveCaretForward(false);
            editor.MoveCaretForward(false);
            editor.MoveCaretForward(false);
            editor.MoveCaretForward(false); // should not throw an exception but keep the cursor at the end of the text
            
            Assert.Equal(3, editor.CaretX);
            Assert.Equal(1, editor.CaretY);
        }
        
        [Fact]
        public void CanMoveCaretBackward()
        {
            var editor = CreateEditor();
            editor.SetText("abc\ndef");
            editor.SetCaretPosition(3, 1);
            editor.MoveCaretBackward(false);
            editor.MoveCaretBackward(false);
            editor.MoveCaretBackward(false);
            
            Assert.Equal(0, editor.CaretX);
            Assert.Equal(1, editor.CaretY);
            
            editor.MoveCaretBackward(false);

            Assert.Equal(3, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            
            editor.MoveCaretBackward(false);
            editor.MoveCaretBackward(false);
            editor.MoveCaretBackward(false);
            editor.MoveCaretBackward(false);
            
            Assert.Equal(0, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
        }

        [Fact]
        public void DeleteCanCombineTwoLines()
        {
            var editor = CreateEditor();
            editor.SetText("abcde\nfghij");
            editor.SetCaretPosition(5,0);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Delete, ModifierKeys.None));

            Assert.Equal(5, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Equal(1, (int)editor.Lines.Count);
            MedjaAssert.Equal(editor.Lines, "abcdefghij");
        }

        [Fact]
        public void DeleteCanHandleSequencesOverLinebreaks()
        {
            var editor = CreateEditor();
            editor.SetText("abcde\nfghij");
            editor.SetCaretPosition(3,0);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Delete, ModifierKeys.None));
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Delete, ModifierKeys.None));
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Delete, ModifierKeys.None));

            Assert.Equal(3, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Equal(1, (int)editor.Lines.Count);
            MedjaAssert.Equal(editor.Lines, "abcfghij");
        }
        
        [Fact]
        public void DeleteCanHandleLimits()
        {
            var editor = CreateEditor();
            editor.SetText("abcde");
            editor.SetCaretPosition(5,0);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Delete, ModifierKeys.None));
            
            Assert.Equal(5, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Equal(1, (int)editor.Lines.Count);
            MedjaAssert.Equal(editor.Lines, "abcde");
        }

        [Fact]
        public void BackspaceCanCombineTwoLines()
        {
            var editor = CreateEditor();
            editor.SetText("abcde\nfghij");
            editor.SetCaretPosition(0,1);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Backspace, ModifierKeys.None));
            
            Assert.Equal(5, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Equal(1, (int)editor.Lines.Count);
            MedjaAssert.Equal(editor.Lines, "abcdefghij");
            
            
            editor.SetText("abcde\nfghij\nklmno");
            editor.SetCaretPosition(1,2);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Backspace, ModifierKeys.None));
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Backspace, ModifierKeys.None));
            
            Assert.Equal(5, editor.CaretX);
            Assert.Equal(1, editor.CaretY);
            Assert.Equal(2, (int)editor.Lines.Count);
            MedjaAssert.Equal(editor.Lines, "abcde", "fghijlmno");
        }
        
        [Fact]
        public void BackspaceCanHandleLimits()
        {
            var editor = CreateEditor();
            editor.SetText("abcde\nfghij");
            editor.SetCaretPosition(0,0);
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs(Keys.Backspace, ModifierKeys.None));
            
            Assert.Equal(0, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Equal(2, (int)editor.Lines.Count);
            MedjaAssert.Equal(editor.Lines, "abcde", "fghij");
        }
    }
}