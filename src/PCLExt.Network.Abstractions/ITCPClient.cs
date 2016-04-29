using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITCPClient : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        String IP { get; }
        /// <summary>
        /// 
        /// </summary>
        UInt16 Port { get; }
        /// <summary>
        /// 
        /// </summary>
        Boolean Connected { get; }
        /// <summary>
        /// 
        /// </summary>
        Int32 DataAvailable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        ITCPClient Connect(String ip, UInt16 port);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ITCPClient Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        void Write(byte[] buffer, int offset, int count);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        int Read(byte[] buffer, int offset, int count);
    }
}