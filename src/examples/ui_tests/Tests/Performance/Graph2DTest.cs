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
            Console.WriteLine("Is assembly optimized: " + Assembly.GetExecutingAssembly().IsAssemblyOptimized());

            var graph = _controlFactory.Create<Graph2D>();
            
            var tenHoursGraphButton = _controlFactory.Create<Button>();
            tenHoursGraphButton.Text = "10 h";
            tenHoursGraphButton.InputState.Clicked += (s, e) => 
            {
                // 10 hours of recorded data (600 fps * 3600 sec * 10)
                graph.DataPoints.Clear();
                GenerateData(graph, 21600000.0f, x => (float)(10.0 + Math.Sin(100.0 / ((x / 5) - 9)) * 200.0 * Math.Sin(x)));
            };

            var oneMilSin = _controlFactory.Create<Button>();
            oneMilSin.Text = "1 Mio Sin";
            oneMilSin.InputState.Clicked += (s, e) => 
            {
                graph.DataPoints.Clear();
                GenerateData(graph, 1000000.0f, x => (float)(Math.Sin(x/2) * 100.0));
            };

            var downsampleI = 0;

            var graph3 = _controlFactory.Create<Button>();
            graph3.Text = "Toggle downsampling";
            graph3.InputState.Clicked += (s, e) => 
            {
                if(downsampleI == 0)
                    graph.DataPoints.Downsampler = new DummyDownsampler(graph.DataPoints.Points);
                else if(downsampleI == 1)
                {
                    graph.DataPoints.Downsampler = new Graph2DSlopDownsampler(graph.DataPoints.Points);
                    graph.SlopThreshold = 0.2f;
                    graph.MinDistance = 3.0f;
                }
                else if(downsampleI == 2)
                {
                    graph.SlopThreshold = 0.1f;
                    graph.MinDistance = 3.0f;
                }
                else if(downsampleI == 3)
                {
                    graph.SlopThreshold = 1.0f;
                    graph.MinDistance = 3.0f;
                }
                else if(downsampleI == 4)
                {
                    graph.SlopThreshold = 1f;
                    graph.MinDistance = 1.0f;
                }
                else if(downsampleI == 5)
                {
                    graph.SlopThreshold = 1f;
                    graph.MinDistance = 2.0f;
                }

                Console.WriteLine("Downsampler " + graph.DataPoints.Downsampler.GetType().Name + " Slope threshold: " + graph.SlopThreshold + " Min distance: " + graph.MinDistance);
                downsampleI = (downsampleI + 1) % 6;
            };

            var dataButtons = _controlFactory.Create<HorizontalStackPanel>();
            dataButtons.ChildrenWidth = 300;
            dataButtons.Children.Add(tenHoursGraphButton);
            dataButtons.Children.Add(oneMilSin);
            dataButtons.Children.Add(graph3);           

            var pointsButton = _controlFactory.Create<Button>();
            pointsButton.Text = "Points";
            pointsButton.InputState.Clicked += (s, e) => 
            {
                graph.RenderMode = 0;
            };

            var linesButton = _controlFactory.Create<Button>();
            linesButton.Text = "Lines";
            linesButton.InputState.Clicked += (s, e) => 
            {
                graph.RenderMode = 1;
            };

            var polyButton = _controlFactory.Create<Button>();
            polyButton.Text = "Polygon";
            polyButton.InputState.Clicked += (s, e) => 
            {
                graph.RenderMode = 2;
            };

            var renderModeButtons = _controlFactory.Create<HorizontalStackPanel>();
            renderModeButtons.ChildrenWidth = 300;
            renderModeButtons.Children.Add(pointsButton);
            renderModeButtons.Children.Add(linesButton);
            renderModeButtons.Children.Add(polyButton);

            var result = _controlFactory.Create<DockPanel>();
            result.Add(Dock.Top, dataButtons);
            result.Add(Dock.Top, renderModeButtons);
            result.Add(Dock.Fill, graph);

            return result;
        }

        private void GenerateData(Graph2D graph, float itemCount, Func<float, float> yValue)
        {
            var maxX = 1000;
            var step = maxX / itemCount;

            var sw = Stopwatch.StartNew();
            
            Console.WriteLine("Use about " + ByteSize.GetHumanReadable(GC.GetTotalMemory(true)));
            
            for (float x = 0; x < maxX; x += step)
            {
                graph.DataPoints.Add(new Point(x*2, yValue(x)));
            }
            
            sw.Stop();

            Console.WriteLine($"Created {graph.DataPoints.Count} points. Took {sw.Elapsed}");
            GC.Collect();
            Console.WriteLine("Use about " + ByteSize.GetHumanReadable(GC.GetTotalMemory(true)));       
        }
    }
}