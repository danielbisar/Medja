using Medja.Controls;
using Medja.Theming;

namespace Medja
{
    /// <summary>
    /// Represents the connection point between Medja and the actual used UI framework.
    /// </summary>
    public interface IMedjaLibrary
    {
        MedjaWindow CreateWindow();

        ControlFactory ControlFactory { get; }

        void Run(MedjaApplication application);        
    }
}
