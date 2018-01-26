using System.Collections.Generic;

namespace Medja.OpenTk.Eval
{
    public class ItemsProvider : IItems
    {
        public ICollection<object> Items { get; }

        public ItemsProvider(ICollection<object> items)
        {
            Items = items;
        }
    }
}
