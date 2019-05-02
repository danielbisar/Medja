using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ContentControlTest
    {
        private readonly IControlFactory _controlFactory;

        public ContentControlTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public ContentControl Create()
        {
            var control = _controlFactory.Create<Control>();
            control.Background = Colors.Green;
            control.Position.Width = 50;
            control.Position.Height = 50;
            control.VerticalAlignment = VerticalAlignment.Bottom;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            
            var result = _controlFactory.Create<ContentControl>();
            result.Content = control;
            
            return result;
        }
    }
}