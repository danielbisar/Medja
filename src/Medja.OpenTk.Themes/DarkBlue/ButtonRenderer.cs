using System;
using Medja.Controls;
using Medja.OpenTk.Rendering;
using Medja.Utils;
using SkiaSharp;

namespace Medja.OpenTk.Themes.DarkBlue
{
    
    /*public class ButtonRenderer : TextControlRendererBase<Button>
    {
        private readonly SKPaint _defaultBackgroundPaint;
        private readonly SKPaint _disabledBackgroundPaint;
        private readonly SKPaint _clickBackgroundPaint;
        
        public ButtonRenderer(Button control) 
            : base(control)
        {
            _defaultBackgroundPaint = new SKPaint();
            _defaultBackgroundPaint.IsAntialias = true;
            _defaultBackgroundPaint.Color = control.Background.ToSKColor();
            //_defaultBackgroundPaint.ImageFilter = DemoThemeValues.DropShadow;

            _disabledBackgroundPaint = new SKPaint();
            _disabledBackgroundPaint.IsAntialias = true;
            _disabledBackgroundPaint.Color = control.Background.GetDisabled().ToSKColor();
            _disabledBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowDisabled;
            
            _clickBackgroundPaint = new SKPaint();
            _clickBackgroundPaint.IsAntialias = true;
            _clickBackgroundPaint.Color = control.Background.ToSKColor();
            _clickBackgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowElevated;
            ;
            // todo update colors on change - required for all renders; find a good clean solution
        }

        protected override void DrawTextControlBackground()
        {
            var backgroundPaint = _defaultBackgroundPaint;

            if (!_control.IsEnabled)
                backgroundPaint = _disabledBackgroundPaint;
            else if (_control.InputState.IsLeftMouseDown)
                backgroundPaint = _clickBackgroundPaint;
            
            _canvas.DrawRoundRect(_rect, 3, 3, backgroundPaint);
        }
    }*/
    
    
    public class ButtonRenderer : TextControlRendererBase<Button>
    {
        private new SKRect _rect;
        private readonly SKPaint _backgroundPaint;
        
        public ButtonRenderer(Button control)
            : base(control)
        {  
            _backgroundPaint = new SKPaint();
            _backgroundPaint.IsAntialias = true;
            
            // todo on dispose remove registered event handlers
            control.Position.OnAnyPropertyChanged(UpdateRect);

            BackgroundDependsOn(control.PropertyBackground, 
                                control.PropertyIsEnabled,
                                control.InputState.PropertyIsMouseOver, 
                                control.InputState.PropertyIsLeftMouseDown);
        }

        private void UpdateRect()
        {
            _rect = _control.Position.ToSKRect();
        }

        private void BackgroundDependsOn(params IProperty[] properties)
        {
            foreach (var property in properties)
                property.PropertyChanged += OnBackgroundPropertyChanged;
        }

        private void OnBackgroundPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_control.IsEnabled)
            {
                _backgroundPaint.Color = _control.Background.ToSKColor();

                if (_control.InputState.IsLeftMouseDown)
                    _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowElevated;
                else
                    _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadow;
            }
            else
            {
                _backgroundPaint.Color = _control.Background.GetDisabled().ToSKColor();
                _backgroundPaint.ImageFilter = DarkBlueThemeValues.DropShadowDisabled;
            }
        }

        protected override void DrawTextControlBackground()
        {
            _canvas.DrawRoundRect(_rect, 3, 3, _backgroundPaint);
        }

        protected override void Dispose(bool disposing)
        {
            _backgroundPaint.Dispose();
            base.Dispose(disposing);
        }
    }
}