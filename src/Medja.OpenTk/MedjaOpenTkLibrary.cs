using Medja.Controls.Images;

namespace Medja.OpenTk
{
    /// <summary>
    /// Implements the connection between OpenTk and Medja.
    /// </summary>
    public class MedjaOpenTkLibrary : IMedjaLibrary
    {
        private OpenTkWindow _mainWindow;

        public BitmapFactory BitmapFactory { get; }
        
        public MedjaOpenTkLibrary()
        {
            BitmapFactory = new SkiaBitmapFactory();
        }

        /// <inheritdoc />
        public void Run(MedjaApplication application)
        {
            using (_mainWindow = (OpenTkWindow) application.MainWindow)
            {
                _mainWindow.Show();
            }
        }
    }
}
