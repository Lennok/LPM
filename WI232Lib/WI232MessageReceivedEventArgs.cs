using System;
using System.Collections.Generic;
using System.Text;

namespace WI232Lib
{
    public class WI232MessageReceivedEventArgs : EventArgs
    {
        private String _message;

        public WI232MessageReceivedEventArgs(String message)
        {
            _message = message;
        }

        public String Message
        {
            get { return _message; }
        }

        public String GetAsBytesString()
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Message);

            StringBuilder sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                String s = BitConverter.ToString(new byte[] { b });
                sb.Append(s + " ");
            }

            return sb.ToString();
        }
    }
}
