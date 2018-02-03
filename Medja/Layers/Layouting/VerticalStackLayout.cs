using System;
using System.Collections.Generic;
using System.Text;
using Medja.Controls;

namespace Medja.Layers.Layouting
{
    public class VerticalStackLayout : LayoutControl
    {
        private readonly List<Control> _children;

        public float SpaceBetweenChildren { get; set; }

        public VerticalStackLayout()
            : this(new Control[0])
        {
        }

        public VerticalStackLayout(IEnumerable<Control> controls)
        {
            _children = new List<Control>();
            SpaceBetweenChildren = 0.012f;
        }

        public void Add(Control child)
        {
            _children.Add(child);
        }

        public override IEnumerable<ControlState> GetLayout(ControlState state)
        {
            var pos = state.Position;
            var curX = pos.X;
            var curY = pos.Y;
            var spacingY = SpaceBetweenChildren;
            var count = _children.Count;
            var childAvailableSize = new Size(pos.Width, (pos.Height / count) - spacingY * count);
            Control child;

            for (int i = 0; i < count; i++)
            {
                child = _children[i];

                yield return new ControlState
                {
                    Control = child,
                    Position = new PositionInfo
                    {
                        X = curX,
                        Y = curY,
                        Width = childAvailableSize.Width,
                        Height = childAvailableSize.Height
                    }
                };

                curY += childAvailableSize.Height + spacingY;
            }
        }
    }
}
