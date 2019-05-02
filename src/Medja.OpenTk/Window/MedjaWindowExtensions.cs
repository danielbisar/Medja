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
			// only default screen supported for now
			var screenWidth = DisplayDevice.Default.Width;
			var screenHeigth = DisplayDevice.Default.Height;

			var position = window.Position;

			position.Width = size.Width;
			position.Height = size.Height;
			position.X = screenWidth / 2 - size.Width / 2;
			position.Y = screenHeigth / 2 - size.Height / 2;
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
