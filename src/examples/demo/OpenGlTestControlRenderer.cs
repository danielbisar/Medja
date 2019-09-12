using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL4;

namespace Medja.Demo
{
    public class OpenGlTestControlRenderer : 
        OpenTKControlRendererBase<OpenGlTestControl>
    //SkiaControlRendererBase<OpenGlTestControl>
    {
        public OpenGlTestControlRenderer(OpenGlTestControl control)
        : base(control)
        {
        }

        protected override void InternalRender()
        {
            GL.ClearColor(1,1,1,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.Flush();
            
            /*_control.Camera.Render();
            _control.Label.Render();*/
            
            /*_paint.TextSize = _control._font.Size;
            _paint.Typeface = SKTypeface.FromFamilyName(_control._font.Name);

            _paint.Color = SKColors.Green;
            _canvas.DrawLine(_rect.Left, _rect.Top, _rect.Right, _rect.Top, _paint);
            _paint.Color = SKColors.Black;
            
            // y = baseline
            var y = _rect.Top - _paint.FontMetrics.Ascent;
            var step = _paint.FontSpacing;
            var text = "PQR_{}!_ÖÄÜ";

            _paint.IsAntialias = true;
            _paint.IsAutohinted = true;
            _paint.SubpixelText = false;
            _canvas.DrawText(text, 10, y, _paint);
            _canvas.DrawLine(_rect.Left, y, _rect.Right, y, _paint);
            //_canvas.DrawLine(_rect.Left, y + _paint.FontMetrics.Bottom, _rect.Right, y + _paint.FontMetrics.Bottom, _paint);
            _canvas.DrawLine(_rect.Left, y + _paint.FontMetrics.Ascent, _rect.Right, y + _paint.FontMetrics.Ascent, _paint);
            //_canvas.DrawLine(_rect.Left, y + _paint.FontMetrics.Top, _rect.Right, y + _paint.FontMetrics.Top, _paint);
            _canvas.DrawLine(_rect.Left, y + _paint.FontMetrics.Descent, _rect.Right, y + _paint.FontMetrics.Descent, _paint);

            y += step;
            _paint.IsAntialias = true;
            _paint.IsAutohinted = true;
            _paint.SubpixelText = true;
            _canvas.DrawText(text, 10, y, _paint);

            y += step;
            _paint.IsAntialias = true;
            _paint.IsAutohinted = false;
            _paint.SubpixelText = true;
            _canvas.DrawText(text, 10, y, _paint);

            y += step;
            _paint.IsAntialias = false;
            _paint.IsAutohinted = false;
            _paint.SubpixelText = true;
            _canvas.DrawText(text, 10, y, _paint);

            y += step;
            _paint.IsAntialias = false;
            _paint.IsAutohinted = true;
            _paint.SubpixelText = true;
            _canvas.DrawText(text, 10, y, _paint);

            y += step;
            _paint.IsAntialias = false;
            _paint.IsAutohinted = true;
            _paint.SubpixelText = false;
            _canvas.DrawText(text, 10, y, _paint);

            y += step;
            _paint.IsAntialias = true;
            _paint.IsAutohinted = true;
            _paint.SubpixelText = true;
            _paint.LcdRenderText = true;
            _canvas.DrawText(text, 10, y, _paint);

            y += step;
            _paint.SubpixelText = false;
            _canvas.DrawText(text, 10, y, _paint);

            _paint.LcdRenderText = false;

            _paint.Typeface.Dispose();*/
        }        
    }
}
