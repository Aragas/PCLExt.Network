﻿using System;

namespace PCLExt.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITCPClient : ISocketClient, IDisposable
    {
        Boolean IsConnected { get; }
    }
}