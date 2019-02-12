using System;
using Medja.Controls;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class NumericKeypad : ContentControl
    {
        private readonly ControlFactory _controlFactory;

        public NumericKeypad(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            Content = CreateContent();
        }

        public Control CreateContent()
        {
            
            var button7 = _controlFactory.Create<Button>();
            button7.Text = "7";
            
            var button8 = _controlFactory.Create<Button>();
            button8.Text = "8";
            
            var button9 = _controlFactory.Create<Button>();
            button9.Text = "9";
            
            var buttonBackspace = _controlFactory.Create<Button>();
            buttonBackspace.Text = "<|";
            
            var button4 = _controlFactory.Create<Button>();
            button4.Text = "4";
            
            var button5 = _controlFactory.Create<Button>();
            button5.Text = "5";
            
            var button6 = _controlFactory.Create<Button>();
            button6.Text = "6";
            
            var buttonClear = _controlFactory.Create<Button>();
            buttonClear.Text = "Clear";

            var button1 = _controlFactory.Create<Button>();
            button1.Text = "1";

            var button2 = _controlFactory.Create<Button>();
            button2.Text = "2";
            
            var button3 = _controlFactory.Create<Button>();
            button3.Text = "3";

            var placeholder = new Control(); 
            
            var buttonNotYet = _controlFactory.Create<Button>();
            buttonNotYet.Text = "X";

            var button0 = _controlFactory.Create<Button>();
            button0.Text = "0";

            var result = _controlFactory.Create<TablePanel>();
            result.Rows.Add(new RowDefinition(100));
            result.Rows.Add(new RowDefinition(100));
            result.Rows.Add(new RowDefinition(100));
            result.Rows.Add(new RowDefinition(100));
            result.Columns.Add(new ColumnDefinition(50));
            result.Columns.Add(new ColumnDefinition(50));
            result.Columns.Add(new ColumnDefinition(50));
            result.Columns.Add(new ColumnDefinition(50));
            result.Children.Add(button7);
            result.Children.Add(button8);
            result.Children.Add(button9);
            result.Children.Add(buttonBackspace);
            result.Children.Add(button4);
            result.Children.Add(button5);
            result.Children.Add(button6);
            result.Children.Add(buttonClear);
            result.Children.Add(button1);
            result.Children.Add(button2);
            result.Children.Add(button3);
            result.Children.Add(placeholder);
            result.Children.Add(buttonNotYet);
            result.Children.Add(button0);
            result.Children.Add(buttonNotYet);
            
            return result;
        }
    }
}