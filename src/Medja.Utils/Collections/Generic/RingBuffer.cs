using System;
using System.Collections;
using System.Collections.Generic;

namespace Medja.Utils.Collections.Generic
{
	/// <summary>
	/// A ring buffer. Thread safe.
	/// </summary>
	public class RingBuffer<T> : IEnumerable<T>
	{
		protected readonly object _lock = new object();
		protected readonly object _capacityLock = new object();

		protected T[] _buffer;

		private int _tail;
		protected int _head;
		private int _count;

		/// <summary>
		/// Gets the maximum number of items that can be stored in the buffer.
		/// </summary>
		public int Capacity
		{
			get
			{
				lock (_capacityLock)
					return _buffer.Length;
			}
		}

		/// <summary>
		/// Gets the count of enqueued/pushed items.
		/// </summary>
		public int Count
		{
			get
			{
				lock (_lock)
					return _count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Medja.Utils.Collections.Generic.RingBuffer`1"/> is full and has at least one entry.
		/// </summary>
		/// <value><c>true</c> if is full and not empty; otherwise, <c>false</c>.</value>
		public bool IsFullAndNotEmpty
		{
			get
			{
				lock (_lock)
				{
					lock (_capacityLock)
					{
						return _count == _buffer.Length && _buffer.Length > 0;
					}
				}
			}
		}

		/// <summary>
		/// Occurs when an item was added or removed.
		/// </summary>
		public event EventHandler<RingBufferEventArgs> Modified;

		public RingBuffer(int length)
		{
			_buffer = new T[length];
			_head = _tail = -1;
			_count = 0;
		}

		private void NotifyModified(RingBufferOperation operation)
		{
			if (Modified != null)
				Modified(this, new RingBufferEventArgs(operation));
		}

		public void Clear()
		{
			lock (_lock)
			{
				_head = _tail = -1;
				_count = 0;
			}

			NotifyModified(RingBufferOperation.Clear);
		}

		public void Push(T item)
		{
			lock (_lock)
			{
				_head = GetNextIndex(_head);
				_buffer[_head] = item;

				if (_count < _buffer.Length)
					_count++;
				else
					_tail = GetNextIndex(_tail);

				if (_tail == -1)
					_tail = 0;
			}

			NotifyModified(RingBufferOperation.Push);
		}

		public void PushItems(IEnumerable<T> items)
		{
			lock (_lock)
			{
				foreach (var item in items)
					Push(item);
			}
		}

		public T Pop()
		{
			var result = default(T);

			lock (_lock)
			{
				if (_count > 0)
				{
					result = _buffer[_tail];

					_tail = GetNextIndex(_tail);
					_count--;
				}
			}

			NotifyModified(RingBufferOperation.Pop);

			return result;
		}

		public T Peek()
		{
			lock (_lock)
			{
				if (_count > 0)
					return _buffer[_tail];
			}

			return default(T);
		}

		public T PeekHead()
		{
			lock (_lock)
			{
				if (_count > 0)
					return _buffer[_head];
			}

			return default(T);
		}

		/// <summary>
		/// Gets the first n items of the buffer (the first pushed not overwritten ones)
		/// </summary>
		/// <returns>The items.</returns>
		/// <param name="length">How much items to get max.</param>
		public T[] PeekItems(int length)
		{
			lock (_lock)
			{
				return PeekItems(length, _tail, _count);
			}
		}

		/// <summary>
		/// Peeks the items.
		/// </summary>
		/// <returns>The items.</returns>
		/// <param name="length">The max amount of items to return.</param>
		/// <param name="tail">The position to start returning items from.</param>
		/// <param name="count">The count of items existing in the buffer. Can be useful if you only want to allow access to a subset of items.</param>
		protected T[] PeekItems(int length, int tail, int count)
		{
			lock (_lock)
			{
				if (length > count)
					length = count;

				var result = new T[length];

				while (length > 0)
				{
					var resIndex = result.Length - length;
					var bufIndex = (tail + resIndex).ExtendedModulo(_buffer.Length);

					result[resIndex] = _buffer[bufIndex];
					length--;
				}

				return result;
			}
		}

		private int GetNextIndex(int currentIndex)
		{
			return (currentIndex + 1) % _buffer.Length;
		}

		public void Resize(int length)
		{
			if (length == _buffer.Length)
				return;

			lock (_capacityLock)
			{
				lock (_lock)
				{
					if (_buffer.Length < length)
					{
						Array.Resize(ref _buffer, length);

						if (_tail >= _head)
						{
							var count = _tail - _head;
							var targetIndex = _buffer.Length - count;

							Array.Copy(_buffer, _tail, _buffer, targetIndex, count);
							_tail = targetIndex;
						}
					}
					else // smaller
					{
						if (_tail >= _head)
						{
							var count = _buffer.Length - length;
							var sourceIndex = _buffer.Length - count;

							Array.Copy(_buffer, sourceIndex, _buffer, _head + 1, count);
							_tail = _head + 1;
						}

						Array.Resize(ref _buffer, length);
					}
				}
			}
		}

		/// <summary>
		/// Gets all items starting from head going to tail.
		/// </summary>
		/// <returns>All items.</returns>
		public IEnumerable<T> FromHead()
		{
			lock (_lock)
			{
				var itemCount = _count;

				while (itemCount > 0)
				{
					var bufIndex = (_head - (_count - itemCount)).ExtendedModulo(_buffer.Length);
					yield return _buffer[bufIndex];

					itemCount--;
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new RingBufferEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class RingBufferEnumerator : IEnumerator<T>
		{
			private readonly RingBuffer<T> _buffer;
			private int _count;
			private int _pos;

			private T _current;
			public T Current
			{
				get { return _current; }
			}

			object IEnumerator.Current
			{
				get { return _current; }
			}

			public RingBufferEnumerator(RingBuffer<T> buffer)
			{
				_buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
				_count = _buffer.Count;
				_pos = -1;
			}

			public void Dispose()
			{
				_current = default(T);
			}

			public bool MoveNext()
			{
				_pos++;

				var result = _pos < _count;

				if (result)
					_current = _buffer._buffer[(_buffer._tail + _pos) % _buffer.Capacity];

				return result;
			}

			public void Reset()
			{
				_pos = -1;
			}
		}
	}
}
