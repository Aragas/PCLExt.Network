using System.Net;
using System.Net.Sockets;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class DesktopTCPListener : ITCPListener
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


        internal DesktopTCPListener(ushort port)
        {
            Port = port;

            var endpoint = new IPEndPoint(IPAddress.Any, Port);
            Listener = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };

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

#if CORE
            Listener.Shutdown(SocketShutdown.Both);
#else
            Listener.Close();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITCPClient AcceptTCPClient()
        {
            if (IsDisposed)
                return null;

            return new DesktopTCPClient(Listener.Accept());
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