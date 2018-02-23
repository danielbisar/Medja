using System;
using Medja.Controls;

namespace Medja
{
    public class MedjaApplication
    {
        private static MedjaApplication _application;

        public static MedjaApplication Create(IMedjaLibrary library)
        {
            if(_application != null)
                throw new InvalidOperationException("Just one instance can be create of " + nameof(MedjaApplication));

            return _application = new MedjaApplication(library);
        }

        private readonly IMedjaLibrary _library;

        private bool _isMainWindowClosed;

        private MedjaWindow _mainWindow;
        public MedjaWindow MainWindow
        {
            get { return _mainWindow; }
            set
            {
                UnregisterMainWindow();
                _mainWindow = value;
                RegisterMainWindow();
            }
        }
        
        private MedjaApplication(IMedjaLibrary library)
        {
            _library = library;
            _isMainWindowClosed = true;
        }

        public MedjaWindow CreateWindow()
        {
            return _library.CreateWindow();
        }

        private void UnregisterMainWindow()
        {
            if (_mainWindow == null)
                return;

            _mainWindow.Closed -= OnMainWindowClosed;
        }

        private void RegisterMainWindow()
        {
            if(_mainWindow == null)
                throw new InvalidOperationException(nameof(MainWindow) + " cannot be null!");

            _mainWindow.Closed += OnMainWindowClosed;
        }

        private void OnMainWindowClosed(object sender, EventArgs e)
        {
            _isMainWindowClosed = true;
            UnregisterMainWindow();
            Shutdown();
        }

        public void Run()
        {
            _library.Run(this);
        }

        public void Shutdown()
        {
            if (!_isMainWindowClosed && _mainWindow != null)
            {
                _mainWindow.Close();
                return; // function will be called by _mainWindow.Closed again
            }

            //_isRunning = false;
        }
    }
}
