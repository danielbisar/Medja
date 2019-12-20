using Medja.OpenTk.Rendering;

namespace Medja.OpenTk.Controls
{
    public class OpenGLEventControlRenderer : OpenTKControlRendererBase<OpenGLEventControl>
    {
        public OpenGLEventControlRenderer(OpenGLEventControl control) 
            : base(control)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _control.RaiseLoad();
        }

        protected override void InternalRender()
        {
            _control.RaiseRender();
            _control.NeedsRendering = true;
        }
    }
}