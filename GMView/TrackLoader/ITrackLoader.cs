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
        /// Gets the loading file filter for track or null if loading does not supported
        /// </summary>
        /// <returns></returns>
        ncFileControls.FileFilter trackLoadFileFilter();

        /// <summary>
        /// Gets the saving file filter for the track or null
        /// </summary>
        /// <returns></returns>
        ncFileControls.FileFilter trackSaveFileFilter();

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
