using System.Collections.Generic;

namespace Medja
{
    /// <summary>
    /// Can store a properties value so you can restore it later.
    /// </summary>
    public class PropertyValueStorage<T>
    {
        private readonly T _oldValue;
        private readonly Property<T> _property;
        
        public PropertyValueStorage(Property<T> property)
        {
            _property = property;
            _oldValue = property.Get();
        }

        public void Restore()
        {
            _property.Set(_oldValue);
        }
    }
}