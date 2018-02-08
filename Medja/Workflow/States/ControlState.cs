using Medja.Controls;
using Medja.Primitives;

namespace Medja
{
    /// <summary>
    /// Contains the state of a control (incl. position, text settings, color settings, styles, ...)
    /// </summary>
    public class ControlState
    {
        public Control Control { get; set; }

        public Position Position { get; set; }

        public InputState InputState { get; set; }

        public ControlState()
        {
            Position = new Position();
        }
    }
}
