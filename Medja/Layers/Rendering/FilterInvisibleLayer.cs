using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medja.Layers.Layouting;

namespace Medja.Layers.Rendering
{
    public class FilterInvisibleLayer : ILayer
    {
        public Size Size { get; set; }

        public IEnumerable<ControlState> Apply(IEnumerable<ControlState> states)
        {
            return states.Where(p => IsVisible(p.Position));
        }

        private bool IsVisible(PositionInfo position)
        {
            // find intersection, could be done better... also we could evaluate if a control is completly hidden behind another one ...
            var xPos = position.X + position.Width;
            var yPos = position.Y + position.Height;

            // xPos or yPos > 0 means a part of the control is within the visible area, if it is smaller Size.Width or Height
            return xPos > 0 && xPos < Size.Width
                && yPos > 0 && yPos < Size.Height;
        }
    }
}
