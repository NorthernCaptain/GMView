using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GMView
{
    public class FrameTimer: IDisposable
    {
        private static FrameTimer global_timer = new FrameTimer(15);

        ulong ticks = 0ul;
        System.Windows.Forms.Timer timer;
        List<IFrameUpdate> clients = new List<IFrameUpdate>();
        private bool need_break;
        private Stopwatch stop = new Stopwatch();

        public event EventHandler onUpdated;

        public static FrameTimer singleton
        {
            get { return global_timer; }
        }

        public FrameTimer(int every_millisec)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = every_millisec;
            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            bool ret = false;
            ticks++;
            need_break = false;
            stop.Stop();
            ulong millis = (ulong)stop.ElapsedMilliseconds;
            stop.Reset();
            stop.Start();
            foreach (IFrameUpdate client in clients)
            {
                ret = client.updateFrame(millis);
                if (need_break)
                    break;
            }
            if (ret && onUpdated != null)
                onUpdated(this, EventArgs.Empty);
            
        }

        public void add(IFrameUpdate client)
        {
            if (client == null)
                return;
            need_break = true;
            clients.Add(client);
            client.registered(ticks);
        }

        public void del(IFrameUpdate client)
        {
            if (client == null)
                return;
            clients.Remove(client);
            client.unregistered(ticks);
            need_break = true;
        }

        public void Start()
        {
            timer.Start();
            stop.Reset();
            stop.Start();
        }

        public void Stop()
        {
            stop.Stop();
            timer.Stop();
        }

        #region IDisposable Members

        public void Dispose()
        {
            timer.Stop();
            stop.Stop();
            timer.Dispose();
            clients.Clear();
        }

        #endregion
    }
}
