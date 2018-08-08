using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Medja
{
	public class PagedListView<TItem> : IEnumerable<TItem>
	{
		private int _currentPos;

		public List<TItem> Source { get; }
		public int PageSize { get; set; }

		public PagedListView(List<TItem> source)
		{
			Source = source;
			PageSize = 10;
		}

		public bool CanMoveNext()
		{
			return _currentPos + PageSize < Source.Count - 1;
		}

		public void MoveNext()
		{
			if (CanMoveNext())
				_currentPos += PageSize;
		}

		public bool CanMovePrevious()
		{
			return _currentPos - PageSize >= 0;
		}

		public void MovePrevious()
		{
			if (CanMovePrevious())
				_currentPos -= PageSize;
		}

		public IEnumerator<TItem> GetEnumerator()
		{
			return GetCurrentItems().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerable<TItem> GetCurrentItems()
		{
			return Source.Skip(_currentPos).Take(PageSize);
		}

		public bool MakeActivePageWith(TItem item)
		{
			var index = Source.IndexOf(item);

			if (index == -1)
				return false;

			_currentPos = 0;

			while (_currentPos + PageSize <= index && CanMoveNext())
				MoveNext();

			return true;
		}
	}
}
