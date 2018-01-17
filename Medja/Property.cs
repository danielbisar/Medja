using System.Collections.Generic;

namespace Medja
{
    public class Property<T> : IProperty
    {
        // static readonly would be slower in access
        private readonly EqualityComparer<T> _comparer;
        private T _value;

        public event PropertyChangedEventHandler PropertyChanged;

        public Property()
        {
            _comparer = EqualityComparer<T>.Default;
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
