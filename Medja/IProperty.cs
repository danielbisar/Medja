using System.ComponentModel;

namespace Medja
{
    public interface IProperty<T> : INotifyPropertyChanged
    {
        T Get();
        void Set(T value);
    }
}