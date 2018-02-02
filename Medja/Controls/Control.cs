namespace Medja.Controls
{
    /// <summary>
    /// Any control should inherit from this class.
    /// </summary>
    /// <remarks>
    /// 
    /// ? Background/Foreground colors should be designed via renderers?
    /// 
    /// </remarks>
    public class Control : MObject
    {
        public Size GetMinSize()
        {
            return new Size(0, 0);
        }
    }
}
