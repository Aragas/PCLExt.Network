namespace PCLExt.Network
{
    public delegate void SocketDataReceivedEventArgs(SocketDataReceivedArgs args);

    public class SocketDataReceivedArgs : SocketEvent
    {
        public byte[] Data { get; set; }

        public SocketDataReceivedArgs(ISocketClient socket, byte[] data) : base(socket) { Data = data; }
    }
}