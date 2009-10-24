using System;
using System.Collections.Generic;
using System.Text;

namespace ncGeo
{
    /// <summary>
    /// Delegate for firing event when track changes
    /// </summary>
    public delegate void onTrackChangedDelegate();

    /// <summary>
    /// Interface for GPS track class
    /// </summary>
    public interface IGPSTrack
    {
        /// <summary>
        /// Number of points in the track
        /// </summary>
        int countPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Distance of the track
        /// </summary>
        double distance
        {
            get;
            set;
        }

        /// <summary>
        /// Total travel time
        /// </summary>
        TimeSpan travel_time
        {
            get;
            set;
        }
        /// <summary>
        /// Start time of the track
        /// </summary>
        DateTime startTime
        {
            get;
        }

        /// <summary>
        /// finish date of the track
        /// </summary>
        DateTime endTime
        {
            get;
        }

        /// <summary>
        /// Average speed on track
        /// </summary>
        double avg_speed
        {
            get;
        }

        /// <summary>
        /// Maximum speed
        /// </summary>
        double max_speed
        {
            get;
        }

        /// <summary>
        /// Last point on the track
        /// </summary>
        NMEA_LL lastData
        {
            get;
        }

        /// <summary>
        /// Last active point on the track
        /// </summary>
        NMEA_LL lastNonZeroPos
        {
            get;
        }

        /// <summary>
        /// Track name
        /// </summary>
        string track_name
        {
            get;
            set;
        }

        /// <summary>
        /// Track data - list of all points
        /// </summary>
        LinkedList<NMEA_LL> trackPointData
        {
            get;
        }
        /// <summary>
        /// Find nearest point on the track using Find context
        /// </summary>
        /// <param name="ctx"></param>
        void findNearest(IFindPoint ctx);

        /// <summary>
        /// Event fires when GPS data in the track changes
        /// </summary>
        event onTrackChangedDelegate onTrackChanged;

        /// <summary>
        /// Return Way (route) of this track
        /// </summary>
        WayBase wayObject
        {
            get;
        }
    }
}
