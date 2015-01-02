using System;
using System.Collections.Generic;
using System.Text;

namespace WI232Lib
{
    public interface IWI232Connector
    {
        event EventHandler<WI232MessageReceivedEventArgs> MessageReceived;

        String PortName { get; set; }

        bool Connect();
        void Disconnect();
        void StartListening(OperatingMode mode);
        void StopListening();

        void SendMsg(String message);
        void SendMsg(byte[] bytes);
    }

    public enum OperatingMode
    {
        /// <summary>
        /// Fires an event everytime a single bytes has been received.
        /// </summary>
        ReadBytes,

        /// <summary>
        /// Fires an event everytime a complete line has been received.
        /// </summary>
        ReadLine
    }
}
