using System;
using System.Collections.Generic;

namespace Medja.Properties.ChangeTracking
{
    [Serializable]
    public class SubListItemChanges : PropertyChange
    {
        public IReadOnlyList<PropertyChange> Changes { get; }
        public int Index { get; }
        
        public SubListItemChanges(string name, int index, IReadOnlyList<PropertyChange> changes) 
            : base(name)
        {
            Index = index;
            Changes = changes;
        }
    }
}