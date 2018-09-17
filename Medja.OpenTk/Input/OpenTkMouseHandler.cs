using System.Drawing;
using System.Linq;
using Medja.Controls;
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
					&& pos.X <= (childPos.X + childPos.Width)
					&& pos.Y <= (childPos.Y + childPos.Height);
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

		private void ApplyMouseToControls(MouseState e)
		{
			var position = e.Position;
			Control relevantControl = null;

			foreach (var control in _medjaWindow.GetAllControls().ToList())
			{
				if (control.IsEnabled
					&& control.IsVisible
					&& IsMouseOver(control, position))
				{
					relevantControl?.InputState.Clear();
					relevantControl = control;
				}
				else
					control.InputState.Clear();
			}

			// ApplyMouse only to the uppermost control
			if (relevantControl != null)
				ApplyMouse(relevantControl, e);
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
			inputState.PointerPosition = ToMedjaPoint(mouseState.Position);
			inputState.IsMouseOver = true;
			inputState.IsLeftMouseDown = mouseState.IsLeftButtonDown;

			if (mouseState.WheelDelta != 0
				&& MedjaMath.AboutEquals(inputState.MouseWheelDelta, mouseState.WheelDelta))
				inputState.PropertyMouseWheelDelta.NotifyPropertyChanged();
			else
				inputState.MouseWheelDelta = mouseState.WheelDelta;
		}

		private Medja.Primitives.Point ToMedjaPoint(Point position)
		{
			return new Primitives.Point(position.X, position.Y);
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
	}
}
