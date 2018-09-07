using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls.Layout
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
				var childPos = Children[i].Position;
				var pos = _positions[i];
				
				childPos.Width = pos.Width;
				childPos.Height = pos.Height;
				childPos.X = pos.X + Position.X;
				childPos.Y = pos.Y + Position.Y;
			}
		}
	}
}
