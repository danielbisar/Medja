using System;

namespace Medja.Rendering
{
    /// <summary>
    /// Factory to allow injection of RendererMap by the consumer. Always use this class if you need a renderer.
    /// </summary>
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
        /// <see cref="RendererMap.Get"/> for details.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IControlRenderer Get(Type type)
        {
            return _renderers.Get(type);
        }
    }
}
