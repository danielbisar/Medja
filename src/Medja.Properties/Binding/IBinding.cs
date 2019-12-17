using System;

namespace Medja.Properties.Binding
{
    /// <summary>
    /// Base interface for all bindings.
    /// </summary>
    public interface IBinding : IDisposable
    {
        /// <summary>
        /// Updates the target property from it's source(s).
        /// </summary>
        void Update();
    }
}