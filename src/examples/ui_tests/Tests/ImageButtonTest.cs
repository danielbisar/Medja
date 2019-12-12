using System;
using Medja.Controls;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ImageButtonTest
    {
        private readonly IControlFactory _controlFactory;

        public ImageButtonTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var imageButton = _controlFactory.Create<ImageButton>();
            imageButton.Image.Path = "Images/button.png";
            imageButton.MouseOverImage.Path = "Images/button_hover.png";
            imageButton.MouseDownImage.Path = "Images/button_press.png";
            imageButton.InputState.Clicked += (s, e) => Console.WriteLine("Button pressed!");
            imageButton.HorizontalAlignment = HorizontalAlignment.Left;

            var imageButton2 = _controlFactory.Create<ImageButton>();
            imageButton2.Image.Path = "Images/button_circle.png";
            imageButton2.InputState.Clicked += (s, e) => Console.WriteLine("Button2 pressed!");
            imageButton2.HorizontalAlignment = HorizontalAlignment.Left;
            imageButton2.Renderer = new ButtonBackgroundRenderer(imageButton2);
            DarkBlueTheme.SetButtonBackground(imageButton2);
            
            var vStack = _controlFactory.Create<VerticalStackPanel>();
            vStack.Add(imageButton);
            vStack.Add(imageButton2);
            
            return vStack;
        }
    }
}