using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Demo
{
    public class DemoTheme : ControlFactory
    {
        protected override Button CreateButton()
        {
            var result = base.CreateButton();
            result.Background = DemoThemeValues.PrimaryColor;
            result.TextColor = DemoThemeValues.PrimaryTextColor;
            result.Position.Height = 40;
            //result.Position.Width = 100;

            result.Renderer = new ButtonRenderer(result);
            
            return result;
        }

        protected override CheckBox CreateCheckBox()
        {
            var result = base.CreateCheckBox();
            result.Background = DemoThemeValues.ControlBackground;
            result.TextColor = DemoThemeValues.PrimaryTextColor;
            result.Position.Height = 19;
            
            result.Renderer = new CheckBoxRenderer(result);

            return result;
        }

        protected override ContentControl CreateContentControl()
        {
            var result = base.CreateContentControl();
            result.Renderer = new ControlRenderer(result);

            return result;
        }

        protected override ProgressBar CreateProgressBar()
        {
            var result = base.CreateProgressBar();
            result.Background = DemoThemeValues.ControlBackground;
            result.Foreground = DemoThemeValues.PrimaryColor;
            result.Position.Height = 25;
            
            result.Renderer = new ProgressBarRenderer(result);

            return result;
        }
        
        protected override Slider CreateSlider()
        {
            var result = base.CreateSlider();
            result.Background = DemoThemeValues.ControlBackground;
            result.Foreground = DemoThemeValues.PrimaryTextColor;
            result.Renderer = new SliderRenderer(result);

            return result;
        }

        protected override TextBox CreateTextBox()
        {
            var result = base.CreateTextBox();
            result.Background = DemoThemeValues.ControlBackground;
            result.TextColor = DemoThemeValues.PrimaryTextColor;
            result.Renderer = new TextBoxRenderer(result);
            result.Position.Height = 37;
            result.Padding.Top = 9;
            result.Padding.Left = 10;

            return result;
        }

        protected override TextBlock CreateTextBlock()
        {
            var result = base.CreateTextBlock();
            result.Background = Colors.White;
            result.TextColor = Colors.Black;
            result.Padding.SetAll(5);
            
            result.Renderer = new TextBlockRenderer(result);
            
            return result;
        }

        protected override VerticalStackPanel CreateVerticalStackPanel()
        {
            var result = base.CreateVerticalStackPanel();
            result.Renderer = new ControlRenderer(result);

            return result;
        }
    }
}