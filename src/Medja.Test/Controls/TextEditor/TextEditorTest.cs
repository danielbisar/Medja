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

        [Fact]
        public void CanMoveCaretUp()
        {
            var editor = CreateEditor();
            editor.SetText("abc\ndef\nghi");
            editor.SetCaretPosition(2, 1);
            editor.MoveCaretUp(false);
            
            Assert.Equal(2, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);
            
            editor.MoveCaretUp(false);

            Assert.Equal(2, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);

            editor.SetCaretPosition(2, 1);
            editor.MoveCaretUp(true);

            Assert.Equal(2, editor.CaretX);
            Assert.Equal(0, editor.CaretY);
            Assert.Equal(new Caret(2, 1), editor.SelectionStart);
            Assert.Equal(new Caret(2, 0), editor.SelectionEnd);
        }

        [Fact]
        public void MoveCaretUpClearsSelection()
        {
            var editor = CreateEditor();
            editor.SetText("abc\ndef\nghi");
            editor.SetCaretPosition(2, 2);
            editor.MoveCaretUp(true);
            
            Assert.Equal(new Caret(2, 2), editor.SelectionStart);
            Assert.Equal(new Caret(2, 1), editor.SelectionEnd);
            
            editor.MoveCaretUp(false);
            
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);
        }
        
        [Fact]
        public void CanMoveCaretDown()
        {
            var editor = CreateEditor();
            editor.SetText("abc\ndef\nghi");
            editor.SetCaretPosition(2, 1);
            editor.MoveCaretDown(false);
            
            Assert.Equal(2, editor.CaretX);
            Assert.Equal(2, editor.CaretY);
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);
            
            editor.MoveCaretDown(false);

            Assert.Equal(2, editor.CaretX);
            Assert.Equal(2, editor.CaretY);
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);

            editor.SetCaretPosition(2, 1);
            editor.MoveCaretDown(true);

            Assert.Equal(2, editor.CaretX);
            Assert.Equal(2, editor.CaretY);
            Assert.Equal(new Caret(2, 1), editor.SelectionStart);
            Assert.Equal(new Caret(2, 2), editor.SelectionEnd);
        }

        [Fact]
        public void MoveCaretDownClearsSelection()
        {
            var editor = CreateEditor();
            editor.SetText("abc\ndef\nghi");
            editor.SetCaretPosition(2, 2);
            editor.MoveCaretUp(true);
            
            Assert.Equal(new Caret(2, 2), editor.SelectionStart);
            Assert.Equal(new Caret(2, 1), editor.SelectionEnd);
            
            editor.MoveCaretDown(false);
            
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);
        }

        [Fact]
        public void SetCaretPositionClearsSelection()
        {
            var editor = CreateEditor();
            editor.SetText("abc\ndef\nghi");
            editor.SetCaretPosition(2, 1);
            editor.MoveCaretUp(true);
            
            Assert.Equal(new Caret(2, 1), editor.SelectionStart);
            Assert.Equal(new Caret(2, 0), editor.SelectionEnd);
            
            editor.SetCaretPosition(2, 1);
            
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);
        }

        [Fact]
        public void CharReplacesSelection()
        {
            var editor = CreateEditor();
            editor.SetText("012 select 456");
            editor.SetCaretPosition(4, 0);

            editor.MoveCaretForward(true); // s
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // l
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // c
            editor.MoveCaretForward(true); // t
            
            editor.InputState.NotifyKeyPressed(new KeyboardEventArgs('c', ModifierKeys.None));

            Assert.Equal("012 c 456", editor.GetText());
        }

        [Fact]
        public void CanGetMultilineText()
        {
            var text = "012\n345\n678";
            var editor = CreateEditor();
            editor.SetText(text);
            
            Assert.Equal(text, editor.GetText());
        }

        [Fact]
        public void RemoveSelectedTextOneLine()
        {
            var editor = CreateEditor();
            editor.SetText("012 select 456");
            editor.SetCaretPosition(4, 0);

            editor.MoveCaretForward(true); // s
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // l
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // c
            editor.MoveCaretForward(true); // t
            
            editor.RemoveSelectedText();
            
            Assert.Equal("012  456", editor.GetText());
        }
        
        [Fact]
        public void RemoveSelectedText()
        {
            var editor = CreateEditor();

            var expected = new[]
            {
                "012select\n456\n789",
                "012elect\n456\n789",
                "012lect\n456\n789",
                "012ect\n456\n789",
                "012ct\n456\n789",
                "012t\n456\n789",
                "012\n456\n789",
                "012456\n789",
                "01256\n789",
                "0126\n789",
                "012\n789",
                "012789",
                "01289",
                "0129",
                "012",
            };

            for (int i = 0; i < expected.Length; i++)
            {
                editor.SetText("012\nselect\n456\n789");
                editor.SetCaretPosition(3, 0);

                for(int n = 0; n <= i; n++)
                    editor.MoveCaretForward(true);
                
                editor.RemoveSelectedText();
                Assert.Equal(expected[i], editor.GetText());
            }
        }
        
        [Fact]
        public void RemoveSelectedTextTwoLines()
        {
            var editor = CreateEditor();
            editor.SetText("012\nselect\n456");
            editor.SetCaretPosition(3, 0);

            editor.MoveCaretForward(true); // \n
            editor.MoveCaretForward(true); // s
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // l
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // c
            editor.MoveCaretForward(true); // t
            
            editor.RemoveSelectedText();
            
            Assert.Equal("012\n456", editor.GetText());
        }
        
        [Fact]
        public void RemoveSelectedTextMultipleLines()
        {
            var editor = CreateEditor();
            editor.SetText("012\nselect\n456\n789");
            editor.SetCaretPosition(3, 0);

            editor.MoveCaretForward(true); // \n
            editor.MoveCaretForward(true); // s
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // l
            editor.MoveCaretForward(true); // e
            editor.MoveCaretForward(true); // c
            editor.MoveCaretForward(true); // t
            editor.MoveCaretForward(true); // \n
            editor.MoveCaretForward(true); // 4
            editor.MoveCaretForward(true); // 5
            editor.MoveCaretForward(true); // 6
            editor.MoveCaretForward(true); // \n
            
            editor.RemoveSelectedText();
            
            Assert.Equal("012789", editor.GetText());
        }

        [Fact]
        public void GetSelectedTextNoSelection()
        {
            var editor = CreateEditor();
            editor.SetText("012\nselect\n456\n789");
            
            Assert.Equal("", editor.GetSelectedText());
        }
        
        [Fact]
        public void GetSelectedText()
        {
            var editor = CreateEditor();
            editor.SetText("012\nselect\n456\n789");
            editor.SetCaretPosition(3, 0);

            editor.MoveCaretForward(true); // \n
            Assert.Equal("\n", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // s
            Assert.Equal("\ns", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // e
            Assert.Equal("\nse", editor.GetSelectedText());

            editor.MoveCaretForward(true); // l
            Assert.Equal("\nsel", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // e
            Assert.Equal("\nsele", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // c
            Assert.Equal("\nselec", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // t
            Assert.Equal("\nselect", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // \n
            Assert.Equal("\nselect\n", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // 4
            Assert.Equal("\nselect\n4", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // 5
            Assert.Equal("\nselect\n45", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // 6
            Assert.Equal("\nselect\n456", editor.GetSelectedText());
            
            editor.MoveCaretForward(true); // \n
            Assert.Equal("\nselect\n456\n", editor.GetSelectedText());
        }

        [Fact]
        public void MoveCaretForward()
        {
            var editor = CreateEditor();
            editor.SetText("012\nselect\n456\n789");
            editor.SetCaretPosition(3, 0);
            
            editor.MoveCaretForward(true);
            Assert.Equal(new Caret(3, 0), editor.SelectionStart);
            Assert.Equal(new Caret(0, 1), editor.SelectionEnd);
            
            editor.MoveCaretForward(true);
            Assert.Equal(new Caret(3, 0), editor.SelectionStart);
            Assert.Equal(new Caret(1, 1), editor.SelectionEnd);

        }

        [Fact]
        public void SetTextClearsSelection()
        {
            var editor = CreateEditor();
            editor.SetText("012\nselect\n456\n789");
            editor.SetCaretPosition(3, 0);
            
            editor.MoveCaretForward(true);
            editor.MoveCaretForward(true);
            
            Assert.Equal(new Caret(3, 0), editor.SelectionStart);
            Assert.Equal(new Caret(1, 1), editor.SelectionEnd);
            
            editor.SetText("012\nselect\n456\n78");
            
            Assert.Null(editor.SelectionStart);
            Assert.Null(editor.SelectionEnd);
        }

        [Fact]
        public void InsertTextClearsSelection()
        {
            Assert.False(true);
        }

        [Fact]
        public void JoinLineAndNext()
        {
            var editor = CreateEditor();
            editor.SetText("0123\n4567\n8901");
            
            editor.JoinLineAndNext(1);
            
            Assert.Equal("0123\n45678901", editor.GetText());

            Assert.Throws<ArgumentOutOfRangeException>(() => editor.JoinLineAndNext(1));
            
            editor.JoinLineAndNext(0);
            
            Assert.Equal("012345678901", editor.GetText());
        }
    }
}