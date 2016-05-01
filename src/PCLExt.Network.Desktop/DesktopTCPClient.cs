using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PCLExt.Network
{
    internal class DesktopTCPClientFactory : ITCPClientFactory
    {
        public ITCPClient Create() => new DesktopTCPClient();
        internal static ITCPClient CreateTCPClient(Socket socket) => new DesktopTCPClient(socket);
    }

    /// <summary>
    /// 
    /// </summary>
    public class DesktopTCPClient : ITCPClient
    {
        /// <summary>
        /// 
        /// </summary>
        public string IP => !IsDisposed && Client != null && Client.Connected ? (Client.RemoteEndPoint as IPEndPoint)?.Address.ToString() : "";
        /// <summary>
        /// 
        /// </summary>
        public ushort Port => (ushort) (!IsDisposed && Client != null && Client.Connected ? (Client.RemoteEndPoint as IPEndPoint)?.Port : 0);
        /// <summary>
        /// 
        /// </summary>
        public bool Connected => !IsDisposed && Client != null && Client.Connected;
        /// <summary>
        /// 
        /// </summary>
        public int DataAvailable => !IsDisposed && Client != null ? Client.Available : 0;

        private Socket Client { get; }

        private bool IsDisposed { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DesktopTCPClient() { Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true }; }
        internal DesktopTCPClient(Socket socket) { Client = socket; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public ITCPClient Connect(string ip, ushort port)
        {
            if (Connected)
                Disconnect();

            Client.Connect(ip, port);

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITCPClient Disconnect()
        {
            if (Connected)
                Client.Disconnect(false);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (IsDisposed)
                return;

            try
            {
                var bytesSend = 0;
                while (bytesSend < count)
                    bytesSend += Client.Send(buffer, bytesSend, count - bytesSend, 0);
            }
            catch (IOException) { }
            catch (SocketException) { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (IsDisposed)
                return -1;

            try
            {
                var bytesReceived = 0;
                while (bytesReceived < count)
                    bytesReceived += Client.Receive(buffer, bytesReceived, count - bytesReceived, 0);

                return bytesReceived;
            }
            catch (IOException) { return -1; }
            catch (SocketException) { return -1; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (Connected)
                Disconnect();

            IsDisposed = true;

            Client?.Dispose();
        }
    }
}
