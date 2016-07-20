using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISocketClient
    {
        IPPort LocalEndPoint { get; }
        IPPort RemoteEndPoint { get; }

        Int32 DataAvailable { get; }


        void Connect(String ip, UInt16 port);
        void Disconnect();

        void Write(Byte[] buffer, Int32 offset, Int32 count);
        Int32 Read(Byte[] buffer, Int32 offset, Int32 count);
    }
}