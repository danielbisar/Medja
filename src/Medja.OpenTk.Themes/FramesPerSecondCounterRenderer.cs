using System;
using Medja.Theming;
using Medja.Utils;

namespace Medja.OpenTk.Themes
{
    public class FramesPerSecondCounterRenderer : IControlRenderer
    {
        private FramesPerSecondCounter _counter;

        public FramesPerSecondCounterRenderer()
        {
            _counter = new FramesPerSecondCounter();
            _counter.FramesCounted += OnFramesCounted;
        }

        private void OnFramesCounted(object sender, ValueEventArgs<float> e)
        {
            Console.WriteLine("FPS: " + e.Value);
        }

        public void Dispose()
        {
        }

        public void Render(object context)
        {
            _counter.Tick();
        }
    }
}