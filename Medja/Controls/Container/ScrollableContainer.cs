using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class ScrollableContainer : ContentControl
    {
        public readonly float ScrollBarWidth;
        
        private readonly VerticalScrollBar _scrollBar;

        public ScrollableContainer(ControlFactory controlFactory)
        {
            _scrollBar = controlFactory.Create<VerticalScrollBar>();
            ScrollBarWidth = _scrollBar.Position.Width;
            InputState.PropertyMouseWheelDelta.PropertyChanged += OnMouseWheelMoved;
            InputState.MouseDragged += OnMouseDragged;
            InputState.PropertyIsLeftMouseDown.PropertyChanged += OnLeftMouseUp;
        }

        private void OnLeftMouseUp(object sender, PropertyChangedEventArgs e)
        {
            _startDragPos = null;
        }

        private float? _startDragPos;
        
        protected virtual void OnMouseDragged(object sender, MouseDraggedEventArgs e)
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
            IsLayoutUpdated = false;
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
            ContentArranger.Stretch(area);

            if (Content != null)
            {
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