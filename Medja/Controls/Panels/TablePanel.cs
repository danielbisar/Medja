using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
	public class TablePanel : Panel
	{
		private readonly List<Position> _positions;
		private bool _refreshPositions;
		
		public List<RowDefinition> Rows { get; }
		public List<ColumnDefinition> Columns { get; }

		public TablePanel()
		{
			_positions = new List<Position>();
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
			}
		}
	}
}
