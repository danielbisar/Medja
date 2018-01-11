using Medja.Controls;
using Medja.Rendering;

namespace Medja.OpenTk.Rendering
{
    public class MedjaOpenTkRendererMap : RendererMap
    {
        public MedjaOpenTkRendererMap()
        {
            Add(typeof(Button), new ButtonRenderer());
        }
    }
}
