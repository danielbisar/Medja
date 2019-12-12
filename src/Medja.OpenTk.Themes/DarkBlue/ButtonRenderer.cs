using Medja.Controls;

namespace Medja.OpenTk.Themes.DarkBlue
{
    /// <summary>
    /// Renders a normal button with text.
    /// </summary>
    public class ButtonRenderer : TextControlRendererBase<Button>
    {
        private readonly ButtonBackgroundRenderer _backgroundRenderer;
        
        public ButtonRenderer(Button control)
            : base(control)
        {
            _backgroundRenderer = new ButtonBackgroundRenderer(control);
        }

        protected override void DrawTextControlBackground()
        {
            _backgroundRenderer.Render(_canvas);
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundRenderer.Dispose();
            base.Dispose(disposing);
        }
    }
}