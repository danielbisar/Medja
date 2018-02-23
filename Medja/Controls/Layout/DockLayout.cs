using System;
using Medja.Controls;
using Medja.Primitives;
using Medja.Property;

namespace Medja.Controls
{
    public class DockLayout : LayoutControl
    {
        private static int DockAttachedId = AttachedProperties.GetNextId();

        public DockLayout()
        {
        }

        public void Add(Control control, Dock dock)
        {
            control.SetAttachedProperty(DockAttachedId, dock);
            Children.Add(control);
        }

        internal override Size Measure(Size availableSize)
        {
            return availableSize;
        }

        internal override void Arrange(Size targetSize)
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
                        childPos.Y = Position.Y;
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
