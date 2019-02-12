using Medja.Controls;
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
            var normalButton = _controlFactory.Create<Button>();
            normalButton.Text = "Normal";

            var disabledButton = _controlFactory.Create<Button>();
            disabledButton.Text = "Disabled";
            disabledButton.IsEnabled = false;
            
            var result = _controlFactory.Create<VerticalStackPanel>();
            result.Children.Add(normalButton);
            result.Children.Add(disabledButton);

            return result;
        }
        
    }
}