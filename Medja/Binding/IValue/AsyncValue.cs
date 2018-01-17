using System;
using System.Threading;
using System.Threading.Tasks;

namespace Medja.Binding
{
    public class AsyncValue<T> : IValue<T>
    {
        private volatile bool _isValueCreated = false;
        
        private Func<T> _factory;

        private T _value;
        public T Value
        {
            get
            {
                while(!_isValueCreated)
                {
                    Thread.Sleep(1);
                }

                return _value;
            }
        }

        public AsyncValue(Func<T> factory)
        {
            _factory = factory;
            Task.Factory.StartNew(Create);
        }

        private void Create()
        {
            _value = _factory();
            _factory = null; // release possible resources
            _isValueCreated = true;
        }
    }
}
