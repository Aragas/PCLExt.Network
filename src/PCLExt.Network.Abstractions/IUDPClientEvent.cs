namespace PCLExt.Network
{
    /// <summary>
    /// Event driven UDPClient
    /// </summary>
    public interface IUDPClientEvent : IUDPClient
    {
        event SocketConnectedEventArgs      Connected;
        event SocketDataReceivedEventArgs   DataReceived;
        event SocketDisconnectedEventArgs   Disconnected;
    }
}