using Medja.Primitives;

namespace Medja.Controls
{
    public class HorizontalStackPanel : Panel
    {
        public readonly Property<float> PropertySpaceBetweenChildren;
        public float SpaceBetweenChildren
        {
            get { return PropertySpaceBetweenChildren.Get(); }
            set { PropertySpaceBetweenChildren.Set(value); }
        }

        public readonly Property<float?> PropertyChildrenWidth;
        public float? ChildrenWidth
        {
            get { return PropertyChildrenWidth.Get(); }
            set { PropertyChildrenWidth.Set(value); }
        }

        public HorizontalStackPanel()
        {
            PropertySpaceBetweenChildren = new Property<float>();
            PropertySpaceBetweenChildren.AffectsLayout(this);
            PropertySpaceBetweenChildren.UnnotifiedSet(10);
            
            PropertyChildrenWidth = new Property<float?>();
            PropertyChildrenWidth.AffectsLayout(this);
        }
        
        public override void Arrange(Size targetSize)
        {
            var height = targetSize.Height - Padding.TopAndBottom;
            var curX = Position.X + Padding.Left;
            var curY = Position.Y + Padding.Top;

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                if (child.Visibility == Visibility.Collapsed)
                    continue;

                var childPos = child.Position;

                if (child.VerticalAlignment != VerticalAlignment.Top
                        && child.VerticalAlignment != VerticalAlignment.Bottom)
                    childPos.Height = height - child.Margin.TopAndBottom;
                
                if(child.VerticalAlignment == VerticalAlignment.Bottom)
                    childPos.Y = curY + height - childPos.Height;
                else
                    childPos.Y = curY + child.Margin.Top;
                
                if (ChildrenWidth.HasValue)
                    childPos.Width = ChildrenWidth.Value - child.Margin.LeftAndRight;

                childPos.X = curX + child.Margin.Left;
                curX += childPos.Width + SpaceBetweenChildren;
                
                child.Arrange(new Size(childPos.Width, childPos.Height));
            }
        }
    }
}