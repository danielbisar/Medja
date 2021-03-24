using System;

namespace Medja.Properties.ChangeTracking.Changes
{
    [Serializable]
    public class ValuePropertyChange : PropertyChange
    {
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

        public ValuePropertyChange(string name, object oldValue, object newValue)
        :base(name)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
