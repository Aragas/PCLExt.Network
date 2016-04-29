namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public static class TCPClientFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITCPClient Create()
        {
#if DESKTOP || ANDROID || __IOS__
            return new DesktopTCPClient();
#endif

            return null;
        }
    }
}
