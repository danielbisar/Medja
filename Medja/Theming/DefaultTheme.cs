using System;
using Medja.Controls;
using Medja.Controls.Animation;
using Medja.Primitives;

namespace Medja.Theming
{
    public class DefaultTheme : ControlFactory
    {
        protected override Button CreateButton()
        {
            var defaultBackground = new Color(0.01f, 0.3f, 0.5f);
            var mouseOverBackground = new Color(0.03f, 0.9f, 0.9f);

            var result = base.CreateButton();
            result.Background = defaultBackground;

            result.InputState.IsMouseOverProperty.PropertyChanged += p =>
            {
                var prop = (Property<bool>)p;
                result.Background = prop.Get() ? mouseOverBackground : defaultBackground;
            };

            /*var ticks = new TimeSpan(0, 0, 1).Ticks;
            var mouseOverAnimation = new ColorAnimation(result.BackgroundProperty, new Color(1, 1, 1), ticks);

            result.InputState.IsMouseOverProperty.PropertyChanged += p =>
            {
                var prop = (Property<bool>)p;

                if (prop.Get())
                    result.AnimationManager.Start(mouseOverAnimation);
                else
                    result.AnimationManager.Revert(mouseOverAnimation);
            };*/

            return result;
        }
    }
}
