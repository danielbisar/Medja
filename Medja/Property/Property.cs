using System.Collections.Generic;

namespace Medja
{
    // TODO implement readonly version
    public class Property<T> : IProperty
    {
        // creating a static variable inside this class makes creation 3X as slow as currently
        private readonly EqualityComparer<T> _comparer;
        private T _value;

        public event PropertyChangedEventHandler PropertyChanged;

        public Property()
        {
            // see EqualityComparerCache header for info why
            _comparer = EqualityComparerCache<T>.Comparer;
        }
        // would allow properties with and without change notification
        public void Set(T value)
        {
            if (_comparer.Equals(_value, value))
                return;

            _value = value;
            NotifyPropertyChanged();
        }

        public T Get()
        {
            return _value;
        }

        private void NotifyPropertyChanged()
        {
            PropertyChanged?.Invoke(this);
        }
    }
}
