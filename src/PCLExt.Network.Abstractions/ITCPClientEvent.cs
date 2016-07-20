using System;

namespace PCLExt.Network
{
    /// <summary>
    /// Event driven TCPClient
    /// </summary>
    public interface ITCPClientEvent : ISocketClientEvent
    {
        Boolean IsConnected { get; }
    }
}