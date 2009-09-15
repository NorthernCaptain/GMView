using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ncGeo;

namespace GMView
{
    public class NMEAThread
    {
        private NMEAVirtualDev nmeadev;
        private Thread ourThread = new Thread(new ParameterizedThreadStart(devReadStub));

        public delegate void OnNMEAStingDelegate(NMEAString nmeastr);
        public delegate void OnNMEACommandDelegate(NMEACommand cmd);

        public event OnNMEAStingDelegate onNMEAString;
        public event OnNMEAStingDelegate onLogNMEAString;
        public event OnNMEACommandDelegate onNMEACommand;

        private delegate NMEACommand OnNewNMEADelegate(NMEAString str);
        private static Dictionary<string, OnNewNMEADelegate> commandCreators = new Dictionary<string,OnNewNMEADelegate>();

        static NMEAThread()
        {
            commandCreators.Add("GLL", new OnNewNMEADelegate(delegate(NMEAString str) { return new NMEA_GLL(str);}));
            commandCreators.Add("GGA", new OnNewNMEADelegate(delegate(NMEAString str) { return new NMEA_GGA(str); }));
            commandCreators.Add("GSA", new OnNewNMEADelegate(delegate(NMEAString str) { return new NMEA_GSA(str); }));
            commandCreators.Add("RMC", new OnNewNMEADelegate(delegate(NMEAString str) { return new NMEA_RMC(str); }));
            commandCreators.Add("VTG", new OnNewNMEADelegate(delegate(NMEAString str) { return new NMEA_VTG(str); }));
            commandCreators.Add("GSV", new OnNewNMEADelegate(delegate(NMEAString str) { return NMEA_GSV.getNewGSV(str); }));
        }

        public NMEAThread()
        {
            
        }

        public void start()
        {
            newDev();
            ourThread.Start(this);
            Program.onShutdown += new Program.ShutdownDelegate(Program_onShutdown);
            Program.opt.onChanged += new Options.OnChangedDelegate(opt_onChanged);
        }

        private void newDev()
        {
            NMEAVirtualDev newdev;
            if(Program.opt.emulate_gps)
                newdev = new NMEAEmuDev();
            else 
                newdev = new NMEACom();
            if(nmeadev != null) nmeadev.close();
            newdev.open(Program.opt.nmea_com_num, Program.opt.nmea_com_speed);
            nmeadev = newdev;
        }

        internal void opt_onChanged()
        {
            newDev();
        }

        void Program_onShutdown()
        {
            nmeadev.close();
            ourThread.Abort();
            ourThread.Join();
        }

        private static void devReadStub(Object o_we)
        {
            NMEAThread we = o_we as NMEAThread;
            if( we != null)
                we.devRead();
        }

        private void devRead()
        {
            while (true)
            {
                NMEAString nmea;

                if (Program.opt.use_gps == false)
                {
                    Thread.Sleep(500);
                    continue;
                }

                nmea = nmeadev.read();
                if(onLogNMEAString != null)
                    onLogNMEAString(nmea);
                if(onNMEAString != null)
                    onNMEAString(nmea);
                if (nmea.error != null)
                {
                    Thread.Sleep(1000);
                    nmeadev.reopen();
                }
                else
                {
                    try
                    {
                        OnNewNMEADelegate newcmd_method = commandCreators[nmea.parts[0]];
                        if (newcmd_method != null)
                        {
                            NMEACommand cmd = newcmd_method(nmea);
                            try
                            {
                                cmd.parse();
                                if (onNMEACommand != null && cmd.readyForProcessing())
                                    onNMEACommand(cmd);
                            }
                            catch { };
                        }
                    }
                    catch { };
                }
            }
        }

        internal static NMEACommand parse_command(NMEAString nmea)
        {
            try
            {
                OnNewNMEADelegate newcmd_method = commandCreators[nmea.parts[0]];
                if (newcmd_method != null)
                {
                    NMEACommand cmd = newcmd_method(nmea);
                    try
                    {
                        cmd.parse();
                        return cmd;
                    }
                    catch { };
                }
            }
            catch { };

            return null;
        }
    }
}
