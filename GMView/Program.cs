using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Drawing;

namespace GMView
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            opt.onChanged +=new Options.OnChangedDelegate(OptionsOnChanged);
            opt.command_line_args = args;
            frm = new GMViewForm();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            Application.Run(frm);
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (onShutdown != null)
                onShutdown();
        }

        static void do_test3()
        {
            GoogleGeo google = new GoogleGeo();
            EniroGeo eniro = new EniroGeo();

            double lon, lat;

            google.zoomLevel = 15;
            eniro.zoomLevel = 15;

            Point xy1 = new Point(2222080, 1220096);

            google.getLonLatByXY(xy1, out lon, out lat);

            Point xy2;

            eniro.getXYByLonLat(lon, lat, out xy2);


        }
        static void do_test1()
        {
            GoogleGeo google = new GoogleGeo();
            YandexGeo yandex = new YandexGeo();

            double lat = 59.8322;
            double lon = 30.3532;

            System.Drawing.Point gooxy;
            System.Drawing.Point yaxy;

            google.zoomLevel = 4;
            yandex.zoomLevel = 4;

            google.getXYByLonLat(lon, lat, out gooxy);
            yandex.getXYByLonLat(lon, lat, out yaxy);

            google.zoomLevel = 12;
            yandex.zoomLevel = 12;

            google.getXYByLonLat(lon, lat, out gooxy);
            yandex.getXYByLonLat(lon, lat, out yaxy);

            System.Drawing.Point goonxny;
            System.Drawing.Point yanxny;

            google.getNXNYByLonLat(lon, lat, 12, out goonxny);
            yandex.getNXNYByLonLat(lon, lat, 12, out yanxny);
        }

        public static void Log(string txt)
        {
            if (logger == null)
                return;
            try
            {
                if (!logger.needInvoke)
                    logger.Log(txt);
                else
                    if (frm != null && opt.all_log)
                    {
                        frm.Invoke(new GMViewForm.OnLogDelegate(logger.Log), new Object[] { txt });
                    }
            }
            catch { };
        }

        public static void Err(string txt)
        {
            if (logger == null)
                return;
            if (opt.all_err)
            {
                if (!logger.needInvoke)
                    logger.Err(txt);
                else
                    if (frm != null)
                    {
                        try
                        {
                            frm.Invoke(new GMViewForm.OnLogDelegate(logger.Err), new Object[] { txt });
                        }
                        catch { }
                    }
            }
        }

        static private GMViewForm frm;
        static public WebClient webclient = new WebClient();

        /// <summary>
        /// Here we store global program options, so need to acces from anywhere
        /// </summary>
        static public Options opt = new Options();
        static public float plus_x = 0.0f;

        public static void OptionsOnChanged()
        {
            switch (Program.opt.httpproxy_idx)
            {
                case -2: // No proxy
                    webclient.Proxy = null;
                    Log("Proxy: NOT using any http proxy");
                    break;
                case -1: // Default IE proxy
                    webclient.Proxy = WebRequest.DefaultWebProxy;
                    Log("Proxy: using default IE proxy");
                    break;
                default: // Our own list of proxies
                    {
                        int idx = Program.opt.httpproxy_idx;
                        try
                        {
                            Log("Proxy: using http proxy: " + Program.opt.httpproxy[idx]);
                            WebProxy prox = new WebProxy(Program.opt.httpproxy[idx], true);
                            webclient.Proxy = prox;
                        }
                        catch (Exception ex)
                        {
                            Err("Proxy creation error: " + ex);
                        }
                    }
                    break;
            }
        }

        public delegate void ShutdownDelegate();
        public static event ShutdownDelegate onShutdown;
        public static ILog logger = null;
    }
}
