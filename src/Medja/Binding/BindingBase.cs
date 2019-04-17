using System;

namespace Medja
{
    public abstract class BindingBase<TTarget, TSource> : IDisposable
    {
        public abstract void Dispose();
    }
}