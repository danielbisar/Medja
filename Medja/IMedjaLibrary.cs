using Medja.Controls;
using Medja.Theming;
using Medja.Utils.Threading.Tasks;

namespace Medja
{
    /// <summary>
    /// Represents the connection point between Medja and the actual used UI framework.
    /// </summary>
    public interface IMedjaLibrary
    {
        /// <summary>
        /// Gets the current <see cref="ControlFactory"/>. This allows theming of the UI.
        /// </summary>
        ControlFactory ControlFactory { get; }
        
        /// <summary>
        /// Gets the task queue - you can use this the same way you would use a Dispatcher. If you want to execute a
        /// task/method on the UI thread.
        /// </summary>
        TaskQueue<object> TaskQueue { get; }

        /// <summary>
        /// Creates a new window; currently we support basically just one.
        /// </summary>
        /// <returns>The new <see cref="MedjaWindow"/> instance.</returns>
        MedjaWindow CreateWindow();

        /// <summary>
        /// Enters the Main-Loop of the program. Do not call this method directly but call <see cref="MedjaApplication.Run"/> instead.
        /// </summary>
        /// <param name="application">The current <see cref="MedjaApplication"/> instance.</param>
        void Run(MedjaApplication application);
    }
}
