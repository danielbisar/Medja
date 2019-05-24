// ReSharper disable InconsistentNaming

namespace Medja.NativeInterfaces
{
    /// <summary>
    /// Interface for various OS dependent functionalities.
    /// </summary>
    public interface IMedjaNative
    {
        /// <summary>
        /// Tries to call a method on the native library.
        /// </summary>
        void CheckIfLibIsAvailable();
        
        /// <summary>
        /// Gets <see cref="MedjaSystemInfo"/>s.
        /// </summary>
        /// <returns>The <see cref="MedjaSystemInfo"/>s.</returns>
        MedjaSystemInfo GetSystemInfo();
    }
}