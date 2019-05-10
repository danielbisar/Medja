using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Medja.Input;

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

        public bool HasSelection
        {
            get { return SelectionStart != null && SelectionEnd != null; }
        }

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

            if (HasSelection && e.Key == null || KeyInsertsValue(e.Key))
                RemoveSelectedText();

            switch (e.Key)
            {
                case Keys.Left:
                    MoveCaretBackward(e.ModifierKeys.HasShift());
                    break;
                case Keys.Right:
                    MoveCaretForward(e.ModifierKeys.HasShift());
                    break;
                case Keys.Up:
                    MoveCaretUp(e.ModifierKeys.HasShift());
                    break;
                case Keys.Down:
                    MoveCaretDown(e.ModifierKeys.HasShift());
                    break;
                case Keys.Backspace:
                    HandleBackspace();
                    break;
                case Keys.Delete:
                    HandleDelete();
                    break;
                case Keys.Return:
                    HandleReturn();
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
        
        /// <summary>
        /// Sets the position of the caret.
        /// </summary>
        /// <param name="posX">The column in the given line.</param>
        /// <param name="posY">The row/line index.</param>
        /// <remarks>Clears the current selection.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">If the given <see cref="posX"/> or <see cref="posY"/> is
        /// invalid.</exception>
        public void SetCaretPosition(int posX, int posY)
        {
            if (posY < 0 || posY > Lines.Count)
                throw new ArgumentOutOfRangeException(nameof(posY));

            if (posX < 0 || posX > Lines[posY].Length)
                throw new ArgumentOutOfRangeException(nameof(posX));

            CaretX = posX;
            CaretY = posY;
            ClearSelection();
        }

        /// <summary>
        /// Moves the caret backward one position. Goes to previous line if the caret was at the beginning of a line.
        /// </summary>
        /// <remarks>Clears previous selection if select is false.</remarks>
        /// <param name="select">If true selects the text the caret moved over.</param>
        public void MoveCaretBackward(bool select)
        {
            AssureSelectionStart(select);

            if (CaretX > 0)
                CaretX--;
            else if (CaretY > 0)
            {
                CaretY--;
                CaretX = Lines[CaretY].Length;
            }

            if (select)
                MarkSelectionEnd();
            else
                ClearSelection();
        }

        /// <summary>
        /// Moves the caret forward one position. Goes to the next line if the caret was at the end of a line.
        /// </summary>
        /// <remarks>Clears previous selection if select is false.</remarks>
        /// <param name="select">If true selects the text the caret moved over.</param>
        public void MoveCaretForward(bool select)
        {
            AssureSelectionStart(select);

            if (CaretX < Lines[CaretY].Length)
                CaretX++;
            else if (CaretY < Lines.Count - 1)
            {
                CaretY++;
                CaretX = 0;
            }

            if (select)
                MarkSelectionEnd();
            else
                ClearSelection();
        }
        
        /// <summary>
        /// Moves the caret up one line.
        /// </summary>
        /// <remarks>Clears previous selection if select is false.</remarks>
        /// <param name="select">If true selects the text the caret moved over (usual editor selection behavior).</param>
        public void MoveCaretUp(bool select)
        {
            AssureSelectionStart(select);

            if (CaretY > 0)
            {
                CaretY--;
                UpdateCaretXAfterCaretYChange();
            }

            MarkSelectionEndOrClear(select);
        }

        /// <summary>
        /// Moves the caret down one line.
        /// </summary>
        /// <remarks>Clears previous selection if select is false.</remarks>
        /// <param name="select">If true selects the text the caret moved over (usual editor selection behavior).</param>
        public void MoveCaretDown(bool select)
        {
            AssureSelectionStart(select);

            if (CaretY < Lines.Count - 1)
            {
                CaretY++;
                UpdateCaretXAfterCaretYChange();
            }

            MarkSelectionEndOrClear(select);
        }

        /// <summary>
        /// Clears the current selection.
        /// </summary>
        private void ClearSelection()
        {
            SelectionStart = null;
            SelectionEnd = null;
        }

        /// <summary>
        /// Gets the logical selection start. This is the lower position interpreted as selection start and the higher
        /// as selection end.
        /// </summary>
        /// <returns>The logical selection start.</returns>
        public Caret GetLogicalSelectionStart()
        {
            if (SelectionStart == null)
                return null;

            return SelectionStart < SelectionEnd ? SelectionStart : SelectionEnd;
        }

        /// <summary>
        /// Gets the logical selection end. This is the lower position interpreted as selection start and the higher
        /// as selection end.
        /// </summary>
        /// <returns>The logical selection end.</returns>
        public Caret GetLogicalSelectionEnd()
        {
            if (SelectionEnd == null)
                return null;

            return SelectionStart < SelectionEnd ? SelectionEnd : SelectionStart;
        }

        private void HandleReturn()
        {
            var line = Lines[CaretY];

            Lines[CaretY] = line.Substring(0, CaretX);

            CaretY++;
            Lines.Insert(CaretY, line.Substring(CaretX, line.Length - CaretX));
            CaretX = 0;
        }

        private void HandleDelete()
        {
            var line = Lines[CaretY];

            if (CaretX < line.Length)
            {
                Lines[CaretY] = line.Substring(0, CaretX) + line.Substring(CaretX + 1);
            }
            else
                JoinLineAndNext(CaretY);
        }

        public void JoinLineAndNext(int index)
        {
            // we need one extra line
            if(index + 1 >= Lines.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Lines[index] += Lines[index + 1];
            Lines.RemoveAt(index + 1);
        }

        private void HandleBackspace()
        {
            var line = Lines[CaretY];

            if (CaretX > 0)
            {
                Lines[CaretY] = line.Substring(0, CaretX - 1) + line.Substring(CaretX);
                CaretX--;
            }
            else
            {
                if (CaretY != 0)
                {
                    Lines[CaretY - 1] = Lines[CaretY - 1] + Lines[CaretY];
                    CaretX = Lines[CaretY - 1].Length - Lines[CaretY].Length;
                    CaretY--;
                    Lines.RemoveAt(CaretY + 1);
                }
            }
        }

        private bool KeyInsertsValue(Keys? key)
        {
            return key == Keys.Backspace || key == Keys.Delete || key == Keys.Return || key == Keys.Tab;
        }

        public void RemoveSelectedText()
        {
            if (!HasSelection)
                return;

            int lineIndex;

            for (int i = lineIndex = SelectionStart.Y; i <= SelectionEnd.Y; i++, lineIndex++)
            {
                if (i == SelectionStart.Y)
                {
                    var line = Lines[lineIndex];

                    if (i == SelectionEnd.Y)
                        Lines[lineIndex] = line.Substring(0, SelectionStart.X) + line.Substring(SelectionEnd.X);
                    else if (SelectionStart.X == line.Length) // selection started at the end of the line
                    {
                        if (SelectionEnd.Y == i + 1)
                        {
                            Lines[lineIndex] += Lines[lineIndex + 1].Substring(SelectionEnd.Y);
                            // todo fix
                        }
                        else
                        {
                            JoinLineAndNext(lineIndex);
                            lineIndex--;
                        }
                    }
                    else
                        Lines[lineIndex] = line.Substring(0, SelectionStart.X);
                }
                else if (i == SelectionEnd.Y)
                {
                    if (SelectionEnd.X > 0)
                    {
                        var line = Lines[lineIndex];
                        Lines[lineIndex] = line.Substring(SelectionEnd.X);
                    }
                    else
                    {
                        lineIndex--;
                        JoinLineAndNext(lineIndex);
                    }
                }
                else
                {
                    Lines.RemoveAt(lineIndex);
                    lineIndex--;
                }
            }
        }

        public string GetSelectedText()
        {
            if (!HasSelection)
                return string.Empty;

            var result = new StringBuilder();

            for (int i = SelectionStart.Y; i <= SelectionEnd.Y; i++)
            {
                if (i == SelectionStart.Y)
                {
                    var line = Lines[i];

                    if (i == SelectionEnd.Y)
                        result.Append(line, 0, SelectionEnd.X - SelectionStart.X);
                    else
                    {
                        result.Append(line, SelectionStart.X, line.Length - SelectionStart.X);
                        result.AppendLine();
                    }
                }
                else if (i == SelectionEnd.Y)
                    result.Append(Lines[i], 0, SelectionEnd.X);
                else
                    result.AppendLine(Lines[i]);
            }

            return result.ToString();
        }

        private void InsertChar(char c)
        {
            InsertText(new string(c, 1));
        }


        /// <summary>
        /// Inserts text at the current caret position. If some text is selected, this text will be replaced.
        /// </summary>
        /// <remarks>Clears selection.</remarks>
        /// <param name="text">The text to insert.</param>
        public void InsertText(string text)
        {
            // todo replace selected text
            var line = Lines[CaretY];
            Lines[CaretY] = line.Substring(0, CaretX) + text + line.Substring(CaretX);
            CaretX++;
            ClearSelection();
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
            ClearSelection();

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

        public string GetText()
        {
            var charCount = GetCharCount();
            var result = new StringBuilder(charCount);

            foreach (var line in Lines)
                result.AppendLine(line);

            Debug.Assert(result.Capacity == charCount);

            return Lines.Count > 0 ? result.ToString(0, result.Length - 1) : string.Empty;
        }

        private int GetCharCount()
        {
            var result = 0;

            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < Lines.Count; i++)
                result += Lines[i].Length;

            return result + Lines.Count * Environment.NewLine.Length;
        }

        private void MarkSelectionEndOrClear(bool select)
        {
            if (select)
                MarkSelectionEnd();
            else
                ClearSelection();
        }

        private void AssureSelectionStart(bool select)
        {
            if (select && SelectionStart == null)
                MarkSelectionStart();
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
    }
}