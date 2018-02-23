using System;
using Medja.Controls;
using OpenTK;

namespace Medja.OpenTk
{
    public class OpenTkWindow : MedjaWindow
    {
        public GameWindow GameWindow { get; }

        public OpenTkWindow()
        {
            GameWindow = new GameWindow();

            GameWindow.Resize += OnGameWindowResize;

            PropertyTitle.PropertyChanged += p => GameWindow.Title = Title;
            Position.PropertyX.PropertyChanged += p => GameWindow.X = (int)Position.X;
            Position.PropertyY.PropertyChanged += p => GameWindow.Y = (int)Position.Y;
            Position.PropertyWidth.PropertyChanged += p => GameWindow.Width = (int)Position.Width;
            Position.PropertyHeight.PropertyChanged += p => GameWindow.Height = (int)Position.Height;
        }

        private void OnGameWindowResize(object sender, EventArgs eventArgs)
        {
            Position.PropertyWidth.UnnotifiedSet(GameWindow.ClientRectangle.Width);
            Position.PropertyHeight.UnnotifiedSet(GameWindow.ClientRectangle.Height);
        }

        public override void Close()
        {
            GameWindow.Close();
            base.Close();
        }
    }
}
