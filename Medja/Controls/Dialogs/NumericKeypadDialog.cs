using System;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class NumericKeypadDialog : ConfirmableDialog
    {
        private NumericKeypad _numericKeypad;

        public string Text
        {
            get { return _numericKeypad.Text; }
            set { _numericKeypad.Text = value; }
        }    

        public NumericKeypadDialog(ControlFactory controlFactory) 
            : base(controlFactory)
        { 
        }

        protected override Control CreateContent()
        {
            _numericKeypad = _controlFactory.Create<NumericKeypad>();

            var result = (DockPanel)base.CreateContent();
            result.Add(Dock.Fill, _numericKeypad);

            return result;
        }

        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);
            
            Console.WriteLine(new ControlTreeStringBuilder(this).GetTree());
        }
    }
}