using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Class provides abstract interface for any device that could read NMEA sentences. This could be a real
    /// device connected via ComPort or any other one including a emulation device. See child classes implemented
    /// real device reading.
    /// </summary>
    public abstract class NMEAVirtualDev
    {
        /// <summary>
        /// Exception that occured during last operation
        /// </summary>
        protected Exception error_ex = null;

        /// <summary>
        /// Return last error as exception
        /// </summary>
        public Exception error
        {
            get
            {
                return error_ex;
            }
        }

        /// <summary>
        /// Opens a device for reading GPS data. Currently this is the ComPort device
        /// </summary>
        /// <param name="portnum"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public abstract bool open(int portnum, int speed);

        /// <summary>
        /// Reads one GPS sentence from device and return it as is (as a string in NMEA format).
        /// </summary>
        /// <returns></returns>
        protected abstract string readNMEASentence();

        /// <summary>
        /// Reopens device. If the device was opened, then close it first.
        /// </summary>
        public abstract void reopen();

        /// <summary>
        /// Read one sentence and return it as NMEAString (pre-parsed)
        /// </summary>
        /// <returns></returns>
        public ncGeo.NMEAString read()
        {
            string buf = readNMEASentence();
            if (buf == null || buf.Length <= 3)
                return new ncGeo.NMEAString(error_ex);
            return new ncGeo.NMEAString(buf);
        }

        /// <summary>
        /// Close the device and free all resources connected to it.
        /// </summary>
        public abstract void close();

    }
}
