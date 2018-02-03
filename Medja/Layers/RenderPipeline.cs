using System.Collections.Generic;

namespace Medja.Layers
{
    public class RenderPipeline
    {
        private readonly List<ILayer> _layers;

        public RenderPipeline()
        {
            _layers = new List<ILayer>();
        }

        public void AddLayer(ILayer layer)
        {
            _layers.Add(layer);
        }

        public void Execute(IEnumerable<ControlState> states)
        {
            foreach (var layer in _layers)
                states = layer.Apply(states);
        }
    }
}
