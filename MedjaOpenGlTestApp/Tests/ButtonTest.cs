using Medja.Controls;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ButtonTest
    {
        private readonly ControlFactory _controlFactory;

        public ButtonTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
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