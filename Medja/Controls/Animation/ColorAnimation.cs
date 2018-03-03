using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Medja.Primitives;

namespace Medja.Controls.Animation
{
    public class ColorAnimation : ControlAnimation
    {
        private readonly Property<Color> _colorProperty;
        private readonly Color _fromColor;
        private readonly Color _toColor;
        private readonly Color _diffColor;

        public ColorAnimation(Property<Color> colorProperty, Color fromColor, Color toColor, long duration)
        {
            _colorProperty = colorProperty;
            _fromColor = fromColor;
            _toColor = toColor;
            _diffColor = GetDiffColor();
            Duration = duration;
        }

        private Color GetDiffColor()
        {
            return new Color(_toColor.Red - _fromColor.Red, _toColor.Green - _fromColor.Green, _toColor.Blue - _fromColor.Blue);
        }

        internal override void Apply()
        {
            base.Apply();
            _colorProperty.Set(GetCurrentColor());

            Debug.WriteLine("ColorAnimation: " + _colorProperty.Get());
        }

        private Color GetCurrentColor()
        {
            return new Color(_fromColor.Red + _diffColor.Red * _precentage, _fromColor.Green + _diffColor.Green * _precentage, _fromColor.Blue + _diffColor.Blue * _precentage);
        }
    }
}
