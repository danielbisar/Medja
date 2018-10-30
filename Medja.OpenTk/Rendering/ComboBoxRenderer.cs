using Medja.Controls;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    public class ComboBoxRenderer : SkiaControlRendererBase<ComboBoxBase>
    {
        public float BoxHeight { get; }

        public ComboBoxRenderer()
        {
            BoxHeight = 200;
        }
        
        protected override void InternalRender()
        {
            RenderBackground();

            if (_control.IsDropDownOpen)
            {
                // TODO logic for ComboBoxes that are too low on the screen (render the items above the box)
                var itemsRect = new SKRect(_rect.Left, _rect.Bottom, _rect.Right, _rect.Bottom + BoxHeight);
                _canvas.DrawRect(itemsRect, _paint);
            }
        }
    }
}