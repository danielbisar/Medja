using System;
using Medja.Properties;

namespace Medja
{
    /// <summary>
    /// Acts as a wrapper for .NET Properties to support binding.
    /// </summary>
    /// <typeparam name="TSourceObject">The object type of the object containing the .NET property you want to wrap.</typeparam>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    public class PropertyWrapper<TSourceObject, TValue> 
        : Property<TValue>
    {
        private readonly TSourceObject _source;
        private readonly Action<TSourceObject, TValue> _setValue;
        private readonly Func<TSourceObject, TValue> _getValue;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="source">The object containing the .NET property you want to wrap.</param>
        /// <param name="getValue">The getter of the property.</param>
        /// <param name="setValue">The setter of the property.</param>
        public PropertyWrapper(TSourceObject source,
            Func<TSourceObject, TValue> getValue,
            Action<TSourceObject, TValue> setValue)
        {
            _source = source;
            _getValue = getValue;
            _setValue = setValue;
        }

        /// <inheritdoc />
        public override void Set(TValue value)
        {
            var oldValue = _getValue(_source);
            
            if(_comparer.Equals(oldValue, value))
                return;
            
            _setValue(_source, value);
            NotifyPropertyChanged(oldValue, value);
        }

        /// <inheritdoc />
        public override void SetSilent(TValue value)
        {
            _setValue(_source, value);
        }

        /// <inheritdoc />
        public override TValue Get()
        {
            return _getValue(_source);
        }

        /// <inheritdoc />
        public override void NotifyPropertyChanged()
        {
            var value = _getValue(_source);
            NotifyPropertyChanged(value, value);
        }
    }
}
