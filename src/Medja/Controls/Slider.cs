using System;
using Medja.Primitives;
using Medja.Properties;

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

        public readonly Property<float> PropertyPercentage;
        public float Percentage
        {
            get { return PropertyPercentage.Get(); }
            private set { PropertyPercentage.Set(value); }
        }
        
        public readonly Property<Color> PropertyThumbColor;
        public Color ThumbColor
        {
            get { return PropertyThumbColor.Get(); }
            set { PropertyThumbColor.Set(value); }
        }

        public Slider()
        {
            PropertyMinValue = new Property<float>();
            PropertyMaxValue = new Property<float>();
            PropertyValue = new Property<float>();
            PropertyThumbColor = new Property<Color>();
            PropertyPercentage = new Property<float>();

            PropertyMinValue.PropertyChanged += OnPercentageRelevantPropertyChanged;
            PropertyMaxValue.PropertyChanged += OnPercentageRelevantPropertyChanged;
            PropertyValue.PropertyChanged += OnPercentageRelevantPropertyChanged;
            
            InputState.Clicked += OnClicked;
            InputState.HandlesDrag = true;
            InputState.OwnsMouseEvents = true;
            InputState.Dragged += OnDragged;
        }

        private void OnPercentageRelevantPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePercentage();
        }

        private void UpdatePercentage()
        {
            var distance = MaxValue - MinValue;
            var value = Value - MinValue;
            PropertyPercentage.Set(distance == 0 ? 0 : value / distance);
        }

        protected virtual void OnDragged(object sender, MouseDraggedEventArgs e)
        {
            ApplyMousePos(e.Target.X);
        }

        protected virtual void OnClicked(object sender, EventArgs e)
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