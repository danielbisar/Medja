using System;

namespace Medja
{
    /// <summary>
    /// Acts as a wrapper for .NET Properties to support Binding. Does not notify on change of the properties value.
    /// Better use the wrapper to set/get the value if you need change notification.
    /// </summary>
    /// <typeparam name="TSourceObject"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class PropertyWrapper<TSourceObject, TValue>
    {
        private readonly TSourceObject _source;
        private readonly Action<TSourceObject, TValue> _setValue;
        
        public readonly Property<TValue> PropertyValue;
        public TValue Value
        {
            get { return PropertyValue.Get(); }
            set { PropertyValue.Set(value); }
        }

        public PropertyWrapper(TSourceObject source, Action<TSourceObject, TValue> setValue, Func<TSourceObject, TValue> getValue)
        {
            _source = source;
            _setValue = setValue;
            
            PropertyValue = new Property<TValue>();
            PropertyValue.PropertyChanged += OnPropertyChanged;
            PropertyValue.UnnotifiedSet(getValue(_source));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _setValue(_source, (TValue)e.NewValue);
        }
    }
}