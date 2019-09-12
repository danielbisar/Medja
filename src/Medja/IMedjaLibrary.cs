using Medja.Controls.Images;

namespace Medja
{
    /// <summary>
    /// Represents the connection point between Medja and the used rendering layer.
    /// </summary>
    public interface IMedjaLibrary
    {
        /// <summary>
        /// Gets the current <see cref="BitmapFactory"/>.
        /// </summary>
        BitmapFactory BitmapFactory { get; }
        
        /// <summary>
        /// Starts the main loop of the program.
        /// Do not call this method directly but call <see cref="MedjaApplication.Run"/> instead.
        /// </summary>
        /// <param name="application">The current <see cref="MedjaApplication"/> instance.</param>
        void Run(MedjaApplication application);
    }
}
