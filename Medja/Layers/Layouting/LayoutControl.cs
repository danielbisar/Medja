using System;
using System.Collections.Generic;
using System.Text;
using Medja.Controls;

namespace Medja.Layers.Layouting
{
    public abstract class LayoutControl : Control, ILayout
    {
        public abstract IEnumerable<ControlState> GetLayout(ControlState state);
    }
}
