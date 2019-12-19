using System;
using System.Collections.Generic;

namespace Medja.Utils.Collections.Generic
{
	public static class IDictionaryExtensions
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
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
		                                            TKey key,
		                                            Func<TKey, TValue> factory = null)
		{
			if (dictionary.TryGetValue(key, out var value))
				return value;

            value = factory != null ? factory(key) : default;
			dictionary.Add(key, value);

			return value;
		}

		/// <summary>
		/// Gets the matching value or default(TValue).
		/// </summary>
		/// <returns>The or default.</returns>
		/// <param name="dictionary">Dictionary.</param>
		/// <param name="key">Key.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		/// <typeparam name="TValue">The 2nd type parameter.</typeparam>
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
														TKey key)
		{
			TValue result;

			if (dictionary.TryGetValue(key, out result))
				return result;

			return default;
		}
	}
}
