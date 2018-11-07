using System.Threading;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class DialogTest
    {
        private readonly ControlFactory _controlFactory;

        public DialogTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var content = _controlFactory.Create<Control>();
            content.Background = Colors.Blue;
            content.VerticalAlignment = VerticalAlignment.Stretch;
            content.HorizontalAlignment = HorizontalAlignment.Stretch;

            var dialogParentControl = DialogService.CreateContainer(_controlFactory, content);
            
            var dialog = _controlFactory.Create<QuestionDialog>();
            dialog.Message = "Really?";
            /*dialog.Closed += (s, e) =>
            {
                if(dialog.IsConfirmed)
                    dialog.Show();
            };*/
            
            dialog.Show();

            return dialogParentControl;
        }
    }
}