using System;

namespace Medja
{
    public class InputState
    {        
        public bool IsMouseOver { get; set; }

        private bool _isMouseDown;
        public bool IsMouseDown
        {
            get { return _isMouseDown; }
            set
            {
                var oldValue = _isMouseDown;
                _isMouseDown = value;

                if (!value && oldValue)
                    NotifyClicked();                
            }
        }        

        public event EventHandler MouseClicked;

        private void NotifyClicked()
        {
            if (MouseClicked != null)
                MouseClicked(this, EventArgs.Empty);
        }
    }
}
