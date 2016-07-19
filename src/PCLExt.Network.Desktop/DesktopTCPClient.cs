using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PCLExt.Network
{
    public class DesktopTCPClient : ITCPClient
    {
        private Socket Socket { get; }
        private bool _disposed;

        public IPPort LocalEndPoint => new IPPort(
            !_disposed && Socket != null && Socket.Connected ? (Socket.LocalEndPoint as IPEndPoint)?.Address.ToString() : "",
            (ushort) (!_disposed && Socket != null && Socket.Connected ? (Socket.LocalEndPoint as IPEndPoint)?.Port : 0));
        public IPPort RemoteEndPoint => new IPPort(
            !_disposed && Socket != null && Socket.Connected ? (Socket.RemoteEndPoint as IPEndPoint)?.Address.ToString() : "",
            (ushort) (!_disposed && Socket != null && Socket.Connected ? (Socket.RemoteEndPoint as IPEndPoint)?.Port : 0));
        
        public bool IsConnected => !_disposed && Socket != null && Socket.Connected;

        public int DataAvailable => !_disposed && Socket != null ? Socket.Available : 0;

        
        public DesktopTCPClient() { Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true }; }
        internal DesktopTCPClient(Socket socket) { Socket = socket; }

        public void Connect(string ip, ushort port)
        {
            if (IsConnected)
                Disconnect();

            Socket.Connect(ip, port);
        }
        public void Disconnect()
        {
            if (IsConnected)
                Socket.Disconnect(false);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (_disposed)
                return;

            try
            {
                var bytesSend = 0;
                while (bytesSend < count)
                    bytesSend += Socket.Send(buffer, bytesSend, count - bytesSend, 0);
            }
            catch (IOException) { }
            catch (SocketException) { }
        }
        public int Read(byte[] buffer, int offset, int count)
        {
            if (_disposed)
                return -1;

            try
            {
                var bytesReceived = 0;
                while (bytesReceived < count)
                    bytesReceived += Socket.Receive(buffer, bytesReceived, count - bytesReceived, 0);

                return bytesReceived;
            }
            catch (IOException) { return -1; }
            catch (SocketException) { return -1; }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            if (IsConnected)
                Disconnect();

            _disposed = true;

            Socket.Dispose();
        }
    }
}
