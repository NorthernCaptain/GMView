using System;
using System.Collections.Generic;
using System.Text;
using ncGeo;

namespace GMView.GPS
{
    /// <summary>
    /// This class implements logic for following up selected track.
    /// It find nearest waypoint in the following track according to the current GPS position,
    /// calculates distance to this waypoint and distance to the end of the track.
    /// Distance to the current following waypoint calculated as direct line distance.
    /// Distance to the finish waypoint calculated according to the track we follow.
    /// </summary>
    public class FollowUpConnector
    {
        /// <summary>
        /// The track we follow
        /// </summary>
        private ncGeo.IGPSTrack      followTrack;
        /// <summary>
        /// Last or current GPS position
        /// </summary>
        private ncGeo.NMEA_LL currentPos;

        /// <summary>
        /// Track that currently in recording mode
        /// </summary>
        private ncGeo.IGPSTrack recordingTrack;

        /// <summary>
        /// Setter for followTrack
        /// </summary>
        public ncGeo.IGPSTrack follower
        {
            get
            {
                return followTrack;
            }
            set
            {
                followTrack = value;
            }
        }

        /// <summary>
        /// Constructor that register our connector with the current track
        /// </summary>
        public FollowUpConnector()
        {

            GPSTrackFactory.singleton.recordingTrack.onTrackChanged += trackDataChanged;
            GPSTrackFactory.singleton.onRecordingTrackChanged += onRecordingTrackChanged;

        }

        /// <summary>
        /// Called from GPS factory when user changes track for recording;
        /// </summary>
        /// <param name="gtrack"></param>
        private void onRecordingTrackChanged(GPSTrack gtrack)
        {
            if (recordingTrack != null)
            {
                recordingTrack.onTrackChanged -= trackDataChanged;
            }
            recordingTrack = gtrack;
            recordingTrack.onTrackChanged += trackDataChanged;
        }

        /// <summary>
        /// Call in NMEA thread when we have new GPS position in our current track. 
        /// We do all our calculation here having new position and follower.
        /// </summary>
        private void trackDataChanged()
        {
            NMEA_LL pos = recordingTrack.lastNonZeroPos;
            if (pos == currentPos)
                return;
            currentPos = pos;

            
        }


    }
}
