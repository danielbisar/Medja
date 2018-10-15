using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class SimpleDockPanelTest
    {
        private readonly ControlFactory _controlFactory;

        public SimpleDockPanelTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public DockPanel Create()
        {
            var controlTop = _controlFactory.Create<Control>();
            controlTop.Background = Colors.Blue;
            controlTop.Position.Height = 50;
            
            var controlRight = _controlFactory.Create<Control>();
            controlRight.Background = Colors.Red;
            controlRight.Position.Width = 50;
            
            var controlBottom = _controlFactory.Create<Control>();
            controlBottom.Background = Colors.Green;
            controlBottom.Position.Height = 50;
            
            var controlFill = _controlFactory.Create<Control>();
            controlFill.Background = Colors.White;
            
            var dockPanel = new DockPanel();
            dockPanel.Add(Dock.Top, controlTop);
            dockPanel.Add(Dock.Bottom, controlBottom);
            dockPanel.Add(Dock.Right, controlRight);
            dockPanel.Add(Dock.Fill, controlFill);

            return dockPanel;
        }
    }
}