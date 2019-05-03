using System;
using System.Collections.Generic;
using System.Text;
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

        public TextEditor()
        {
            Lines = new List<string>();
            Lines.Add("");

            PropertyCaretX = new Property<int>();
            PropertyCaretY = new Property<int>();

            InputState.KeyPressed += OnKeyPressed;
            InputState.OwnsMouseEvents = true;
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            var line = Lines[CaretY];

            // todo check modifier keys
            switch (e.Key)
            {
                case (char)Keys.Left:
                    if (CaretX > 0)
                        CaretX--;
                    break;
                case (char)Keys.Right:
                    if (CaretX < Lines[CaretY].Length)
                        CaretX++;
                    break;
                case (char)Keys.Up:
                    if (CaretY > 0)
                    {
                        CaretY--;
                        UpdateCaretXAfterCaretYChange();
                    }
                    break;
                case (char)Keys.Down:
                    if (CaretY < Lines.Count - 1)
                    {
                        CaretY++;
                        UpdateCaretXAfterCaretYChange();
                    }
                    break;
                case '\b':
                    if (CaretX > 0)
                    {
                        Lines[CaretY] = line.Substring(0, CaretX - 1) + line.Substring(CaretX);
                        CaretX--;
                    }
                    else
                    {
                        // todo: move part of line up handling if line.Length > 0, else just remove line
                    }
                    break;
                case (char)Keys.Delete:
                    if (CaretX < line.Length)
                    {
                        Lines[CaretY] = line.Substring(0, CaretX) + line.Substring(CaretX + 1);
                    }
                    else
                    {
                        // todo: move part of line up handling if line.Length > 0, else just remove line
                    }
                    break;
                // todo return/enter key handling case (char)
                // todo ctrl+v
                // todo shift + left
                // todo shift + right
                // todo ctrl+c, ctrl+x
                default:
                    Lines[CaretY] = line.Substring(0, CaretX) + e.Key + line.Substring(CaretX);
                    CaretX++;
                    break;
            }
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

        public void SetCaretPosition(int posY, int posX)
        {
            if (posY < 0 || posY > Lines.Count)
                throw new System.ArgumentOutOfRangeException("argument for Y-Position out of range");
            CaretY = posY;
            if (posX < 0 || posX > Lines[posY].Length)
                throw new System.ArgumentOutOfRangeException("argument for X-Position out of range");
            CaretX = posX;
        }


    }
}