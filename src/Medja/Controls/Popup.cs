using Medja.Controls.Container;

namespace Medja.Controls
{
    /// <summary>
    /// A popup control which can be used to display a control over others.
    /// </summary>
    public class Popup : ContentControl
    {
        public Popup()
        {
            IsTopMost = true;
        }
    }
}
