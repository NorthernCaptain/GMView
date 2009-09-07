using System;
using System.Collections.Generic;
using System.Text;

namespace ncUtils
{
    /// <summary>
    /// Interface that provide run() method, usefull for task processing
    /// </summary>
    public interface IRunnable
    {
        /// <summary>
        /// Implement the task processing.
        /// </summary>
        void run();
    }
}
