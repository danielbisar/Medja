using System;
using System.Drawing;
using System.Linq;
using Medja.Controls;
using OpenTK;
using OpenTK.Input;

namespace Medja.OpenTk
{
    public class OpenTkMouseHandler
    {
        private readonly MedjaWindow _medjaWindow;
        private readonly GameWindow _window;

        public OpenTkMouseHandler(MedjaWindow medjaWindow, GameWindow window)
        {
            _medjaWindow = medjaWindow;
            _window = window;

            _window.MouseDown += OnMouseDown;
            _window.MouseMove += OnMouseMove;
            _window.MouseUp += OnMouseUp;
        }
        
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ApplyMouseToControls(GetInputState(e));
        }        

        private void ApplyMouseToControls(MouseState e)
        {
            var position = e.Position;

            foreach (var control in _medjaWindow.GetAllControls().ToList())
            {
                if (IsMouseOver(control, position))
                    ApplyMouse(control, e);
                else
                    ClearInputState(control);
            }
        }

        private bool IsMouseOver(Control control, Point pos)
        {
            var childPos = control.Position;
            return pos.X >= childPos.X
                && pos.Y >= childPos.Y
                && pos.X <= (childPos.X + childPos.Width)
                && pos.Y <= (childPos.Y + childPos.Height);
        }

        private void ApplyMouse(Control control, MouseState mouseState)
        {
            var inputState = control.InputState;

            // order is important
            inputState.PointerPosition = ToMedjaPoint(mouseState.Position);
            inputState.IsMouseOver = true;
            inputState.IsLeftMouseDown = mouseState.IsLeftButtonDown;
        }

        private Medja.Primitives.Point ToMedjaPoint(Point position)
        {
            return new Primitives.Point(position.X, position.Y);
        }

        private void ClearInputState(Control control)
        {
            var inputState = control.InputState;
            inputState.Clear();
        }

        private MouseState GetInputState(MouseButtonEventArgs e)
        {
            return new MouseState
            {
                Position = e.Position,
                IsLeftButtonDown = e.Mouse.LeftButton == ButtonState.Pressed,
                IsMouseMove = false
            };
        }

        private void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            ApplyMouseToControls(GetInputState(e));
        }

        private MouseState GetInputState(MouseMoveEventArgs e)
        {
            return new MouseState
            {
                Position = e.Position,
                IsLeftButtonDown = e.Mouse.LeftButton == ButtonState.Pressed,
                IsMouseMove = true
            };
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ApplyMouseToControls(GetInputState(e));
        }
    }
}
