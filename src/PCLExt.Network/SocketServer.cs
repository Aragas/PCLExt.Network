using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public static class SocketServer
    {
        private static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException(@"This functionality is not implemented in the portable version of this assembly.
You should reference the PCLExt.Network NuGet package from your main application project in order to reference the platform-specific implementation.");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ITCPListener CreateTCP(ushort port)
        {
#if DESKTOP || ANDROID || __IOS__ || MAC
            return new DesktopTCPListener(port);
#endif

            throw NotImplementedInReferenceAssembly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ITCPListenerEvent CreateTCPEvent(ushort port)
        {
#if DESKTOP || ANDROID || __IOS__ || MAC
            return new DesktopTCPListenerEvent(port);
#endif

            throw NotImplementedInReferenceAssembly();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IUDPListener CreateUDP(ushort port)
        {
#if DESKTOP || ANDROID || __IOS__ || MAC
            return new DesktopUDPListener(port);
#endif

            throw NotImplementedInReferenceAssembly();
        }
    }
}
