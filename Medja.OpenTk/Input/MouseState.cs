using System.Drawing;

namespace Medja.OpenTk
{
    internal class MouseState
    {
        public Point Position { get; set; }
        public bool IsLeftButtonDown { get; set; }
        public bool IsMouseMove { get; set; }
    }
}
