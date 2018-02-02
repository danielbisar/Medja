using System.Collections.Generic;

namespace Medja.Layers
{
    public interface ILayer
    {
        IEnumerable<ControlState> Apply(IEnumerable<ControlState> states);
    }
}
