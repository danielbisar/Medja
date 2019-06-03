
using Medja.Properties;

namespace Medja.Controls
{
    public class VerticalScrollBar : ProgressBar
    {
        public VerticalScrollBar()
        {
            InputState.Dragged += OnDragged;
            InputState.PropertyIsLeftMouseDown.PropertyChanged += OnIsLeftMouseDownChanged;
            InputState.HandlesDrag = true;
            InputState.OwnsMouseEvents = true;
        }

        private void OnIsLeftMouseDownChanged(object sender, PropertyChangedEventArgs e)
        {
            var isDown = e.NewValue as bool?;

            if (isDown == true)
                Value = YToValue(InputState.PointerPosition.Y); 
        }
        
        private void OnDragged(object sender, MouseDraggedEventArgs e)
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