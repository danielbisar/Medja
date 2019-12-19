using System;
using System.Collections.Generic;
using Medja.Utils.Collections.Generic;

namespace Medja.Properties
{
    /// <summary>
    /// A list of properties that can be attached to an object.
    /// </summary>
    /// <remarks>
    /// Later the base class might change to some kind of ObservableDictionary if required.
    /// </remarks>
    public class AttachedProperties : Dictionary<int, IProperty>
    {
        /// <summary>
        /// Sets the value of an attached property.
        /// </summary>
        /// <param name="id">The attached property id.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The property object.</returns>
        /// <typeparam name="T">The value type.</typeparam>
        /// <remarks>If the property did not exist a new <see cref="Property{T}"/> will be added.</remarks>
        public IProperty Set<T>(int id, T value)
        {
            var property = GetOrAddProperty<T>(id);
            property.SetValue(value);

            return property;
        }

        /// <summary>
        /// Gets the value of an attached property.
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <typeparam name="T">The value type.</typeparam>
        /// <returns>The value of the property.</returns>
        /// <exception cref="KeyNotFoundException">If the property did not exist.</exception>
        public T Get<T>(int id)
        {
            return (T)this[id].GetValue();
        }

        /// <summary>
        /// Gets the value or adds a new one.
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <param name="defaultValue">The default value if the property didn't exist.</param>
        /// <typeparam name="T">The value type.</typeparam>
        /// <returns>The value.</returns>
        public T GetOrAdd<T>(int id, T defaultValue)
        {
            var property = GetOrAddProperty<T>(id, i => CreateProperty(defaultValue));
            return property.GetValue<T>();
        }

        private IProperty CreateProperty<T>(T defaultValue)
        {
            var result = new Property<T>();
            result.SetSilent(defaultValue);

            return result;
        }

        /// <summary>
        /// Gets or adds the property with the given id.
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <param name="factory">The factory method. If null a new <see cref="Property{T}"/> will be created if needed.</param>
        /// <typeparam name="T">The value type of the property.</typeparam>
        /// <returns>The property object.</returns>
        public IProperty GetOrAddProperty<T>(int id, Func<int, IProperty> factory = null)
        {
            if (factory == null)
                factory = i => new Property<T>();
            
            return IDictionaryExtensions.GetOrAdd(this, id, factory);
        }

        /// <summary>
        /// Gets or adds a property. Sets default value if not existing.
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <param name="defaultValue">The default value to set if the property does not exist.</param>
        /// <typeparam name="T">The value type.</typeparam>
        /// <returns>The property.</returns>
        public IProperty GetOrAddProperty<T>(int id, T defaultValue)
        {
            return GetOrAddProperty<T>(id, i => CreateProperty(defaultValue));
        }
    }
}