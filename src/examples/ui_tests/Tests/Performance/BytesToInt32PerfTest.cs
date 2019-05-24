using System;
using System.Collections.Generic;
using System.Diagnostics;
using Medja.Utils;

namespace MedjaOpenGlTestApp.Tests.Performance
{
    public class BytesToInt32PerfTest
    {
        // BitConverter.ToInt32 direct call - for loop
        // Conversion took: 00:00:00.7743267
        // Conversion took: 00:00:00.7721407
        // Conversion took: 00:00:00.7796819
        
        // BitConverter.ToInt32 direct call - foreach
        // Conversion took: 00:00:00.5944272
        // Conversion took: 00:00:00.5903243
        // Conversion took: 00:00:00.5875147

        
        // ArraySegment ToInt32 extension - using BitConverter - for loop
        // Conversion took: 00:00:00.5012890
        // Conversion took: 00:00:00.5031995
        // Conversion took: 00:00:00.5024147
        
        // ArraySegment ToInt32 extension - using BitConverter - foreach
        // Conversion took: 00:00:00.5743267
        // Conversion took: 00:00:00.5759832
        // Conversion took: 00:00:00.5546931
        
        // it seems the access[i] multiple times is slower than using item in foreach
        // but the overhead of the enumerator is still there so the best combination 
        // in this case for loop with extension method
        
        public void Run()
        {
            var count = 50000000;
            var items = new List<ArraySegment<byte>>(count);
            int i = 0;
            
            for (i = 0; i < count; i++)
            {
                items.Add(new ArraySegment<byte>(BitConverter.GetBytes(i)));
            }

            var sw = new Stopwatch();
            sw.Start();

            i = 0;

            foreach(var item in items)
            {
                //var n = BitConverter.ToInt32(item.Array, item.Offset);
                //var n = BitConverter.ToInt32(items[i].Array, items[i].Offset);
                //var n = items[i].ToInt32();
                var n = item.ToInt32();
				
                if(n != i)
                    Console.WriteLine("error...");
                
                i++;
            }
			
            sw.Stop();
            Console.WriteLine("Conversion took: " + sw.Elapsed);
        }
    }
}