using System;
using System.Collections.Generic;

namespace Medja.Binding
{
    public class MultiSourcesToTargetBinding<T, T2> : IDisposable
    {
        private readonly T _targetObject;
        private readonly Property<T2> _targetProperty;
        private readonly IList<IProperty> _sourceProperties;
        private Func<T, T2> _getTargetPropertyValue;
        
        public MultiSourcesToTargetBinding(
            T control, 
            Property<T2> targetProperty,
            Func<T, T2> getPropertyValue)
        {
            _sourceProperties = new List<IProperty>();
            _targetObject = control;
            _targetProperty = targetProperty ?? throw new ArgumentNullException(nameof(targetProperty));
            _getTargetPropertyValue =
                getPropertyValue ?? throw new ArgumentNullException(nameof(getPropertyValue));
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