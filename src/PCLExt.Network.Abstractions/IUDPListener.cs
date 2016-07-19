using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUDPListener : ISocketServer, IDisposable
    {
        void Write(Byte[] buffer, Int32 offset, Int32 count, String ip, UInt16 port);
        Byte[] Read();
    }
}