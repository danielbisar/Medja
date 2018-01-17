using System;
using System.Reflection;

namespace Medja.Binding.PropertyAccessors
{
    public abstract class PropertyAccessorFactoryBase : IPropertyAccessorFactory
    {
        public PropertyAccessor Get(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);

            Func<object, object> getter = null;

            if (propertyInfo.CanRead)
                getter = CreateGetter(type, propertyInfo);

            Action<object, object> setter = null;

            if (propertyInfo.CanWrite)
                setter = CreateSetter(type, propertyInfo);

            return new PropertyAccessor(getter, setter);
        }

        protected abstract Func<object, object> CreateGetter(Type type, PropertyInfo propertyInfo);
        protected abstract Action<object, object> CreateSetter(Type type, PropertyInfo propertyInfo);
    }
}
