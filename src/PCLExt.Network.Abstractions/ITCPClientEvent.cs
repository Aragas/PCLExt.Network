namespace PCLExt.Network
{
    /// <summary>
    /// Event driven TCPClient
    /// </summary>
    public interface ITCPClientEvent : ITCPClient
    {
        event SocketConnectedEventArgs      Connected;
        event SocketDataReceivedEventArgs   DataReceived;
        event SocketDisconnectedEventArgs   Disconnected;
    }
}