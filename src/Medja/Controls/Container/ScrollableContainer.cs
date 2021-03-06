using System.Collections.Generic;
using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;

namespace Medja.Controls.Container
{
    public class ScrollableContainer : ContentControl
    {
        private readonly VerticalScrollBar _scrollBar;
        private float? _startDragPos;

        public ScrollableContainer(IControlFactory controlFactory)
        {
            _scrollBar = controlFactory.Create<VerticalScrollBar>();
            _scrollBar.PropertyValue.AffectsLayoutOf(this);
            _scrollBar.PropertyMaxValue.AffectsLayoutOf(this);

            InputState.Dragged += OnDragged;
            InputState.PropertyMouseWheelDelta.PropertyChanged += OnMouseWheelMoved;
            InputState.PropertyIsLeftMouseDown.PropertyChanged += OnLeftMouseUp;
            InputState.HandlesDrag = true;
            InputState.OwnsMouseEvents = true;
        }

        protected override void OnContentChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnContentChanged(sender, e);

            var oldContent = e.OldValue as Control;

            if (oldContent != null)
            {
                oldContent.Position.PropertyHeight.PropertyChanged -= OnContentHeightChanged;
            }

            var newContent = e.NewValue as Control;

            if (newContent != null)
            {
                UpdateScrollBarMaxValue();
                newContent.Position.PropertyHeight.PropertyChanged += OnContentHeightChanged;
            }
        }

        private void OnContentHeightChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateScrollBarMaxValue();
        }

        private void OnLeftMouseUp(object sender, PropertyChangedEventArgs e)
        {
            _startDragPos = null;
        }

        protected virtual void OnDragged(object sender, MouseDraggedEventArgs e)
        {
            if (!_startDragPos.HasValue)
                _startDragPos = _scrollBar.Value;

            UpdateScrollValue(_startDragPos.Value - e.Vector.Y);
        }

        private void UpdateScrollValue(float newValue)
        {
            if (newValue > _scrollBar.MaxValue)
                newValue = _scrollBar.MaxValue;

            if (newValue < 0)
                newValue = 0;

            _scrollBar.Value = newValue;
        }

        protected virtual void OnMouseWheelMoved(object sender, PropertyChangedEventArgs e)
        {
            var diff = (float) e.NewValue * -10;
            var newValue = _scrollBar.Value + diff;

            UpdateScrollValue(newValue);
        }

        public override void Arrange(Size availableSize)
        {
            var area = Rect.Subtract(Position, Margin);
            area.Subtract(Padding);

            var scrollBarWidth = _scrollBar.Position.Width;

            _scrollBar.Position.X = area.X + area.Width - scrollBarWidth;
            _scrollBar.Position.Y = area.Y;
            _scrollBar.Position.Height = area.Height;

            area.Width -= scrollBarWidth;
            area.Y -= _scrollBar.Value;

            ContentArranger.Position(area);
            ContentArranger.StretchWidth(area);

            if (Content != null)
            {
                Content.ClippingArea.X = area.X;
                Content.ClippingArea.Y = Position.Y + Margin.Top + Padding.Top;
                Content.ClippingArea.Height = area.Height;
                Content.ClippingArea.Width = area.Width;

                if (_scrollBar.Value > _scrollBar.MaxValue)
                    _scrollBar.Value = _scrollBar.MaxValue;
            }
        }

        private void UpdateScrollBarMaxValue()
        {
            if (Content == null)
            {
                _scrollBar.MaxValue = 0;
                return;
            }

            var notVisibleHeight = Content.Position.Height - Position.Height;
            var maxValue = 0;

            if (notVisibleHeight > 0)
                maxValue = (int)notVisibleHeight;

            if (maxValue < 0)
                maxValue = 0;

            _scrollBar.MaxValue = maxValue;
        }

        public override IEnumerable<Control> GetChildren()
        {
            if (Content != null)
                yield return Content;

            yield return _scrollBar;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(Content != null)
                Content.Position.PropertyHeight.PropertyChanged -= OnContentHeightChanged;
        }

        public void ScrollToTop()
        {
            _scrollBar.Value = 0;
        }

        public void ScrollToBottom()
        {
            _scrollBar.Value = _scrollBar.MaxValue;
        }
    }
}
