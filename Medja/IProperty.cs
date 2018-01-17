using System.ComponentModel;

namespace Medja
{
    public interface IProperty
    {
        event PropertyChangedEventHandler PropertyChanged;
    }

    // do not use this interface for performance reasons,
    // if you use the Property Object directly it is a significant performance boost
    // the interface above exists just to be able to use the property for the
    // PropertyChanged delegate

    //public interface IProperty<T> : IProperty
    //{
    //    T Get();
    //    void Set(T value);
    //}
}