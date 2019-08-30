namespace Medja.Properties
{
    /// <summary>
    /// A thread safe version of <see cref="Property{T}"/>
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
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

        public override void SetSilent(T value)
        {
            lock(_lock)
                base.SetSilent(value);
        }
    }
}