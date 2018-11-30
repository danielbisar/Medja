using System.Linq;
using System.Threading.Tasks;
using Medja.Utils.Collections.Concurrent;
using Medja.Utils.Threading.Tasks;
using Xunit;

namespace Medja.Utils.Test.Collections.Concurrent
{
    public class ConcurrentHashSetTest
    {
        // single threaded tests
        
        [Fact]
        public void CountCounts()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();
            
            Assert.Empty(concurrentHashSet);
            concurrentHashSet.Add(1);
            Assert.Collection(concurrentHashSet, i => Assert.Equal(1, i));
            
            concurrentHashSet.Add(2);
            Assert.Collection(concurrentHashSet, i => Assert.Equal(1, i), i => Assert.Equal(2, i));
            
            concurrentHashSet.Remove(1);
            Assert.Collection(concurrentHashSet, i => Assert.Equal(2, i));
            
            concurrentHashSet.Remove(2);
            Assert.Empty(concurrentHashSet);
        }
        
        [Fact]
        public void AddsNoDuplicate()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();
            
            Assert.True(concurrentHashSet.Add(1));
            Assert.False(concurrentHashSet.Add(1));
            Assert.Single(concurrentHashSet, i => 1 == i);
        }
        
        [Fact]
        public void RemovesItem()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();
            
            Assert.True(concurrentHashSet.Add(1));
            Assert.True(concurrentHashSet.Remove(1));
            Assert.Empty(concurrentHashSet);
        }

        [Fact]
        public void ClearRemovesAll()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();
            concurrentHashSet.Add(1);
            concurrentHashSet.Add(2);
            concurrentHashSet.Add(3);
            
            concurrentHashSet.Clear();
            
            Assert.Empty(concurrentHashSet);
        }
        
        [Fact]
        public void GetCopyDoesNotReturnInternalReference()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();
            concurrentHashSet.Add(1);

            var copyOfHashSet = concurrentHashSet.GetCopy();
            copyOfHashSet.Clear();
            
            Assert.NotEqual(copyOfHashSet.Count, concurrentHashSet.Count);
        }
        
        [Fact]
        public void GetThreadSafeCopyDoesNotReturnOrUseReference()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();
            concurrentHashSet.Add(1);

            var copyOfHashSet = concurrentHashSet.GetThreadSafeCopy();
            copyOfHashSet.Clear();
            
            Assert.NotEqual(copyOfHashSet.Count, concurrentHashSet.Count);
        }
        
        // multi threaded tests
        
        [Fact]
        public void MultipleAddThreadsDoNotProduceException()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();
            var tasks = TaskHelper.CreateNAsList(5, () =>
            {
                for (int i = 0; i < 100; i++)
                    concurrentHashSet.Add(i);
            });

            tasks.StartAll();
            tasks.WaitAll();
            
            Assert.Equal(100, concurrentHashSet.Count);
        }
        
        [Fact]
        public void MultipleRemoveThreadsDoNotProduceException()
        {
            var concurrentHashSet = new ConcurrentHashSet<int>();

            for (int i = 0; i < 100; i++)
                concurrentHashSet.Add(i);

            Parallel.ForEach(concurrentHashSet, item => concurrentHashSet.Remove(item));
            Assert.Empty(concurrentHashSet);
        }
    }
}