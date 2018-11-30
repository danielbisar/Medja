using System;
using System.Diagnostics;
using System.Threading;
using ZeroMQ;

namespace Medja.ZeroMQ
{
    public static class ZeroMQThreadFactory
    {
        /// <summary>
        /// Starts a new thread that is used for a <see cref="ZeroMQSocket"/>. This method helps coordinating the
        /// sockets lifetime and disposing because it only can be access from one thread at a time and simple mistakes
        /// can lead to not close or dispose the socket cleanly which could cause your program to hang forever.
        /// </summary>
        /// <param name="settings">The <see cref="ZeroMQSettings"/>.</param>
        /// <param name="loopCallback">The callback that should be run inside a loop in the thread.</param>
        /// <param name="onDispose">A callback that is executed when someone tried to Dispose the
        /// <see cref="ZeroMQSocket"/>. If any part inside of your <see cref="loopCallback"/> is blocking (not the
        /// <see cref="ZeroMQSocket"/> methods) then you should release return from the method on call of onDispose.
        /// The callback is not guaranteed to be executed on any specific thread but is guaranteed to be
        /// executed.</param>
        /// <returns></returns>
        public static Thread StartThread(ZeroMQSettings settings, Action<ZeroMQSocket> loopCallback, Action onDispose = null)
        {
            void OnDisposeWasCalled(object sender, EventArgs e)
            {
                onDispose?.Invoke();
            }
            
            var identifier = settings.Endpoint + " as " + (settings.IsServer ? "Server" : "Client");
            
            var thread = new Thread(() =>
            {
                Trace.WriteLine(identifier + ": Create new thread for ZeroMQSocket " + Thread.CurrentThread.ManagedThreadId);
                var socket = new ZeroMQSocket(settings);
                
                // the event is helpful if you onDispose will release a WaitHandle or something like that
                // so other thread will get notified even it is is blocking
                socket.DisposeWasCalled += OnDisposeWasCalled;
                
                try
                {
                    while (!socket.GoingToDispose)
                    {
                        loopCallback(socket);
                    }
                }
                catch (ObjectDisposedException ode)
                {
                    Trace.WriteLine(identifier + ": Socket was already disposed...");
                }
                catch (ZException ze)
                {
                    Trace.WriteLine(identifier + ": " + ze);
                }
                finally
                {
                    socket.Dispose();
                    socket.DisposeWasCalled -= OnDisposeWasCalled;
                    
                    Trace.WriteLine($"{identifier}: Thread {Thread.CurrentThread.ManagedThreadId} done.");
                }
            });
            
            thread.Start();

            return thread;
        }
    }
}