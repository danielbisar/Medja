using System;

namespace Medja
{
    /// <summary>
    /// Base class for bindings.
    /// </summary>
    /// <typeparam name="TTarget">The target value type.</typeparam>
    /// <typeparam name="TSource">The source value type.</typeparam>
    public abstract class BindingBase<TTarget, TSource> : IDisposable
    {
        /// <summary>
        /// Should free all resources (unregister event handlers etc).
        /// </summary>
        public abstract void Dispose();
    }
}
