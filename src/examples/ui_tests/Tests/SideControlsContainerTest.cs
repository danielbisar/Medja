using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class SideControlsContainerTest
    {
        private readonly IControlFactory _controlFactory;

        public SideControlsContainerTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public ContentControl Create()
        {
            var result = _controlFactory.Create<SideControlsContainer>();
            result.Content = _controlFactory.Create<TouchItemList<string>>(p =>
            {
                p.Background = Colors.Blue;
                
                for(int i = 0; i < 10; i++)
                    p.AddItem("Item " + i);
            });
            result.SideContent = _controlFactory.Create<Control>(p =>
            {
                p.Background = Colors.Green;
            });
            
            return result;
        }
    }
}