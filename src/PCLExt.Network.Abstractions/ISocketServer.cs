using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISocketServer
    {
        UInt16 Port { get; }


        void Start();
        void Stop();
    }
}
