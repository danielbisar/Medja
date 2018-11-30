using ZeroMQ;

namespace Medja.ZeroMQ
{
    /// <summary>
    /// Represents the settings required for ZeroMQBase implementation.
    /// </summary>
    /// <remarks>Implemented as struct so we can assure that references will not change the values.</remarks>
    public struct ZeroMQSettings
    {
        /// <summary>
        /// A default timeout for REPLY/RESPONSE - REQUEST connections.
        /// </summary>
        public static readonly int CommandReceiveTimeout = 750;
        
        /// <summary>
        /// Gets or sets if you implement a Server or Client class.
        /// </summary>
        public bool IsServer { get; set; }
        
        /// <summary>
        /// The endpoint the socket will connect/bind to.
        /// </summary>
        public string Endpoint { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="ZSocketType"/> to use for the new socket.
        /// </summary>
        public ZSocketType SocketType { get; set; }
        
        /// <summary>
        /// Gets or sets the prefix used for messages. This is manly used when using PUB/SUB pattern. The publisher
        /// will mark the messages with the given prefix and the subscriber will subscribe to this messages.
        /// ZeroMQ requires a prefix with this pattern.
        /// </summary>
        public byte[] MessagePrefix { get; set; }
        
        /// <summary>
        /// Gets or sets the timeout for <see cref="ZeroMQSocket.Receive"/> calls in milliseconds.
        /// </summary>
        public int? ReceiveTimeout { get; set; }
    }
}