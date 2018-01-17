using System;

namespace Medja.Binding.PropertyAccessors
{
    public interface IPropertyAccessorFactory
    {
        PropertyAccessor Get(Type type, string propertyName);
    }
}