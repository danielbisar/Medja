using System;

namespace Medja.Binding.PropertyAccessors
{
    public class PropertyAccessor
    {
        private readonly Func<object, object> _getter;
        private readonly Action<object, object> _setter;

        public PropertyAccessor(Func<object, object> getter, Action<object, object> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        public void Set(object target, object value)
        {
            _setter(target, value);
        }

        public object Get(object target)
        {
            return _getter(target);
        }
    }
}
