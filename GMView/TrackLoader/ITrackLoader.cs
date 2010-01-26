using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Interface for loading track file into GPS Track + loading waypoint for the track
    /// if the file has it.
    /// </summary>
    public interface ITrackLoader
    {
        /// <summary>
        /// Check file format and return true if the file is our recognized format
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool isOurFormat(GPS.TrackFileInfo info);

        /// <summary>
        /// Do pre-loading of the file and fills infomation in FileInfo object
        /// </summary>
        /// <param name="info"></param>
        void preLoad(GPS.TrackFileInfo info);

        /// <summary>
        /// Loads data from file supplied by FileInfo and return GPSTrack
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        GPSTrack load(GPS.TrackFileInfo info);
    }
}
