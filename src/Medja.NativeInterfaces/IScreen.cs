namespace Medja.NativeInterfaces
{
    /// <summary>
    /// Representing a screen.
    /// </summary>
    public interface IScreen
    {
        /// <summary>
        /// Gets the dpi value used for width.
        /// </summary>
        /// <returns>The current DPI.</returns>
        float GetWidthDPI();
        
        /// <summary>
        /// Gets the dpi value used for height.
        /// </summary>
        /// <returns>The current DPI.</returns>
        float GetHeigthDPI();
    }
}