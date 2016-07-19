namespace PCLExt.Network
{
    public delegate void SocketDisconnectedEventArgs(SocketDisconnectedArgs args);

    public class SocketDisconnectedArgs : SocketEvent
    {
        public string Reason { get; set; }

        public SocketDisconnectedArgs(ISocketClient socket, string reason) : base(socket) { Reason = reason; }
    }
}