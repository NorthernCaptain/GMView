using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace GMView
{
    /// <summary>
    /// Class provides comminucation with NMEA GPS device through
    /// COM ports. It only reads NMEA strings and do simple parsing
    /// This class does not provide full NMEA sentences parsing.
    /// </summary>
    public class NMEACom: NMEAVirtualDev
    {
        private SerialPort comport = null;

        public NMEACom()
        {

        }

        public override bool open(int portnum, int speed)
        {
            comport = new SerialPort("COM" + portnum, speed, Parity.None, 8, StopBits.One);
            comport.DtrEnable = true;
            comport.RtsEnable = true;
            comport.ReadTimeout = 5000;
            comport.Handshake = Handshake.None;
            try
            {
                comport.Open();
            }
            catch (Exception ex)
            {
                error_ex = ex;
                return false;
            }

            return true;
        }

        protected override string readNMEASentence()
        {
            string line;

            if (!comport.IsOpen)
            {
                return null;
            }

            try
            {
                do
                {
                    line = comport.ReadLine();
                } while(line.Substring(0, 3) != "$GP");
            }
            catch (Exception ex)
            {
                error_ex = ex;
                return null;
            }
            return line;
        }

        public override void reopen()
        {
            try
            {
                comport.Close();
            }
            catch (Exception ex)
            {
                error_ex = ex;
            }

            try
            {
                comport.Open();
            }
            catch (Exception ex)
            {
                error_ex = ex;
            }
        }

        public override void close()
        {
            comport.Close();
        }
    }
}
