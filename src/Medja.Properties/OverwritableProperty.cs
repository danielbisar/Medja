namespace Medja.Properties
{
    /// <summary>
    /// Allows temporary overwriting of the actual value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OverwritableProperty<T> : Property<T>
    {
        private T _overwrittenValue;
        private bool _isValueOverwritten;
        
        protected override void InternalSet(T value)
        {
            var oldValue = _value;
            _value = value;
            
            if(!_isValueOverwritten)
                NotifyPropertyChanged(oldValue, value);
        }

        /// <summary>
        /// Sets the temporary value.
        /// </summary>
        /// <param name="value">The temporary value.</param>
        public void OverwriteSet(T value)
        {
            if (_isValueOverwritten)
            {
                if (!_comparer.Equals(_overwrittenValue, value))
                {
                    var oldValue = _overwrittenValue;
                    _overwrittenValue = value;
                    
                    NotifyPropertyChanged(oldValue, _overwrittenValue);
                }
            }
            else if (!_comparer.Equals(_value, value))
            {
                _overwrittenValue = value;
                _isValueOverwritten = true;
                
                NotifyPropertyChanged(_value, _overwrittenValue);
            }
        }

        /// <summary>
        /// Clears the temporary value (resets to the original value).
        /// </summary>
        public void ClearOverwrittenValue()
        {
            if (_isValueOverwritten)
            {
                var oldValue = _overwrittenValue;
                
                _isValueOverwritten = false;
                _overwrittenValue = default;
                
                if(!_comparer.Equals(oldValue, _value))
                    NotifyPropertyChanged(oldValue, _value);
            }
        }

        /// <summary>
        /// Gets the properties value.
        /// </summary>
        /// <returns></returns>
        public override T Get()
        {
            return _isValueOverwritten ? _overwrittenValue : _value;
        }
    }
}