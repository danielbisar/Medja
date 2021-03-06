using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Medja.Input;
using Medja.Properties;
using Medja.Utils;

namespace Medja.Controls.TextEditor
{
    // todo refactor theoretical base class: textcontrol
    public class TextEditor : Control
    {
        private readonly Timer _timer;
        private readonly TaskQueueFinder _taskQueueFinder;

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

        public readonly Property<bool> PropertyIsCaretVisible;
        public bool IsCaretVisible
        {
            get { return PropertyIsCaretVisible.Get(); }
            set { PropertyIsCaretVisible.Set(value); }
        }

        public event EventHandler TextChanged;

        public TextEditor()
        {
            Lines = new List<string>();
            Lines.Add("");

            PropertyCaretX = new Property<int>();
            PropertyCaretY = new Property<int>();
            PropertySelectionStart = new Property<Caret>();
            PropertySelectionEnd = new Property<Caret>();
            PropertyIsCaretVisible = new Property<bool>();
            PropertyIsFocused.PropertyChanged += OnFocusChanged;

            InputState.KeyPressed += OnKeyPressed;
            InputState.OwnsMouseEvents = true;

            _taskQueueFinder = new TaskQueueFinder(this);
            _timer = new Timer(OnTimerTick, null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000));
        }

        private void OnFocusChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsFocused == false)
                IsCaretVisible = false;
        }

        private void OnTimerTick(object state)
        {
            _taskQueueFinder.TaskQueue?.TryEnqueue(p =>
            {
                if(IsFocused)
                    IsCaretVisible = !IsCaretVisible;

                return null;
            }, null);
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key != null)
            {
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
                }
            }
            else
                InsertChar(e.KeyChar);
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

            RaiseTextChanged();
        }

        private void RaiseTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void HandleBackspace()
        {
            // CaretX change sets NeedsRendering
            if (HasSelection)
            {
                RemoveSelectedText();
                return;
            }

            var line = Lines[CaretY];

            if (CaretX > 0)
            {
                Lines[CaretY] = line.Substring(0, CaretX - 1) + line.Substring(CaretX);
                CaretX--;

                RaiseTextChanged();
            }
            else
            {
                if (CaretY != 0)
                {
                    Lines[CaretY - 1] = Lines[CaretY - 1] + Lines[CaretY];
                    CaretX = Lines[CaretY - 1].Length - Lines[CaretY].Length;
                    CaretY--;
                    Lines.RemoveAt(CaretY + 1);

                    RaiseTextChanged();
                }
            }
        }

        private void HandleDelete()
        {
            if (HasSelection)
            {
                RemoveSelectedText();
                return;
            }

            var line = Lines[CaretY];

            if (CaretX < line.Length)
            {
                Lines[CaretY] = line.Substring(0, CaretX) + line.Substring(CaretX + 1);
                RaiseTextChanged();
            }
            else if (CaretY + 1 < Lines.Count)
            {
                JoinLineAndNext(CaretY);
                RaiseTextChanged();
            }

            NeedsRendering = true;
        }

        /// <summary>
        /// Joins the line at <see cref="index"/> and the following one.
        /// </summary>
        /// <param name="index">The index of the line you want to join with the following line.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="index"/> is >= the last lines index.</exception>
        public void JoinLineAndNext(int index)
        {
            // we need one extra line
            if(index + 1 >= Lines.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Lines[index] += Lines[index + 1];
            Lines.RemoveAt(index + 1);
        }

        /// <summary>
        /// Removes the selected text.
        /// </summary>
        public void RemoveSelectedText()
        {
            if (!HasSelection)
                return;

            var logicalSelectionStart = GetLogicalSelectionStart();
            var logicalSelectionEnd = GetLogicalSelectionEnd();

            // handle first line
            var line = Lines[logicalSelectionStart.Y];

            // just part of one line selected
            if (logicalSelectionStart.Y == logicalSelectionEnd.Y)
            {
                Lines[logicalSelectionStart.Y] =
                    line.Substring(0, logicalSelectionStart.X) + line.Substring(logicalSelectionEnd.X);

                RaiseTextChanged();
            }
            else
            {
                Lines[logicalSelectionStart.Y] = line.Substring(0, logicalSelectionStart.X);

                // join the ending line with the starting one
                Lines[logicalSelectionStart.Y] += Lines[logicalSelectionEnd.Y].Substring(logicalSelectionEnd.X);

                // lines in after first and before last line
                for (int i = logicalSelectionStart.Y + 1; i <= logicalSelectionEnd.Y; i++)
                    Lines.RemoveAt(logicalSelectionStart.Y + 1);

                RaiseTextChanged();
            }

            CaretX = logicalSelectionStart.X;
            CaretY = logicalSelectionStart.Y;

            ClearSelection();
        }

        /// <summary>
        /// Gets the selected text as string.
        /// </summary>
        /// <returns>The selected text or string.Empty.</returns>
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
            if(HasSelection)
                RemoveSelectedText();

            var insertLines = text.Split(new string[] {"\n", "\r\n", "\r"}, StringSplitOptions.None);
            var linePart1 = Lines[CaretY].Substring(0, CaretX);
            var linePart2 = Lines[CaretY].Substring(CaretX);

            if (insertLines.Length == 1)
            {
                Lines[CaretY] = linePart1 + insertLines[0] + linePart2;
                CaretX += insertLines[0].Length;
            }
            else if (insertLines.Length > 1)
            {
                Lines[CaretY] = linePart1 + insertLines[0];

                for(int i = 1; i + 1 < insertLines.Length; i++, CaretY++)
                    Lines.Insert(CaretY, insertLines[i]);

                CaretY++;
                var lastInsertLine = insertLines[insertLines.Length - 1];

                Lines.Insert(CaretY, lastInsertLine + linePart2);

                CaretX = lastInsertLine.Length;
            }
            else
                throw new InvalidOperationException();

            RaiseTextChanged();
            NeedsRendering = true;
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

            RaiseTextChanged();
        }

        /// <summary>
        /// Gets the content of the editor as string.
        /// </summary>
        /// <returns>The editors text.</returns>
        public string GetText()
        {
            var charCount = GetCharCount();
            var result = new StringBuilder(charCount);

            foreach (var line in Lines)
                result.AppendLine(line);

            Debug.Assert(result.Capacity == charCount);

            return Lines.Count > 0 ? result.ToString(0, result.Length - Environment.NewLine.Length) : string.Empty;
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
