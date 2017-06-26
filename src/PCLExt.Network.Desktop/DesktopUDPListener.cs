using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class DesktopUDPListener : IUDPListener
    {
        /// <summary>
        /// 
        /// </summary>
        public ushort Port { get; }

        private Socket Listener { get; }
        private EndPoint _endPoint;

        private bool IsDisposed { get; set; }


        internal DesktopUDPListener(ushort port)
        {
            Port = port;

            _endPoint = new IPEndPoint(IPAddress.Any, Port);
            Listener = new Socket(_endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp) { EnableBroadcast = true };

            Listener.Bind(_endPoint);
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
        public void Write(byte[] buffer, int offset, int count, string ip, ushort port)
        {
            if (IsDisposed)
                return;

            try
            {
                var endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                var bytesSend = 0;
                while (bytesSend < count)
                    bytesSend += Listener.SendTo(buffer, bytesSend, count - bytesSend, 0, endPoint);
            }
            catch (IOException) { }
            catch (SocketException) { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] Read()
        {
            if (IsDisposed)
                return new byte[0];

            try
            {
                var recBuffer = new byte[65536]; // Max. size
                var dataRead = Listener.ReceiveFrom(recBuffer, ref _endPoint);
                if (dataRead < recBuffer.Length)
                    recBuffer = CutArray(recBuffer, dataRead);

                return recBuffer;
            }
            catch (IOException) { return new byte[0]; }
            catch (SocketException) { return new byte[0]; ; }
        }
        private static byte[] CutArray(byte[] orig, int length)
        {
            byte[] newArray = new byte[length];
            Buffer.BlockCopy(orig, 0, newArray, 0, length);

            return newArray;
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