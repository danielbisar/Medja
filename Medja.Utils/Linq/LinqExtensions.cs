using System;
using System.Collections.Generic;
using System.Linq;

namespace Medja.Utils.Linq
{
    public static class LinqExtensions
    {
        public static uint Sum(this IEnumerable<uint> values)
        {
            uint result = 0;

            foreach (var item in values)
                result += item;

            return result;
        }

        /// <summary>
        /// Removes items from the end of the list as long as the given condition is true.
        /// </summary>
        /// <param name="list">The list to remove the items from.</param>
        /// <param name="condition">The condition that should be true until removing is stopped.</param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveFromEndWhile<T>(this IList<T> list, Func<IList<T>, bool> condition)
        {
            while(list.Count > 0 && condition(list))
                list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// Removes all items from list where the given condition is true.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="condition"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> condition)
        {
            var items = collection.Where(condition).ToList();

            foreach (var item in items)
                collection.Remove(item);
        }
    }
}