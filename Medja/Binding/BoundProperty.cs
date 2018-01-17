using System;
using Medja.Binding.PropertyAccessors;

namespace Medja.Binding
{
    public class BoundProperty
    {
        public Type Type { get; }
        public string PropertyName { get; }
        public IValue<PropertyAccessor> Accessors { get; }

        public BoundProperty(Type type, string propertyName)
        {
            Type = type;
            PropertyName = propertyName;
            Accessors = BindingLibrary.ValueFactory.Get(GetAccessor);
        }

        private PropertyAccessor GetAccessor()
        {
            return BindingLibrary.PropertyAccessorFactory.Get(Type, PropertyName);
        }
    }
}
