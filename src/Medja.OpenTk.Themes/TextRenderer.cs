using System;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Utils;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    /// <summary>
    /// A helper class to render text.
    /// </summary>
    public class TextRenderer : IDisposable
    {
        private readonly SKPaint _paint;
        
        /// <summary>
        /// X start position.
        /// </summary>
        public float X { get; set; }
        
        /// <summary>
        /// Y start position.
        /// </summary>
        public float Y { get; set; }
        
        public Font Font { get; }

        public TextRenderer(Font font)
        {
            _paint = new SKPaint();
            _paint.IsAntialias = true;
            
            Font = font ?? throw new ArgumentNullException(nameof(font));
            Font.PropertyName.PropertyChanged += OnFontNameChanged;
            Font.PropertySize.PropertyChanged += OnFontSizeChanged;
            Font.PropertyColor.PropertyChanged += OnFontColorChanged;
            
            UpdateTypeface();
            UpdateFontSize();
            UpdateColor();
        }

        private void OnFontColorChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateColor();
        }

        private void UpdateColor()
        {
            _paint.Color = Font.Color.ToSKColor();
        }

        private void OnFontSizeChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateFontSize();
        }

        private void UpdateFontSize()
        {
            _paint.TextSize = Font.Size;
        }

        private void OnFontNameChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTypeface();
        }

        private void UpdateTypeface()
        {
            _paint.Typeface?.Dispose();
            _paint.Typeface = Font.Name == null ? null : SKTypeface.FromFamilyName(Font.Name);
        }

        public void Render(string text, SKCanvas canvas)
        {
            canvas.DrawTextSafe(text, X, Y + _paint.FontSpacing, _paint);
        }

        public void Dispose()
        {
            Font.PropertyName.PropertyChanged -= OnFontNameChanged;
            Font.PropertySize.PropertyChanged -= OnFontSizeChanged;
            Font.PropertyColor.PropertyChanged -= OnFontColorChanged;
            _paint?.Dispose();
        }
    }
}