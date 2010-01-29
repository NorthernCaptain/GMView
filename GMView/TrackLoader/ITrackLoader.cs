using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Interface for loading track file into GPS Track + loading waypoint for the track
    /// if the file has it.
    /// </summary>
    public interface ITrackLoader: IFormatLoader
    {
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
        GPSTrack load(GPS.TrackFileInfo info, BookMarkFactory poiFact, Bookmarks.POIGroupFactory igroupFact);

        /// <summary>
        /// Saves track and optionally surrounding POI into a file
        /// </summary>
        /// <param name="track"></param>
        /// <param name="fi"></param>
        /// <param name="poiFact"></param>
        /// <param name="igroupFact"></param>
        void save(GPSTrack track, GPS.TrackFileInfo fi, BookMarkFactory poiFact, Bookmarks.POIGroupFactory igroupFact);
    }
}
