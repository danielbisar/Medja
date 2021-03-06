using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ScrollingGridTest
    {
        private readonly IControlFactory _controlFactory;

        public ScrollingGridTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var scrollingGrid = _controlFactory.Create<ScrollingGrid>();
            scrollingGrid.SpacingX = 10;
            scrollingGrid.SpacingY = 10;
            scrollingGrid.RowHeight = 200;
            scrollingGrid.Background = Colors.LightGray;

            for (int i = 0; i < 4; i++)
            {
                var control = _controlFactory.Create<Control>(p => p.Background = Colors.GetByIndex(i));
                scrollingGrid.Children.Add(control);
            }

            return scrollingGrid;
        }
    }
}