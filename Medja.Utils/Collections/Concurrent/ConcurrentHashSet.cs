using System;
using System.Collections;
using System.Collections.Generic;

namespace Medja.Utils.Collections.Concurrent
{
    /// <summary>
    /// A thread-safe <see cref="HashSet{T}"/>. Does use lock.
    /// </summary>
    public class ConcurrentHashSet<T> : ICollection<T>
    {
        private readonly object _lock;
        private readonly HashSet<T> _hashSet;

        public int Count
        {
            get { lock (_lock) return _hashSet.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ConcurrentHashSet()
        {
            _lock = new object();
            _hashSet = new HashSet<T>();
        }

        public ConcurrentHashSet(IEqualityComparer<T> comparer)
        {
            // not clear from source code how HashSet with equality comparer is releated to the other constructors
            // this is why implement a sepearte one for this overload
            _lock = new object();
            _hashSet = new HashSet<T>(comparer); 
        }

        public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            _lock = new object();
            _hashSet = new HashSet<T>(collection, comparer);
        }

        public bool Add(T item)
        {
            lock (_lock) return _hashSet.Add(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock) _hashSet.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            lock (_lock) return _hashSet.Remove(item);
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            lock(_lock) _hashSet.Clear();
        }

        public bool Contains(T item)
        {
            lock (_lock) return _hashSet.Contains(item);
        }

        /// <summary>
        /// Gets a copy of that HashSet as non thread-safe version.
        /// </summary>
        /// <returns></returns>
        public HashSet<T> GetCopy()
        {
            lock (_lock) return new HashSet<T>(_hashSet);
        }

        /// <summary>
        /// Gets a thread-safe copy of that hashset.
        /// </summary>
        /// <returns></returns>
        public ConcurrentHashSet<T> GetThreadSafeCopy()
        {
            lock(_lock) return new ConcurrentHashSet<T>(_hashSet, null);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetCopy().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}