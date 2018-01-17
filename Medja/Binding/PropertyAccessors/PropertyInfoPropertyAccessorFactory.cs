using System;
using System.Reflection;

namespace Medja.Binding.PropertyAccessors
{
    /// <summary>
    /// Very naiive implementation. No caching, no dynamic compilation, no lazy loading of getter/setter.
    /// </summary>
    public class PropertyInfoPropertyAccessorFactory : PropertyAccessorFactoryBase
    {
        protected override Func<object, object> CreateGetter(Type type, PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue;
        }

        protected override Action<object, object> CreateSetter(Type type, PropertyInfo propertyInfo)
        {
            return propertyInfo.SetValue;
        }
    }
}
