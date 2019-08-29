using System.Collections.Generic;

namespace Medja.Properties
{
    /// <summary>
    /// Provides change notification for properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>A fundamental class in Medja. Using this class adds data binding features support. This class is
    /// performance optimized and is almost as fast as using .NET properties. WPF Properties for instance are &gt;
    /// factor 100 slower.</remarks>
    public class Property<T> : IProperty
    {
        // creating a static variable inside this class makes creation 3X as slow as currently
        protected readonly EqualityComparer<T> _comparer;
        protected T _value;

        /// <summary>
        /// The <see cref="PropertyChanged"/> event fires when the value of this property changed or an explicit
        /// notification was requested.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public Property()
        {
            // register empty handler, see thread safety in wiki
            PropertyChanged += (s, e) => { };
            _comparer = EqualityComparerCache<T>.Comparer;
        }
        
        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>If the value given is different from the value stored the <see cref="PropertyChanged"/> event
        /// will be fired.</remarks>
        public virtual void Set(T value)
        {
            if (_comparer.Equals(_value, value))
                return;

            InternalSet(value);
        }
        
        /// <summary>
        /// Updates the value and triggers the PropertyChanged event.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected virtual void InternalSet(T value)
        {
            var oldValue = _value;
            _value = value;
            NotifyPropertyChanged(oldValue, value);
        }

        /// <summary>
        /// Sets the value of the property. Does not fire the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>This method is mainly used for initialization.</remarks>
        public virtual void SetSilent(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the properties value.
        /// </summary>
        /// <returns>The current value of the property.</returns>
        public virtual T Get()
        {
            return _value;
        }

        /// <summary>
        /// Triggers the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The value before the update.</param>
        /// <param name="newValue">The current value.</param>
        protected void NotifyPropertyChanged(T oldValue, T newValue)
        {
            // ReSharper disable once PossibleNullReferenceException
            // see ctor empty delegate registration
            PropertyChanged(this, new PropertyChangedEventArgs(oldValue, newValue));
        }
        
        /// <summary>
        /// Triggers the <see cref="PropertyChanged"/> event, settings oldValue and newValue to the current value.
        /// </summary>
        public virtual void NotifyPropertyChanged()
        {
            NotifyPropertyChanged(_value, _value);
        }
    }
}
