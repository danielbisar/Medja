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
		private double _dragThreshold;
		private Point _mouseDownPointerPosition;

		/// <summary>
		/// Gets the control this InputState belongs to.
		/// </summary>
		/// <value>The control.</value>
		public Control Control { get; }

		public Property<bool> IsMouseOverProperty { get; }
		public bool IsMouseOver
		{
			get { return IsMouseOverProperty.Get(); }
			set { IsMouseOverProperty.Set(value); }
		}

		public Property<bool> IsLeftMouseDownProperty { get; }
		public bool IsLeftMouseDown
		{
			get { return IsLeftMouseDownProperty.Get(); }
			set { IsLeftMouseDownProperty.Set(value); }
		}

		public Property<bool> IsDragProperty { get; }
		public bool IsDrag
		{
			get { return IsDragProperty.Get(); }
			set { IsDragProperty.Set(value); }
		}

		public Property<Point> PointerPositionProperty { get; }
		public Point PointerPosition
		{
			get { return PointerPositionProperty.Get(); }
			set { PointerPositionProperty.Set(value); }
		}

		public Property<float> MouseWheelDeltaProperty { get; }
		public float MouseWheelDelta
		{
			get { return MouseWheelDeltaProperty.Get(); }
			set { MouseWheelDeltaProperty.Set(value); }
		}

		public event EventHandler MouseClicked;
		public event EventHandler<MouseDraggedEventArgs> MouseDragged;

		public InputState(Control control)
		{
			Control = control;
			IsLeftMouseDownProperty = new Property<bool>();
			IsLeftMouseDownProperty.PropertyChanged += OnIsLeftMouseDownChanged;

			IsMouseOverProperty = new Property<bool>();
			IsDragProperty = new Property<bool>();
			PointerPositionProperty = new Property<Point>();
			PointerPositionProperty.PropertyChanged += OnPointerPositionChanged;
			MouseWheelDeltaProperty = new Property<float>();

			_dragThreshold = 5;
		}

		private void OnPointerPositionChanged(IProperty property)
		{
			if (IsLeftMouseDown && MedjaMath.Distance(_mouseDownPointerPosition, PointerPosition) > _dragThreshold)
				IsDrag = true;

			if (IsDrag)
				NotifyMouseDragged();
		}

		private void NotifyMouseDragged()
		{
			if (MouseDragged != null)
				MouseDragged(this, new MouseDraggedEventArgs(_mouseDownPointerPosition, PointerPosition));
		}

		private void OnIsLeftMouseDownChanged(IProperty property)
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
