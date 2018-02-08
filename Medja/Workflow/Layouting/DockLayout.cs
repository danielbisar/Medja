using System;
using Medja.Layers.Layouting;
using Medja.Primitives;
using Medja.Property;

namespace Medja.Layouting
{
    public class DockLayout : LayoutControl
    {
        private static int DockAttachedId = AttachedProperties.GetNextId();

        public DockLayout()
        {
        }

        public void Add(ControlState control, Dock dock)
        {
            control.SetAttachedProperty(DockAttachedId, dock);
            Children.Add(control);
        }

        public override Size Measure(Size availableSize)
        {
            return availableSize;
        }

        public override void Arrange(Point pos, Size targetSize)
        {
            foreach (var child in Children)
            {
                var dock = (Dock?)child.GetAttachedProperty(DockAttachedId);

                if (dock == null)
                    dock = Dock.Top;

                var childPos = child.Position;

                switch (dock)
                {
                    case Dock.Top:
                        break;
                    case Dock.Left:
                        break;
                    case Dock.Right:
                        childPos.X = targetSize.Width - child.Position.Width;
                        childPos.Y = pos.Y;
                        childPos.Height = targetSize.Height; // TODO later calculate the left over space
                        break;
                    case Dock.Bottom:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Dock");
                }
            }
        }        
    }
}
