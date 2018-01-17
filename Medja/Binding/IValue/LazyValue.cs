using System;

namespace Medja.Binding
{
    public class LazyValue<T> : IValue<T>
    {
        private readonly Lazy<T> _lazy;

        public T Value { get { return _lazy.Value; } }

        public LazyValue(Func<T> factory)
        {
            _lazy = new Lazy<T>(factory, true);
        }
    }
}
