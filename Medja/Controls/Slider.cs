using System;

namespace Medja.Controls
{
    /// <summary>
    /// A slider control.
    /// </summary>
    public class Slider : Control
    {
        public readonly Property<float> PropertyMinValue;
        public float MinValue
        {
            get { return PropertyMinValue.Get(); }
            set { PropertyMinValue.Set(value); }
        }

        public readonly Property<float> PropertyMaxValue;
        public float MaxValue
        {
            get { return PropertyMaxValue.Get(); }
            set { PropertyMaxValue.Set(value); }
        }

        public readonly Property<float> PropertyValue;
        public float Value
        {
            get { return PropertyValue.Get(); }
            set { PropertyValue.Set(value); }
        }

        public Slider()
        {
            PropertyMinValue = new Property<float>();
            PropertyMaxValue = new Property<float>();
            PropertyValue = new Property<float>();

            InputState.MouseClicked += OnMouseClicked;
            InputState.HandlesDrag = true;
            InputState.OwnsMouseEvents = true;
            InputState.MouseDragged += OnMouseDragged;
        }

        protected virtual void OnMouseDragged(object sender, MouseDraggedEventArgs e)
        {
            ApplyMousePos(e.Target.X);
        }

        protected virtual void OnMouseClicked(object sender, EventArgs e)
        {
            ApplyMousePos(InputState.PointerPosition.X);
        }

        protected virtual void ApplyMousePos(float x)
        {
            var width = Position.Width;
            var mouseWidthPos = x - Position.X;
            var mousePercentage = mouseWidthPos / width;
            var distance = MaxValue - MinValue;
            var newValue = distance * mousePercentage + MinValue;

            if (newValue < MinValue)
                newValue = MinValue;
            else if (newValue > MaxValue)
                newValue = MaxValue;

            Value = newValue;
        }
    }
}