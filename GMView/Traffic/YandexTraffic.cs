using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ncGeo;

namespace GMView
{
    /// <summary>
    /// Uses yandex traffic information system
    /// </summary>
    public class YandexTraffic: BaseTraffic
    {
        private string lastTimet = null;
        private DateTime lastRequest = DateTime.Today;
        private int delay = 4;

        public YandexTraffic()
        {
            trafficAreas = new Rectangle[2];
            trafficGeoAreas = new RectangleF [2];
            trafficGeoAreas[0] = new RectangleF(35.1432f, 56.9613f, 40.2043f, 54.2561f);
            trafficGeoAreas[1] = new RectangleF(26.5802f, 61.3309f, 35.9671f, 58.4171f);
        }

        public override string getTimetInfo(System.Net.WebClient requestFrom)
        {
            TimeSpan diff = DateTime.Now - lastRequest;
            if (lastTimet == null || diff.TotalMinutes > delay)
            {
                requestTimet(requestFrom);
                lastRequest = DateTime.Now;
            }
            return lastTimet;
        }

        private void requestTimet(System.Net.WebClient requestFrom)
        {
            string url = Program.opt.yatraftimehttp;
            
            url = url.Replace("{RND}", ncUtils.Glob.rnd.NextDouble().ToString("F17",ncUtils.Glob.numformat));
            try
            {
                requestFrom.Headers.Add("User-Agent", @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en; rv:1.8.1.12) Gecko/20080201 Firefox/2.0.0.12.l");
                requestFrom.Headers.Add("Accept", @"text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5");

                string buf = requestFrom.DownloadString(url);
                int idx = buf.IndexOf("timestamp");
                if (idx < 0)
                    return;

                while (!char.IsDigit(buf, idx))
                    idx++;
                string result = "";
                while (char.IsDigit(buf, idx))
                {
                    result += buf[idx];
                    idx++;
                }
                lastTimet = result;

            }
            catch (Exception ex)
            {
                Program.Err("YaTraf time webclient got error: " + ex.ToString());
                return;
            }
        }

        public override MapTileType trafficTileType
        {
            get { return MapTileType.YandexTraffic; }
        }
    }
}
