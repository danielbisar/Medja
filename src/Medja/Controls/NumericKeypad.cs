using System;
using System.Collections.Generic;
using Medja.Input;
using Medja.Theming;

namespace Medja.Controls
{
    public class NumericKeypad : ContentControl
    {
        private readonly ControlFactory _controlFactory;
        private readonly List<List<string>> _buttonLayout;

        private TextBox _textBox;
        public string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

        public NumericKeypad(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;

            var factory = new NumericKeypadTextFactory();
            _buttonLayout = factory.Translate(@"7 8 9 c
4 5 6 b
1 2 3 -
- 0 -");

            Content = CreateContent();
        }

        private Control CreateContent()
        {
            _textBox = _controlFactory.Create<TextBox>();

            var keyboard = CreateButtonPanel();

            var dockPanel = _controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Top, _textBox);
            dockPanel.Add(Dock.Fill, keyboard);

            return dockPanel;
        }

        private TablePanel CreateButtonPanel()
        {
            const float rowHeight = 60;
            const float columnWidth = 50;

            // todo: calculate row and column height based on real values
            // might need another panel

            var keyboard = _controlFactory.Create<TablePanel>();

            // assumption: all rows have the same amount of columns
            var rowCount = _buttonLayout.Count;
            var columnCount = _buttonLayout[0].Count;

            for (int i = 0; i < rowCount; i++)
            {
                keyboard.Rows.Add(new RowDefinition(rowHeight));
            }

            for (int i = 0; i < columnCount; i++)
                keyboard.Columns.Add(new ColumnDefinition(columnWidth));

            CreateButtons(keyboard);

            return keyboard;
        }

        private void CreateButtons(TablePanel keyboard)
        {
            foreach (var row in _buttonLayout)
            {
                foreach (var column in row)
                {
                    keyboard.Children.Add(CreateButton(column));
                }
            }
        }

        private Control CreateButton(string text)
        {
            if (string.IsNullOrEmpty(text))
                return _controlFactory.Create<Control>();

            var result = _controlFactory.Create<Button>();
            result.Text = text;

            if (text == Globalization.Clear)
                result.InputState.Clicked += OnClearButtonClicked;
            else if (text == Globalization.Back)
                result.InputState.Clicked += OnBackspaceButtonClicked;
            else if (text.Length == 1 && char.IsNumber(text[0]))
                result.InputState.Clicked += OnNumericButtonClicked;

            return result;
        }

        private void OnNumericButtonClicked(object sender, EventArgs e)
        {
            var inputState = (InputState)sender;
            var button = (Button)inputState.Control;

            _textBox.Text += button.Text;
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            _textBox.Text = "";
        }

        private void OnBackspaceButtonClicked(object sender, EventArgs e)
        {
            var text = _textBox.Text;

            if (text.Length <= 0)
                return;

            _textBox.Text = text.Substring(0, text.Length - 1);
        }
    }
}