using Medja.Controls;
using Medja.Theming;

namespace Medja.Demo
{
    public class TransformationsTab : ContentControl
    {
        private readonly IControlFactory _controlFactory;

        public TransformationsTab(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            Content = CreateContent();
        }

        private Control CreateContent()
        {
            var result = _controlFactory.Create<TransformContainer>();
            result.Content = _controlFactory.CreateTextBlock("Sdfj sdfl8 wjsddfn jsaf");

            return result;
        }
    }
}