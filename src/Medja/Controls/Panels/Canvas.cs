using Medja.Primitives;
using System.Linq;

namespace Medja.Controls
{
	/// <summary>
	/// A Panel that allows its children to be positioned freely.
	/// Use attached property X and Y available in this class.
	/// </summary>
	public class Canvas : Panel
	{
		// the attached X and Y is the initial position provided by the control
		public static readonly int AttachedXId = AttachedPropertyIdFactory.NewId();
		public static readonly int AttachedYId = AttachedPropertyIdFactory.NewId();

		public override void Arrange(Size availableSize)
		{
			var x = Position.X;
			var y = Position.Y;

			foreach (var child in Children.Where(HasAttachedXY))
			{
				child.Position.X = x + child.GetAttachedProperty<float>(AttachedXId) + child.Margin.Left;
				child.Position.Y = y + child.GetAttachedProperty<float>(AttachedYId) + child.Margin.Top;

				child.Arrange(new Size(child.Position.Width, child.Position.Height));
			}
		}

		public bool HasAttachedXY(Control child)
		{
			return child.AttachedProperties.ContainsKey(AttachedXId)
						&& child.AttachedProperties.ContainsKey(AttachedYId);
		}
	}
}
