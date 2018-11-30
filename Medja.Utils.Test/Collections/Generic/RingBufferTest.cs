using System;
using System.Linq;
using Medja.Utils.Collections.Generic;
using Xunit;

namespace Medja.Utils.Test.Collections.Generic
{
	public class RingBufferTest
	{
		[Fact]
		public void ConstructorTest()
		{
			var ringBuffer = new RingBuffer<byte>(0);
			Assert.NotNull(ringBuffer);
		}

		[Fact]
		public void ConstructorTest2()
		{
			var ringBuffer = new RingBuffer<byte>(1);
			Assert.NotNull(ringBuffer);
		}

		[Fact]
		public void LengthTest()
		{
			var ringBuffer = new RingBuffer<byte>(1);
			Assert.Equal(1, ringBuffer.Capacity);
		}

		[Fact]
		public void OverflowBehaviorTest()
		{
			var ringBuffer = new RingBuffer<byte>(5);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);
			ringBuffer.Push(4);
			ringBuffer.Push(5); // overwrite 0
			ringBuffer.Push(6); // overwrite 1
			ringBuffer.Push(7); // overwrite 2
			ringBuffer.Push(8); // overwrite 3

			Assert.Equal(4, ringBuffer.Pop());
			Assert.Equal(5, ringBuffer.Pop());
			Assert.Equal(6, ringBuffer.Pop());
			Assert.Equal(7, ringBuffer.Pop());
			Assert.Equal(8, ringBuffer.Pop());

			Assert.Equal(0, ringBuffer.Pop());
		}

		[Fact]
		public void OverflowBehaviorEdgeTest()
		{
			var ringBuffer = new RingBuffer<byte>(3);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);

			Assert.Equal(1, ringBuffer.Pop());
			Assert.Equal(2, ringBuffer.Pop());
			Assert.Equal(3, ringBuffer.Pop());

