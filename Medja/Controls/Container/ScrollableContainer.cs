using System;
using System.Collections.Generic;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class ScrollableContainer : ContentControl
    {
        public readonly float ScrollBarWidth;
        private readonly VerticalScrollBar _scrollBar;
        
        private float? _startDragPos;

        public ScrollableContainer(IControlFactory controlFactory)
        {
            _scrollBar = controlFactory.Create<VerticalScrollBar>();
            _scrollBar.PropertyValue.AffectsLayout(this);
            
            ScrollBarWidth = _scrollBar.Position.Width;
            
            InputState.Dragged += OnDragged;
            InputState.PropertyMouseWheelDelta.PropertyChanged += OnMouseWheelMoved;
            InputState.PropertyIsLeftMouseDown.PropertyChanged += OnLeftMouseUp;
            InputState.HandlesDrag = true;
            InputState.OwnsMouseEvents = true;
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

            _scrollBar.Position.X = area.X + area.Width - ScrollBarWidth;
            _scrollBar.Position.Y = area.Y;
            _scrollBar.Position.Height = area.Height;
            
            area.Width -= ScrollBarWidth;
            area.Y -= _scrollBar.Value;
			
            ContentArranger.Position(area);
            ContentArranger.StretchWidth(area);

            if (Content != null)
            {
                Content.ClippingArea.X = area.X;
                Content.ClippingArea.Y = Position.Y + Margin.Top + Padding.Top;
                Content.ClippingArea.Height = area.Height;
                Content.ClippingArea.Width = area.Width;
                
                _scrollBar.MaxValue = Math.Max(area.Height, Content.Position.Height - area.Height);
               
                if (_scrollBar.Value > _scrollBar.MaxValue)
                    _scrollBar.Value = _scrollBar.MaxValue;
            }
        }

        public override IEnumerable<Control> GetChildren()
        {
            if (Content != null)
                yield return Content;

            yield return _scrollBar;
        }
    }
}