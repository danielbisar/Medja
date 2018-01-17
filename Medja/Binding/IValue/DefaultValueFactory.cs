using System;

namespace Medja.Binding
{
    public class DefaultValueFactory : IValueFactory
    {
        public IValue<T> Get<T>(Func<T> factory)
        {
            return new SyncValue<T>(factory);
        }
    }
}
