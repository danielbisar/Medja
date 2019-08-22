using Medja.Primitives;
using Medja.Controls;
using OpenTK;

namespace Medja.OpenTk
{
    public static class MedjaWindowExtensions
    {
        /// <summary>
        /// Centers window on the screen.
        /// </summary>
        /// <param name="window">The Window.</param>
        /// <param name="size">The target size of the window.</param>
        public static void CenterOnScreen(this MedjaWindow window, Size size)
        {
            var primaryDisplay = DisplayDevice.GetDisplay(DisplayIndex.Primary);
            var screenWidth = primaryDisplay.Width;
            var screenHeight = primaryDisplay.Height;

            var position = window.Position;

            position.Width = size.Width;
            position.Height = size.Height;
            position.X = primaryDisplay.Bounds.X + screenWidth / 2f - size.Width / 2f;
            position.Y = primaryDisplay.Bounds.Y + screenHeight / 2f - size.Height / 2f;
        }

        /// <summary>
        /// Centers window on the screen.
        /// </summary>
        /// <param name="window">The Window.</param>
        /// <param name="width">The target width of the window.</param>
        /// /// <param name="height">The target height of the window.</param>
        public static void CenterOnScreen(this MedjaWindow window, float width, float height)
        {
            CenterOnScreen(window, new Size(width, height));
        }
    }
}
