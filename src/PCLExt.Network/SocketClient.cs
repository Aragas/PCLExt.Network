using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public static class SocketClient
    {
        private static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException(@"This functionality is not implemented in the portable version of this assembly.
You should reference the PCLExt.Network NuGet package from your main application project in order to reference the platform-specific implementation.");


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITCPClient CreateTCP()
        {
#if DESKTOP || ANDROID || __IOS__ || MAC || NETSTANDARD2_0
            return new DesktopTCPClient();
#endif

            throw NotImplementedInReferenceAssembly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITCPClientEvent CreateTCPEvent()
        {
#if DESKTOP || ANDROID || __IOS__ || MAC || NETSTANDARD2_0
            return new DesktopTCPClientEvent();
#endif

            throw NotImplementedInReferenceAssembly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IUDPClient CreateUDP()
        {
#if DESKTOP || ANDROID || __IOS__ || MAC || NETSTANDARD2_0
            return new DesktopUDPClient();
#endif

            throw NotImplementedInReferenceAssembly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IUDPClientEvent CreateUPDEvent()
        {
#if DESKTOP || ANDROID || __IOS__ || MAC || NETSTANDARD2_0
            return new DesktopUDPClientEvent();
#endif

            throw NotImplementedInReferenceAssembly();
        }
    }
}