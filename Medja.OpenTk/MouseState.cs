using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace Medja.OpenTk
{
    internal class MouseState
    {
        public Point Position { get; set; }
        public bool IsLeftButtonDown { get; set; }
        public bool IsMouseMove { get; set; }
    }
}
