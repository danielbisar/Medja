using System.Collections.Generic;
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

        public Dictionary<int, object> AttachedProperties { get; set; }

        public ControlState()
        {
            Position = new Position();
            AttachedProperties = new Dictionary<int, object>();
            InputState = new InputState();
        }

        public void SetAttachedProperty(int id, object value)
        {
            AttachedProperties[id] = value;
        }

        public object GetAttachedProperty(int id)
        {
            if (AttachedProperties.TryGetValue(id, out var result))
                return result;

            return null;
        }
    }
}
