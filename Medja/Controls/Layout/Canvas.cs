using System;
using Medja.Primitives;

namespace Medja.Controls
{
	/// <summary>
	/// A Panel that allows its children to be positioned freely.
	/// </summary>
	public class Canvas : Panel
	{
		public Canvas()
		{
		}

		public override void Arrange(Size availableSize)
		{
			foreach (var child in Children)
				child.Arrange(new Size(child.Position.Width, child.Position.Height));
		}
	}
}
