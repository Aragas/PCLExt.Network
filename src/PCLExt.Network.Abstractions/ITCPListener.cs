using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITCPListener : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        UInt16 Port { get; }
        /// <summary>
        /// 
        /// </summary>
        Boolean AvailableClients { get; }

        /// <summary>
        /// 
        /// </summary>
        void Start();
        /// <summary>
        /// 
        /// </summary>
        void Stop();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ITCPClient AcceptTCPClient();
    }
}