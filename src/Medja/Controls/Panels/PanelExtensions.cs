namespace Medja.Controls
{
    public static class PanelExtensions
    {
        public static void Add(this Panel panel, Control child)
        {
            panel.Children.Add(child);
        }
    }
}