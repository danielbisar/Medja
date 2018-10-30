using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ScrollableContainerTest
    {
        private readonly ControlFactory _controlFactory;

        public ScrollableContainerTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var textBlock = _controlFactory.Create<TextBlock>(p =>
            {
                p.Background = Colors.Green;
                p.Position.Height = 1500;
                p.HorizontalAlignment = HorizontalAlignment.Stretch;
            });

            for (int i = 0; i < 100; i++)
                textBlock.Text += "Line " + i + "\n";
            
            var result = _controlFactory.Create<ScrollableContainer>();
            result.Content = textBlock; 
            result.VerticalAlignment = VerticalAlignment.Stretch;
            result.HorizontalAlignment = HorizontalAlignment.Stretch;

            return result;
        }
    }
}