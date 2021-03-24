namespace Medja.Controls.Panels
{
    public static class PanelExtensions
    {
        public static void Add(this Panel panel, Control child)
        {
            panel.Children.Add(child);
        }

        public static void Remove(this Panel panel, Control child)
        {
            panel.Children.Remove(child);
        }
    }
}
