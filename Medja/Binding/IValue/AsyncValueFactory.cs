using System;

namespace Medja.Binding
{
    public class AsyncValueFactory : IValueFactory
    {
        public IValue<T> Get<T>(Func<T> factory)
        {
            return new AsyncValue<T>(factory);
        }
    }
}
