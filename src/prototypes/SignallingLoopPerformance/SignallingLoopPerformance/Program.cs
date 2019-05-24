using System;
using System.Diagnostics;
using System.Threading;

namespace SignallingLoopPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
        }

        public Program()
        {
            var fpsLimit = new FramesPerSecondLimiter(5, RenderAction);
            fpsLimit.Run();
            
            while(true)
                fpsLimit.Render();
        }

        private int _frameCount;
        private Stopwatch _renderingStopwatch = Stopwatch.StartNew();

        float y=15.45f;
        
        private void RenderAction()
        {
            _frameCount++;

            if (_renderingStopwatch.ElapsedMilliseconds > 1000)
            {
                var fps = _frameCount / (_renderingStopwatch.ElapsedMilliseconds / 1000.0f);
                Console.WriteLine("FPS: " + fps);
                _renderingStopwatch.Restart();
                _frameCount = 0;
            }
                
            // create some load, DateTime.Now is slow
            for (int i = 0; i < 10000; i++)
                y = (DateTime.Now.Millisecond / 10.0f) * y;
        }
    }
}