			// this is an empty item not the pushed 0
			Assert.Equal(0, ringBuffer.Pop());
		}

		[Fact]
		public void PushPopTest()
		{
			var ringBuffer = new RingBuffer<byte>(3);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);

			Assert.Equal(0, ringBuffer.Pop());
			Assert.Equal(1, ringBuffer.Pop());
			Assert.Equal(2, ringBuffer.Pop());
		}

		[Fact]
		public void Resize_SmallerTest()
		{
			var ringBuffer = new RingBuffer<byte>(5);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);
			ringBuffer.Push(4);
			ringBuffer.Push(5);

			ringBuffer.Resize(3);

			Assert.Equal(3, ringBuffer.Pop());
			Assert.Equal(4, ringBuffer.Pop());
			Assert.Equal(5, ringBuffer.Pop());
		}

		[Fact]
		public void Resize_MakeBigger()
		{
			var ringBuffer = new RingBuffer<byte>(5);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);
			ringBuffer.Push(4);
			ringBuffer.Push(5);
			ringBuffer.Push(6);
			ringBuffer.Push(7);
			ringBuffer.Push(8);

			ringBuffer.Resize(8);

			Assert.Equal(4, ringBuffer.Pop());
			Assert.Equal(5, ringBuffer.Pop());
			Assert.Equal(6, ringBuffer.Pop());
			Assert.Equal(7, ringBuffer.Pop());
			Assert.Equal(8, ringBuffer.Pop());

			// empty
			Assert.Equal(0, ringBuffer.Pop());

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);
			ringBuffer.Push(4);
			ringBuffer.Push(5);
			ringBuffer.Push(6);
			ringBuffer.Push(7);

			Assert.Equal(0, ringBuffer.Pop());
			Assert.Equal(1, ringBuffer.Pop());
			Assert.Equal(2, ringBuffer.Pop());
			Assert.Equal(3, ringBuffer.Pop());
			Assert.Equal(4, ringBuffer.Pop());
			Assert.Equal(5, ringBuffer.Pop());
			Assert.Equal(6, ringBuffer.Pop());
			Assert.Equal(7, ringBuffer.Pop());

			// empty
			Assert.Equal(0, ringBuffer.Pop());
		}

		[Fact]
		public void EnumeratorTest()
		{
			var ringBuffer = new RingBuffer<int>(5);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);
			ringBuffer.Push(4);

			var expectedValues = new[] {0, 1, 2, 3, 4};
			var loopCount = 0;
			var expectedEnumerator = expectedValues.GetEnumerator();

			foreach (var item in ringBuffer)
			{
				loopCount++;
				Assert.True(expectedEnumerator.MoveNext());
				Assert.Equal(expectedEnumerator.Current, item);
			}

			// "Loop was executed not the same times actual items exist"
			Assert.Equal(5, loopCount);
		}

		[Fact]
		public void EnumeratorWithOverflowTest()
		{
			var ringBuffer = new RingBuffer<int>(5);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);
			ringBuffer.Push(4);
			ringBuffer.Push(5);

			var expectedValues = new[] {1, 2, 3, 4, 5};
			var expectedEnumerator = expectedValues.GetEnumerator();
			var loopCount = 0;

			foreach (var item in ringBuffer)
			{
				loopCount++;
				Assert.True(expectedEnumerator.MoveNext());
				Assert.Equal(expectedEnumerator.Current, item);
			}

			// "Loop was executed not the same times actual items exist"
			Assert.Equal(5, loopCount);
		}

		[Fact]
		public void EnumeratorSmallerTest()
		{
			var ringBuffer = new RingBuffer<int>(5);

			ringBuffer.Push(0);
			ringBuffer.Push(1);
			ringBuffer.Push(2);
			ringBuffer.Push(3);

			var expectedValues = new[] {0, 1, 2, 3};
			var loopCount = 0;
			var expectedEnumerator = expectedValues.GetEnumerator();

			foreach (var item in ringBuffer)
			{
				loopCount++;
				Assert.True(expectedEnumerator.MoveNext());
				Assert.Equal(expectedEnumerator.Current, item);
			}

			// "Loop was executed not the same times actual items exist"
			Assert.Equal(4, loopCount);
		}

		[Fact]
		public void PeekItemsTest()
		{
			var ringBuffer = new RingBuffer<int>(10);

			for (int i = 0; i < 10; i++)
				ringBuffer.Push(i);

			var items = ringBuffer.PeekItems(10);

			Assert.Equal(10, ringBuffer.Count);
			Assert.Equal(10, items.Length);

			foreach (var item in items)
				Console.WriteLine(item);

			for (int i = 0; i < items.Length; i++)
				Assert.Equal(i, items[i]);
		}

		[Fact]
		public void PeekItemsTest2()
		{
			// tests how peek does behave if _tail is never moved
			var ringBuffer = new RingBuffer<int>(1000);

			for (int i = 0; i < 10; i++)
				ringBuffer.Push(i);

			var items = ringBuffer.PeekItems(5);

			Assert.Equal(10, ringBuffer.Count);
			Assert.Equal(5, items.Length);

			foreach (var item in items)
				Console.WriteLine(item);

			for (int i = 0; i < items.Length; i++)
				Assert.Equal(i, items[i]);
		}

		[Fact]
		public void PeekMoreItemsTest()
		{
			var ringBuffer = new RingBuffer<int>(5);

			for (int i = 0; i < 5; i++)
				ringBuffer.Push(i);

			var items = ringBuffer.PeekItems(10);

			Assert.Equal(5, items.Length);
		}

		[Fact]
		public void PeekLessItemsTest()
		{
			var ringBuffer = new RingBuffer<int>(5);

			for (int i = 0; i < 5; i++)
				ringBuffer.Push(i);

			var items = ringBuffer.PeekItems(2);

			Assert.Equal(2, items.Length);
			Assert.Equal(5, ringBuffer.Count);

			foreach (var item in items)
				Console.WriteLine(item);

			for (int i = 0; i < items.Length; i++)
				Assert.Equal(i, items[i]);
		}

		[Fact]
		public void FromHeadWhileTest()
		{
			var ringBuffer = new RingBuffer<int>(10);

			for (int i = 0; i < 10; i++)
				ringBuffer.Push(i);

			var items = ringBuffer.FromHead().Where(p => p > 5).ToList();

			foreach (var item in items)
				Console.WriteLine(item);

			Assert.Equal(9, items[0]);
			Assert.Equal(8, items[1]);
			Assert.Equal(7, items[2]);
			Assert.Equal(6, items[3]);
			Assert.Equal(4, items.Count);
		}

		[Fact]
		public void FromHeadTest()
		{
			var ringBuffer = new RingBuffer<int>(10);

			for (int i = 0; i < 10; i++)
				ringBuffer.Push(i);

			var items = ringBuffer.FromHead().ToList();

			foreach (var item in items)
				Console.WriteLine(item);

			Assert.Equal(9, items[0]);
			Assert.Equal(8, items[1]);
			Assert.Equal(7, items[2]);
			Assert.Equal(6, items[3]);
			Assert.Equal(5, items[4]);
			Assert.Equal(4, items[5]);
			Assert.Equal(3, items[6]);
			Assert.Equal(2, items[7]);
			Assert.Equal(1, items[8]);
			Assert.Equal(0, items[9]);

			Assert.Equal(10, items.Count);
		}
	}
}