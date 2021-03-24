using System;
using Medja.Primitives;
using Medja.Properties;

namespace Medja.Controls.Panels
{
    /// <summary>
    /// A container that allows its content to be bigger than the available area and offers scrolling (todo).
    /// </summary>
    public class ScrollingGrid : Panel
    {
        public readonly Property<bool> IsVerticalScrollBarVisibleProperty;
        public bool IsVerticalScrollBarVisible
        {
            get { return IsVerticalScrollBarVisibleProperty.Get(); }
            protected set { IsVerticalScrollBarVisibleProperty.Set(value); }
        }

        private int _columns;
        public int Columns
        {
            get { return _columns; }
            set
            {
                if(value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Columns), "must be > 0");

                _columns = value;
            }
        }

        /// <summary>
        /// The space between each column.
        /// </summary>
        public float SpacingX { get; set; }

        /// <summary>
        /// The space between each row.
        /// </summary>
        public float SpacingY { get; set; }

        /// <summary>
        /// The height of each row. If null the height of the first element will be used.
        /// </summary>
        public float? RowHeight { get; set; }

        public ScrollingGrid()
        {
            Columns = 2;
            IsVerticalScrollBarVisibleProperty = new Property<bool>();
        }

        public override void Arrange(Size availableSize)
        {
            if (Children.Count != 0)
            {
                var area = Rect.Subtract(Position, Margin);
                area.Subtract(Padding);

                var columnWidth = area.Width / _columns - SpacingX / 2.0f;
                var rowHeight = (RowHeight ?? Children[0].Position.Height);
                var row = 0;
                var column = 0;

                foreach (var child in Children)
                {
                    child.Position.X = Position.X + Margin.Left + Padding.Left + column * columnWidth +
                                       column * SpacingX;
                    child.Position.Width = columnWidth;
                    child.Position.Y = Position.Y + Margin.Top + Padding.Top + row * rowHeight + row * SpacingY;
                    child.Position.Height = rowHeight;

                    column++;

                    if (column >= Columns)
                    {
                        column = 0;
                        row++;
                    }
                }
            }

            base.Arrange(availableSize);
        }
    }
}
