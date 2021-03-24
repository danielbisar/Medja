using Medja.Primitives;

namespace Medja.Controls.Panels
{
    /// <summary>
    /// A panel that resizes it's children to it's own size.
    /// </summary>
    public class LayerPanel : Panel
    {
        public override void Arrange(Size availableSize)
        {
            var x = Position.X + Padding.Left;
            var y = Position.Y + Padding.Top;

            var height = availableSize.Height - Padding.TopAndBottom;
            var width = availableSize.Width - Padding.LeftAndRight;

            foreach (var child in Children)
            {
                var pos = child.Position;
                var margin = child.Margin;

                if (child.VerticalAlignment == VerticalAlignment.Top
                    || child.VerticalAlignment == VerticalAlignment.None
                    || child.VerticalAlignment == VerticalAlignment.Stretch)
                    pos.Y = y + margin.Top;
                else if (child.VerticalAlignment == VerticalAlignment.Bottom)
                    pos.Y = y + height - (pos.Height + margin.Bottom);

                if(child.VerticalAlignment == VerticalAlignment.None
                   || child.VerticalAlignment == VerticalAlignment.Stretch)
                    pos.Height = height - margin.TopAndBottom;

                if(child.HorizontalAlignment == HorizontalAlignment.Left
                   || child.HorizontalAlignment == HorizontalAlignment.None
                   || child.HorizontalAlignment == HorizontalAlignment.Stretch)
                    pos.X = x + margin.Left;
                else if(child.HorizontalAlignment == HorizontalAlignment.Right)
                    pos.X = x + width - (pos.Width + margin.Right);

                if(child.HorizontalAlignment == HorizontalAlignment.None
                   || child.HorizontalAlignment == HorizontalAlignment.Stretch)
                    pos.Width = width - margin.LeftAndRight;

                child.Arrange(new Size(pos.Width, pos.Height));
            }

            base.Arrange(availableSize);
        }
    }
}
