using System;
using Medja.Controls;
using Medja.Primitives;

namespace Medja.Controls
{
    public class VerticalStackPanel : Panel
    {        
        public float SpaceBetweenChildren { get; set; }
        public float ChildrenHeight { get; set; }

        public VerticalStackPanel()
        {
            SpaceBetweenChildren = 10;
            ChildrenHeight = 50; // todo could be calculated on the fly, currently we set it
        }        

        // TODO margin and padding

        internal override Size Measure(Size availableSize)
        {
            //var result = new Size();

            //result.Width = availableSize.Width;

            //foreach(var child in Children)
            //{
            //    // todo child margin etc
            //    // todo concept of measure children (like button -> text)
            //    result.Height += child.Position.Height + SpaceBetweenChildren;                
            //}

            //result.Height = Math.Min(result.Height, availableSize.Height);
            //result.Width = Math.Min(result.Width, availableSize.Width);

            //return result;
            return availableSize;
        }

        internal override void Arrange(Size targetSize)
        {
            var spacingY = SpaceBetweenChildren;
            var count = Children.Count;
            var childAvailableSize = new Size(targetSize.Width, (targetSize.Height - spacingY * (count - 1)) / count);

            var curX = Position.X;
            var curY = Position.Y;

            Control child;

            for (int i = 0; i < count; i++)
            {
                child = Children[i];

                var position = child.Position;
                position.X = curX;
                position.Y = curY;
                position.Width = childAvailableSize.Width;
                position.Height = ChildrenHeight;

                //curY += childAvailableSize.Height + spacingY; <-- stretch
                curY += position.Height + spacingY; // <-- just stack
            }
        }
    }
}
