using System.Drawing;

namespace Medja.OpenTk
{
	internal class MouseState
	{
		public Point Position { get; set; }
		public bool IsLeftButtonDown { get; set; }
		public bool IsMouseMove { get; set; }
		public float WheelDelta { get; set; }

		public override string ToString()
		{
			return string.Format("IsLeftButtonDown = {0}, IsMouseMove = {1}, Position = {2}, WheelDelta = {3}", IsLeftButtonDown, IsMouseMove, Position, WheelDelta);
		}
	}
}
