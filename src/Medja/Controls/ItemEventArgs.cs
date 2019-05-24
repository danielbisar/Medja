using System;

namespace Medja.Controls
{
    public class ItemEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; }
        public Control Control { get; }

        public ItemEventArgs(TItem item, Control control)
        {
            Item = item;
            Control = control;
        }
    }
}