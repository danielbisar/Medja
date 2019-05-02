using System;
using System.Threading;

namespace Medja.Utils.Threading
{
    public static class WaitHandleExtensions
    {
        public static bool IsDisposed(this WaitHandle waitHandle)
        {
            try
            {
                return waitHandle.SafeWaitHandle.IsClosed;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
        }
    }
}