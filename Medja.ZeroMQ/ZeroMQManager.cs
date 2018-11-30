using System;
using System.Diagnostics;
using Medja.Utils.Collections.Concurrent;
using Medja.Utils.Collections.Generic;
using ZeroMQ;

namespace Medja.ZeroMQ
{
    /// <summary>
    /// ZeroMQ is a bitch regarding closing, so this helper assures everything is going clean.
    /// </summary>
    public class ZeroMQManager : IDisposable
    {
        public static readonly ZeroMQManager Instance;
        
        static ZeroMQManager()
        {
            Instance = new ZeroMQManager();
        }
        
        private readonly ConcurrentHashSet<ZeroMQSocket> _sockets;

        public ZeroMQManager()
        {
            _sockets = new ConcurrentHashSet<ZeroMQSocket>(new ReferenceEqualityComparer<ZeroMQSocket>());
        }
        
        internal ZSocket Create(ZeroMQSettings settings)
        {
            var result = new ZSocket(settings.SocketType);

            // this must be called at least before closing the ZContext
            // since we don't want any other behavior yet, we setup
            // the socket always with Linger = 0
            result.Linger = new TimeSpan(0); // do not linger when socket is closed
            
            if(settings.ReceiveTimeout.HasValue)
                result.ReceiveTimeout = TimeSpan.FromMilliseconds(settings.ReceiveTimeout.Value);

            if (settings.IsServer)
                result.Bind(settings.Endpoint);
            else
                result.Connect(settings.Endpoint);

            if(!settings.IsServer && settings.MessagePrefix != null)
                result.Subscribe(settings.MessagePrefix);
           
            return result;
        }

        internal void Add(ZeroMQSocket socket)
        {
            if(_sockets.Add(socket))
                socket.Disposing += OnSocketDisposing;
        }

        private void OnSocketDisposing(object sender, EventArgs e)
        {
            var socket = (ZeroMQSocket) sender;
            
            if (_sockets.Remove(socket))
                socket.Disposing -= OnSocketDisposing;
        }

        public void Dispose()
        {
            Trace.WriteLine(nameof(ZeroMQManager) + "." + nameof(Dispose));
            
            foreach(var socket in _sockets.GetCopy())
                socket.Dispose();
            
            ZContext.Current.Shutdown();
                
            // call to Dispose and or Terminate leeds to always blocking even if all Sockets have been disposed
            // not disposing the sockets before can but also can not cause blocking for terminate
            // it is unclear how to solve this issue. when not calling dispose everything seems to be working...
            ZContext.Current.Dispose();
        }
    }
}