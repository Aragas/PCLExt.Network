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

        private static Exception NotImplementedInNetCore() =>
            new NotImplementedException(@"This functionality is not implemented in the current version of this .NET Standard.");


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITCPClient CreateTCP()
        {
#if DESKTOP || ANDROID || __IOS__ || MAC || CORE
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
#if DESKTOP || ANDROID || __IOS__ || MAC
            return new DesktopTCPClientEvent();
#elif CORE
            throw NotImplementedInNetCore();
#endif

            throw NotImplementedInReferenceAssembly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IUDPClient CreateUDP()
        {
#if DESKTOP || ANDROID || __IOS__ || MAC || CORE
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
#if DESKTOP || ANDROID || __IOS__ || MAC
            return new DesktopUDPClientEvent();
#elif CORE
            throw NotImplementedInNetCore();
#endif

            throw NotImplementedInReferenceAssembly();
        }
    }
}