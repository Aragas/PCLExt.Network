namespace PCLExt.Network
{
    public class IPPort
    {
        public string IP { get; }
        public ushort Port { get; }


        public IPPort(string ip, ushort port) { IP = ip; Port = port; }
    }
}