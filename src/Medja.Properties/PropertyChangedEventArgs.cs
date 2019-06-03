using System;

namespace Medja.Properties
{
    public class PropertyChangedEventArgs : EventArgs
    {
        public object OldValue { get; }
        public object NewValue { get; }

        public PropertyChangedEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}