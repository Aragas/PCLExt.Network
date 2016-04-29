using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public static class TCPClientFactory
    {
        private static Lazy<ITCPClientFactory> _instance = new Lazy<ITCPClientFactory>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static ITCPClientFactory CreateInstance()
        {
#if COMMON
            return new DesktopTCPClientFactory();
#endif

            return null;
        }

        private static ITCPClientFactory Instance
        {
            get
            {
                var ret = _instance.Value;
                if (ret == null)
                    throw NotImplementedInReferenceAssembly();
                return ret;
            }
        }

        internal static Exception NotImplementedInReferenceAssembly() => new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the PCLExt.Network NuGet package from your main application project in order to reference the platform-specific implementation.");


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITCPClient Create() => Instance.Create();
    }
}
