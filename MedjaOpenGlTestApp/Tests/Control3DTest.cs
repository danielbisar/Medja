using Medja.Controls;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class Control3DTest
    {
        private readonly IControlFactory _controlFactory;

        public Control3DTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var controls = new Control[4];

            for (int i = 0; i < 1; i++)
            {
                var openglctrl =new OpenGlTestControl();
                controls[i] = openglctrl;
                controls[i].Renderer = new OpenGlTestControlRenderer(openglctrl);

                //controls[i] = _controlFactory.Create<Button>();
                controls[i].Margin.SetAll(10);
            }

            var scrollingGrid = _controlFactory.Create<ScrollingGrid>();
            scrollingGrid.SpacingX = 10;
            scrollingGrid.SpacingY = 10;
            scrollingGrid.RowHeight = 200;
            scrollingGrid.Background = Colors.LightGray;
            scrollingGrid.Margin.SetAll(10);
            
            for(int i = 0; i < controls.Length && controls[i] != null; i++)
                scrollingGrid.Children.Add(controls[i]);
            
            // doesn't make too much sense, just for testing
            var dockPanel = _controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Fill, scrollingGrid);

            return dockPanel;
        }
    }
}