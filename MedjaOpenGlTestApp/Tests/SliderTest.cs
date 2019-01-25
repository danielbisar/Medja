using System;
using Medja.Controls;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class SliderTest
    {
        private readonly ControlFactory _controlFactory;

        public SliderTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var slider = _controlFactory.Create<Slider>();
            slider.MinValue = -10;
            slider.MaxValue = 10;
            slider.Value = 5;

            slider.PropertyValue.PropertyChanged += (s, e) =>
            {
                Console.WriteLine(slider.Value);
            };
            
            var result = _controlFactory.Create<VerticalStackPanel>();
            result.Children.Add(slider);

            return slider;
        }
    }
}