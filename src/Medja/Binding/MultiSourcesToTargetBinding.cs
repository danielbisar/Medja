using System;
using System.Collections.Generic;
using Medja.Properties;

namespace Medja
{
    /// <summary>
    /// Allows you to react on changes of multiple properties and generate a target value.
    /// </summary>
    /// <typeparam name="TTargetObject">The type of the object containing the target property.</typeparam>
    /// <typeparam name="TTargetProperty">The type of the target property.</typeparam>
    public class MultiSourcesToTargetBinding<TTargetObject, TTargetProperty> 
        : IBinding
    {
        private readonly TTargetObject _targetObject;
        private readonly Property<TTargetProperty> _targetProperty;
        private readonly List<IProperty> _sourceProperties;
        private Func<TTargetObject, TTargetProperty> _getTargetPropertyValue;
        
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="targetObject">The target object (will be used as parameter for <see cref="getTargetValue"/>).
        /// </param>
        /// <param name="targetProperty">The target property.</param>
        /// <param name="getTargetValue">The function that creates the target value based on the target object.</param>
        public MultiSourcesToTargetBinding(
            TTargetObject targetObject, 
            Property<TTargetProperty> targetProperty,
            Func<TTargetObject, TTargetProperty> getTargetValue)
        {
            _sourceProperties = new List<IProperty>();
            _targetObject = targetObject;
            _targetProperty = targetProperty ?? throw new ArgumentNullException(nameof(targetProperty));
            _getTargetPropertyValue = getTargetValue 
                                      ?? throw new ArgumentNullException(nameof(getTargetValue));
        }

        /// <summary>
        /// Adds a source property.
        /// </summary>
        /// <param name="property">The source property.</param>
        /// <remarks>
        /// Listens to changes of that property and calls getTargetValue
        /// (<see cref="MultiSourcesToTargetBinding{TTargetObject,TTargetProperty}"/>).
        /// </remarks>
        public void AddSource(IProperty property)
        {
            _sourceProperties.Add(property);
            property.PropertyChanged += OnSourcePropertyChanged;
        }

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _targetProperty.Set(_getTargetPropertyValue(_targetObject));
        }

        /// <summary>
        /// Clears all references and event handlers.
        /// </summary>
        public void Dispose()
        {
            foreach (var sourceProperty in _sourceProperties)
                sourceProperty.PropertyChanged -= OnSourcePropertyChanged;
            
            _sourceProperties.Clear();
            _sourceProperties.TrimExcess();
            _getTargetPropertyValue = null;
        }
    }
}
