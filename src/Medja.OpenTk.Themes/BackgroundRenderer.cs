using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using SkiaSharp;

namespace Medja.OpenTk.Themes
{
    public class BackgroundRenderer : IDisposable
    {
        protected readonly SKPaint _paint;
        private Control _control;
        protected SKRect _rect;
        
        public BackgroundRenderer(Control control)
        {
            _control = control;

            _paint = new SKPaint {IsAntialias = true};
            UpdatePaint();
            
            _control.PropertyBackground.PropertyChanged += OnBackgroundRelevantPropertyChanged;
            _control.PropertyIsEnabled.PropertyChanged += OnBackgroundRelevantPropertyChanged;

            _control.Position.PropertyX.PropertyChanged += OnPositionChanged;
            _control.Position.PropertyY.PropertyChanged += OnPositionChanged;
            _control.Position.PropertyWidth.PropertyChanged += OnPositionChanged;
            _control.Position.PropertyHeight.PropertyChanged += OnPositionChanged;
        }

        private void OnPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateRect();
        }

        private void UpdateRect()
        {
            _rect = _control.Position.ToSKRect();
        }

        private void OnBackgroundRelevantPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePaint();
        }

        protected virtual void UpdatePaint()
        {
            if(_control.Background == null)
                _paint.Color = SKColor.Empty;
            else if (_control.IsEnabled)
                _paint.Color = _control.Background.ToSKColor();
            else
                _paint.Color = _control.Background.GetDisabled().ToSKColor();
        }

        public virtual void Render(SKCanvas canvas)
        {
            canvas.DrawRect(_rect, _paint);
        }

        public void Dispose()
        {
            _control.PropertyBackground.PropertyChanged -= OnBackgroundRelevantPropertyChanged;
            _control.Position.PropertyX.PropertyChanged -= OnPositionChanged;
            _control.Position.PropertyY.PropertyChanged -= OnPositionChanged;
            _control.Position.PropertyWidth.PropertyChanged -= OnPositionChanged;
            _control.Position.PropertyHeight.PropertyChanged -= OnPositionChanged;
            _control = null;
            
            _paint.Dispose();
        }
    }
}