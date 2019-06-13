using System;
using System.Collections.Specialized;

namespace Medja.Properties.ChangeTracking
{
    [Serializable]
    public class CollectionPropertyChange : PropertyChange
    {
        public NotifyCollectionChangedAction Action { get; }
        public int Index { get; }
        public object Item { get; }
        public object[] Items { get; }
        
        public CollectionPropertyChange(string name, NotifyCollectionChangedAction action, object item, int index) 
            : base(name)
        {
            if(action != NotifyCollectionChangedAction.Add 
               && action != NotifyCollectionChangedAction.Remove)
                throw new ArgumentOutOfRangeException(nameof(action));
            
            Action = action;
            Item = item;
            Index = index;
        }

        public CollectionPropertyChange(string name, NotifyCollectionChangedAction action, object[] items)
            : base(name)
        {
            if(action != NotifyCollectionChangedAction.Reset)
                throw new ArgumentOutOfRangeException(nameof(action));

            Action = action;
            Items = items;
            Index = 0;
        }
    }
}