using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public static class TCPListener
    {
        private static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException(@"This functionality is not implemented in the portable version of this assembly.
You should reference the PCLExt.Network NuGet package from your main application project in order to reference the platform-specific implementation.");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ITCPListener Create(ushort port)
        {
#if DESKTOP || ANDROID || __IOS__ || MAC
            return new DesktopTCPListener(port);
#endif

            throw NotImplementedInReferenceAssembly();
        }
    }
}
