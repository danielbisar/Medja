using System;
using System.Collections.Generic;
using System.Linq;

namespace Medja.Utils.Linq
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Calls <see cref="Enumerable.ToArray{TSource}"/> only if the given <see cref="IEnumerable{T}"/> is not
        /// already an array, otherwise it just returns the casted <see cref="source"/>.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <typeparam name="T">The items type.</typeparam>
        /// <returns><see cref="source"/> if it is an array or the result of <see cref="Enumerable.ToArray{TSource}"/>.
        /// </returns>
        public static T[] AssureIsArray<T>(this IEnumerable<T> source)
        {
            if (source is T[] array)
                return array;

            return source.ToArray();
        }
        
        /// <summary>
        /// Sums up all uints.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The sum.</returns>
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

        // do not use IList or IEnumerable, this would impact performance too much
        // create own overload for this kind of versions
        // tested with for instead of foreach: result: slower
        public static void ForEachSplitSeq<T>(this List<T> items, Func<T, bool> condition, Action<T> matches, Action afterMatches, Action<T> doesNotMatch)
        {
            var nonMatching = new List<T>();
            
            foreach(var item in items)
            {
                if(condition(item))
                    matches(item);
                else
                    nonMatching.Add(item);
            }

            afterMatches?.Invoke();

            foreach(var item in nonMatching)
            {
                doesNotMatch(item);
            }
        }
    }
}