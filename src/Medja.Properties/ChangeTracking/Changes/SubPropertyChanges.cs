using System;
using System.Collections.Generic;

namespace Medja.Properties.ChangeTracking.Changes
{
    [Serializable]
    public class SubPropertyChanges : PropertyChange
    {
        public IReadOnlyList<PropertyChange> Changes { get; }

        public SubPropertyChanges(string name, IReadOnlyList<PropertyChange> changes)
            : base(name)
        {
            Changes = changes;
        }
    }
}
