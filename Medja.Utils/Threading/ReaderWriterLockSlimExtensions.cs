using System;
using System.Threading;

namespace Medja.Utils.Threading
{
    /// <summary>
    /// Defines some simple extension methods for <see cref="ReaderWriteLockSlim"/>.
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {
        public static void ReadLock(this ReaderWriterLockSlim rwls, Action action)
        {
            try
            {
                rwls.EnterReadLock();
                action();
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }
        
        public static void WriteLock(this ReaderWriterLockSlim rwls, Action action)
        {
            try
            {
                rwls.EnterWriteLock();
                action();
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }
    }
}