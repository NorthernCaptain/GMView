using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMView.UIHelper
{
    /// <summary>
    /// Class holds timer and organizes methods invocation once in the future after a given timeout
    /// </summary>
    public class RunMeOnce
    {
        private static RunMeOnce instance = null;

        public static RunMeOnce singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new RunMeOnce();
                }
                return instance;
            }
        }

        private RunMeOnce()
        {
            runOnceTimer = new Timer();
            runOnceTimer.Tick += runOnceTimer_Tick;
            runOnceTimer.Interval = 1000;
        }

        private Timer runOnceTimer;
        private List<MethodInvoker> runOnce = new List<MethodInvoker>();
        private List<MethodInvoker> runOnceClone = new List<MethodInvoker>();

        /// <summary>
        /// Schedule method for invocation after given msec
        /// </summary>
        /// <param name="meth"></param>
        /// <param name="msec"></param>
        public void runMeOnce(MethodInvoker meth, int msec)
        {
            if (runOnceTimer.Enabled)
                runOnceTimer.Stop();
            runOnceTimer.Interval = msec;
            runOnce.Add(meth);
            runOnceTimer.Start();
        }

        /// <summary>
        /// Stops the timer and clear runOnce queue
        /// </summary>
        public void stop()
        {
            runOnceTimer.Stop();
            runOnce.Clear();
        }

        private void runOnceTimer_Tick(object sender, EventArgs e)
        {
            List<MethodInvoker> tmp = runOnce;
            runOnce = runOnceClone;
            runOnceClone = tmp;
            runOnceTimer.Stop();
            foreach (MethodInvoker meth in runOnceClone)
                meth();
            runOnceClone.Clear();
        }
    }
}
