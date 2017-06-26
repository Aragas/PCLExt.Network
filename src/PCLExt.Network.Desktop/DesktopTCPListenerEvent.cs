using System.Net;
using System.Net.Sockets;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class DesktopTCPListenerEvent : ITCPListenerEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public ushort Port { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool AvailableClients => Listener.Poll(0, SelectMode.SelectRead);

        private Socket Listener { get; }

        private bool IsDisposed { get; set; }


        internal DesktopTCPListenerEvent(ushort port)
        {
            Port = port;

            var endpoint = new IPEndPoint(IPAddress.Any, Port);
            Listener = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Listener.Bind(endpoint);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (IsDisposed)
                return;

            Listener.Listen(1000);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (IsDisposed)
                return;

            Listener.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITCPClientEvent AcceptTCPClient()
        {
            if (IsDisposed)
                return null;

            return new DesktopTCPClientEvent(Listener.Accept());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            Listener?.Dispose();
        }
    }
}