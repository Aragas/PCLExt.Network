using System;

namespace PCLExt.Network
{
    /// <summary>
    /// Returns an event driven TCPClient
    /// </summary>
    public interface ITCPListenerEvent : ISocketServer, IDisposable
    {
        Boolean AvailableClients { get; }


        ITCPClientEvent AcceptTCPClient();
    }
}