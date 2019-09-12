using System;
using Medja.Controls;
using Medja.Properties;

namespace Medja
{
    /// <summary>
    /// Represents an application instance (handling windows and other library stuff)
    /// </summary>
    public class MedjaApplication
    {
        private static MedjaApplication _application;
        /// <summary>
        /// Gets the default instance. This is initialized via <see cref="Create"/>.
        /// </summary>
        public static MedjaApplication Instance
        {
            get { return _application; }
        }

        /// <summary>
        /// Initializes the MedjaApplication, not thread-safe.
        /// </summary>
        /// <param name="library">The used library implementation.</param>
        /// <returns>The application instance.</returns>
        public static MedjaApplication Create(IMedjaLibrary library)
        {
            if (_application != null)
                throw new InvalidOperationException("Just one instance can be create of " + nameof(MedjaApplication));

            return _application = new MedjaApplication(library);
        }
        

        public readonly Property<Window> PropertyMainWindow;
        public Window MainWindow
        {
            get { return PropertyMainWindow.Get(); }
            set { PropertyMainWindow.Set(value); }
        }

        public IMedjaLibrary Library { get; }

        public event EventHandler<ShutdownEventArgs> ShutdownEvent;

        private MedjaApplication(IMedjaLibrary library)
        {
            Library = library;
            
            PropertyMainWindow = new Property<Window>();
            PropertyMainWindow.PropertyChanged += OnMainWindowChanged;
        }

        private void OnMainWindowChanged(object sender, PropertyChangedEventArgs e)
        {
            var oldWindow = e.OldValue as Window;
            var newWindow = e.NewValue as Window;
            
            UnregisterMainWindow(oldWindow);
            RegisterMainWindow(newWindow);
        }

        private void UnregisterMainWindow(Window window)
        {
            if (window == null)
                return;

            window.Closed -= OnMainWindowClosed;
        }

        private void RegisterMainWindow(Window window)
        {
            if (window == null)
                throw new InvalidOperationException(nameof(MainWindow) + " cannot be null!");

            window.Closed += OnMainWindowClosed;
        }

        private void OnMainWindowClosed(object sender, EventArgs e)
        {
            UnregisterMainWindow(MainWindow);
            Shutdown();
        }

        /// <summary>
        /// Closes the application in a soft way (does not quit if the window prevents it).
        /// </summary>
        public void Shutdown()
        {
            var hasMainWindow = MainWindow != null && !MainWindow.IsClosed;

            if (hasMainWindow)
            {
                var cancelShutdown = NotifyShutdown();

                if (cancelShutdown)
                    return;

                MainWindow.Close();
                // this function will be called by _mainWindow.Closed again
            }
        }

        private bool NotifyShutdown()
        {
            var eventArgs = new ShutdownEventArgs();
            ShutdownEvent?.Invoke(this, eventArgs);

            return eventArgs.Cancel;
        }

        /// <summary>
        /// Starts the application (shows the main window, starts message queue, ...)
        /// </summary>
        public void Run()
        {
            Library.Run(this);
        }
    }
}
