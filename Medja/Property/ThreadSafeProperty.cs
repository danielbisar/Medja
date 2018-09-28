namespace Medja
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>The call to unnotified set and set default value are not thread safe yet.</remarks>
    public class ThreadSafeProperty<T> : Property<T>
    {
        private readonly object _lock;

        public ThreadSafeProperty()
        {
            _lock = new object();
        }
        
        protected override void InternalSet(T value)
        {
            lock (_lock)
            {
                base.InternalSet(value);
            }
        }
        
        // TODO SetDefaultValue is not thread safe yet
    }
}