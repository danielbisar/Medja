using Medja.Controls;
using Medja.Primitives;

namespace MedjaOpenGlTestApp.Tests
{
    public class DialogParentControlTest
    {
        private readonly ControlFactory _controlFactory;

        public DialogParentControlTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var content = _controlFactory.Create<Control>();
            content.Background = Colors.Blue;
            content.VerticalAlignment = VerticalAlignment.Stretch;
            content.HorizontalAlignment = HorizontalAlignment.Stretch;

            var dialog = _controlFactory.Create<QuestionDialog>();
            dialog.Message = "Really?";
            
            var dialogParentControl = _controlFactory.Create<DialogParentControl>();
            dialogParentControl.Content = content;
            dialogParentControl.DialogControl = dialog;
            dialogParentControl.IsDialogVisible = true;

            return dialogParentControl;
        }
    }
}