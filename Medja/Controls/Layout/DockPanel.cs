using System;
using System.Collections.Generic;
using Medja.Primitives;
using System.Linq;

namespace Medja.Controls
{
	/// <summary>
	/// A simple dock panel.
	/// 
	/// To add or remove children use Add, Remove of this class not the Children list directly.
	/// </summary>
	public class DockPanel : Panel
	{
		private Dictionary<Control, Dock> _docks;

		public DockPanel()
		{
			_docks = new Dictionary<Control, Dock>();
		}

		public void Add(Dock dock, Control control)
		{
			_docks.Add(control, dock);
			Children.Add(control);
		}

		public void Remove(Control control)
		{
			_docks.Remove(control);
			Children.Remove(control);
		}

		public override Size Measure(Size availableSize)
		{
			//foreach (var child in Children)
			//    child.Measure(availableSize);

			return availableSize;
		}

		public override void Arrange(Size availableSize)
		{
			var left = Position.X + Padding.Left;
			var top = Position.Y + Padding.Top;

			var height = availableSize.Height - Padding.TopAndBottom;
			var width = availableSize.Width - Padding.LeftAndRight;

			foreach (var child in Children.Where(p => p.Visibility != Visibility.Collapsed))
			{
				var dock = _docks[child];
				var childPos = child.Position;

				switch (dock)
				{
					case Dock.Top:
						childPos.X = GetChildPosX(child, left, width);
						childPos.Y = top;
						childPos.Width = GetChildWidth(child, width);

						top += childPos.Height;
						height -= childPos.Height;
						break;
					case Dock.Left:
						childPos.X = left;
						childPos.Y = top;
						childPos.Height = height;

						left += childPos.Width;
						width -= childPos.Width;
						break;
					case Dock.Right:
						childPos.X = left + width - childPos.Width;
						childPos.Y = top;
						childPos.Height = height;

						width -= childPos.Width;
						break;
					case Dock.Bottom:
						childPos.X = GetChildPosX(child, left, width);
						childPos.Y = top + height - childPos.Height;
						childPos.Width = GetChildWidth(child, width);

						height -= childPos.Height;
						break;
					case Dock.Fill:
						if (Children[Children.Count - 1] != child)
							throw new InvalidOperationException("Only the last child can be set to Dock.Fill");

						childPos.X = left;
						childPos.Y = top;
						childPos.Height = GetFillHeight(child, height);
						childPos.Width = width;

						break;
					default:
						throw new ArgumentOutOfRangeException("Dock");
				}

				child.Arrange(new Size(childPos.Width, childPos.Height));
			}
		}

		private float GetChildPosX(Control child, float left, float width)
		{
			if (child.HorizontalAlignment != HorizontalAlignment.Right)
				return left;

			return left + width - GetChildWidth(child, width);
		}

		private float GetChildWidth(Control child, float width)
		{
			return child.HorizontalAlignment != HorizontalAlignment.Left && child.HorizontalAlignment != HorizontalAlignment.Right
									   ? width
							: Math.Min(child.Position.Width, width);
		}

		private float GetFillHeight(Control child, float availableHeight)
		{
			var verticalAlignment = child.VerticalAlignment;

			if (verticalAlignment == VerticalAlignment.None
				|| verticalAlignment == VerticalAlignment.Stretch)
				return availableHeight;

			return child.Position.Height;
		}
	}
}
