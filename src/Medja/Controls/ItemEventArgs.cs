using System;

namespace Medja.Controls
{
    public class ItemEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; }
        public Button Control { get; }

        public ItemEventArgs(TItem item, Button control)
        {
            Item = item;
            Control = control;
        }
    }
}