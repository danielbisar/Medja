using System;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class NumericKeypadTest
    {
        private readonly IControlFactory _controlFactory;

        public NumericKeypadTest(IControlFactory controlFactory)
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

            var result = DialogService.CreateContainer(_controlFactory, stackPanel);
           
            button.InputState.Clicked += (s, e) =>
            {
                var dialog = _controlFactory.Create<NumericKeypadDialog>();
                dialog.Text = textBox.Text;
                dialog.Closed+= (sender, eventArgs) => 
                {
                    if (dialog.IsConfirmed)
                        textBox.Text = dialog.Text;           
                };

                DialogService.Show(dialog);
            };
             
            return result;
        }
    }
}