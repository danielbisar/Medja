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
            var result = _controlFactory.Create<ScrollableContainer>();
            result.Content = _controlFactory.Create<Control>(p =>
            {
                p.Background = Colors.Green;
                p.Position.Height = 1500;
                p.HorizontalAlignment = HorizontalAlignment.Stretch;
            });
            result.VerticalAlignment = VerticalAlignment.Stretch;
            result.HorizontalAlignment = HorizontalAlignment.Stretch;

            return result;
        }
    }
}