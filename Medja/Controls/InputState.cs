using System;

namespace Medja
{
    public class InputState
    {
        public Property<bool> IsMouseOverProperty { get; }
        public bool IsMouseOver
        {
            get { return IsMouseOverProperty.Get(); }
            set { IsMouseOverProperty.Set(value); }
        }

        public Property<bool> IsLeftMouseDownProperty { get; }
        public bool IsLeftMouseDown
        {
            get { return IsLeftMouseDownProperty.Get(); }
            set { IsLeftMouseDownProperty.Set(value); }
        }

        public Property<bool> IsDragProperty { get; }
        public bool IsDrag
        {
            get { return IsDragProperty.Get(); }
            set { IsDragProperty.Set(value); }
        }

        public event EventHandler MouseClicked;

        public InputState()
        {
            IsLeftMouseDownProperty = new Property<bool>();
            IsLeftMouseDownProperty.PropertyChanged += OnIsLeftMouseDownChanged;

            IsMouseOverProperty = new Property<bool>();
            IsDragProperty = new Property<bool>();
        }

        private void OnIsLeftMouseDownChanged(IProperty property)
        {
            if (!IsLeftMouseDown && !IsDrag)
                NotifyClicked();
        }

        public void MouseMoved()
        {
            IsDrag = IsLeftMouseDown;
        }

        private void NotifyClicked()
        {
            if (MouseClicked != null)
                MouseClicked(this, EventArgs.Empty);
        }

        public void Clear()
        {
            IsDrag = false;
            IsLeftMouseDown = false;
            IsMouseOver = false;
        }
    }
}
