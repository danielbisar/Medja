using System;

namespace Medja.Binding
{
    public interface IValueFactory
    {
        IValue<T> Get<T>(Func<T> factory);
    }
}
