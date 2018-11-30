using System;
using System.Diagnostics;
using System.Threading;
using ZeroMQ;

namespace Medja.ZeroMQ
{
    /// <summary>
    /// A wrapper around <see cref="ZSocket"/> to help to cleanly dispose the socket and context.
    /// </summary>
    public class ZeroMQSocket : IDisposable
    {
        private readonly ZSocket _socket;
        private readonly int _managedThreadId;
        private readonly ZeroMQSettings _settings;
        private readonly object _goingToDisposeLock;

        private volatile bool _isDisposed;
        
        private volatile bool _goingToDispose;
        /// <summary>
        /// Is true if the <see cref="Dispose"/> was called from another thread. This flags the socket to be disposed
        /// as soon as any of the sockets methods are called. 
        /// </summary>
        public bool GoingToDispose
        {
            get { lock(_goingToDisposeLock) return _goingToDispose; }
            set { lock(_goingToDisposeLock) _goingToDispose = value; }
        }

        /// <summary>
        /// True if in the raw communication a byte prefix is used.
        /// </summary>
        public bool HasMessagePrefix
        {
            get { return _settings.MessagePrefix != null && _settings.MessagePrefix.Length > 0; }
        }

        /// <summary>
        /// <see cref="ZSocket.ReceiveMore"/>.
        /// </summary>
        public bool ReceiveMore
        {
            get
            {
                AssurePreconditionsAndHandleDispose();
                return _socket.ReceiveMore;
            }
        }

        /// <summary>
        /// Returns whether this <see cref="ZeroMQSocket"/> belongs to <see cref="Thread.CurrentThread"/>, i.e.
        /// the <see cref="Thread"/> it was created on.
        /// </summary>
        public bool OwnedByCurrentThread
        {
            get { return _managedThreadId == Thread.CurrentThread.ManagedThreadId; }
        }

        /// <summary>
        /// Gets notified whenever Dispose was called, no matter on the status of the object.
        /// </summary>
        public event EventHandler DisposeWasCalled;
        
        /// <summary>
        /// Gets notified when the internal socket is actually disposed.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="settings">The settings to use to create the socket.</param>
        /// <remarks>Every instance automatically registers itself to <see cref="ZeroMQManager"/>.</remarks>
        public ZeroMQSocket(ZeroMQSettings settings)
        {
            _settings = settings;
            _managedThreadId = Thread.CurrentThread.ManagedThreadId;
            _socket = ZeroMQManager.Instance.Create(settings);
            _goingToDisposeLock = new object();
            
            Disposing += (s, e) => { };
            
            ZeroMQManager.Instance.Add(this);
        }
        
        private void AssureCorrectThread()
        {
            if(!OwnedByCurrentThread)
                throw new InvalidOperationException("The operation must be performed on the same thread " +
                                                    "the object was created on.");
        }

        private bool DisposeIfRequested()
        {
            if (GoingToDispose)
            {
                Dispose();
                return true;
            }

            return false;
        }

        private void AssurePreconditionsAndHandleDispose()
        {
            AssureCorrectThread();

            // now we are on the correct thread so we might call Dispose if it was from another thread before
            if (DisposeIfRequested())
                throw new ObjectDisposedException(nameof(ZeroMQSocket));
        }

        /// <summary>
        /// Sends the given bytes. Prepended by the <see cref="ZeroMQSettings.MessagePrefix"/> if any is set.
        /// </summary>
        /// <param name="bytes">The bytes to send.</param>
        public void Send(params byte[] bytes)
        {
            AssurePreconditionsAndHandleDispose();

            try
            {
                var messagePrefix = _settings.MessagePrefix ?? new byte[0];
            
                using (var frame = ZFrame.Create(bytes.Length + messagePrefix.Length))
                {
                    frame.Write(messagePrefix);
                    frame.Write(bytes);

                    _socket.Send(frame);
                }
            }
            finally
            {
                DisposeIfRequested();
            }
        }

        /// <summary>
        /// Receives bytes. If <see cref="ZeroMQSettings.MessagePrefix"/> was set, the offset of the returned
        /// <see cref="ArraySegment{T}"/> will be configured to start after the prefix.
        /// </summary>
        /// <returns>An array segment pointing to the actual message start.</returns>
        public ArraySegment<byte> Receive()
        {
            AssurePreconditionsAndHandleDispose();
            
            try
            {
                // receive might block for a long time
                using (var frame = _socket.ReceiveFrame())
                {
                    var data = frame.Read();
                    
                    return HasMessagePrefix
                            ? new ArraySegment<byte>(data, _settings.MessagePrefix.Length, data.Length - _settings.MessagePrefix.Length)
                            : new ArraySegment<byte>(data);
                }
            }
            catch (ZException exception)
            {
                Dispose();

                if (exception.Error.Number == ZError.ETERM)
                    Trace.WriteLine(nameof(ZeroMQSocket) + ": ZContext termination detected.");
                
                throw;
            }
            finally
            {
                DisposeIfRequested();
            }
        }
        
        /// <summary>
        /// Since <see cref="Dispose"/> of <see cref="ZSocket"/>s is a little complicated this method assures that
        /// <see cref="Dispose"/> is only allowed from the correct thread. If another thread then the one
        /// the <see cref="ZSocket"/> was created on, it marks this instance for disposing and any other methods call
        /// will trigger the actual <see cref="Dispose"/>. This works around the problem of non-blocking sockets that
        /// need to be Disposed. Your thread should look something like this:
        ///
        /// var thread = new Thread(() =>
        /// {
        ///     using(var socket = ... // create a new one)
        ///     {
        ///         socket.Send/Receive(...);
        ///     }
        /// });
        ///
        /// For sockets that are created on the MainThread and only used there (synchronious calls), Dispose will be
        /// called from <see cref="ZeroMQManager.Destroy"/>. 
        /// </summary>
        public void Dispose()
        {
            GoingToDispose = true;
            DisposeWasCalled?.Invoke(this, EventArgs.Empty);
            
            if (!OwnedByCurrentThread)
                return;

            if (!_isDisposed)
            {
                NotifyDisposing();
                Trace.WriteLine(nameof(ZeroMQSocket) + ": Dispose socket on thread: " + Thread.CurrentThread.ManagedThreadId);
                
                _isDisposed = true;
                _socket.Dispose();
            }
        }

        protected void NotifyDisposing()
        {
            Disposing(this, EventArgs.Empty);
        }
    }
}