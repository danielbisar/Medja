using System;
using System.Text;
using System.Collections.Generic;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class NumericKeypad : ContentControl
    {
        private readonly ControlFactory _controlFactory;

        private TextBox _textBox;
        public string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        } 
        
        public NumericKeypad(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
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
            const float rowWidth = 50;

            var keyboard = _controlFactory.Create<TablePanel>();

            keyboard.Rows.Add(new RowDefinition(rowHeight));
            keyboard.Rows.Add(new RowDefinition(rowHeight));
            keyboard.Rows.Add(new RowDefinition(rowHeight));
            keyboard.Rows.Add(new RowDefinition(rowHeight));
            
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));
            keyboard.Columns.Add(new ColumnDefinition(rowWidth));

            CreateButtons(keyboard);

            return keyboard;
        }

        // private char[][] KeypadMap = 
        // {
        //     {''}
        // };

        private void CreateButtons(TablePanel keyboard)
        {
            var placeholder = new Control();

            var backspaceButton = CreateButton("<x");
            backspaceButton.InputState.Clicked += OnBackspaceButtonClicked;

            var clearButton = CreateButton(Globalization.Clear);
            clearButton.InputState.Clicked += OnClearButtonClicked;

            AddNumericButton(keyboard.Children, "7");
            AddNumericButton(keyboard.Children, "8");
            AddNumericButton(keyboard.Children, "9");

            keyboard.Children.Add(backspaceButton);
            
            AddNumericButton(keyboard.Children, "4");
            AddNumericButton(keyboard.Children, "5");
            AddNumericButton(keyboard.Children, "6");
            
            keyboard.Children.Add(clearButton);

            AddNumericButton(keyboard.Children, "1");
            AddNumericButton(keyboard.Children, "2");
            AddNumericButton(keyboard.Children, "3");

            keyboard.Children.Add(placeholder);
            keyboard.Children.Add(CreateButton("X"));

            AddNumericButton(keyboard.Children, "0");

            keyboard.Children.Add(CreateButton("X"));
        }

        private void AddNumericButton(ICollection<Control> collection, string text)
        {
            collection.Add(_controlFactory.Create<Button>(p => 
            {
                p.Text = text;
                p.InputState.Clicked += OnNumericButtonClicked;                
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