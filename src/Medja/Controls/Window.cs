using System;
using Medja.Controls.Container;
using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;
using Medja.Utils.Threading.Tasks;

namespace Medja.Controls
{
    /// <summary>
    /// Base class for all windows.
    /// </summary>
    public class Window : ContentControl
    {
        public readonly Property<string> PropertyTitle;
        /// <summary>
        /// The text to display in the window title.
        /// </summary>
        public string Title
        {
            get { return PropertyTitle.Get(); }
            set { PropertyTitle.Set(value); }
        }

        [NonSerialized]
        public readonly Property<WindowState> PropertyState;

        /// <summary>
        /// Gets or sets the window state.
        /// </summary>
        public WindowState State
        {
            get { return PropertyState.Get(); }
            set { PropertyState.Set(value); }
        }

        /// <summary>
        /// Gets if the window is closed or not.
        /// </summary>
        public bool IsClosed { get; private set; }

        public IControlFactory ControlFactory { get; }

        public FocusManager FocusManager { get; }

        public TaskQueue<object> TaskQueue { get; }

        public event EventHandler Closed;

        public Window(IControlFactory controlFactory)
        {
            ControlFactory = controlFactory ?? throw new ArgumentNullException(nameof(controlFactory));
            TaskQueue = new TaskQueue<object>();
            FocusManager = new FocusManager();
            PropertyState = new Property<WindowState>();
            PropertyTitle = new Property<string>();
            PropertyTitle.SetSilent("");


        }

        public override void Arrange(Size availableSize)
        {
            //base.Arrange(availableSize);
            // the base class would use Position and forward it to its content
            // this doesn't make sense for windows, because their position
            // is relative to the desktop and the controls position relative
            // to the window

            var area = new Rect(0, 0, availableSize.Width, availableSize.Height);
            area.Subtract(Margin);
            area.Subtract(Padding);

            ContentArranger.Position(area);
            ContentArranger.Stretch(area);
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        public virtual void Show()
        {
        }

        public virtual void Close()
        {
            IsClosed = true;
            NotifyClosed();
        }

        private void NotifyClosed()
        {
            if (Closed != null)
                Closed(this, EventArgs.Empty);
        }
    }
}
