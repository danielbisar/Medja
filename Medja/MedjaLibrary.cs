using Medja.Rendering;

namespace Medja
{
    public static class MedjaLibrary
    {
        public static void Initialize(RendererMap rendererMap)
        {
            RendererFactory.SetRenderer(rendererMap);
        }
    }
}
