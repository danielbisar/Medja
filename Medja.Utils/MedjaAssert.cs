using System;
using System.Collections.Generic;

namespace Medja.Utils
{
    public static class MedjaAssert
    {
        public static void Equal<T>(IEnumerable<T> items, params T[] expectedItems)
        {
            using (var enumerator = items.GetEnumerator())
            {
                int n = 0;

                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;

                    if (!EqualityComparer<T>.Default.Equals(item, expectedItems[n]))
                        throw new Exception($"Item at position {n} {item} is not equal expected item {expectedItems[n]}");

                    n++;
                }

                if (n != expectedItems.Length)
                    throw new Exception("Items contains less items than expected items count.");
            }
        }

        public static void Equal<T>(T[,] actual, T[,] expected)
        {
            if(actual.GetLength(0) != expected.GetLength(0)
            || actual.GetLength(1) != expected.GetLength(1))
                throw new Exception("Array dimensions do not match");

            var length0 = actual.GetLength(0);
            var length1 = actual.GetLength(1);
        
            for(int i = 0; i < length0; i++)
            {
                for(int n = 0; n < length1; n++)
                {
                    if(EqualityComparer<T>.Default.Equals(actual[i, n], expected[i, n]))
                        throw new Exception($"Expected value at ({i}, {n}) {expected[i,n]} is != actual value {actual[i,n]}.");
                }
            }
        }        
    }
}