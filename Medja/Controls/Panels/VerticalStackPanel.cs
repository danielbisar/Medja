using Medja.Primitives;

namespace Medja.Controls
{
    public class VerticalStackPanel : Panel
    {
        public float SpaceBetweenChildren { get; set; }
        public float? ChildrenHeight { get; set; }

        public VerticalStackPanel()
        {
            SpaceBetweenChildren = 10;
            ChildrenHeight = null;
        }

        public override void Arrange(Size targetSize)
        {
            var width = targetSize.Width - Padding.LeftAndRight;
            var curX = Position.X + Padding.Left;
            var curY = Position.Y + Padding.Top;

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                if (child.Visibility == Visibility.Collapsed)
                    continue;

                var childPos = child.Position;
                
                if(child.HorizontalAlignment != HorizontalAlignment.Left 
                        && child.HorizontalAlignment != HorizontalAlignment.Right)
                    childPos.Width = width - child.Margin.LeftAndRight;

                if(child.HorizontalAlignment == HorizontalAlignment.Right)
                    childPos.X = curX + width - childPos.Width;
                else
                    childPos.X = curX + child.Margin.Left;
                
                if (ChildrenHeight.HasValue)
                    childPos.Height = ChildrenHeight.Value - child.Margin.TopAndBottom;

                childPos.Y = curY + child.Margin.Top;
                curY += childPos.Height + SpaceBetweenChildren;
                
                child.Arrange(new Size(childPos.Width, childPos.Height));
            }
        }
    }
}