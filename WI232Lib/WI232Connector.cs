using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
//using System.Windows.Forms;
using System.Windows.Forms;

namespace WI232Lib
{
    public class WI232Connector : IWI232Connector
    {
        #region constant definitions
        /// <summary>
        /// Baud-Rate used for communication with WI232 on the drone.
        /// </summary>
        public const int BAUDRATE = 57600;

        /// <summary>
        /// Number of data bits send per message.
        /// </summary>
        public const int DATABITS = 8;

        /// <summary>
        /// Type of parity check.
        /// </summary>
        public const Parity PARITY = Parity.None;

        /// <summary>
        /// Number of stop bits contained within each message.
        /// </summary>
        public const StopBits STOPBITS = StopBits.One;

        /// <summary>
        /// Timeout for sending data to the drone.
        /// </summary>
        public const int SNDTIMEOUT = 350;

        /// <summary>
        /// Timeout for receiving data from the drone.
        /// </summary>
        public const int RCVTIMEOUT = SerialPort.InfiniteTimeout;

        /// <summary>
        /// Buffersize used for communication with the drone.
        /// </summary>
        public const int RCVBUFFERSIZE = 128;

        /// <summary>
        /// Stopsequence which must be contained in every message sent
        /// to the drone or received from the drone.
        /// </summary>
        public const String STOPSEQUENCE = "~";
        #endregion

        public event EventHandler<WI232MessageReceivedEventArgs> MessageReceived;

        private String _portname;
        private SerialPort _comPort;
        private Thread _tWorker;
        private bool _listening = false;

        public string PortName
        {
            get { return _portname; }
            set { _portname = value; }
        }

        public bool Connect()
        {
            if (_comPort != null)
            {
                if (_comPort.IsOpen)
                {
                    _comPort.Close();
                    _comPort = null;
                }
            }

            try
            {
                _comPort = new SerialPort(_portname, BAUDRATE, PARITY, DATABITS);
                _comPort.WriteTimeout = SNDTIMEOUT;
                _comPort.ReadTimeout = RCVTIMEOUT;
                _comPort.ReadBufferSize = RCVBUFFERSIZE;
                _comPort.NewLine = STOPSEQUENCE;
                _comPort.Open();
               
                return true;
            }
            catch (Exception)
            {
                _comPort = null;
                return false;
            }
        }

        public void StartListening(OperatingMode mode)
        {
            try
            {
                if (_comPort == null)
                    throw new InvalidOperationException();

                if (!_listening)
                {
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(DoListening);
                    _tWorker = new Thread(pts);

                    _listening = true;
                    _tWorker.IsBackground = true;
                    _tWorker.Start(mode);
                }
            }
            catch
            {
                MessageBox.Show("Com-Port für Sensoren konnte nicht geöffnet werden. Wahrscheinlich wird der Port bereits verwendet, wurde entfernt oder steht nicht zur Verfügung.", "COM Port kann nicht geöffnet werden", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void DoListening(object param)
        {
            OperatingMode mode = (OperatingMode)param;

            while (_listening)
            {
                try
                {
                    String message = null;

                    if (mode == OperatingMode.ReadBytes)
                        message = "" + _comPort.ReadByte();
                    else
                        message = _comPort.ReadLine();

                    if (MessageReceived != null)
                    {
                        MessageReceived(this, new WI232MessageReceivedEventArgs(message));
                    }
                }
                catch (IOException) { MessageBox.Show("IOException"); }
                catch (ThreadInterruptedException) { MessageBox.Show("ThreadInterruptedException"); }
                catch (ThreadAbortException) { MessageBox.Show("ThreadAbortException"); }
            }

            Thread.Sleep(1);
        }

        public void Disconnect()
        {
            try
            {
                if (_comPort == null)
                    throw new InvalidOperationException();
                _comPort.Close();
               
            }
            catch (Exception) { }

            _comPort = null;

        }

        public void StopListening()
        {
            try
            {
                _listening = false;
                try
                {
                    _tWorker.Interrupt();
                }
                catch (ThreadInterruptedException) { }
            }
            catch (Exception) { }
            _tWorker = null;
        }

        public void SendMsg(Byte[] bytes)
        {
            String message = Encoding.ASCII.GetString(bytes);
            SendMsg(message);
        }

        public void SendMsg(String message)
        {
            _comPort.WriteLine(message);
        }
    }
}
