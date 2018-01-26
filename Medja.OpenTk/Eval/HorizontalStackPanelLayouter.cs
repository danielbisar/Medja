namespace Medja.OpenTk.Eval
{
    public class HorizontalStackPanelLayouter
    {
        private readonly IItems _items;

        public HorizontalStackPanelLayouter(IItems items)
        {
            _items = items;
        }

        public Layout Layout(PositionInfo positionInfo)
        {
            var layout = new Layout();
            var items = _items.Items;

            var curX = positionInfo.X;
            var curY = positionInfo.Y;
            const float spacingY = 0.012f;
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

            return layout;
        }
}
}
