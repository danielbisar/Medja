using System.Collections.Generic;
using System.Linq;

namespace Medja.Utils.Collections.Generic
{
    /// <summary>
    /// A bi-directional dictionary.
    /// </summary>
    public class Map<T, T2>
    {
        private readonly Dictionary<T, T2> _dictionary;
        private readonly Dictionary<T2, T> _reverseDictionary;

        public int Count
        {
            get => _dictionary.Count;
        }
        
        public Map()
        {
            _dictionary = new Dictionary<T, T2>();
            _reverseDictionary = new Dictionary<T2, T>();
        }

        public void Add(T key, T2 value)
        {
            _dictionary.Add(key, value);
            _reverseDictionary.Add(value, key);
        }

        public void RemoveByKey(T key)
        {
            _reverseDictionary.Remove(_dictionary[key]);
            _dictionary.Remove(key);
        }

        public void RemoveByVal(T2 value)
        {
            _dictionary.Remove(_reverseDictionary[value]);
            _reverseDictionary.Remove(value);
        }

        public T GetKey(T2 value)
        {
            return _reverseDictionary[value];
        }

        public IEnumerable<KeyValuePair<T, T2>> GetKeyAndValues()
        {
            return _dictionary;
        }

        public IEnumerable<T> GetKeys()
        {
            return _dictionary.Keys;
        }
        
        public T2 GetValue(T key)
        {
            return _dictionary[key];
        }

        public IEnumerable<T2> GetValues()
        {
            return _reverseDictionary.Keys;
        }

        public void Clear()
        {
            _dictionary.Clear();
            _reverseDictionary.Clear();
        }
    }
}