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
            slider.Margin.SetAll(10);

            var slider2 = _controlFactory.Create<Slider>();
            slider2.MinValue = 10;
            slider2.MaxValue = 20;
            slider2.Value = 15;
            slider2.Margin.SetAll(10);
            
            
            var slider3 = _controlFactory.Create<Slider>();
            slider3.MinValue = -20;
            slider3.MaxValue = -10;
            slider3.Value = -15;
            slider3.Margin.SetAll(10);
            
            // slider.PropertyValue.PropertyChanged += (s, e) =>
            // {
            //     Console.WriteLine(slider.Value);
            // };
            
            var result = _controlFactory.Create<VerticalStackPanel>();
            result.SpaceBetweenChildren = 20;
            result.Children.Add(slider);
            result.Children.Add(slider2);
            result.Children.Add(slider3);

            return result;
        }
    }
}