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
        private readonly StringBuilder _stringBuilder;

        public NumericKeypad(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _stringBuilder = new StringBuilder();
            
            Content = CreateContent();
        }

        public Control CreateContent()
        {
            var placeholder = new Control(); 

            var result = _controlFactory.Create<TablePanel>();
            result.Rows.Add(new RowDefinition(100));
            result.Rows.Add(new RowDefinition(100));
            result.Rows.Add(new RowDefinition(100));
            result.Rows.Add(new RowDefinition(100));
            result.Columns.Add(new ColumnDefinition(50));
            result.Columns.Add(new ColumnDefinition(50));
            result.Columns.Add(new ColumnDefinition(50));
            result.Columns.Add(new ColumnDefinition(50));

            AddNumericButton(result.Children, "7");
            AddNumericButton(result.Children, "8");
            AddNumericButton(result.Children, "9");

            var backspaceButton = CreateButton("<|");
            backspaceButton.InputState.MouseClicked += OnBackspaceButtonClicked;
            result.Children.Add(backspaceButton);
            
            AddNumericButton(result.Children, "4");
            AddNumericButton(result.Children, "5");
            AddNumericButton(result.Children, "6");

            var clearButton = CreateButton("Clear");
            clearButton.InputState.MouseClicked += OnClearButtonClicked;
            result.Children.Add(clearButton);

            AddNumericButton(result.Children, "1");
            AddNumericButton(result.Children, "2");
            AddNumericButton(result.Children, "3");

            result.Children.Add(placeholder);
            result.Children.Add(CreateButton("X"));

            AddNumericButton(result.Children, "0");

            result.Children.Add(CreateButton("X"));
            
            return result;
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

        private Button RegisterClick(Button button, EventHandler eventHandler)
        {
            button.InputState.MouseClicked += eventHandler;
            return button;
        }

        private void OnNumericButtonClicked(object sender, EventArgs e)
        {
            var inputState = (InputState) sender;
            var button = (Button)inputState.Control;
            
            _stringBuilder.Append(button.Text);

            Console.WriteLine(_stringBuilder.ToString());
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            _stringBuilder.Clear();
        }

        private void OnBackspaceButtonClicked(object sender, EventArgs e)
        {
            _stringBuilder.Length--;
        }

    }
}