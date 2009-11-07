using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ncGeo
{
    /// <summary>
    /// Base class for ways with waypoints
    /// </summary>
    public abstract class WayBase
    {
        protected LinkedList<WayPoint> points = new LinkedList<WayPoint>();
        protected double total_distance = 0.0;
        protected TimeSpan total_time;
        protected int total_points = 0;
        protected WayPoint currentWayPoint = null;
        protected string way_name = "Route: manual";

        public class WayPoint
        {
            public int num = 1;
            public int x, y;
            public NMEA_LL point;
            public NMEA_LL.PointType ptype = NMEA_LL.PointType.TP;
            public double distance_from_prev = 0;
            public TimeSpan time_from_prev;
            public double distance_to_next = 0;
            public TimeSpan time_to_next;
            public double distance_from_start = 0;
            public TimeSpan time_from_start;

            public string typeString
            {
                get
                {
                    switch (ptype)
                    {
                        case NMEA_LL.PointType.STARTP:
                            return "Start WP";
                        case NMEA_LL.PointType.ENDTP:
                            return "Finish WP";
                        case NMEA_LL.PointType.SWP:
                            return "Delay WP";
                        default:
                            return "Waypoint";
                    }
                }
            }
        }

        /// <summary>
        /// Base constructor for Way
        /// </summary>
        public WayBase()
        {
        }


        /// <summary>
        /// Recalculate parameters for last waypoint
        /// </summary>
        /// <param name="mapo"></param>
        public virtual void recalc_last_waypoint(BaseGeo geo)
        {
            WayPoint lastwp = points.Last.Value;
            Point xy;
            geo.getXYByLonLat(lastwp.point.lon, lastwp.point.lat, out xy);
            lastwp.x = xy.X;
            lastwp.y = xy.Y;
            if (points.Last.Previous != null)
            {
                WayPoint prevwp = points.Last.Previous.Value;
                lastwp.time_from_prev = lastwp.point.utc_time - prevwp.point.utc_time;
                prevwp.time_to_next = lastwp.time_from_prev;
                prevwp.distance_to_next = lastwp.distance_from_prev;
            }
        }

        /// <summary>
        /// Name of this way
        /// </summary>
        public virtual string name
        {
            get { return way_name; }
            set { way_name = value; }
        }

        /// <summary>
        /// Adds new waypoint to the way with the given point and distance
        /// </summary>
        /// <param name="point"></param>
        /// <param name="distance"></param>
        public virtual void add(NMEA_LL point, double distance)
        {
            WayPoint wp = new WayPoint();
            wp.point = point;
            wp.distance_from_prev = distance;
            add(wp, point.ptype);
        }

        /// <summary>
        /// Adds waypoint to the way with the given type
        /// </summary>
        /// <param name="wp"></param>
        /// <param name="pt"></param>
        public virtual void add(WayPoint wp, NMEA_LL.PointType pt)
        {
            wp.distance_from_prev -= total_distance;
            total_distance += wp.distance_from_prev;
            wp.ptype = pt;
            wp.point.ptype = pt;

            if (total_points > 0)
            {
                WayPoint lastwp = points.Last.Value;
                wp.time_from_prev = wp.point.utc_time - lastwp.point.utc_time;
                if(wp.time_from_prev.TotalHours < 0.0)
                    wp.time_from_prev = lastwp.point.utc_time - wp.point.utc_time;

                total_time = wp.point.utc_time - points.First.Value.point.utc_time;
                if(total_time.TotalHours < 0.0)
                    total_time = points.First.Value.point.utc_time - wp.point.utc_time;
                wp.time_from_start = total_time;
                wp.distance_from_start = total_distance;
                lastwp.time_to_next = wp.time_from_prev;
                lastwp.distance_to_next = wp.distance_from_prev;
                wp.num = lastwp.num + 1;
            }
            else
                wp.time_from_prev = new TimeSpan();

            points.AddLast(wp);
            currentWayPoint = wp;
            total_points++;
        }

        /// <summary>
        /// Deletes last waypoint from the way
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual WayPoint delLastFromWay(NMEA_LL point)
        {
            if (total_points == 0)
                return null;
            WayPoint wp = points.Last.Value;
            if (wp.point != point)
                return null;
            points.RemoveLast();
            total_points--;
            total_distance -= wp.distance_from_prev;

            if (total_points > 1)
                total_time = points.Last.Value.time_from_start;
            else
                total_time = new TimeSpan();

            if (currentWayPoint == wp)
            {
                stepNextPoint();
            }
            return wp;
        }

        /// <summary>
        /// Clear the way and delete all waypoints
        /// </summary>
        public virtual void clear()
        {
            points.Clear();
            total_distance = 0.0;
            total_points = 0;
            currentWayPoint = null;
        }

        /// <summary>
        /// Update xy position of each waypoint (in pixel) based on the given Geo system
        /// </summary>
        /// <param name="geo"></param>
        public virtual void updateXY(BaseGeo geo)
        {
            Point xy;
            foreach (WayPoint wplnk in points)
            {
                geo.getXYByLonLat(wplnk.point.lon, wplnk.point.lat, out xy);
                wplnk.x = xy.X;
                wplnk.y = xy.Y;
            }
        }

        /// <summary>
        /// Adds new waypoint by a given point and type if it does not exist
        /// </summary>
        /// <param name="last_ll"></param>
        /// <param name="distance"></param>
        /// <param name="pt"></param>
        public virtual void markWay(NMEA_LL last_ll, double distance, NMEA_LL.PointType pt)
        {
            if (points.Last == null || points.Last.Value.point != last_ll)
            {
                add(last_ll, distance);
            }

            points.Last.Value.ptype = pt;
            points.Last.Value.point.ptype = pt;
        }

        /// <summary>
        /// Return current WayPoint
        /// </summary>
        public WayPoint currentWP
        {
            get { return currentWayPoint; }
        }

        /// <summary>
        /// Return a linked list of all waypoints of this way
        /// </summary>
        public LinkedList<WayPoint> wayPoints
        {
            get { return points; }
        }

        /// <summary>
        /// Go to next waypoint in our way and make it current
        /// </summary>
        /// <returns></returns>
        public virtual WayPoint stepNextPoint()
        {
            LinkedListNode<WayPoint> wplink = points.First;
            while (wplink != null && wplink.Value != currentWayPoint)
                wplink = wplink.Next;
            if (wplink == null)
            {
                currentWayPoint = null;
                return null;
            }
            else
                wplink = wplink.Next;
            if (wplink == null)
                wplink = points.First;
            if (wplink == null)
                currentWayPoint = null;
            else
                currentWayPoint = wplink.Value;
            return currentWayPoint;
        }

        /// <summary>
        /// Go to previous waypoint and make it current
        /// </summary>
        /// <returns></returns>
        public virtual WayPoint stepPrevPoint()
        {
            LinkedListNode<WayPoint> wplink = points.Last;
            while (wplink != null && wplink.Value != currentWayPoint)
                wplink = wplink.Previous;
            if (wplink == null)
            {
                currentWayPoint = null;
                return null;
            }
            else
                wplink = wplink.Previous;
            if (wplink == null)
                wplink = points.Last;
            if (wplink == null)
                currentWayPoint = null;
            else
                currentWayPoint = wplink.Value;
            return currentWayPoint;
        }

    }
}
