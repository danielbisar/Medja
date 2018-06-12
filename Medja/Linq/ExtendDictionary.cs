using System;
using System.Collections.Generic;

namespace Medja.Linq
{
	public static class ExtendDictionary
	{
		/// <summary>
		/// Gets or adds the value for the given key.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="dictionary">The Dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="factory">A method that returns a new object for the 
		/// given key.</param>
		/// <typeparam name="TKey">The key type parameter.</typeparam>
		/// <typeparam name="TValue">The value type parameter.</typeparam>
		public static TValue GetOrAdd<TKey, TValue>(
			this IDictionary<TKey, TValue> dictionary,
			TKey key,
			Func<TKey, TValue> factory)
		{
			if (dictionary.TryGetValue(key, out var value))
				return value;

			value = factory(key);
			dictionary.Add(key, value);

			return value;
		}
	}
}
