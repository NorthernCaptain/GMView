using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ncUtils
{
    /// <summary>
    /// Class privides worker thread for processing time consuming tasks and retrieve the results.
    /// It uses SyncQueue for receiving tasks and event method for delivering results
    /// </summary>
    public class WorkerThread<TaskType> where TaskType : IRunnable
    {
        /// <summary>
        /// Delegate that will be called in worker thread after task is completed
        /// </summary>
        /// <param name="completedTask"></param>
        public delegate void onTaskCompleteDelegate(TaskType completedTask);

        /// <summary>
        /// Event that is called on task completion in the worker thread.
        /// </summary>
        public event onTaskCompleteDelegate taskCompleted;

        /// <summary>
        /// Queue of our tasks to perform
        /// </summary>
        protected SyncQueue<TaskType> taskQueue = new SyncQueue<TaskType>();

        /// <summary>
        /// Worker thread itself
        /// </summary>
        protected Thread worker;

        /// <summary>
        /// Starts the worker thread.
        /// </summary>
        public virtual void start()
        {
            worker = new Thread(new ParameterizedThreadStart(processWorkerStub));
            worker.IsBackground = true;
            worker.Start(this);
        }

        /// <summary>
        /// Stops the thread with abort call.
        /// </summary>
        public virtual void stop()
        {
            done = true;
            if (worker == null)
                return;
            try
            {
                worker.Abort();
            }
            catch { };

            worker = null;
        }

        /// <summary>
        /// Stops the tread and wait for its completion during 3 sec.
        /// </summary>
        public virtual void stopWait()
        {
            done = true;
            if (worker == null)
                return;
            try
            {
                worker.Join(3000);
            }
            catch { };

            if (worker.IsAlive)
                this.stop();
        }

        /// <summary>
        /// Put the task into the queue for processing
        /// </summary>
        /// <param name="task"></param>
        public void addTask(TaskType task)
        {
            this.taskQueue.Enqueue(task);
        }


        /// <summary>
        /// Stub method for calling worker with our object
        /// </summary>
        /// <param name="o"></param>
        static private void processWorkerStub(Object o)
        {
            WorkerThread<TaskType> mgr = (WorkerThread<TaskType>)o;
            mgr.processWorkerThread();
        }

        protected bool threadDone = false;

        /// <summary>
        /// Flag for stopping worker thread after it finished the task.
        /// </summary>
        public virtual bool done
        {
            get { return threadDone; }
            set { threadDone = value; }
        }

        /// <summary>
        /// Processing method works in the worker thread and pulls tasks from the queue to execute them.
        /// </summary>
        protected virtual void processWorkerThread()
        {
            while (!threadDone)
            {
                TaskType task = taskQueue.Dequeue();

                task.run();

                notifyTaskCompletion(task);
            }
        }

        /// <summary>
        /// Calls by worker thread when the task is completed. Allows to override notification behavior
        /// </summary>
        /// <param name="task"></param>
        protected virtual void notifyTaskCompletion(TaskType task)
        {
            if (taskCompleted != null)
                taskCompleted(task);
        }
    }
}
