using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class DesktopUDPClient : IUDPClient
    {
        public IPPort LocalEndPoint => new IPPort(
            !IsDisposed && Client != null && Client.Connected ? (Client.LocalEndPoint as IPEndPoint)?.Address.ToString() : "",
            (ushort) (!IsDisposed && Client != null && Client.Connected ? (Client.LocalEndPoint as IPEndPoint)?.Port : 0));
        public IPPort RemoteEndPoint => new IPPort(
            !IsDisposed && Client != null && Client.Connected ? (Client.RemoteEndPoint as IPEndPoint)?.Address.ToString() : "",
            (ushort) (!IsDisposed && Client != null && Client.Connected ? (Client.RemoteEndPoint as IPEndPoint)?.Port : 0));

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected => !IsDisposed && Client != null && Client.Connected;
        /// <summary>
        /// 
        /// </summary>
        public int DataAvailable => !IsDisposed && Client != null ? Client.Available : 0;

        private Socket Client { get; }

        private bool IsDisposed { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DesktopUDPClient() { Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); }
        internal DesktopUDPClient(Socket socket) { Client = socket; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public void Connect(string ip, ushort port)
        {
            if (IsConnected)
                Disconnect();

            Client.Connect(ip, port);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Disconnect()
        {
            if (IsConnected)
                Client.Disconnect(false);
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
            if (IsConnected)
                Disconnect();

            IsDisposed = true;

            Client?.Dispose();
        }
    }
}