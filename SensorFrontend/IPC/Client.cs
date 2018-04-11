using System;
using ZeroMQ;

namespace SensorFrontend.IPC
{
    public class Client : IDisposable
    {
        private readonly ZContext _zeroMqContext;
        private readonly ZSocket _zSocket;

        public Client()
        {
            _zeroMqContext = new ZContext();

            using (_zSocket = new ZSocket(_zeroMqContext, ZSocketType.REQ))
            {
                _zSocket.Connect("tcp://127.0.0.1:5555");

                for (int n = 0; n < 10; ++n)
                {
                    string requestText = "Hello";
                    Console.Write("Sending {0}…", requestText);

                    // Send
                    _zSocket.Send(new ZFrame(requestText));

                    // Receive
                    using (ZFrame reply = _zSocket.ReceiveFrame())
                    {
                        Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
                    }
                }
            }
        }

        public void Send(string str)
        {
            using (var frame = new ZFrame(str))
            {
                _zSocket.Send(frame);
            }
        }

        public string SendAndReceive(string str)
        {
            using (var frame = new ZFrame(str))
            {
                _zSocket.Send(frame);
            }

            using(var frame = _zSocket.ReceiveFrame())
            {
                return frame.ReadString();
            }

            //for (int n = 0; n < 10; ++n)
            //{
            //    string requestText = "Hello";
            //    Console.Write("Sending {0}…", requestText);

            //    // Send
            //    requester.Send(new ZFrame(requestText));

            //    // Receive
            //    using (ZFrame reply = requester.ReceiveFrame())
            //    {
            //        Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
            //    }
            //}
        }

        public void Dispose()
        {
            _zeroMqContext.Dispose();
        }
    }
}
