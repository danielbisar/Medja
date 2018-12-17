using Medja.Controls;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class Control3DTest
    {
        private readonly ControlFactory _controlFactory;

        public Control3DTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var controls = new OpenGlTestControl[4];

            for (int i = 0; i < 4; i++)
            {
                controls[i] = new OpenGlTestControl();
                controls[i].Renderer = new OpenGlTestControlRenderer(controls[i]);
                controls[i].Margin.SetAll(10);
            }

            var scrollingGrid = _controlFactory.Create<ScrollingGrid>();
            scrollingGrid.SpacingX = 10;
            scrollingGrid.SpacingY = 10;
            scrollingGrid.RowHeight = 200;
            scrollingGrid.Background = Colors.LightGray;
            scrollingGrid.Margin.SetAll(10);
            scrollingGrid.Children.Add(controls[0]);
            scrollingGrid.Children.Add(controls[1]);
            scrollingGrid.Children.Add(controls[2]);
            scrollingGrid.Children.Add(controls[3]);
            
            // doesn't make too much sense, just for testing
            var dockPanel = _controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Fill, scrollingGrid);

            return dockPanel;
        }
    }
}