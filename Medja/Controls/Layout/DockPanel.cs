using System;
using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
    public class DockPanel : Panel
    {
        private Dictionary<Control, Dock> _docks;

        public DockPanel()
        {
            _docks = new Dictionary<Control, Dock>();
        }

        public void Add(Control control, Dock dock)
        {
            _docks.Add(control, dock);
            Children.Add(control);

            // TODO handle remove of child: remove dock info also, else memory leak
        }

        internal override Size Measure(Size availableSize)
        {
            //foreach (var child in Children)
            //    child.Measure(availableSize);

            return availableSize;
        }

        internal override void Arrange(Size targetSize)
        {
            foreach (var child in Children)
            {
                var dock = _docks[child];
                var childPos = child.Position;

                switch (dock)
                {
                    case Dock.Top:
                        break;
                    case Dock.Left:
                        break;
                    case Dock.Right:
                        childPos.X = targetSize.Width - childPos.Width;
                        childPos.Y = Position.Y;
                        childPos.Height = targetSize.Height; // TODO later calculate the left over space
                        break;
                    case Dock.Bottom:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Dock");
                }

                child.Arrange(new Size(childPos.Width, childPos.Height));
            }
        }        
    }
}
