using System;
using Medja.Controls;
using Medja.Primitives;
using Medja.Properties;
using Medja.Utils;

namespace Medja.Input
{
    public class InputState
    {
        // will be true if Clear was called to notify the control that 
        // it has no mouse input.
        private bool _isClearing;

        /// <summary>
        /// Defines the threshold in pixel that is used for a drag operation (the min. distance the mouse must move to indicate a drag)
        /// </summary>
        private readonly double _dragThreshold;
        private Point _pointerDownPosition;

        /// <summary>
        /// Gets the control this InputState belongs to.
        /// </summary>
        /// <value>The control.</value>
        public Control Control { get; }

        public Property<bool> PropertyIsMouseOver { get; }
        public bool IsMouseOver
        {
            get { return PropertyIsMouseOver.Get(); }
            set { PropertyIsMouseOver.Set(value); }
        }

        public Property<bool> PropertyIsLeftMouseDown { get; }
        public bool IsLeftMouseDown
        {
            get { return PropertyIsLeftMouseDown.Get(); }
            set { PropertyIsLeftMouseDown.Set(value); }
        }

        public Property<bool> PropertyIsDrag { get; }
        public bool IsDrag
        {
            get { return PropertyIsDrag.Get(); }
            set { PropertyIsDrag.Set(value); }
        }

        public Property<Point> PropertyPointerPosition { get; }
        /// <summary>
        /// Position of the mouse pointer relative to the containing window.
        /// </summary>
        public Point PointerPosition
        {
            get { return PropertyPointerPosition.Get(); }
            set { PropertyPointerPosition.Set(value); }
        }

        public Property<float> PropertyMouseWheelDelta { get; }
        public float MouseWheelDelta
        {
            get { return PropertyMouseWheelDelta.Get(); }
            set { PropertyMouseWheelDelta.Set(value); }
        }
        
        public readonly Property<bool> PropertyOwnsMouseEvents;
        /// <summary>
        /// Indicates that this control does not allow controls behind it to handle mouse events.
        /// </summary>
        public bool OwnsMouseEvents
        {
            get { return PropertyOwnsMouseEvents.Get(); }
            set { PropertyOwnsMouseEvents.Set(value); }
        }
        
        public readonly Property<bool> PropertyHandlesDrag;
        /// <summary>
        /// Indicates whether this controls handles Drag&amp;Drop or just ignores it.
        /// </summary>
        public bool HandlesDrag
        {
            get { return PropertyHandlesDrag.Get(); }
            set { PropertyHandlesDrag.Set(value); }
        }

        /// <summary>
        /// A touch or mouse device clicked the control.
        /// </summary>
        public event EventHandler Clicked;
        
        /// <summary>
        /// A touch or mouse device dragged inside the control.
        /// </summary>
        public event EventHandler<MouseDraggedEventArgs> Dragged;
        public event EventHandler<KeyboardEventArgs> KeyPressed;

        public InputState(Control control)
        {
            Control = control;

            PropertyHandlesDrag = new Property<bool>();
            PropertyIsDrag = new Property<bool>();

            PropertyIsLeftMouseDown = new Property<bool>();
            PropertyIsLeftMouseDown.PropertyChanged += OnIsLeftMouseDownChanged;

            PropertyIsMouseOver = new Property<bool>();
            PropertyMouseWheelDelta = new Property<float>();
            PropertyOwnsMouseEvents = new Property<bool>();
            PropertyPointerPosition = new Property<Point>();
            PropertyPointerPosition.PropertyChanged += OnPointerPositionChanged;

            _dragThreshold = 5;
        }

        public void SendKeyPress(KeyboardEventArgs keyboardEventArgs)
        {
            KeyPressed?.Invoke(this, keyboardEventArgs);
        }

        private void OnPointerPositionChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (IsLeftMouseDown 
                && _pointerDownPosition != null
                && _pointerDownPosition.Distance(PointerPosition) > _dragThreshold)
                IsDrag = true;
            else
                IsDrag = false;

            if (IsDrag)
                NotifyMouseDragged();
        }

        private void NotifyMouseDragged()
        {
            if (Dragged != null)
                Dragged(this, new MouseDraggedEventArgs(_pointerDownPosition, PointerPosition));
        }

        private void OnIsLeftMouseDownChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (IsLeftMouseDown)
                _pointerDownPosition = PointerPosition;

            if (!_isClearing && !IsLeftMouseDown && !IsDrag)
                NotifyClicked();
        }

        private void NotifyClicked()
        {
            if (Clicked != null)
                Clicked(this, EventArgs.Empty);
        }

        public void Clear()
        {
            _isClearing = true;

            IsDrag = false;
            IsLeftMouseDown = false;
            IsMouseOver = false;
            MouseWheelDelta = 0;

            _isClearing = false;
        }

        public override string ToString()
        {
            return "InputState: { " 
                   + nameof(HandlesDrag) + ": " + HandlesDrag + ", "
                   + nameof(IsDrag) + ": " + IsDrag + ", "
                   + nameof(IsLeftMouseDown) + ": " + IsLeftMouseDown + ", "
                   + nameof(IsMouseOver) + ": " + IsMouseOver + ", "
                   + nameof(MouseWheelDelta) + ": " + MouseWheelDelta + ", "
                   + nameof(OwnsMouseEvents) + ": " + OwnsMouseEvents + ", "
                   + nameof(PointerPosition) + ": " + PointerPosition + ", "
                   + " }";
        }

        public void SendClick(Point point = null)
        {
            PointerPosition = point ?? new Point(0, 0);
            NotifyClicked();
        }
    }
}
