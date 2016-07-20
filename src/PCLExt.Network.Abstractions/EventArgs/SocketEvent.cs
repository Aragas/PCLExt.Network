using System;

namespace PCLExt.Network
{
    public abstract class SocketEvent : EventArgs
    {
        public ISocketClient Socket { get; set; }

        public SocketEvent(ISocketClient socket) { Socket = socket; }
    }
}