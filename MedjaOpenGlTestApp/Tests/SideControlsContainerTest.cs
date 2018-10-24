using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class SideControlsContainerTest
    {
        private readonly ControlFactory _controlFactory;

        public SideControlsContainerTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public ContentControl Create()
        {
            var result = _controlFactory.Create<SideControlsContainer>();
            result.Content = _controlFactory.Create<Control>(p =>
            {
                p.Background = Colors.Blue;
                p.HorizontalAlignment = HorizontalAlignment.Stretch;
                p.VerticalAlignment = VerticalAlignment.Stretch;
            });
            result.SideContent = _controlFactory.Create<Control>(p =>
            {
                p.Background = Colors.Green;
                p.HorizontalAlignment = HorizontalAlignment.Stretch;
                p.VerticalAlignment = VerticalAlignment.Stretch;
            });
            
            return result;
        }
    }
}