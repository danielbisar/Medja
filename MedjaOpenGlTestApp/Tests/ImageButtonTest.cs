using System;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ImageButtonTest
    {
        private readonly ControlFactory _controlFactory;

        public ImageButtonTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var imageButton = _controlFactory.Create<ImageButton>();
            imageButton.Image.Path = "Images/button.png";
            imageButton.MouseOverImage.Path = "Images/button_hover.png";
            imageButton.MouseDownImage.Path = "Images/button_press.png";
            imageButton.HorizontalAlignment = HorizontalAlignment.None;
            imageButton.VerticalAlignment = VerticalAlignment.None;
            imageButton.InputState.Clicked += (s, e) => Console.WriteLine("Button pressed!"); 

            var contentControl = _controlFactory.Create<ContentControl>();
            contentControl.AutoSetContentAlignment = false;
            contentControl.Content = imageButton;
            
            return contentControl;
        }
    }
}