using System.Collections.Generic;
using System.Linq;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    // idea: if necessary reuse point objects
    // idea: use a type of list that adds points ordered by x so that max and min methods could operate faster?
    // do not use ObservableCollection for performance reasons
    
    /// <summary>
    /// A 2D graph control that allows different usage scenarios
    /// - big data: put in you x,y data, the graph will downsample and display it
    /// </summary>
    public class Graph2D : ContentControl
    {
        private readonly IControlFactory _controlFactory;
        
        public Graph2DAxis AxisX { get; }
        public Graph2DAxis AxisY { get; }
        
        public Graph2D(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            
            AxisX = controlFactory.Create<Graph2DAxis>();
            AxisX.Orientation = AxisOrientation.Vertical;
            AxisY = controlFactory.Create<Graph2DAxis>();

            Content = CreateContent();
        }

        private Control CreateContent()
        {
            var dockPanel = _controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Left, AxisX);
            dockPanel.Add(Dock.Bottom, AxisY);

            return dockPanel;
        }
    }
}