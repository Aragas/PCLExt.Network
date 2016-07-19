using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITCPListener : ISocketServer, IDisposable
    {
        Boolean AvailableClients { get; }


        ITCPClient AcceptTCPClient();
    }
}