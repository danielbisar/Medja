using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class DockPanelTest
    {
        private readonly ControlFactory _controlFactory;

        public DockPanelTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public DockPanel Create()
        {
            var controlLeft = _controlFactory.Create<Control>();
            controlLeft.Background = Colors.Blue;
            controlLeft.Margin.SetAll(10);
            controlLeft.Position.Width = 50;
            
            var controlLeftTop = _controlFactory.Create<Control>();
            controlLeftTop.Background = Colors.Blue;
            controlLeftTop.Margin.SetAll(10);
            controlLeftTop.Position.Width = 20;
            controlLeftTop.Position.Height = 20;
            controlLeftTop.VerticalAlignment = VerticalAlignment.Top;
            
            var controlLeftBottom = _controlFactory.Create<Control>();
            controlLeftBottom.Background = Colors.Blue;
            controlLeftBottom.Margin.SetAll(10);
            controlLeftBottom.Position.Width = 20;
            controlLeftBottom.Position.Height = 20;
            controlLeftBottom.VerticalAlignment = VerticalAlignment.Bottom;
			
            var controlRight = _controlFactory.Create<Control>();
            controlRight.Background = Colors.Blue;
            controlRight.Position.Width = 50;
            controlRight.Margin.SetAll(10);
            
            var controlRightTop = _controlFactory.Create<Control>();
            controlRightTop.Background = Colors.Blue;
            controlRightTop.Position.Width = 20;
            controlRightTop.Position.Height = 20;
            controlRightTop.Margin.SetAll(10);
            controlRightTop.VerticalAlignment = VerticalAlignment.Top;
            
            var controlRightBottom = _controlFactory.Create<Control>();
            controlRightBottom.Background = Colors.Blue;
            controlRightBottom.Position.Width = 20;
            controlRightBottom.Position.Height = 20;
            controlRightBottom.Margin.SetAll(10);
            controlRightBottom.VerticalAlignment = VerticalAlignment.Bottom;
			
            var controlTop = _controlFactory.Create<Control>();
            controlTop.Background = Colors.Red;
            controlTop.Position.Height = 50;
            controlTop.Margin.SetAll(10);
            
            var controlTopLeft = _controlFactory.Create<Control>();
            controlTopLeft.Background = Colors.Red;
            controlTopLeft.HorizontalAlignment = HorizontalAlignment.Left;
            controlTopLeft.Position.Height = 20;
            controlTopLeft.Position.Width = 20;
            controlTopLeft.Margin.SetAll(10);
			
            var controlTopRight = _controlFactory.Create<Control>();
            controlTopRight.Background = Colors.Red;
            controlTopRight.HorizontalAlignment = HorizontalAlignment.Right;
            controlTopRight.Position.Height = 20;
            controlTopRight.Position.Width = 20;
            controlTopRight.Margin.SetAll(10);
            
            var controlBottom = _controlFactory.Create<Control>();
            controlBottom.Background = Colors.Green;
            controlBottom.Position.Height = 50;
            controlBottom.Margin.SetAll(10);
            
            var controlBottomLeft = _controlFactory.Create<Control>();
            controlBottomLeft.Background = Colors.Green;
            controlBottomLeft.Position.Width = 20;
            controlBottomLeft.Position.Height = 20;
            controlBottomLeft.Margin.SetAll(10);
            controlBottomLeft.HorizontalAlignment = HorizontalAlignment.Left;
            
            var controlBottomRight = _controlFactory.Create<Control>();
            controlBottomRight.Background = Colors.Green;
            controlBottomRight.Position.Width = 20;
            controlBottomRight.Position.Height = 20;
            controlBottomRight.Margin.SetAll(10);
            controlBottomRight.HorizontalAlignment = HorizontalAlignment.Right;
            
            var controlFill = _controlFactory.Create<Control>();
            controlFill.Background = Colors.White;
            controlFill.Position.Height = 50; // check if ignored
            controlFill.Margin.SetAll(10);
			
            var dockPanel = _controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Left, controlLeft);
            dockPanel.Add(Dock.Left, controlLeftTop);
            dockPanel.Add(Dock.Left, controlLeftBottom);
            dockPanel.Add(Dock.Right, controlRight);
            dockPanel.Add(Dock.Right, controlRightTop);
            dockPanel.Add(Dock.Right, controlRightBottom);
            dockPanel.Add(Dock.Top, controlTop);
            dockPanel.Add(Dock.Top, controlTopLeft);
            dockPanel.Add(Dock.Top, controlTopRight);
            dockPanel.Add(Dock.Bottom, controlBottom);
            dockPanel.Add(Dock.Bottom, controlBottomLeft);
            dockPanel.Add(Dock.Bottom, controlBottomRight);
            dockPanel.Add(Dock.Fill, controlFill);

            return dockPanel;
        }
    }
}