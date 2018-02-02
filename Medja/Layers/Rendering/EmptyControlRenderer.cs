using Medja.Controls;

namespace Medja.Rendering
{
    /// <summary>
    /// Empty renderer does nothing. This class exists so we don't need to check the renderer inside of controls for null value.
    /// </summary>
    public class EmptyControlRenderer : IControlRenderer
    {
        public static IControlRenderer Default { get; }

        static EmptyControlRenderer()
        {
            Default = new EmptyControlRenderer();
        }

        private EmptyControlRenderer()
        { }

        public void Render(Control control, RenderContext context)
        {
        }
    }
}
