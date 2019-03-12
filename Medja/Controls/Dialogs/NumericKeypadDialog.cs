using System;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class NumericKeypadDialog : ConfirmableDialog
    {
        public NumericKeypadDialog(ControlFactory controlFactory) : base(controlFactory)
        {
            
        }

        protected override Control CreateContent()
        {
            var result = (DockPanel)base.CreateContent();
            var numericKeypad = _controlFactory.Create<NumericKeypad>();
            result.Add(Dock.Fill, numericKeypad);

            return result;
        }

        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);
            
            Console.WriteLine(new ControlTreeStringBuilder(this).GetTree());
        }
    }
}