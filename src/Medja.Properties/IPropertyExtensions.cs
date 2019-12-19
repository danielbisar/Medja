using System;

namespace Medja.Properties
{
    public static class IPropertyExtensions
    {
        /// <summary>
        /// Gets the value type of a property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The value type of the property.</returns>
        /// <exception cref="InvalidOperationException">If the given property has no or more than one generic type
        /// argument.</exception>
        public static Type GetValueType(this IProperty property)
        {
            var propertyType = property.GetType();

            if(!propertyType.IsGenericType)
                throw new InvalidOperationException(nameof(property) 
                                                    + " is not a generic type, cannot read value type");
            
            if(propertyType.GenericTypeArguments.Length != 1)
                throw new InvalidOperationException(nameof(property) + " has != 1 generic argument");
            
            var typeArgument = propertyType.GenericTypeArguments[0];

            return typeArgument;
        }

        /// <summary>
        /// Uses reflection to get the value of a property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="InvalidOperationException">If no public Get method could be found.</exception>
        public static object GetValue(this IProperty property)
        {
            var type = property.GetType();
            var method = type.GetMethod("Get");

            if (method == null)
                throw new InvalidOperationException("Get method not found on property of type " + type.FullName);

            return method.Invoke(property, null);
        }

        /// <summary>
        /// Uses reflection to get the value of a property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="InvalidOperationException">If no public Get method could be found.</exception>
        public static T GetValue<T>(this IProperty property)
        {
            return (T) property.GetValue();
        }

        /// <summary>
        /// Sets the value of a property via reflection.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="InvalidOperationException">If no public Set method could be found.</exception>
        public static void SetValue(this IProperty property, object value)
        {
            var type = property.GetType();
            var method = type.GetMethod("Set");

            if (method == null)
                throw new InvalidOperationException("Set method not found on property of type " + type.FullName);

            method.Invoke(property, new[] {value});
        }
    }
}