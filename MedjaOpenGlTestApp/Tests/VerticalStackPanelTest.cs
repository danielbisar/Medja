using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class VerticalStackPanelTest
    {
        private readonly IControlFactory _controlFactory;

        public VerticalStackPanelTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var stackPanel = _controlFactory.Create<VerticalStackPanel>();
            stackPanel.SpaceBetweenChildren = 20;
            stackPanel.Children.Add(CreateControl(Colors.Red, HorizontalAlignment.None));
            stackPanel.Children.Add(CreateControl(Colors.Blue, HorizontalAlignment.Left));
            stackPanel.Children.Add(CreateControl(Colors.Green, HorizontalAlignment.Right));
            stackPanel.Children.Add(CreateControl(Colors.White, HorizontalAlignment.Center));
            stackPanel.Children.Add(CreateControl(Colors.LightGray, HorizontalAlignment.Stretch));

            return stackPanel;
        }

        private Control CreateControl(Color color, HorizontalAlignment horizontalAlignment)
        {
            var control = _controlFactory.Create<Control>();
            control.Position.Width = 100;
            control.Position.Height = 50;
            control.Background = color;
            control.HorizontalAlignment = horizontalAlignment;

            return control;
        }
    }
}