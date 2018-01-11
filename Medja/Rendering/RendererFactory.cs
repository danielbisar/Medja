using System;

namespace Medja.Rendering
{
    public class RendererFactory
    {
        private static RendererMap _renderers;

        public static void SetRenderer(RendererMap renderers)
        {
            _renderers = renderers;
        }

        private RendererFactory()
        {
        }

        /// <summary>
        /// Returns the renderer for the control or an empty renderer if no renderer is registered.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IControlRenderer Get(Type type)
        {
            if (_renderers.TryGetValue(type, out var result))
                return result;

            return EmptyControlRenderer.Default;
        }
    }
}
