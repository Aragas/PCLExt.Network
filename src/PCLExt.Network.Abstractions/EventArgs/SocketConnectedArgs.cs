namespace PCLExt.Network
{
    public delegate void SocketConnectedEventArgs(SocketConnectedArgs args);

    public class SocketConnectedArgs : SocketEvent
    {
        public SocketConnectedArgs(ISocketClient socket) : base(socket) { }
    }
}