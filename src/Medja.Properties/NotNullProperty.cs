using System;

namespace Medja.Properties
{
    /// <summary>
    /// A void safe property (is never allowed to be null)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotNullProperty<T> : Property<T>
    {
        public NotNullProperty(T value)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
        }

        public override void SetSilent(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            base.SetSilent(value);
        }
    }
}