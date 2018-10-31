
namespace Medja.Controls
{
    public class VerticalScrollBar : ProgressBar
    {
        private float? _dragStartValue;
        
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
            {
                Value = YToValue(InputState.PointerPosition.Y);
            }
            else
                _dragStartValue = null;
        }
        
        private void OnMouseDragged(object sender, MouseDraggedEventArgs e)
        {
            if (_dragStartValue == null)
                _dragStartValue = Value;

            Value = _dragStartValue.Value + YToValue(e.Vector.Y);
        }

        private float YToValue(float y)
        {
            var relativeY = y - Position.Y;
            var percentage = relativeY / Position.Height;

            return percentage * MaxValue;
        }
    }
}