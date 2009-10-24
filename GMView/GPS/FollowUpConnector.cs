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
        /// The way we follow by
        /// </summary>
        private ncGeo.WayBase followWay;
        /// <summary>
        /// Current waypoint that we follow to
        /// </summary>
        private LinkedListNode<ncGeo.WayBase.WayPoint> currentWPNode;
        /// <summary>
        /// Last, finish waypoint - our destination
        /// </summary>
        private ncGeo.WayBase.WayPoint finishWP;

        /// <summary>
        /// Distance to the current following node
        /// </summary>
        private double currentDistance = 99999999;

        /// <summary>
        /// Distance to the finish waypoint
        /// </summary>
        private double finishDistance = 0;

        /// <summary>
        /// Angle that directs our GPS position to the finish waypoint
        /// </summary>
        private double finishAngle = 0;

        /// <summary>
        /// Angle that directs our current position to the current waypoint we follow to.
        /// </summary>
        private double currentAngle = 0;

        /// <summary>
        /// Distance to the nearest point as a string
        /// </summary>
        private string distanceS = string.Empty;

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
                followWay = followTrack.wayObject;
                currentWPNode = followWay.wayPoints.First;
                finishWP = followWay.wayPoints.Last.Value;
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
            if (currentWPNode == null || pos == currentPos)
                return;
            currentPos = pos;

            LinkedListNode <WayBase.WayPoint> wpt = currentWPNode.Next;
            double dist = currentDistance;
            bool found = false;
            while (wpt != null)
            {
                dist = CommonGeo.getDistanceByLonLat2(wpt.Value.point.lon, wpt.Value.point.lat, 
                                                      currentPos.lon, currentPos.lat);
                if (dist < currentDistance)
                {
                    currentWPNode = wpt;
                    currentDistance = dist;
                    found = true;
                }
                wpt = wpt.Next;
            }

            if (found)
            {
                currentAngle = calculateAngle(currentPos, currentWPNode.Value.point);
                wpt = currentWPNode;
                finishDistance = currentDistance;
                while (wpt != null)
                {
                    finishDistance += wpt.Value.distance_to_next;
                    wpt = wpt.Next;
                }
                finishAngle = calculateAngle(currentPos, finishWP.point);
            }

        }

        /// <summary>
        /// Calculates and returns angle (azimuth) between north and the line given by two points
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private double calculateAngle(NMEA_LL from, NMEA_LL to)
        {
            double dx = from.lon - to.lon;
            double dy = from.lat - to.lat;

            double angle = Math.Atan(Math.Abs(dx) / Math.Abs(dy));
            if (dy < 0 && dx < 0)
                angle += 180.0 * CommonGeo.deg2rad;
            else
                if (dy < 0)
                    angle += 90.0 * CommonGeo.deg2rad;
                else
                    if (dx < 0)
                        angle += 270.0 * CommonGeo.deg2rad;

            return angle;
        }
    }
}
