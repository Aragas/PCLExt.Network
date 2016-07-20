using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PCLExt.Network
{
    /*
    Inspired by 'Umby24', Project 'Managed Sockets'
    */
    public class DesktopTCPClientEvent : ITCPClientEvent
    {
        public event SocketConnectedEventArgs       Connected;
        public event SocketDataReceivedEventArgs    DataReceived;
        public event SocketDisconnectedEventArgs    Disconnected;

        private Socket _socket;

        public IPPort LocalEndPoint => new IPPort(IPLocalEndPoint.Address.ToString(), (ushort) IPLocalEndPoint.Port);
        public IPPort RemoteEndPoint => new IPPort(IPRemoteEndPoint.Address.ToString(), (ushort) IPRemoteEndPoint.Port);

        private IPEndPoint IPLocalEndPoint => _socket?.LocalEndPoint != null ? _socket.LocalEndPoint as IPEndPoint : new IPEndPoint(0, 0);
        private IPEndPoint IPRemoteEndPoint => _socket?.RemoteEndPoint != null ? _socket.RemoteEndPoint as IPEndPoint : new IPEndPoint(0, 0);

        public bool IsConnected { get; private set; }

        public int DataAvailable => !_disposed && _socket != null ? _socket.Available : 0;
        

        private const int ConnectTimeout = 15000;
        private const int ReadBufferSize = 16 * 4096;

        private bool _closing, _disposed;
        private byte[] _readBuffer = new byte[ReadBufferSize];

        
        public DesktopTCPClientEvent() { _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true }; }
        internal DesktopTCPClientEvent(Socket socket)
        {
            _socket = socket;
            if (!_socket.Connected)
                throw new ArgumentException("Socket is not connected!");

            IsConnected = true;

            try { _socket.BeginReceive(_readBuffer, 0, ReadBufferSize, 0, ReceiveCallback, null);}
            catch (Exception e) when (e is SocketException || e is IOException) { Disconnect($"Socket exception occured: {e.HResult}; InnerException: {e.InnerException?.HResult}"); }
        }

        public void Connect(string endpoint, ushort port)
        {
            if (IsConnected || _disposed)
                Disconnect("Connect() Called");

            IAsyncResult handle = _socket.BeginConnect(endpoint, port, ConnectCallback, null);
            if (handle.AsyncWaitHandle.WaitOne(ConnectTimeout)) // -- Handle connection timeouts
            {
                IsConnected = true;

                try { _socket.BeginReceive(_readBuffer, 0, ReadBufferSize, 0, ReceiveCallback, null); }
                catch (Exception e) when (e is SocketException || e is IOException) { Disconnect($"Socket exception occured: {e.HResult}; InnerException: {e.InnerException?.HResult}"); }
            }


            _socket.Close();
            throw new TimeoutException("Failed to connect to the server");
        }
        public void Disconnect() => Disconnect("Disconnect() Called");
        private void Disconnect(string reason)
        {
            if (!IsConnected || _closing || _disposed)
                return;

            _closing = true;

            _socket.Close();

            IsConnected = false;
            Disconnected?.Invoke(new SocketDisconnectedArgs(this, reason));

            _closing = false;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (_closing || _disposed)
                return;

            try
            {
                var bytesSend = 0;
                while (bytesSend < count)
                    bytesSend += _socket.Send(buffer, bytesSend, count - bytesSend, 0);
            }
            catch (IOException) { }
            catch (SocketException) { }
        }
        public int Read(byte[] buffer, int offset, int count) { throw new NotSupportedException(); }

        public void Dispose()
        {
            if (_disposed)
                return;

            if (IsConnected)
                Disconnect("Dispose() Called");

            _disposed = true;

            _socket?.Dispose();
        }


        #region Callbacks
        private void ConnectCallback(IAsyncResult ar)
        {
            _socket.EndConnect(ar); // -- End the connection event..
            IsConnected = true; // -- Flag the system as connected
            _socket.BeginReceive(_readBuffer, 0, ReadBufferSize, 0, ReceiveCallback, null); /* Begin reading data */

            Connected?.Invoke(new SocketConnectedArgs(this));
            // Task.Run(() => Connected(new SocketConnectedArgs(this))); // -- Trigger the socket connected event.
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            int received;

            try { received = _socket.EndReceive(ar); }
            catch (ObjectDisposedException) { return; /* Socket closed by client */ }
            catch (Exception e) when (e is SocketException || e is IOException) { Disconnect($"Socket exception occured: {e.HResult}; InnerException: {e.InnerException?.HResult}"); return; }

            if (received == 0) { Disconnect("Connection closed by remote host"); return; /* Socket Disconnected */ }

            var newMem = new byte[received];
            Buffer.BlockCopy(_readBuffer, 0, newMem, 0, received); // -- Copy the received data so the end user can use it however they wish


            DataReceived?.Invoke(new SocketDataReceivedArgs(this, newMem)); // -- Call the data received event. (Unblocks immediately, async).


            try { if (!_closing) _socket.BeginReceive(_readBuffer, 0, ReadBufferSize, 0, ReceiveCallback, null); /* Read again! */ }
            catch { Disconnect("Socket closing"); }
        }
        #endregion Callbacks
    }
}