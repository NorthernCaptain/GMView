using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.TrackLoader
{
    public interface IExporter: IDisposable
    {
        /// <summary>
        /// Called when we about to start our exporting work
        /// </summary>
        void startWork();

        /// <summary>
        /// Called every time we need to export a tile of a map
        /// </summary>
        /// <param name="tile"></param>
        void processOneTile(ImgTile tile);

        /// <summary>
        /// Called when we finishes our work, so to flush all buffers
        /// </summary>
        void finalizeWork();
    }
}
