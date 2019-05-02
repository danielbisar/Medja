namespace Medja
{
    /// <summary>
    /// A thread safe version of <see cref="Property{T}"/>
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
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
                base.InternalSet(value);
        }

        public override void UnnotifiedSet(T value)
        {
            lock(_lock)
                base.UnnotifiedSet(value);
        }
    }
}