using System.Linq;
using Medja.Primitives;
using Medja.Properties;

namespace Medja.Controls
{
    public class UniformWidthPanel : Panel
    {
        public readonly Property<float> PropertyMaxChildWidth;
        /// <summary>
        /// Gets or sets the maximum width for children. 0 means use the available space.
        /// </summary>
        public float MaxChildWidth
        {
            get => PropertyMaxChildWidth.Get();
            set => PropertyMaxChildWidth.Set(value);
        }

        public readonly Property<float> PropertySpaceBetweenChildren;
        /// <summary>
        /// Gets or sets the space between children.
        /// </summary>
        public float SpaceBetweenChildren
        {
            get => PropertySpaceBetweenChildren.Get();
            set => PropertySpaceBetweenChildren.Set(value);
        }

        public UniformWidthPanel()
        {
            PropertyMaxChildWidth = new Property<float>();
            PropertySpaceBetweenChildren = new Property<float>();
        }

        public override void Arrange(Size availableSize)
        {
            var visibleChildrenCount = Children.Count(p => p.Visibility != Visibility.Collapsed);
            
            if (visibleChildrenCount == 0)
                return;

            var y = Position.Y + Padding.Top;
            var x = Position.X + Padding.Left;
            var height = availableSize.Height - Padding.TopAndBottom;
            var width = availableSize.Width - Padding.LeftAndRight;
            var childWidth = width / visibleChildrenCount;

            if (MaxChildWidth > 0 && childWidth > MaxChildWidth)
                childWidth = MaxChildWidth;

            foreach (var child in Children.Where(p => p.Visibility != Visibility.Collapsed))
            {
                var childPos = child.Position;

                childPos.X = x;
                childPos.Width = childWidth;

                if (child.VerticalAlignment == VerticalAlignment.Bottom)
                    childPos.Y = y + height - childPos.Height;
                else
                    childPos.Y = y + child.Margin.Top;
                
                if (child.VerticalAlignment != VerticalAlignment.Top
                    && child.VerticalAlignment != VerticalAlignment.Bottom)
                    childPos.Height = height - child.Margin.TopAndBottom;

                x += childWidth + SpaceBetweenChildren;

                child.Arrange(new Size(childPos.Width, childPos.Height));
            }
            
            base.Arrange(availableSize);
        }
    }
}