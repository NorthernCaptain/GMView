using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ncUtils
{
    /// <summary>
    /// Worker thread that talk back with caller throught the Control.Invoke method that guaranties calling
    /// in the main GUI thread
    /// </summary>
    public class GUIWorkerThread<TaskType> : WorkerThread<TaskType> where TaskType : IRunnable
    {
        protected Control resultCaller = null;

        public GUIWorkerThread(Control resultC)
        {
            resultCaller = resultC;
        }

        /// <summary>
        /// Gets or sets result control for notification of task completion
        /// </summary>
        public Control resultControl
        {
            get { return resultCaller; }
            set { resultCaller = value; }
        }

        /// <summary>
        /// Nofities about task completion by calling Invoke on the control
        /// </summary>
        /// <param name="task"></param>
        protected override void notifyTaskCompletion(TaskType task)
        {
            if (resultCaller != null)
            {
                resultCaller.Invoke(new onTaskCompleteDelegate(this.controlThreadCaller), new Object[] { task });
            }
        }

        /// <summary>
        /// Called in the Control GUI thread and provides event notification
        /// </summary>
        /// <param name="task"></param>
        private void controlThreadCaller(TaskType task)
        {
            base.notifyTaskCompletion(task);
        }
    }
}
