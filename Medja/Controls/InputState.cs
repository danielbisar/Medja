using System;
using Medja.Controls;
using Medja.Primitives;

namespace Medja
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
		private Point _mouseDownPointerPosition;

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
		/// Indicates whether this controls handles Drag&Drop or just ignores it.
		/// </summary>
		public bool HandlesDrag
		{
			get { return PropertyHandlesDrag.Get(); }
			set { PropertyHandlesDrag.Set(value); }
		}

		public event EventHandler MouseClicked;
		public event EventHandler<MouseDraggedEventArgs> MouseDragged;
		public event EventHandler<KeyboardEventArgs> KeyPressed;

		public InputState(Control control)
		{
			Control = control;
			PropertyIsLeftMouseDown = new Property<bool>();
			PropertyIsLeftMouseDown.PropertyChanged += OnIsLeftMouseDownChanged;

			PropertyIsMouseOver = new Property<bool>();
			PropertyIsDrag = new Property<bool>();
			PropertyPointerPosition = new Property<Point>();
			PropertyPointerPosition.PropertyChanged += OnPointerPositionChanged;
			PropertyMouseWheelDelta = new Property<float>();
			PropertyOwnsMouseEvents = new Property<bool>();
			PropertyHandlesDrag = new Property<bool>();

			_dragThreshold = 5;
		}

		public void NotifyKeyPressed(char key)
		{
			KeyPressed?.Invoke(this, new KeyboardEventArgs(key));
		}

		private void OnPointerPositionChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			if (IsLeftMouseDown 
				&& MedjaMath.Distance(_mouseDownPointerPosition, PointerPosition) > _dragThreshold)
				IsDrag = true;
			else
				IsDrag = false;

			if (IsDrag)
				NotifyMouseDragged();
		}

		private void NotifyMouseDragged()
		{
			if (MouseDragged != null)
				MouseDragged(this, new MouseDraggedEventArgs(_mouseDownPointerPosition, PointerPosition));
		}

		private void OnIsLeftMouseDownChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			if (IsLeftMouseDown)
				_mouseDownPointerPosition = PointerPosition;

			if (!_isClearing && !IsLeftMouseDown && !IsDrag)
				NotifyClicked();
		}

		private void NotifyClicked()
		{
			if (MouseClicked != null)
				MouseClicked(this, EventArgs.Empty);
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
	}
}
