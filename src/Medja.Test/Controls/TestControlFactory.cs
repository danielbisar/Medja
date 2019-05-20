using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Test.Controls
{
    public class TestControlFactory : ControlFactory
    {
        public override void ComboBoxMenuItemStyle(MenuItem menuItem)
        {
            base.ComboBoxMenuItemStyle(menuItem);
            menuItem.Background = Colors.Green;
        }
    }
}