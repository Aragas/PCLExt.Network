using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class DesktopUDPClientEvent : IUDPClientEvent
    {
        public event SocketConnectedEventArgs       Connected;
        public event SocketDataReceivedEventArgs    DataReceived;
        public event SocketDisconnectedEventArgs    Disconnected;

        public IPPort LocalEndPoint => new IPPort(
            !IsDisposed && Client != null && Client.Connected ? (Client.LocalEndPoint as IPEndPoint)?.Address.ToString() : "",
            (ushort) (!IsDisposed && Client != null && Client.Connected ? (Client.LocalEndPoint as IPEndPoint)?.Port : 0));
        public IPPort RemoteEndPoint => new IPPort(
            !IsDisposed && Client != null && Client.Connected ? (Client.RemoteEndPoint as IPEndPoint)?.Address.ToString() : "",
            (ushort) (!IsDisposed && Client != null && Client.Connected ? (Client.RemoteEndPoint as IPEndPoint)?.Port : 0));

        public bool IsConnected { get; set; }

        public int DataAvailable => !IsDisposed && Client != null ? Client.Available : 0;

        private Socket Client { get; }

        private bool IsDisposed { get; set; }


        private const int ReadSize = 16 * 4096;
        private const int ConnectTimeout = 10000;

        private byte[] _readBuffer = new byte[ReadSize]; // -- Small buffer that async reads will place data into
        private bool _closing, _sending; // -- Internal state flags

        private ConcurrentQueue<byte[]> _receiveBuffer = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<byte[]> _sendBuffer = new ConcurrentQueue<byte[]>();


        public DesktopUDPClientEvent() { Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); }
        internal DesktopUDPClientEvent(Socket socket) { Client = socket; }

        public void Connect(string ip, ushort port)
        {
            if (IsConnected)
                Disconnect();

            Client.Connect(ip, port);
        }
        public void Disconnect() => Disconnect(null);
        public void Disconnect(string reason)
        {
            if (!IsConnected || _closing)
                return;

            _closing = true;

            // -- Wait for everything in send buffer to clear??
            while (_sending) { Thread.Sleep(1); }

            _sendBuffer = new ConcurrentQueue<byte[]>(); // -- Reset everything
            _readBuffer = new byte[ReadSize];

            Client.Close();

            IsConnected = false; // -- Flag any users that we are no longer connected.

            // -- Call the disconnected event.
            Disconnected?.Invoke(new SocketDisconnectedArgs(this, reason));
            //Disconnected.SafeRaise(new SocketDisconnectedArgs(this, reason));

            _closing = false; // -- Done closing our connection
        }

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

        public void Dispose()
        {
            if (IsConnected)
                Disconnect();

            IsDisposed = true;

            Client?.Dispose();
        }


        #region Async Callbacks
        private void SendLoop()
        { // -- Potential Bug: Data that needs to be in order, sending in an incorrect order due to async stuff.
            while (_sendBuffer.Count > 0)
            {
                if (!_sendBuffer.TryDequeue(out var data))
                {
                    _sending = false;
                    return;
                }

                try
                {
                    Client.BeginSend(data, 0, data.Length, 0, DataSent, null);
                    //if (_baseStream.CanWrite)
                    //    Client.BeginSend(data, 0, data.Length, 0, DataSent, null);
                    //else
                    //    Disconnect("Socket closing");
                }
                catch { Disconnect("Socket closing"); }
            }

            _sending = false;
        }
        private void DataSent(IAsyncResult ar)
        {
            try { Client.EndSend(ar); }
            catch { Disconnect("Socket closing"); }
        }
        private void ConnectComplete(IAsyncResult ar)
        {
            Client.EndConnect(ar); // -- End the connection event..
            IsConnected = true; // -- Flag the system as connected
            Client.BeginReceive(_readBuffer, 0, ReadSize, 0, ReadComplete, null); /* Begin reading data */

            Connected?.Invoke(new SocketConnectedArgs(this));
            // Task.Run(() => Connected(new SocketConnectedArgs(this))); // -- Trigger the socket connected event.
        }
        private void ReadComplete(IAsyncResult ar)
        {
            int received;

            try { received = Client.EndReceive(ar); /* End the async op */ }
            catch (ObjectDisposedException) { return; /* Socket closed by client */ }
            catch (IOException e) { Disconnect("Socket exception occured: " + e.InnerException.HResult); return; }

            if (received == 0) { Disconnect("Connection closed by remote host."); return; /* Socket Disconnected */ }

            var newMem = new byte[received];
            Buffer.BlockCopy(_readBuffer, 0, newMem, 0, received); // -- Copy the received data so the end user can use it however they wish
            DataReceived?.Invoke(new SocketDataReceivedArgs(this, newMem)); // -- Call the data received event. (Unblocks immediately, async).

            try { if (!_closing) Client.BeginReceive(_readBuffer, 0, ReadSize, 0, ReadComplete, null); /* Read again! */ }
            catch { Disconnect("Socket closing"); }
        }
        #endregion
    }
}