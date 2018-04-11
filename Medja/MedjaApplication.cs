﻿using System;
using Medja.Controls;

namespace Medja
{
    /// <summary>
    /// Represents an application instance (handling windows and other library stuff)
    /// 
    /// currently we just allow one actual window per application.
    /// </summary>
    public class MedjaApplication
    {
        private static MedjaApplication _application;
        
        /// <summary>
        /// Initializes the MedjaApplication, not thread-safe.
        /// </summary>
        /// <param name="library">The used library implementation.</param>
        /// <returns>The application instance.</returns>
        public static MedjaApplication Create(IMedjaLibrary library)
        {
            if(_application != null)
                throw new InvalidOperationException("Just one instance can be create of " + nameof(MedjaApplication));

            return _application = new MedjaApplication(library);
        }

        private readonly IMedjaLibrary _library;

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
            UnregisterMainWindow();
            Shutdown();
        }

        /// <summary>
        /// Closes the application in a soft way (does not quit if the window prevents it).
        /// </summary>
        public void Shutdown()
        {
            if (_mainWindow != null && !_mainWindow.IsClosed)
            {
                _mainWindow.Close();
                return; // function will be called by _mainWindow.Closed again
            }

            //_isRunning = false;
        }

        /// <summary>
        /// Creates a new window. This method exists so you don't need to worry about the acutal used library instance.
        /// </summary>
        /// <returns>The new window instance provided by the library.</returns>
        public MedjaWindow CreateWindow()
        {
            return _library.CreateWindow();
        }

        /// <summary>
        /// Starts the application (shows the main window, starts message queue, ...)
        /// </summary>
        public void Run()
        {
            _library.Run(this);
        }
    }
}