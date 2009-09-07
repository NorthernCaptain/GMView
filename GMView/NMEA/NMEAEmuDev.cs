using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// This class emulates GPS device by reading NMEA sentences from the emulation file.
    /// </summary>
    public class NMEAEmuDev: NMEAVirtualDev
    {
        private string fname;
        private System.IO.StreamReader reader = null;

        public NMEAEmuDev()
        {
            fname = Program.opt.emu_nmea_file;
        }

        public override bool open(int portnum, int speed)
        {
            try
            {
                reader = new System.IO.StreamReader(fname);
            }
            catch (Exception gex)
            {
                error_ex = gex;
                return false;
            }
            return true;
        }

        protected override string readNMEASentence()
        {
            if (reader == null)
            {
                return null;
            }
            try
            {
                string line;
                do
                {
                    line = reader.ReadLine();
                } while (line.Substring(0, 3) != "$GP");
                System.Threading.Thread.Sleep(150);
                return line;
            }
            catch (Exception ex)
            {
                error_ex = ex;
            }
            return null;
        }

        public override void reopen()
        {
            close();
            open(0, 0);
        }

        public override void close()
        {
            try
            {
                if(reader != null)
                    reader.Close();
            }
            catch (Exception ex)
            {
                error_ex = ex;
            }
        }
    }
}
