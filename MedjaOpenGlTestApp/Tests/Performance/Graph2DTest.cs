using System;
using System.Diagnostics;
using System.Reflection;
using Medja;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using Medja.Utils;
using Medja.Utils.Reflection;

namespace MedjaOpenGlTestApp.Tests.Performance
{
    public class Graph2DTest
    {
        private readonly IControlFactory _controlFactory;

        public Graph2DTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var graph = _controlFactory.Create<Graph2D>();
            var itemCount = 17280000.0f;
            //var itemCount = 1000000.0f;
            //var itemCount = 21600000.0f; // 10 hours of recorded data (600 fps * 3600 sec * 10)
            var maxX = 1000;
            var step = maxX / itemCount;

            var sw = Stopwatch.StartNew();
            
            Console.WriteLine("Use about " + ByteSize.GetHumanReadable(GC.GetTotalMemory(true)));
            
            for (float x = 0; x < maxX; x += step)
            {
                //graph.DataPoints.Add(new Point(x*10, (float)Math.Sin(x)*100));
                var y = 10.0f + Math.Sin(100.0 / ((x / 5) - 9)) * 200.0f * Math.Sin(x);
                //var y = 10.0f + Math.Sin(x / 100.0) * 200 * Math.Sin(x / 5.0) * Math.Sin(x / 5.0);
                graph.DataPoints.Add(new Point(x*2, (float)y));

            }
            
            sw.Stop();

            Console.WriteLine("Is assembly optimized: " + Assembly.GetExecutingAssembly().IsAssemblyOptimized());
            Console.WriteLine($"Created {itemCount} points. Took {sw.Elapsed}");
            GC.Collect();
            Console.WriteLine("Use about " + ByteSize.GetHumanReadable(GC.GetTotalMemory(true)));
            
            
            return graph;
        }
    }
}