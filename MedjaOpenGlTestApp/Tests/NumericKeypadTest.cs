using System;
using Medja.Controls;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class NumericKeypadTest
    {
        private readonly ControlFactory _controlFactory;

        public NumericKeypadTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var result = _controlFactory.Create<DialogParentControl>();
            result.Content = _controlFactory.Create<Control>(p => p.Background = Colors.Red);  // background
            result.DialogControl = _controlFactory.Create<QuestionDialog>(p =>
                {
                    p.Content = _controlFactory.Create<NumericKeypad>();
                });
            result.IsDialogVisible = true;
            
            
            return result;
        }
    }
}