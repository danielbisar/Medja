using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls.Panels
{
	/// <summary>
	/// Creates a table like layout.
	/// </summary>
	/// <remarks>
	/// The children are automatically arranged in the cells resulting of the row and column definitions.
	/// Lets say you have a 2 rows and 3 columns. The first 3 children are added to the first row, the next 3 children
	/// are added to the 2nd row and so forth.
	/// </remarks>
	public class TablePanel : Panel
	{
		private readonly List<Rect> _positions;
		private bool _refreshPositions;

		/// <summary>
		/// Add or remove rows as required.
		/// </summary>
		public List<RowDefinition> Rows { get; }

		/// <summary>
		/// Add or remove columns as required.
		/// </summary>
		public List<ColumnDefinition> Columns { get; }

		public TablePanel()
		{
			_positions = new List<Rect>();
			_refreshPositions = true;
			Rows = new List<RowDefinition>();
			Columns = new List<ColumnDefinition>();
		}

		public void RefreshPositions()
		{
			_refreshPositions = true;
		}

		public override void Arrange(Size availableSize)
		{
			if (_refreshPositions)
			{
				var positions = new ColumnAndRowEnumerator(Rows, Columns, availableSize.Width)
					.GetPositions();

				_positions.Clear();
				_positions.AddRange(positions);
				_refreshPositions = false;
			}

			for (int i = 0; i < Children.Count && i < _positions.Count; i++)
			{
				var child = Children[i];
				var childPos = child.Position;
				var pos = _positions[i];

				if (child.HorizontalAlignment != HorizontalAlignment.Left &&
						child.HorizontalAlignment != HorizontalAlignment.Right)
					childPos.Width = pos.Width - Margin.LeftAndRight;

				if (child.VerticalAlignment != VerticalAlignment.Bottom
						&& child.VerticalAlignment != VerticalAlignment.Top)
					childPos.Height = pos.Height - Margin.TopAndBottom;

				childPos.X = pos.X + Position.X + child.Margin.Left;
				childPos.Y = pos.Y + Position.Y + child.Margin.Right;

				child.Arrange(new Size(childPos.Width, childPos.Height));
			}

            base.Arrange(availableSize);
		}
	}
}
