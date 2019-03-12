using System;
using System.Text;
using System.Collections.Generic;
using Medja.Controls;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class NumericKeypad : ContentControl
    {
        private readonly ControlFactory _controlFactory;
        private TextBox _textBox;
        
        public NumericKeypad(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            
            Content = CreateContent();
        }

        private Control CreateContent()
        {
            var placeholder = new Control();

            _textBox = _controlFactory.Create<TextBox>();
            
            var keyboard = _controlFactory.Create<TablePanel>();
            int rowHeight = 90;
            int rowWidth = 50;
            keyboard.Rows.Add(new RowDefinition(rowHeight));
            keyboard.Rows.Add(new RowDefinition(rowHeight));
            keyboard.Rows.Add(new RowDefinition(rowHeight));
            keyboard.Rows.Add(new RowDefinition(rowHeight));
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));

            AddNumericButton(keyboard.Children, "7");
            AddNumericButton(keyboard.Children, "8");
            AddNumericButton(keyboard.Children, "9");

            var backspaceButton = CreateButton("<x");
            backspaceButton.InputState.MouseClicked += OnBackspaceButtonClicked;
            keyboard.Children.Add(backspaceButton);
            
            AddNumericButton(keyboard.Children, "4");
            AddNumericButton(keyboard.Children, "5");
            AddNumericButton(keyboard.Children, "6");

            var clearButton = CreateButton("Clear");
            clearButton.InputState.MouseClicked += OnClearButtonClicked;
            keyboard.Children.Add(clearButton);

            AddNumericButton(keyboard.Children, "1");
            AddNumericButton(keyboard.Children, "2");
            AddNumericButton(keyboard.Children, "3");

            keyboard.Children.Add(placeholder);
            keyboard.Children.Add(CreateButton("X"));

            AddNumericButton(keyboard.Children, "0");

            keyboard.Children.Add(CreateButton("X"));
            
            var dock = _controlFactory.Create<DockPanel>();
            dock.Add(Dock.Top, _textBox);
            dock.Add(Dock.Fill, keyboard);
            
            return dock;
        }

        private void AddNumericButton(ICollection<Control> collection, string text)
        {
            collection.Add(_controlFactory.Create<Button>(p => 
            {
                p.Text = text;
                p.InputState.MouseClicked += OnNumericButtonClicked;                
            }));
        }

        private Button CreateButton(string text)
        {
            var result = _controlFactory.Create<Button>();
            result.Text = text;

            return result;
        }

        private void OnNumericButtonClicked(object sender, EventArgs e)
        {
            var inputState = (InputState) sender;
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
            
            _textBox.Text = text.Substring(0 , text.Length-1);
        }
        
    }
}