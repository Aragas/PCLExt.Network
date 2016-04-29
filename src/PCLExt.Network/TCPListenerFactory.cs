using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public static class TCPListenerFactory
    {
        private static Lazy<ITCPListenerFactory> _instance = new Lazy<ITCPListenerFactory>(CreateInstance, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static ITCPListenerFactory CreateInstance()
        {
#if COMMON
            return new DesktopTCPServerFactory();
#endif

            return null;
        }

        private static ITCPListenerFactory Instance
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
        /// <param name="port"></param>
        /// <returns></returns>
        public static ITCPListener Create(ushort port) => Instance.Create(port);
    }
}
