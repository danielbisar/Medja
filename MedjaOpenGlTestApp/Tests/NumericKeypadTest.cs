using System;
using Medja.Controls;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class NumericKeypadTest
    {
        private readonly ControlFactory _controlFactory;

        public NumericKeypadTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var textBox = _controlFactory.Create<TextBox>();
            textBox.Text = "2345";

            var button = _controlFactory.Create<Button>();
            button.Text = "OpenInput";
            
            
            var stackPanel = _controlFactory.Create<VerticalStackPanel>();
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(button);
            
            var result = _controlFactory.Create<DialogParentControl>();
            result.Content = stackPanel;
            result.DialogControl = _controlFactory.Create<QuestionDialog>(p =>
                {
                    p.Content = _controlFactory.Create<NumericKeypad>();
                });

            button.InputState.MouseClicked += (s, e) =>
            {
                result.IsDialogVisible = true;                
            };
            
                     
             
            return result;
        }
    }
}