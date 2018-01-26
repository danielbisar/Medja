using System;

namespace Medja.OpenTk.Eval
{
    public class MenuControllerLayouter
    {
        private readonly MenuController _controller;

        public MenuControllerLayouter(MenuController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public Layout Layout(PositionInfo positionInfo)
        {
            var layout = new Layout();
            var menu = _controller.CurrentMenu;

            if (menu != null)
            {
                // add the menu (f.e. if we need to draw a background for it etc)
                layout.Add(_controller.CurrentMenu, positionInfo);

                var curX = positionInfo.X;
                var curY = positionInfo.Y;
                const float spacingY = 0.012f;
                var items = menu.Items;
                var childAvailableSize = new Size(positionInfo.Width, (positionInfo.Height / items.Count) - spacingY * items.Count);
                var yIncPerChild = childAvailableSize.Height + spacingY;

                foreach (var item in items)
                {
                    var childPositionInfo = new PositionInfo();
                    childPositionInfo.X = curX;
                    childPositionInfo.Y = curY;
                    childPositionInfo.Width = childAvailableSize.Width;
                    childPositionInfo.Height = childAvailableSize.Height;

                    layout.Add(item, childPositionInfo);

                    curY += yIncPerChild;
                }
            }

            return layout;
        }
    }
}
