using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
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
    public class FollowUpConnector: ISprite
    {
        /// <summary>
        /// The flag indicates the need of displaying information
        /// </summary>
        private bool shown = false;

        /// <summary>
        /// Is our widget shown
        /// </summary>
        public bool isShown
        {
            get { return shown; }
        }

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
        /// Return angle of the finish point against our position
        /// </summary>
        public double finishAng
        {
            get
            {
                return finishAngle;
            }
        }

        /// <summary>
        /// Angle that directs our current position to the current waypoint we follow to.
        /// </summary>
        private double currentAngle = 0;

        /// <summary>
        /// Angle between our position and next waypoint
        /// </summary>
        public double currentAng
        {
            get
            {
                return currentAngle;
            }
        }

        /// <summary>
        /// Distance to the nearest point as a string
        /// </summary>
        private string curDistanceS = string.Empty;

        /// <summary>
        /// Return distance to the current WP as string
        /// </summary>
        public string currentDistanceS
        {
        		get
        		{
                    return curDistanceS;
        		}
        }

        /// <summary>
        /// Distance to the finish WP as a string
        /// </summary>
        private string finDistanceS = string.Empty;

        /// <summary>
        /// Return distance to the finish WP as a string
        /// </summary>
        public string finishDistanceS
        {
            get
            {
                return finDistanceS;
            }
        }

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
                if (followTrack == null)
                {
                    shown = false;
                    followWay = null;
                    currentWPNode = null;
                    finishWP = null;
                }
                else
                {
                    followWay = followTrack.wayObject;
                    currentWPNode = followWay.wayPoints.First;
                    if (currentWPNode == null)
                    {
                        throw new ArgumentNullException("Waypoint", "Selected empty route. There is no waypoints to follow.\nPlease, choose another track.");
                    }
                    finishWP = followWay.wayPoints.Last.Value;
                    shown = true;
                }
            }
        }

        /// <summary>
        /// Constructor that register our connector with the current track
        /// </summary>
        public FollowUpConnector()
        {
            GPSTrackFactory.singleton.recordingTrack.onTrackChanged += trackDataChanged;
            GPSTrackFactory.singleton.onRecordingTrackChanged += onRecordingTrackChanged;
            onRecordingTrackChanged(GPSTrackFactory.singleton.recordingTrack);

            bgdot = TextureFactory.singleton.getImg(TextureFactory.TexAlias.FollowInfo);
            bgtex = TextureFactory.singleton.getTex(bgdot);
            fnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Big24R);
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
        /// Hit distance - 300 meters - if we are near out WP (less than 300 m) then we hit it
        /// </summary>
        private const double hitDist = 0.1;

        private NMEA_LL.PointType oldwayType = NMEA_LL.PointType.TP;

        /// <summary>
        /// fills in all info about new current WP
        /// </summary>
        /// <param name="wpt"></param>
        /// <param name="dist"></param>
        private void setCurrentWP(LinkedListNode<WayBase.WayPoint> wpt, double dist)
        {
            currentDistance = dist;
            currentWPNode = wpt;
            currentAngle = calculateAngle(currentPos, currentWPNode.Value.point);

            if (currentWPNode.Value.ptype != NMEA_LL.PointType.MARKWP)
            {
                oldwayType = currentWPNode.Value.ptype;
                currentWPNode.Value.ptype = NMEA_LL.PointType.MARKWP;
            }

            finishDistance = currentDistance;
            while (wpt != null)
            {
                finishDistance += wpt.Value.distance_to_next;
                wpt = wpt.Next;
            }
            finishAngle = calculateAngle(currentPos, finishWP.point);

            curDistanceS = currentDistance.ToString("F2", ncUtils.Glob.numformat);
            finDistanceS = finishDistance.ToString("F2", ncUtils.Glob.numformat);
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

            //first we need to check our current WP - do we hit it or not?
            //if we hit it then switch to next WP
            LinkedListNode <WayBase.WayPoint> wpt = currentWPNode;

            currentDistance = CommonGeo.getDistanceByLonLat2(wpt.Value.point.lon, wpt.Value.point.lat,
                                                      currentPos.lon, currentPos.lat);
            //we hit it!
            if (currentDistance <= hitDist)
            {
                if (currentWPNode.Value.ptype == NMEA_LL.PointType.MARKWP)
                {
                    currentWPNode.Value.ptype = oldwayType;
                }

                wpt = currentWPNode.Next;
                if (wpt != null)
                {
                    setCurrentWP(wpt, CommonGeo.getDistanceByLonLat2(wpt.Value.point.lon,
                                                                     wpt.Value.point.lat,
                                                                     currentPos.lon,
                                                                     currentPos.lat));
                }
                else
                    return;
            }

            //find nearest WP down the route
            double dist = currentDistance;
            bool found = false;
            while (wpt != null)
            {
                dist = CommonGeo.getDistanceByLonLat2(wpt.Value.point.lon, wpt.Value.point.lat, 
                                                      currentPos.lon, currentPos.lat);
                if (dist < currentDistance)
                {
                    if (currentWPNode.Value.ptype == NMEA_LL.PointType.MARKWP)
                    {
                        currentWPNode.Value.ptype = oldwayType;
                    }
                    currentWPNode = wpt;
                    currentDistance = dist;
                    found = true;
                }
                wpt = wpt.Next;
            }

            setCurrentWP(currentWPNode, currentDistance);
        }

        /// <summary>
        /// Calculates and returns angle (azimuth) between north and the line given by two points
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private double calculateAngle(NMEA_LL from, NMEA_LL to)
        {
            double dx = to.lon - from.lon;
            double dy = to.lat - from.lat;

            double angle = Math.Atan(Math.Abs(dx) / Math.Abs(dy)) / CommonGeo.deg2rad;
//            double angle = Math.Asin(Math.Abs(dx) / Math.Sqrt(dx * dx + dy * dy)) / CommonGeo.deg2rad;
            
            if (dy < 0 && dx < 0)
                angle += 180.0;
            else
                if (dy < 0)
                    angle = 180 - angle;
                else
                    if (dx < 0)
                        angle = 360.0 - angle;
            
            return angle;
        }

        #region ISprite Members

        /// <summary>
        /// Image info about our background
        /// </summary>
        private ImageDot bgdot;

        /// <summary>
        /// Background image loaded as texture
        /// </summary>
        private object bgtex;

        private Color cOrange = Color.FromArgb(252, 165, 27);

        /// <summary>
        /// Font for drawing distance
        /// </summary>
        private IGLFont fnt;

        public void glDraw(int centerx, int centery)
        {
           if (!shown)
               return;

            int sx, sy;
            if (Program.opt.dash_right_side)
            {
                sx = -centerx;
                sy = centery;
            }
            else
            {
                sx = centerx - bgdot.delta_x;
                sy = centery;
            }

            GML.device.pushMatrix();
            GML.device.identity();
            GML.device.texDrawBegin();

            GML.device.texDraw(bgtex, sx, sy, 5, bgdot.img.Width, bgdot.img.Height);

            GML.device.texDrawEnd();

            GML.device.color(cOrange);

            fnt.drawright(curDistanceS, sx + 102, sy - 11, 5);
            fnt.drawright(finDistanceS, sx + 102, sy - 52, 5);

            GML.device.color(Color.White);
            GML.device.popMatrix();
        }

        public int dLevel
        {
            get
            {
                return 3;
            }
            set
            {
            }
        }

        public void show()
        {
        }

        public void hide()
        {
        }

        public void draw(System.Drawing.Graphics gr)
        {

        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {

        }


        #endregion
    }
}
