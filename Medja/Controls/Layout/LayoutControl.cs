using System.Collections.Generic;
using Medja.Controls;
using Medja.Primitives;

namespace Medja.Controls
{
    public abstract class LayoutControl : Control
    {
        public List<Control> Children { get; }

        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }

        protected LayoutControl()
        {
            Children = new List<Control>();
            Margin = new Thickness();
            Padding = new Thickness();
        }

        public override IEnumerable<Control> GetAllControls()
        {
            yield return this;

            foreach (var control in Children)
            {
                // GetAllControls also returns self
                foreach (var subControl in control.GetAllControls())
                    yield return subControl;
            }
        }
    }
}
