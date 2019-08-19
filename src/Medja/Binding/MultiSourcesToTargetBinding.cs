using System;
using System.Collections.Generic;
using Medja.Properties;

namespace Medja.Binding
{
    public class MultiSourcesToTargetBinding<TTargetObject, TTargetProperty> 
        : IDisposable
    {
        private readonly TTargetObject _targetObject;
        private readonly Property<TTargetProperty> _targetProperty;
        private readonly IList<IProperty> _sourceProperties;
        private Func<TTargetObject, TTargetProperty> _getTargetPropertyValue;
        
        public MultiSourcesToTargetBinding(
            TTargetObject targetObject, 
            Property<TTargetProperty> targetProperty,
            Func<TTargetObject, TTargetProperty> getTargetValue)
        {
            _sourceProperties = new List<IProperty>();
            _targetObject = targetObject;
            _targetProperty = targetProperty ?? throw new ArgumentNullException(nameof(targetProperty));
            _getTargetPropertyValue =
                getTargetValue ?? throw new ArgumentNullException(nameof(getTargetValue));
        }

        public void AddSource(IProperty property)
        {
            _sourceProperties.Add(property);
            property.PropertyChanged += OnSourcePropertyChanged;
        }

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _targetProperty.Set(_getTargetPropertyValue(_targetObject));
        }

        public void Dispose()
        {
            foreach (var sourceProperty in _sourceProperties)
                sourceProperty.PropertyChanged -= OnSourcePropertyChanged;
            
            _sourceProperties.Clear();
            _getTargetPropertyValue = null;
        }
    }
}