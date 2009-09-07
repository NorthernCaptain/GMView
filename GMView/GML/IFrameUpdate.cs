using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    public interface IFrameUpdate
    {
        /// <summary>
        /// Called by timer indicating that it's time to do update
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns>return true if we need to redraw screen after our update</returns>
        bool updateFrame(ulong ticks);

        bool registered(ulong start_ticks);
        bool unregistered(ulong end_ticks);
    }
}
