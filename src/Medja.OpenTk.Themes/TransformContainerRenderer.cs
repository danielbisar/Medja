using Medja.Controls;

namespace Medja.OpenTk.Themes
{
    public class TransformContainerRenderer : SkiaControlRendererBase<TransformContainer>
    {
        public TransformContainerRenderer(TransformContainer control) 
            : base(control)
        {
            control.AffectsRendering(control.PropertyRotation);
        }

        protected override void InternalRender()
        {
            _canvas.RotateRadians(_control.Rotation);
        }
    }
}