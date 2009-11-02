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
        private LinkedList<WayBase.WayPoint> followWay = new LinkedList<WayBase.WayPoint>();
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
        /// Estimated travel time to finish waypoint
        /// </summary>
        private string finTravelTimeS = string.Empty;

        /// <summary>
        /// Estimated travel time to finish waypoint as string
        /// </summary>
        public string finishEstimTimeS
        {
            get
            {
                return finTravelTimeS;
            }
        }

        /// <summary>
        /// Estimated travel time to the next waypoint
        /// </summary>
        private string curTravelTimeS = string.Empty;

        /// <summary>
        /// Estimated travel time to the next waypoint
        /// </summary>
        public string currentEstimTimeS
        {
            get
            {
                return curTravelTimeS;
            }
        }

        private bool reverse = false;

        /// <summary>
        /// Setter for reverse direction of the follower
        /// </summary>
        public bool reverseDir
        {
            get
            {
                return reverse;
            }
            set
            {
            	reverse = value;
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
                    followWay.Clear();
                    currentWPNode = null;
                    finishWP = null;
                }
                else
                {
                    followWay.Clear();
                    if (followTrack.wayObject.wayPoints.Count < 4)
                        followTrack.sliceTrackIntoWay();

                    if (!reverse)
                    {
                        LinkedListNode<WayBase.WayPoint> wpt = followTrack.wayObject.wayPoints.First;
                        while (wpt != null)
                        {
                            followWay.AddLast(wpt.Value);
                            wpt = wpt.Next;
                        }
                    }
                    else
                    {
                        LinkedListNode<WayBase.WayPoint> wpt = followTrack.wayObject.wayPoints.Last;
                        while (wpt != null)
                        {
                            followWay.AddLast(wpt.Value);
                            wpt = wpt.Previous;
                        }
                    }

                    currentWPNode = followWay.First;
                    if (currentWPNode == null)
                    {
                        throw new ArgumentNullException("Waypoint", "Selected empty route. There is no waypoints to follow.\nPlease, choose another track.");
                    }
                    finishWP = followWay.Last.Value;
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
            sfnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Big20I);
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
        /// Hit distance - 200 meters - if we are near out WP (less than 200 m) then we hit it
        /// </summary>
        private const double hitDist = 0.2;

        private NMEA_LL.PointType oldwayType = NMEA_LL.PointType.TP;

        /// <summary>
        /// fills in all info about new current WP
        /// </summary>
        /// <param name="wpt"></param>
        /// <param name="dist"></param>
        private void setCurrentWP(LinkedListNode<WayBase.WayPoint> wpt, double dist, double totalDist)
        {
            currentDistance = dist;
            currentWPNode = wpt;
            currentAngle = calculateAngle(currentPos, currentWPNode.Value.point);

            if (currentWPNode.Value.ptype != NMEA_LL.PointType.MARKWP)
            {
                oldwayType = currentWPNode.Value.ptype;
                currentWPNode.Value.ptype = NMEA_LL.PointType.MARKWP;
            }

            if (totalDist >= 0)
            {
                finishDistance = totalDist;
            }
            else
            {
                finishDistance = currentDistance;
                while (wpt != null)
                {
                    finishDistance += wpt.Value.distance_to_next;
                    wpt = wpt.Next;
                }
            }

            finishAngle = calculateAngle(currentPos, finishWP.point);

            curDistanceS = currentDistance.ToString("F2", ncUtils.Glob.numformat);
            finDistanceS = finishDistance.ToString("F2", ncUtils.Glob.numformat);

            double speed = recordingTrack.avg_speed;
            if (speed > 0.5)
            {
                DateTime dat = DateTime.MinValue.Add(TimeSpan.FromHours(finishDistance / speed));
                finTravelTimeS = dat.ToString("HH:mm:ss");
            }

            speed = recordingTrack.lwp_avg_speed;
            if(speed > 0.5)
            {
                DateTime dat = DateTime.MinValue.Add(TimeSpan.FromHours(currentDistance / speed));
                curTravelTimeS = dat.ToString("HH:mm:ss");
            }
        }

        /// <summary>
        /// Distance to the current waypoint from our previous GPS position
        /// </summary>
        private double previousDistance = 100.0;

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
            if (currentDistance <= hitDist && currentDistance > previousDistance + 0.01)
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
                                                                     currentPos.lat), -1);
                }
                else
                {
                    currentWPNode = null;
                    curDistanceS = "---";
                    finDistanceS = "---";
                    curTravelTimeS = "xx:xx";
                    finTravelTimeS = "xx:xx";
                    currentAngle = 0;
                    finishAngle = 0;
                    return;
                }
            }

            previousDistance = currentDistance;

            //find nearest WP down the route
            double dist = currentDistance;
            double totalDist = dist;

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
                }
                totalDist += wpt.Value.distance_to_next;

                wpt = wpt.Next;
            }

            setCurrentWP(currentWPNode, currentDistance, totalDist);
        }

        /// <summary>
        /// Calculates and returns angle (azimuth) between north and the line given by two points
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private double calculateAngle(NMEA_LL from, NMEA_LL to)
        {
            ncGeo.BaseGeo geo = followTrack.geosystem;

            Point xy1, xy2;

            geo.getXYByLonLat(from.lon, from.lat, out xy1);
            geo.getXYByLonLat(to.lon, to.lat, out xy2);
            double dx = xy1.X - xy2.X;
            double dy = xy1.Y - xy2.Y;

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
        private IGLFont sfnt;

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

            fnt.drawright(curDistanceS, sx + 100, sy - 11, 5);
            sfnt.drawright(curTravelTimeS, sx + 100, sy - 39, 5);
            fnt.drawright(finDistanceS, sx + 100, sy - 78, 5);
            sfnt.drawright(finTravelTimeS, sx + 100, sy -104, 5);

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
