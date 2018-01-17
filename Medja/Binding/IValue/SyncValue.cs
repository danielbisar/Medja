using System;

namespace Medja.Binding
{
    public class SyncValue<T> : IValue<T>
    {
        public T Value { get; }
        
        public SyncValue(Func<T> factory)
        {
            Value = factory();
        }
    }
}
