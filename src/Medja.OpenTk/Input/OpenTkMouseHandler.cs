using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Medja.Controls;
using Medja.OpenTk.Utils;
using Medja.Utils.Math;
using OpenTK;
using OpenTK.Input;

namespace Medja.OpenTk
{
	public class OpenTkMouseHandler
	{
		private static MouseState GetInputState(MouseWheelEventArgs e)
		{
			return new MouseState
			{
					Position = e.Position,
					IsLeftButtonDown = e.Mouse.LeftButton == ButtonState.Pressed,
					WheelDelta = e.DeltaPrecise
			};
		}
		
		private static bool IsMouseOver(Control control, Point pos)
		{
			var childPos = control.Position;
			
			return pos.X >= childPos.X
					&& pos.Y >= childPos.Y
					&& pos.X <= childPos.X + childPos.Width
					&& pos.Y <= childPos.Y + childPos.Height;
		}
		
		private static MouseState GetInputState(MouseEventArgs e)
		{
			return new MouseState
			{
				Position = e.Position,
				IsLeftButtonDown = e.Mouse.LeftButton == ButtonState.Pressed,
				IsMouseMove = true
			};
		}
		
		private static MouseState GetInputState(MouseButtonEventArgs e)
		{
			return new MouseState
			{
				Position = e.Position,
				IsLeftButtonDown = e.Mouse.LeftButton == ButtonState.Pressed,
				IsMouseMove = false
			};
		}
		
		private readonly MedjaWindow _medjaWindow;
		private readonly GameWindow _window;
		private readonly FocusManager _focusManager;
		private Control _currentDragControl;
		
		public List<Control> Controls { get; set; }

		public OpenTkMouseHandler(MedjaWindow medjaWindow, GameWindow window, FocusManager focusManager)
		{
			_medjaWindow = medjaWindow;
			_window = window;
			_focusManager = focusManager;

			_window.MouseDown += OnMouseDown;
			_window.MouseMove += OnMouseMove;
			_window.MouseUp += OnMouseUp;
			_window.MouseWheel += OnMouseWheel;
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			ApplyMouseToControls(GetInputState(e));
		}
		
		private void OnMouseMove(object sender, MouseMoveEventArgs e)
		{
			ApplyMouseToControls(GetInputState(e));
		}

		private void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			ApplyMouseToControls(GetInputState(e));
		}

		private void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ApplyMouseToControls(GetInputState(e));
		}

		private void ApplyMouseToControls(MouseState e)
		{
			if (Controls == null)
				return;
			
			var position = e.Position;

			if (e.IsLeftButtonDown && _currentDragControl != null)
			{
				ApplyMouse(_currentDragControl, e);
				
				foreach(var control in Controls.Where(p => p != _currentDragControl))
					control.InputState.Clear();
			}
			else
			{
				if (!e.IsLeftButtonDown)
					_currentDragControl = null;
				
				// controls are in z-Order from back to front, so we go through them in reverse order
				for (int i = Controls.Count - 1; i >= 0; i--)
				{
					var control = Controls[i];

					if (control.IsEnabled && control.IsVisible && IsMouseOver(control, position))
					{
						ApplyMouse(control, e);

						if (_currentDragControl != null || control.InputState.OwnsMouseEvents && !control.InputState.IsDrag)
							break;
					}
					else
						control.InputState.Clear();
				}
			}
		}

		private void ApplyMouse(Control control, MouseState mouseState)
		{
			var inputState = control.InputState;

			// we are going to receive a clicked event
			if (inputState.IsLeftMouseDown && !mouseState.IsLeftButtonDown)
			{
				_focusManager.SetFocus(control);
			}

			// order is important
			inputState.PointerPosition = mouseState.Position.ToMedjaPoint();
			inputState.IsMouseOver = true;
			inputState.IsLeftMouseDown = mouseState.IsLeftButtonDown;

			// compare with 0, because only if the value is exactly 0 we want to send the event
			if (mouseState.WheelDelta != 0
				&& inputState.MouseWheelDelta.AboutEquals(mouseState.WheelDelta))
				inputState.PropertyMouseWheelDelta.NotifyPropertyChanged();
			else
				inputState.MouseWheelDelta = mouseState.WheelDelta;

			if (inputState.IsDrag && inputState.HandlesDrag)
				_currentDragControl = control;
		}

		
	}
}
