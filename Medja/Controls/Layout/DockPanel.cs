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

		public void Add(Control control, Dock dock)
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
			var left = Position.X;
			var top = Position.Y;

			var height = availableSize.Height;
			var width = availableSize.Width;

			foreach (var child in Children.Where(p => p.Visibility != Visibility.Collapsed))
			{
				var dock = _docks[child];
				var childPos = child.Position;

				switch (dock)
				{
					case Dock.Top:
						childPos.X = left;
						childPos.Y = top;
						childPos.Width = width;

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
						childPos.X = width - childPos.Width;
						childPos.Y = top;
						childPos.Height = height;

						width -= childPos.Width;
						break;
					case Dock.Bottom:
						childPos.X = left;
						childPos.Y = top + height - childPos.Height;
						childPos.Width = width;

						height -= childPos.Height;
						break;
					case Dock.Fill:
						if (Children[Children.Count - 1] != child)
							throw new InvalidOperationException("Only the last child can be set to Dock.Fill");

						childPos.X = left;
						childPos.Y = top;
						childPos.Height = height;
						childPos.Width = width;

						break;
					default:
						throw new ArgumentOutOfRangeException("Dock");
				}

				child.Arrange(new Size(childPos.Width, childPos.Height));
			}
		}
	}
}
