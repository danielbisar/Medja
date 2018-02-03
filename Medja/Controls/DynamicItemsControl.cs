using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Controls
{
    public class DynamicItemsControl<TItemsHost, TDataItem> : Control
        where TItemsHost: Control
    {
        public TItemsHost ItemsHost { get; set; }
        public Action<TItemsHost> Clear { get; set; }


        public TDataItem DataItem { get; set; }
        public Action<TItemsHost, TDataItem> Apply { get; set; }
        
        public void AssureCreated()
        {
            Clear(ItemsHost);
            Apply(ItemsHost, DataItem);
        }
    }
}
