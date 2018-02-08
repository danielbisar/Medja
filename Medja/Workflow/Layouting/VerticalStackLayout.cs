using System;
using Medja.Layers.Layouting;
using Medja.Primitives;

namespace Medja.Layouting
{
    public class VerticalStackLayout : LayoutControl
    {        
        public float SpaceBetweenChildren { get; set; }

        public VerticalStackLayout()
        {
            SpaceBetweenChildren = 10;
        }        

        // TODO margin and padding

        public override Size Measure(Size availableSize)
        {
            var result = new Size();

            foreach(var child in Children)
            {
                result.Height += child.Position.Height + SpaceBetweenChildren;
                result.Width = Math.Max(result.Width, child.Position.Width);
            }

            result.Height = Math.Min(result.Height, availableSize.Height);
            result.Width = Math.Min(result.Width, availableSize.Width);

            return result;
        }

        public override void Arrange(Point pos, Size targetSize)
        {
            var spacingY = SpaceBetweenChildren;
            var count = Children.Count;
            var childAvailableSize = new Size(targetSize.Width, (targetSize.Height - spacingY * (count - 1)) / count);

            var curX = pos.X;
            var curY = pos.Y;

            ControlState child;

            for (int i = 0; i < count; i++)
            {
                child = Children[i];

                var position = child.Position;
                position.X = curX;
                position.Y = curY;
                position.Width = childAvailableSize.Width;
                position.Height = childAvailableSize.Height;

                curY += childAvailableSize.Height + spacingY;
            }
        }
    }
}
