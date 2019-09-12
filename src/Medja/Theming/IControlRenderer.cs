using System;
using Medja.Primitives;

namespace Medja.Theming
{
    public interface IControlRenderer : IDisposable
    {
        bool IsInitialized { get; }
        void Initialize();
        void Resize(MRect position);
        
        void Render(object context);
    }
}
