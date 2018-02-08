using System.Collections.Generic;
using Medja.Controls;
using Medja.Primitives;

namespace Medja.Layers.Layouting
{
    public abstract class LayoutControl : Control
    {
        public List<ControlState> Children { get; }

        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }

        protected LayoutControl()
        {
            Children = new List<ControlState>();
            Margin = new Thickness();
            Padding = new Thickness();
        }

        public abstract Size Measure(Size availableSize);
        public abstract void Arrange(Point pos, Size targetSize);
    }
}
