using System;
using System.Collections.Generic;

namespace Medja
{
    public static class IDictionaryExtensions
    {
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createValue)
		{
			TValue result;

			if (dictionary.TryGetValue(key, out result))
				return result;

			result = createValue(key);
			dictionary[key] = result;

			return result;
		}
    }
}
