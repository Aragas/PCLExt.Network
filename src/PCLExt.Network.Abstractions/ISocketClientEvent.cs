namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISocketClientEvent : ISocketClient
    {
        event SocketConnectedEventArgs      Connected;
        event SocketDataReceivedEventArgs   DataReceived;
        event SocketDisconnectedEventArgs   Disconnected;
    }
}