using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medja.Input;
using Medja.Primitives;

namespace Medja.Controls
{
    // todo refactor theoretical base class: textcontrol
    public class TextEditor : Control
    {
        // maybe we will use another class instead of string later on
        // on every typing a line must be updated, since strings in c#
        // are read only this copies the whole line and adds the char
        // at the given position...
        public List<string> Lines { get; }

        public readonly Property<int> PropertyCaretX;
        public int CaretX
        {
            get { return PropertyCaretX.Get(); }
            private set { PropertyCaretX.Set(value); }
        }

        public readonly Property<int> PropertyCaretY;
        public int CaretY
        {
            get { return PropertyCaretY.Get(); }
            private set { PropertyCaretY.Set(value); }
        }

        public readonly Property<Caret> PropertySelectionStart;
        public Caret SelectionStart
        {
            get { return PropertySelectionStart.Get(); }
            private set { PropertySelectionStart.Set(value); }
        }

        public readonly Property<Caret> PropertySelectionEnd;
        public Caret SelectionEnd
        {
            get { return PropertySelectionEnd.Get(); }
            private set { PropertySelectionEnd.Set(value); }
        }
        
        public TextEditor()
        {
            Lines = new List<string>();
            Lines.Add("");

            PropertyCaretX = new Property<int>();
            PropertyCaretY = new Property<int>();
            PropertySelectionStart = new Property<Caret>();
            PropertySelectionEnd = new Property<Caret>();
            
            InputState.KeyPressed += OnKeyPressed;
            InputState.OwnsMouseEvents = true;
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.KeyChar == 0)
                return;

            var line = Lines[CaretY];

            // todo check modifier keys
            switch (e.Key)
            {
                case Keys.Left:
                    MoveCaretBackward(e.ModifierKeys.HasShift());
                    break;
                case Keys.Right:
                    MoveCaretForward(e.ModifierKeys.HasShift());
                    break;
                case Keys.Up:
                    if (CaretY > 0)
                    {
                        CaretY--;
                        UpdateCaretXAfterCaretYChange();
                    }
                    break;
                case Keys.Down:
                    if (CaretY < Lines.Count - 1)
                    {
                        CaretY++;
                        UpdateCaretXAfterCaretYChange();
                    }
                    break;
                case Keys.Backspace:
                    if (CaretX > 0)
                    {
                        Lines[CaretY] = line.Substring(0, CaretX - 1) + line.Substring(CaretX);
                        CaretX--;
                    }
                    else
                    {
                        if (CaretY != 0)
                        {
                            Lines[CaretY - 1] = Lines[CaretY - 1] + Lines[CaretY].Substring(0, line.Length);
                            CaretX = Lines[CaretY - 1].Length - Lines[CaretY].Length;
                            CaretY--;
                            Lines.RemoveAt(CaretY + 1);                            
                        }
 
                    }
                    break;
                case Keys.Delete:
                    if (CaretX < line.Length)
                    {
                        Lines[CaretY] = line.Substring(0, CaretX) + line.Substring(CaretX + 1);
                    }
                    else
                    {
                        // TODO: handle delete for macOS
                        if(CaretY+1 != Lines.Count)
                        {
                            Lines[CaretY] = line + Lines[CaretY + 1].Substring(0, line.Length);
                            Lines.RemoveAt(CaretY + 1); 
                        }  
                    }
                    break;
                case Keys.Return:
                    Lines[CaretY] = line.Substring(0, CaretX);
                    Lines.Insert(CaretY + 1, line.Substring(CaretX, line.Length - CaretX));
                    SetCaretPosition(0, CaretY + 1);
                    break;
                case Keys.Tab:
                    InsertText("    ");
                    break;
                // todo ctrl+v
                // todo shift + left
                // todo shift + right
                // todo ctrl+c, ctrl+x
                default:
                    InsertChar(e.KeyChar);
                    break;
            }
        }

        private void InsertChar(char c)
        {
            InsertText(new string(c, 1));
        }


        /// <summary>
        /// Inserts text at the current caret position.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        public void InsertText(string text)
        {
            var line = Lines[CaretY];
            Lines[CaretY] = line.Substring(0, CaretX) + text + line.Substring(CaretX);
            CaretX++;
        }
        
        private void UpdateCaretXAfterCaretYChange()
        {
            if (Lines[CaretY].Length <= CaretX)
                CaretX = Lines[CaretY].Length;

            // todo remember MaxCaretY and if possible use that value
        }

        ///<summary>Replaces the complete text (content) of the Editor.</summary>
        ///<remarks>Function because the text will be parsed into lines, so we don't want to support binding as it 
        /// would look like this operation is very fast and does not use a lot resources.</remarks>
        public void SetText(string text)
        {
            Lines.Clear();

            var line = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n' || c == '\r')
                {
                    Lines.Add(line.ToString());
                    line.Clear();

                    if (c == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                        i++;
                }
                else
                    line.Append(c);
            }

            Lines.Add(line.ToString());

            // todo reduce capacity of lines if they used a large amount before and now just a few...
        }

        public void SetCaretPosition(int posX, int posY)
        {
            if (posY < 0 || posY > Lines.Count)
                throw new System.ArgumentOutOfRangeException(nameof(posY));

            if (posX < 0 || posX > Lines[posY].Length)
                throw new System.ArgumentOutOfRangeException(nameof(posX));

            CaretX = posX;
            CaretY = posY;
        }

        public void MoveCaretBackward(bool select)
        {
            if (select && SelectionStart == null)
                MarkSelectionStart();
            
            if (CaretX > 0)
                CaretX--;
            else if(CaretY > 0)
            {
                CaretY--;
                CaretX = Lines[CaretY].Length;
            }
            
            if(select)
                MarkSelectionEnd();
            else
                ClearSelection();
        }

        public void MoveCaretForward(bool select)
        {
            if (select && SelectionStart == null)
                MarkSelectionStart();
            
            if (CaretX < Lines[CaretY].Length)
                CaretX++;
            else if (CaretY < Lines.Count - 1)
            {
                CaretY++;
                CaretX = 0;
            }

            if(select)
                MarkSelectionEnd();
            else
                ClearSelection();
        }

        private void MarkSelectionStart()
        {
            SelectionStart = new Caret(CaretX, CaretY);
            SelectionEnd = SelectionStart;
        }
        
        private void MarkSelectionEnd()
        {
            SelectionEnd = new Caret(CaretX, CaretY);
        }

        private void ClearSelection()
        {
            SelectionStart = null;
            SelectionEnd = null;
        }

        public Caret GetLogicalSelectionStart()
        {
            if (SelectionStart == null)
                return null;
            
            return SelectionStart < SelectionEnd ? SelectionStart : SelectionEnd;
        }

        public Caret GetLogicalSelectionEnd()
        {
            if (SelectionEnd == null)
                return null;
            
            return SelectionStart < SelectionEnd ? SelectionEnd : SelectionStart;
        }
    }
}