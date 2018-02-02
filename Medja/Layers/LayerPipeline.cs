using System.Collections.Generic;

namespace Medja.Layers
{
    public class LayerPipeline
    {
        private readonly List<ILayer> _layers;

        public LayerPipeline()
        {
            _layers = new List<ILayer>();
        }

        public IEnumerable<ControlState> Execute()
        {
            IEnumerable<ControlState> states = null;

            foreach (var layer in _layers)
                states = layer.Apply(states);

            return states;
        }

        public void AddLayer(ILayer layer)
        {
            _layers.Add(layer);
        }
    }
}
