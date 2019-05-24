using System;
using Medja.Primitives;
using Medja.Controls;
using Medja.OpenTk.Rendering;
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
			var openTkWindow = ((OpenTkWindow) window).GameWindow;

			// find the display the window is displayed on
			// todo #462
//			var first = DisplayDevice.GetDisplay(DisplayIndex.First);
//			var second = DisplayDevice.GetDisplay(DisplayIndex.Second);
//
//			Console.WriteLine(first.Bounds.Contains((int) window.Position.X, (int) window.Position.Y));
//			Console.WriteLine(second.Bounds.Contains((int) window.Position.X, (int) window.Position.Y));
			
			
			
			// only default screen supported for now
			var screenWidth = DisplayDevice.Default.Width;
			var screenHeight = DisplayDevice.Default.Height;

			var position = window.Position;

			position.Width = size.Width;
			position.Height = size.Height;
			position.X = screenWidth / 2 - size.Width / 2;
			position.Y = screenHeight / 2 - size.Height / 2;
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
