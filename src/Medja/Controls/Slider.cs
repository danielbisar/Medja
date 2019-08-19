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
        private Action<Point> ApplyMousePos;

        [NonSerialized]
        public readonly Property<float> PropertyMinValue;
        public float MinValue
        {
            get { return PropertyMinValue.Get(); }
            set { PropertyMinValue.Set(value); }
        }

        [NonSerialized]
        public readonly Property<float> PropertyMaxValue;
        public float MaxValue
        {
            get { return PropertyMaxValue.Get(); }
            set { PropertyMaxValue.Set(value); }
        }

        [NonSerialized]
        public readonly Property<float> PropertyValue;
        public float Value
        {
            get { return PropertyValue.Get(); }
            set { PropertyValue.Set(value); }
        }

        [NonSerialized]
        public readonly Property<float> PropertyPercentage;
        public float Percentage
        {
            get { return PropertyPercentage.Get(); }
            private set { PropertyPercentage.Set(value); }
        }

        [NonSerialized]
        public readonly Property<Color> PropertyThumbColor;
        public Color ThumbColor
        {
            get { return PropertyThumbColor.Get(); }
            set { PropertyThumbColor.Set(value); }
        }

        [NonSerialized]
        public readonly Property<Orientation> PropertyOrientation;
        public Orientation Orientation
        {
            get { return PropertyOrientation.Get(); }
            set { PropertyOrientation.Set(value); }
        }

        [NonSerialized] 
        public readonly Property<bool> PropertyIsInverted;
        public bool IsInverted
        {
            get { return PropertyIsInverted.Get(); }
            set { PropertyIsInverted.Set(value); }
        }

        public Slider()
        {
            PropertyIsInverted = new Property<bool>();
            PropertyMinValue = new Property<float>();
            PropertyMaxValue = new Property<float>();
            PropertyOrientation = new Property<Orientation>();
            PropertyThumbColor = new Property<Color>();
            PropertyPercentage = new Property<float>();
            PropertyValue = new Property<float>();

            PropertyMinValue.PropertyChanged += OnPercentageRelevantPropertyChanged;
            PropertyMaxValue.PropertyChanged += OnPercentageRelevantPropertyChanged;
            PropertyValue.PropertyChanged += OnPercentageRelevantPropertyChanged;
            PropertyOrientation.PropertyChanged += OnOrientationChanged;
            
            InputState.Clicked += OnClicked;
            InputState.HandlesDrag = true;
            InputState.OwnsMouseEvents = true;
            InputState.Dragged += OnDragged;
            
            UpdateApplyMousePos();
        }

        private void OnOrientationChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateApplyMousePos();
        }

        private void UpdateApplyMousePos()
        {
            ApplyMousePos = Orientation == Orientation.Horizontal
                ? ApplyMousePosHorizontal
                : (Action<Point>) ApplyMousePosVertical;
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
            ApplyMousePos(e.Target);
        }

        protected virtual void OnClicked(object sender, EventArgs e)
        {
            ApplyMousePos(InputState.PointerPosition);
        }

        protected void ApplyMousePosHorizontal(Point mouse)
        {
            var width = Position.Width;
            var relativeX = mouse.X - Position.X; // relative to the control
            var mousePercentage = relativeX / width;
            
            if(IsInverted)
                mousePercentage = 1 - mousePercentage;
            
            var distance = MaxValue - MinValue;
            var newValue = distance * mousePercentage + MinValue;

            if (newValue < MinValue)
                newValue = MinValue;
            else if (newValue > MaxValue)
                newValue = MaxValue;

            Value = newValue;
        }

        protected void ApplyMousePosVertical(Point mouse)
        {
            var height = Position.Height;
            var mouseHeightPos = mouse.Y - Position.Y;
            var mousePercentage = mouseHeightPos / height;

            if (IsInverted)
                mousePercentage = 1 - mousePercentage;
            
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