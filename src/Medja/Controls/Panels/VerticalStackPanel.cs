using Medja.Primitives;
using Medja.Properties;

namespace Medja.Controls
{
    public class VerticalStackPanel : Panel
    {
        public readonly Property<float> PropertySpaceBetweenChildren;
        public float SpaceBetweenChildren
        {
            get { return PropertySpaceBetweenChildren.Get(); }
            set { PropertySpaceBetweenChildren.Set(value); }
        }
        
        public readonly Property<float?> PropertyChildrenHeight;
        public float? ChildrenHeight
        {
            get { return PropertyChildrenHeight.Get(); }
            set { PropertyChildrenHeight.Set(value); }
        }

        public VerticalStackPanel()
        {
            PropertySpaceBetweenChildren = new Property<float>();
            PropertySpaceBetweenChildren.AffectsLayout(this);
            PropertySpaceBetweenChildren.UnnotifiedSet(10);
            
            PropertyChildrenHeight = new Property<float?>();
            PropertyChildrenHeight.AffectsLayout(this);
        }

        public override void Arrange(Size availableSize)
        {
            var width = availableSize.Width - Padding.LeftAndRight;
            var curX = Position.X + Padding.Left;
            var curY = Position.Y + Padding.Top;

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                if (child.Visibility == Visibility.Collapsed)
                    continue;

                var childPos = child.Position;
                
                if(child.HorizontalAlignment != HorizontalAlignment.Left
                && child.HorizontalAlignment != HorizontalAlignment.Right
                && child.HorizontalAlignment != HorizontalAlignment.Center)
                    childPos.Width = width - child.Margin.LeftAndRight;

                if(child.HorizontalAlignment == HorizontalAlignment.Right)
                    childPos.X = curX + width - childPos.Width;
                else if (child.HorizontalAlignment == HorizontalAlignment.Center)
                    childPos.X = (width - childPos.Width) / 2.0f;
                else
                    childPos.X = curX + child.Margin.Left;
                
                if (ChildrenHeight.HasValue)
                    childPos.Height = ChildrenHeight.Value - child.Margin.TopAndBottom;

                childPos.Y = curY + child.Margin.Top;
                curY += childPos.Height + SpaceBetweenChildren;
                
                child.Arrange(new Size(childPos.Width, childPos.Height));
            }

            base.Arrange(availableSize);
        }
    }
}