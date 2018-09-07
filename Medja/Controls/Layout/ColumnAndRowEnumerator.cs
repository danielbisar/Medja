using System.Collections.Generic;
using System.Linq;
using Medja.Primitives;

namespace Medja.Controls.Layout
{
    public class ColumnAndRowEnumerator
    {
        private readonly float _width;
        public IEnumerable<RowDefinition> Rows { get; }
        public IEnumerable<ColumnDefinition> Columns { get; }

        private float _currentX;
        private float _currentY;

        public ColumnAndRowEnumerator(IEnumerable<RowDefinition> rows, IEnumerable<ColumnDefinition> columns, float width)
        {
            _width = width;
            Rows = rows;
            Columns = columns;
        }
        
        /// <summary>
        /// Gets the positions of each element based on the given row and column definitions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Position> GetPositions()
        {
            _currentX = 0;
            _currentY = 0;

            var columnWidth = _width / (float)Columns.Count();
            
            foreach (var row in Rows)
            {
                _currentX = 0;
                
                foreach (var column in Columns)
                {
                    var position = GetPosition(row, column, columnWidth);
                    yield return position;

                    _currentX += position.Width;
                }

                _currentY += row.Height;
            }
        }

        private Position GetPosition(RowDefinition row, ColumnDefinition column, float columnWidth)
        {
            return new Position
            {
                Width = column.Width ?? columnWidth,
                Height = row.Height,
                X = _currentX,
                Y = _currentY
            };
        }
    }
}