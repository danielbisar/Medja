using System;
using System.Collections.Generic;

namespace Medja.Utils
{
    public static class IDisposableExtensions
    {
        public static void DisposeAll(this IEnumerable<IDisposable> disposables)
        {
            foreach(var disposable in disposables)
                disposable?.Dispose();
        }
    }
}