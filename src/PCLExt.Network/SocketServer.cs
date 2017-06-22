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

        private static Exception NotImplementedInNetCore() =>
            new NotImplementedException(@"This functionality is not implemented in the current version of this .NET Standard.");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ITCPListener CreateTCP(ushort port)
        {
#if DESKTOP || ANDROID || __IOS__ || MAC || CORE
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
#elif CORE
            throw NotImplementedInNetCore();
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
#if DESKTOP || ANDROID || __IOS__ || MAC || CORE
            return new DesktopUDPListener(port);
#endif

            throw NotImplementedInReferenceAssembly();
        }
    }
}