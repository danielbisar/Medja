using System.Collections.Generic;

namespace Medja.Utils
{
    public static class EqualityUtils
    {
        /// <summary>
        /// Gets an equality comparer that can be used for items (ItemsManager, DockPanel, ...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEqualityComparer<T> GetItemEqualityComparer<T>()
            where T: class
        {
            if (typeof(T) == typeof(string))
                return EqualityComparerCache<T>.Comparer;
            
            return new ReferenceEqualityComparer<T>();
        }
    }
}