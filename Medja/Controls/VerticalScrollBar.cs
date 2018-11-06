
using System;

namespace Medja.Controls
{
    public class VerticalScrollBar : ProgressBar
    {
        public VerticalScrollBar()
        {
            InputState.OwnsMouseEvents = true;
            InputState.PropertyIsLeftMouseDown.PropertyChanged += OnIsLeftMouseDownChanged;
            InputState.MouseDragged += OnMouseDragged;
        }

        private void OnIsLeftMouseDownChanged(object sender, PropertyChangedEventArgs e)
        {
            var isDown = e.NewValue as bool?;

            if (isDown == true)
                Value = YToValue(InputState.PointerPosition.Y); 
        }
        
        private void OnMouseDragged(object sender, MouseDraggedEventArgs e)
        {
            Value = YToValue(InputState.PointerPosition.Y);
        }

        private float YToValue(float y)
        {
            var relativeY = y - Position.Y;
            var percentage = relativeY / Position.Height;

            var result = percentage * MaxValue;
            return result;
        }
    }
}