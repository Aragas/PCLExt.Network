namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public static class TCPListenerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ITCPListener Create(ushort port)
        {
#if DESKTOP || ANDROID || __IOS__
            return new DesktopTCPListener(port);
#endif

            return null;
        }
    }
}
