using Medja.Controls;

namespace Medja
{
    public interface IMedjaLibrary
    {
        void Run(MedjaApplication application);

        MedjaWindow CreateWindow();
    }
}
