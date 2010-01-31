using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Basic interface for loaders from different formats
    /// </summary>
    public interface IFormatLoader: ICloneable
    {
        /// <summary>
        /// Check file format and return true if the file is our recognized format
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool isOurFormat(GPS.TrackFileInfo info);

    }
}
