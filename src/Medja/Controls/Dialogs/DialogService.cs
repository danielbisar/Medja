using System;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls.Dialogs
{
    /// <summary>
    /// Helper class to make working with dialogs easier.
    /// </summary>
    public static class DialogService
    {
        private static readonly object _lock;
        private static DialogParentControl _dialogParentControl;

        static DialogService()
        {
            _lock = new object();
        }

        public static DialogParentControl CreateContainer(IControlFactory controlFactory, Control content)
        {
            lock (_lock)
            {
                if(_dialogParentControl != null)
                    throw new InvalidOperationException("Container was created already.");

                _dialogParentControl = controlFactory.Create<DialogParentControl>();
                _dialogParentControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                _dialogParentControl.VerticalAlignment = VerticalAlignment.Stretch;
                _dialogParentControl.Content = content;

                return _dialogParentControl;
            }
        }

        public static void Show(Dialog dialog)
        {
            lock (_lock)
            {
                if(_dialogParentControl == null)
                    throw new InvalidOperationException("Call create container first.");

                _dialogParentControl.DialogControl = dialog;
                _dialogParentControl.IsDialogVisible = true;
            }
        }

        public static void Hide(Dialog dialog = null)
        {
            lock (_lock)
            {
                if(_dialogParentControl == null)
                    throw new InvalidOperationException("Call create container first.");

                if (dialog == null || ReferenceEquals(_dialogParentControl.DialogControl, dialog))
                {
                    _dialogParentControl.IsDialogVisible = false;
                    _dialogParentControl.DialogControl = null;
                }
            }
        }
    }
}
