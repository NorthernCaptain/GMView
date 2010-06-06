using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ncGeo;
using GMView.GPS;

namespace GMView
{
    /// <summary>
    /// Class holds and record GPS position during a period of time
    /// This class can draw tracks on the map also.
    /// </summary>
    [ XmlRoot("gps_track") ]
    public class GPSTrack: IGPSTrack, ISprite, IDisposable
    {
        protected string name = "Track: Current track";
        protected string filename = "track1.gpx";

        /// <summary>
        /// Draw staff
        /// </summary>
        protected bool shown = false;
        protected Pen pen = new Pen(Color.Red, 3);
        protected Pen pen_arrow = new Pen(Color.Blue, 6);
        protected Pen pen_right = new Pen(Color.DarkGreen, 4);
        protected int drawLevel = 1;
        protected ImageDot cursor;
        protected ImageDot end_cursor;
        protected ImageDot start_cursor;
        protected ImageDot arrows;


        protected MapObject mapo;
        /// <summary>
        /// our latest position
        /// </summary>
        protected NMEA_LL lastPos;
        /// <summary>
        /// latest position saved in track
        /// </summary>
        private NMEA_LL lastTrackPos;

        public NMEA_LL LastTrackPos
        {
            get { return lastTrackPos; }
            set { lastTrackPos = value; }
        }
        /// <summary>
        /// last position with non-zero speed
        /// </summary>
        protected NMEA_LL lastSpeedPos;
        protected LinkedList<NMEA_LL> trackData = new LinkedList<NMEA_LL>();
        protected List<LinkedListNode<NMEA_LL> > reducedTrackData = new List<LinkedListNode<NMEA_LL> >(100);
        protected int reduced_running_count;
        protected const int reduced_max_step = 10;
        protected double distance_km = 0.0;
        protected TimeSpan trav_time = new TimeSpan();
        protected double travel_avg_speed = 0.0;
        protected double travel_max_speed = 0.0;

        public enum TrackMode { PositionOnly, RecordingTrack, ViewTrack, ViewSaved };

        protected TrackMode mode = TrackMode.PositionOnly;
        protected List<Point> drawPoints = new List<Point>();

        [XmlIgnore]
        public GPS.Way way = new GPS.Way();

        [XmlAttributeAttribute()]
        public bool need_arrows = true;

        public event onTrackChangedDelegate onTrackChanged;

        private int lastSavedPoint = -1;

        #region Sub classes with additional information about track
        /// <summary>
        /// Text info about GPS track and its waypoints needed for quick display in dashboard
        /// </summary>
        public class TextInfo
        {
            public string track_name;
            public string total_distance;
            public string total_time;
            public string avg_speed;

            public string start_time;
            public string end_time;

            public string waypoint_num;
            public string waypoint_lonlat;
            public string waypoint_time;
            public string waypoint_distance_to_next;
            public string waypoint_distance_from_prev;
            public string waypoint_distance_from_start;
            public string waypoint_time_from_prev;
            public string waypoint_time_to_next;
            public string waypoint_time_from_start;

            /// <summary>
            /// Fills text information from GPSTrack for displaying in the Track dashboard
            /// </summary>
            /// <param name="gtrack"></param>
            internal void fill_all_info(GPSTrack gtrack)
            {
                try
                {
                    track_name = gtrack.track_name;
                    total_distance = (gtrack.distance_km / Program.opt.km_or_miles).ToString("F2");
                    DateTime total_date = DateTime.MinValue.Add(gtrack.trav_time);
                    total_time = total_date.ToString("HH:mm:ss");
                    avg_speed = (gtrack.travel_avg_speed / Program.opt.km_or_miles).ToString("F2");
                    start_time = gtrack.startTime.ToString("dd-MM-yy HH:mm");
                    end_time = gtrack.endTime.ToString("dd-MM-yy HH:mm");
                    Way.WayPoint wp = gtrack.way.currentWP;
                    if (wp != null)
                    {

                        waypoint_num = wp.typeString + " " + wp.num.ToString("D2");
                        waypoint_lonlat = ncUtils.Glob.latString(wp.point.lat) + " / " + ncUtils.Glob.lonString(wp.point.lon);
                        waypoint_time = wp.point.utc_time.ToLocalTime().ToString("dd-MM-yy HH:mm");
                        waypoint_distance_from_prev = (wp.distance_from_prev / Program.opt.km_or_miles).ToString("F2");
                        waypoint_distance_to_next = (wp.distance_to_next / Program.opt.km_or_miles).ToString("F2");
                        waypoint_time_from_prev = DateTime.MinValue.Add(wp.time_from_prev).ToString("HH:mm:ss");
                        waypoint_time_to_next = DateTime.MinValue.Add(wp.time_to_next).ToString("HH:mm:ss");
                        waypoint_time_from_start = DateTime.MinValue.Add(wp.time_from_start).ToString("HH:mm:ss");
                        waypoint_distance_from_start = (wp.distance_from_start / Program.opt.km_or_miles).ToString("F2");
                    }
                    else
                    {
                        waypoint_num = "No waypoint";
                        waypoint_time = string.Empty;
                        waypoint_lonlat = string.Empty;
                        waypoint_distance_from_prev = string.Empty;
                        waypoint_distance_from_start = string.Empty;
                        waypoint_distance_to_next = string.Empty;
                        waypoint_time_from_prev = string.Empty;
                        waypoint_time_from_start = string.Empty;
                        waypoint_time_to_next = string.Empty;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Current information about GPS position, speed, direction displaying in dashboard
        /// during driving a car
        /// </summary>
        public class GPSInfo
        {
            public int curSpeed;
            public string curSpeedS = "";
            public string curLon = "";
            public string curLat = "";
            public string curDir = "";
            public string curAlt = "";

            public string travelTime = "";
            public string travelDistance = "";

            public string satUsed = "";
            public string satHDOP = "";
            public string utcTime = "";
            public string timeZone = "";

            /// <summary>
            /// Fills text information from GPSTrack for displaying in the GPS dashboard
            /// </summary>
            /// <param name="gtrack"></param>
            internal void fill_all_info(GPSTrack gtrack)
            {
                NMEA_LL pos = gtrack.lastPos;
                if (pos == null)
                    return;

                curSpeed = (int)(pos.speed / Program.opt.km_or_miles);
                curSpeedS = curSpeed.ToString();
                curLat = ncUtils.Glob.latString(pos.lat);
                curLon = ncUtils.Glob.lonString(pos.lon);
                curDir = ncUtils.Glob.courseString(pos.dir_angle);
                curAlt = pos.height.ToString("F1");

                travelDistance = (gtrack.distance / Program.opt.km_or_miles).ToString("F2");
                TimeSpan tt = pos.utc_time - gtrack.startTime.ToUniversalTime();
                if (tt.TotalHours > 0.0)
                    travelTime = DateTime.MinValue.Add(tt).ToString("HH:mm:ss");
                else
                    travelTime = "UNDEF";

                satUsed = pos.usedSats.ToString() + "/" + pos.visibleSats;
                satHDOP = pos.HDOP.ToString("F1");

                utcTime = pos.utc_time.ToString("dd/MM/yy HH:mm:ss");
                timeZone = pos.utc_time.ToLocalTime().ToString("zzz");
            }
        }

        /// <summary>
        /// Quick information about any track point that pops up if we hove under the track
        /// </summary>
        public class PointInfo
        {
            public string point_time;
            public string point_speed;

            public string dist_from_start;
            public string time_from_start;

            public string dist_from_lwp;
            public string time_from_lwp;

            internal void fill_all_info(IFindPoint ctx)
            {
                point_time = ctx.resultPoint.Value.utc_time.ToLocalTime().ToString("dd-MM-yy HH:mm:ss");
                point_speed = (ctx.resultPoint.Value.speed / Program.opt.km_or_miles).ToString("F1");
                {
                    TimeSpan tt = ctx.resultPoint.Value.utc_time - ctx.track.trackPointData.First.Value.utc_time;
                    if (tt.TotalHours > 0.0)
                        time_from_start = DateTime.MinValue.Add(tt).ToString("HH:mm:ss");
                    else
                        time_from_start = "??:??:??";
                }

                {
                    double dist_start = 0.0;
                    double dist_lwp = 0.0;

                    LinkedListNode<NMEA_LL> curnode = ctx.resultPoint.Previous;
                    LinkedListNode<NMEA_LL> prevnode;
                    NMEA_LL lwp = null;
                    while (curnode != null)
                    {
                        prevnode = curnode.Next;
                        dist_start += CommonGeo.getDistanceByLonLat2(prevnode.Value.lon, prevnode.Value.lat,
                                curnode.Value.lon, curnode.Value.lat);
                        if (curnode.Value.ptype != NMEA_LL.PointType.TP && lwp == null)
                        {
                            dist_lwp = dist_start;
                            lwp = curnode.Value;
                        }
                        curnode = curnode.Previous;
                    }

                    dist_from_lwp = (dist_lwp / Program.opt.km_or_miles).ToString("F2");
                    dist_from_start = (dist_start / Program.opt.km_or_miles).ToString("F2");
                    time_from_lwp = "??:??:??";

                    if (lwp != null)
                    {
                        TimeSpan tt = ctx.resultPoint.Value.utc_time - lwp.utc_time;
                        if (tt.TotalHours > 0.0)
                            time_from_lwp = DateTime.MinValue.Add(tt).ToString("HH:mm:ss");
                    }
                }
            }
        };


        #endregion
        
        [XmlIgnore]
        public TextInfo textInfo = new TextInfo();

        [XmlIgnore]
        public GPSInfo gpsinfo = new GPSInfo();

        public GPSTrack()
        {
            track_name = "Current GPS track";
            cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.GPSPoint);
            end_cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.ArrowSmall);
            start_cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.StartPos);
            TextureFactory.singleton.onInited += initGLData;
        }

        public GPSTrack(MapObject imapo)
        {
            map = imapo;
            track_name = "Current GPS track";
            cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.GPSPoint);
            end_cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.ArrowSmall);
            start_cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.StartPos);
            pen_arrow.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            pen_right.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            TextureFactory.singleton.onInited += initGLData;
        }

        public GPSTrack(MapObject imapo, ImageDot imd)
        {
            map = imapo;
            track_name = "Current GPS track";
            cursor = imd;
            end_cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.ArrowSmall);
            start_cursor = TextureFactory.singleton.getImg(TextureFactory.TexAlias.StartPos);
            pen_arrow.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            pen_right.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            TextureFactory.singleton.onInited += initGLData;
        }

        #region Set/Get properties and XML serialization

        public void generate_test_track()
        {
            NMEA_RMC rmc = new NMEA_RMC(new NMEAString("$GPRMC,113650.0,A,5548.607,N,03739.387,E,000.01,255.6,210403,08.7,E*69"));
            rmc.parse();
            trackData.AddLast(rmc);
        }

        [ XmlArray("track_points"),
          XmlArrayItem("rmc", typeof(NMEA_RMC)), XmlArrayItem("gga", typeof(NMEA_GGA)) ]
        public NMEA_LL[] trackPoints
        {
            get { NMEA_LL[] ar = new NMEA_LL[trackData.Count]; trackData.CopyTo(ar, 0); return ar; }
            set
            {
                reducedReset();
                for (int i = 0; i < value.Length; i++)
                {
                    lastPos = value[i];
                    lastPos.calculate_dir_xy();
                    trackData.AddLast(lastPos);
                    reducedAdd(trackData.Last);
                }
                reducedAddLast(trackData.Last);
                lastTrackPos = lastPos;
            }
        }

        public LinkedList<NMEA_LL> trackPointData
        {
            get { return trackData; }
        }

        [XmlAttributeAttribute()]
        public string who
        {
            get { return "Knowhere v." + Options.program_version; }
            set { }
        }

        [XmlAttribute("total_points")]
        public int countPoints
        {
            get { return trackData.Count; }
            set { }
        }

        [XmlAttribute("travel_distance")]
        public double distance
        {
            get { return distance_km; }
            set { distance_km = value; }
        }

        [XmlIgnore]
        public TimeSpan travel_time
        {
            get { return trav_time; }
            set { trav_time = value; }
        }

        [XmlIgnore]
        public string fileName
        {
            get { return filename; }
            set
            {
            	filename = value;
            }
        }

        [XmlAttribute("travel_time")]
        public string travel_time_string
        {
            get { return trav_time.ToString(); }
            set { }
        }

        [XmlIgnore]
        public Color trackColor
        {
            get { return pen.Color; }
            set { pen.Color = value; }
        }

        [XmlIgnore]
        public MapObject map
        {
            get { return mapo; }
            set
            {
                mapo = value;
                mapo.onZoomChanged += updateOnZoomChange;
                calculateParameters();
                updateOnZoomChange(-1, -1);
            }
        }

        [XmlIgnore]
        public bool on_air
        {
            get { return mode == TrackMode.RecordingTrack; }
            set
            {
                if (value)
                    mode = TrackMode.RecordingTrack;
                else
                {
                    if (trackData.Count > 0)
                        mode = TrackMode.ViewTrack;
                    else
                        mode = TrackMode.PositionOnly;
                }
            }
        }

        [ XmlIgnore ]
        public TrackMode trackMode
        {
            get { return mode; }
            set { mode = value; }
        }

        public DateTime startTime
        {
            get
            {
                if (trackData.Count == 0)
                    return DateTime.Now;
                else
                    return trackData.First.Value.utc_time.ToLocalTime();
            }
        }

        public DateTime endTime
        {
            get
            {
                if (lastTrackPos != null)
                    return lastTrackPos.utc_time.ToLocalTime();
                return DateTime.Now;
            }
        }

        public double avg_speed
        {
            get { return travel_avg_speed; }
        }

        public double max_speed
        {
            get { return travel_max_speed; }
        }

        public NMEA_LL lastData
        {
            get { return lastPos; }
            set { lastPos = value; }
        }

        /// <summary>
        /// Get or set last speed position (non-zero speed)
        /// </summary>
        public NMEA_LL lastNonZeroPos
        {
            get { return lastSpeedPos; }
            set { lastSpeedPos = value;}
        }

        public override string ToString()
        {
            return name;
        }

        [XmlIgnore]
        public string track_name
        {
            get { return name; }
            set 
            { 
                if(!value.StartsWith("Track:"))
                    name = "Track: " + value; 
                else
                    name = value;

                textInfo.fill_all_info(this); 
            }
        }
        #endregion

        #region Track points processing methods

        private void reducedReset()
        {
            reducedTrackData.Clear();
            reduced_running_count = reduced_max_step;
        }

        private void reducedAdd(LinkedListNode<NMEA_LL> point)
        {
            if (reduced_running_count >= reduced_max_step || point.Value.ptype != NMEA_LL.PointType.TP)
            {
                reducedTrackData.Add(point);
                reduced_running_count = 0;
            }
            else
                reduced_running_count++;
        }

        private void reducedAddLast(LinkedListNode<NMEA_LL> point)
        {
            if(point != null)
                reducedTrackData.Add(point);
        }

        /// <summary>
        /// Removes last point from reduced list of points needed by quick search
        /// </summary>
        private void reducedRemoveLast()
        {
            if (reducedTrackData.Count > 0)
                reducedTrackData.RemoveAt(reducedTrackData.Count - 1);
        }
        /// <summary>
        /// Find point on the track using provided find point context
        /// </summary>
        /// <param name="ctx"></param>
        public void findNearest(IFindPoint ctx)
        {
            if (reducedTrackData.Count < 2)
                return;

            ctx.findStart(this, reducedTrackData[0]);
            foreach (LinkedListNode<NMEA_LL> lnode in reducedTrackData)
            {
                ctx.checkPoint(lnode);
            }

            int count = 0;

            {
                LinkedListNode<NMEA_LL> lnode = ctx.resultPoint.Next;
                for (count = 0; count < reduced_max_step && lnode != null; count++)
                {
                    ctx.checkPoint(lnode);
                    lnode = lnode.Next;
                }
            }

            {
                LinkedListNode<NMEA_LL> lnode = ctx.resultPoint.Previous;
                for (count = 0; count < reduced_max_step && lnode != null; count++)
                {
                    ctx.checkPoint(lnode);
                    lnode = lnode.Previous;
                }
            }

            ctx.findFinish();
        }

        const double stopMinutes = 2.0; //Number of minutes between two points that we consider as stop point
        /// <summary>
        /// Goes through all track points and calculate parameters of the track
        /// </summary>
        public void calculateParameters()
        {
            way.clear();
            trav_time = endTime - startTime;
            if (trav_time.TotalHours < 0.0)
                trav_time = startTime - endTime;

            distance = 0.0;
            travel_max_speed = 0.0;

            reducedReset();

            if (trackData.Count == 0)
                return;

            NMEA_LL first_nm = trackData.First.Value;
            first_nm.ptype = NMEA_LL.PointType.STARTP;
            way.add(first_nm, 0.0);
            LinkedListNode<NMEA_LL> linked_nm = trackData.First;
            while(linked_nm != null && linked_nm.Value != null)
            {
                NMEA_LL nm = linked_nm.Value;
                if (nm != first_nm)
                {
                    distance_km += CommonGeo.getDistanceByLonLat2(first_nm.lon, first_nm.lat,
                                                            nm.lon, nm.lat);
                    if (nm.speed > travel_max_speed)
                        travel_max_speed = nm.speed;

                    if ((nm.utc_time - first_nm.utc_time).TotalMinutes >= stopMinutes)
                    {
                        nm.ptype = NMEA_LL.PointType.SWP;
                    }

                    if (nm.ptype != NMEA_LL.PointType.TP)
                    {
                        way.add(nm, distance_km);
                    }

                    first_nm = nm;
                }
                reducedAdd(linked_nm);
                linked_nm = linked_nm.Next;
            }
            reducedAddLast(trackData.Last);
            travel_avg_speed = distance / trav_time.TotalHours;
            if (mode == TrackMode.ViewSaved)
                way.markWay(lastPos, distance_km, NMEA_LL.PointType.ENDTP);
            textInfo.fill_all_info(this);
        }


        /// <summary>
        /// Process new data from GPS receiver. Do it in the receiver thread, not in the main one.
        /// </summary>
        /// <param name="nmea_ll"></param>
        public void newGPSData(NMEA_LL nmea_ll)
        {
            //currently we use only RMC command, not GGA...
            //and only valid commands are accepted
            if (nmea_ll.type == "RMC" && nmea_ll.state == NMEACommand.Status.DataOK)
            {
                lock (this)
                {
                    lastPos = nmea_ll;

                    if (lastSpeedPos == null || nmea_ll.speed > Program.opt.zero_speed)
                        lastSpeedPos = nmea_ll;

                    if (on_air)
                    {
                        if (!(lastTrackPos != null && (System.Math.Abs(lastTrackPos.lon - nmea_ll.lon) <= Program.opt.gps_same_pos_delta &&
                            System.Math.Abs(lastTrackPos.lat - nmea_ll.lat) <= Program.opt.gps_same_pos_delta)))
                        {
                            if (lastTrackPos != null)
                            {
                                distance_km += CommonGeo.getDistanceByLonLat2(lastTrackPos.lon, lastTrackPos.lat,
                                                                            nmea_ll.lon, nmea_ll.lat);
                                trav_time = nmea_ll.utc_time - trackData.First.Value.utc_time;
                                travel_avg_speed = distance_km / trav_time.TotalHours;
                            }

                            lastTrackPos = nmea_ll;
                            trackData.AddLast(lastTrackPos);
                            if (trackData.Count == 1) //our first point = it's a start
                            {
                                way.markWay(nmea_ll, 0.0, NMEA_LL.PointType.STARTP);
                                way.recalc_last_waypoint(mapo.geosystem);
                            }
                            updateOnZoomChangeNoLock(-1, -1);
                            textInfo.fill_all_info(this);
                        }
                        else
                        {
                            if (trackData.Count > 0)
                            {
                                trav_time = nmea_ll.utc_time - trackData.First.Value.utc_time;
                                travel_avg_speed = distance_km / trav_time.TotalHours;
                            }
                        }
                    }
                    gpsinfo.fill_all_info(this);
                }
                if (onTrackChanged != null)
                    onTrackChanged();

                if (Program.opt.do_autosave && trackData.Count > lastSavedPoint + 20)
                {
                    try
                    {
                        this.save(new GPS.TrackFileInfo((filename.Length > 0 ? filename : Program.opt.autosavefile),
                            TrackFileInfo.SourceType.FileName), BookMarkFactory.singleton, Bookmarks.POIGroupFactory.singleton());
                    }
                    catch (System.Exception)
                    {
                    	
                    }
                    lastSavedPoint = trackData.Count;
                }
            }
        }


        internal void addGPSDataInternal(NMEA_LL nmea_ll)
        {
            lastPos = nmea_ll;

            if (lastSpeedPos == null || nmea_ll.speed > Program.opt.zero_speed)
                lastSpeedPos = nmea_ll;

//            if (on_air)
            {
                if (!(lastTrackPos != null && (System.Math.Abs(lastTrackPos.lon - nmea_ll.lon) <= Program.opt.gps_same_pos_delta &&
                    System.Math.Abs(lastTrackPos.lat - nmea_ll.lat) <= Program.opt.gps_same_pos_delta)))
                {
                    if (lastTrackPos != null)
                    {
                        distance_km += CommonGeo.getDistanceByLonLat2(lastTrackPos.lon, lastTrackPos.lat,
                                                                    nmea_ll.lon, nmea_ll.lat);
                        trav_time = nmea_ll.utc_time - trackData.First.Value.utc_time;
                        travel_avg_speed = distance_km / trav_time.TotalHours;
                    }

                    trackData.AddLast(nmea_ll);
                    lastPos = lastTrackPos = nmea_ll;
                    if (trackData.Count == 1) //our first point = it's a start
                    {
                        way.markWay(nmea_ll, 0.0, NMEA_LL.PointType.STARTP);
                    }
                }
                else
                {
                    if (trackData.Count > 0)
                    {
                        trav_time = nmea_ll.utc_time - trackData.First.Value.utc_time;
                        travel_avg_speed = distance_km / trav_time.TotalHours;
                    }
                }
            }
        }
        /// <summary>
        /// Clear all track data
        /// </summary>
        public void resetTrackData()
        {
            //lastPos = null;
            trackData.Clear();
            drawPoints.Clear();
            way.clear();
            distance_km = 0.0;
            travel_avg_speed = Program.opt.manual_avg_speed;
            travel_time = TimeSpan.FromHours(distance_km / travel_avg_speed);
            textInfo.fill_all_info(this);
        }

        const int delta_inv = 10;

        /// <summary>
        /// Recalculates all visual information for the track, i.e. screen coordinates 
        /// of the track and waypoints
        /// </summary>
        public void updateVisual()
        {
            updateOnZoomChange(mapo.zoom, mapo.zoom);
        }

        public void updateOnZoomChange(int old_zoom, int new_zoom)
        {
            lock (this)
            {
                updateOnZoomChangeNoLock(old_zoom, new_zoom);
            }
        }

        /// <summary>
        /// Calls when our zoom level has been changed. Here we recalculate all our point coordinates in
        /// screen positions, later we use this coords for quick drawing of our track
        /// </summary>
        /// <param name="old_zoom"></param>
        /// <param name="new_zoom"></param>
        private void updateOnZoomChangeNoLock(int old_zoom, int new_zoom)
        {
            if(trackData.Count == 0)
                return;

            Point lastP;
            Point curP;

            drawPoints.Clear();

            mapo.getXYByLonLat(trackData.First.Value.lon, trackData.First.Value.lat, out lastP);
            drawPoints.Add(lastP);
            trackData.First.Value.draw_idx = 0;

            LinkedListNode<NMEA_LL> pointNode = trackData.First.Next;
            while(pointNode != null)
            {
                NMEA_LL point = pointNode.Value;
                mapo.getXYByLonLat(point.lon, point.lat, out curP);
                if ((Math.Abs(curP.X - lastP.X) < delta_inv && Math.Abs(curP.Y - lastP.Y) < delta_inv)
                    && pointNode.Next != null)
                {
                    point.draw_idx = -1;
                    pointNode = pointNode.Next;
                    continue;
                }
                point.draw_idx = drawPoints.Count;
                drawPoints.Add(curP);
                lastP = curP;
                pointNode = pointNode.Next;
            }

            way.updateXY(mapo.geosystem);
        }

        #endregion

        #region Manual track operations

        /// <summary>
        /// Deletes a list of points from the track.
        /// </summary>
        /// <param name="selected"></param>
        public void deleteSelectedPoints(List<LinkedListNode<NMEA_LL>> selected)
        {
            foreach (LinkedListNode<ncGeo.NMEA_LL> pt in selected)
            {
                pt.List.Remove(pt);
            }

            lastPos = (trackData.Last != null ? trackData.Last.Value : null);
            lastSpeedPos = lastPos;
            calculateParameters();
        }

        /// <summary>
        /// Updates position of the given point on the track to the given lon, lat coordinates.
        /// Also updates visible on-screen xy coordinates for point and for waypoint
        /// </summary>
        /// <param name="pointNode"></param>
        /// <param name="newlon"></param>
        /// <param name="newlat"></param>
        public void updatePointPosition(LinkedListNode<NMEA_LL> pointNode, double newlon, double newlat)
        {
            NMEA_LL point = pointNode.Value;
            point.lon = newlon;
            point.lat = newlat;

            if(point.draw_idx >= 0)
            {
                Point xy;
                mapo.getXYByLonLat(newlon, newlat, out xy);
                drawPoints[point.draw_idx] = xy;
            }
        }

        /// <summary>
        /// Marks last track point with the given type
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool markLastPoint(NMEA_LL.PointType pt)
        {
            if (lastTrackPos == null || lastTrackPos.ptype != NMEA_LL.PointType.TP)
                return false;
            way.markWay(lastTrackPos, distance_km, NMEA_LL.PointType.MWP);
            way.recalc_last_waypoint(mapo.geosystem);
            textInfo.fill_all_info(this);
            return true;
        }

        /// <summary>
        /// Slices track into segments and fills Way (route)
        /// </summary>
        public void sliceTrackIntoWay()
        {
            if (countPoints == 0)
                return;

            Way nway = new Way();

            double step = distance_km / 50;
            if (step < 1)
                step = 1;
            else
                if (step > 50)
                    step = 50;

            double dist = 0;
            double totaldist = 0;

            LinkedListNode<NMEA_LL> ppt = trackData.First;
            LinkedListNode<NMEA_LL> oldppt = trackData.First;

            nway.markWay(ppt.Value, dist, NMEA_LL.PointType.STARTP);
            nway.recalc_last_waypoint(mapo.geosystem);

            ppt = ppt.Next;
            while (ppt != null)
            {
                dist += ncGeo.CommonGeo.getDistanceByLonLat2(ppt.Value.lon, ppt.Value.lat,
                                       oldppt.Value.lon, oldppt.Value.lat);

                if (dist > step)
                {
                    totaldist += dist;
                    ppt.Value.ptype = NMEA_LL.PointType.AWP;
                    nway.markWay(ppt.Value, totaldist, NMEA_LL.PointType.AWP);
                    nway.recalc_last_waypoint(mapo.geosystem);
                    dist = 0;
                }
                oldppt = ppt;
                ppt = ppt.Next;
            }
            nway.markWay(trackData.Last.Value, totaldist + dist, NMEA_LL.PointType.ENDTP);
            nway.recalc_last_waypoint(mapo.geosystem);

            nway.initGLData();
            way = nway;
        }

        /// <summary>
        /// Updates last point to a new lon/lat coords
        /// </summary>
        /// <param name="newlon"></param>
        /// <param name="newlat"></param>
        /// <returns></returns>
        public bool updateManualPoint(double newlon, double newlat)
        {
            if (lastTrackPos == null)
                return false;
            lastTrackPos.lon = newlon;
            lastTrackPos.lat = newlat;
            way.recalc_last_waypoint(mapo.geosystem);
            updateOnZoomChangeNoLock(-1, -1);
            return true;
        }

        /// <summary>
        /// Deletes last point in the track (in manual edit mode)
        /// </summary>
        internal void delLastPoint()
        {
            if (trackData.Count == 0)
                return;
            NMEA_LL point = trackData.Last.Value;
            trackData.Remove(point);
            reducedRemoveLast();
            int count = trackData.Count;
            if (count == 0)
            {
                lastTrackPos = lastPos = lastSpeedPos = null;
                distance_km = 0.0;
                travel_avg_speed = Program.opt.manual_avg_speed;
                travel_time = TimeSpan.FromHours(distance_km / travel_avg_speed);
                way.clear();
            }
            else
            {
                lastTrackPos = lastPos = lastSpeedPos = trackData.Last.Value;
                lastPos.ptype = (count == 1 ? NMEA_LL.PointType.STARTP : NMEA_LL.PointType.ENDTP);

                Way.WayPoint wp = way.delLastFromWay(point);
                if (wp != null)
                {
                    distance_km -= wp.distance_from_prev;
                    travel_avg_speed = Program.opt.manual_avg_speed;
                    travel_time = TimeSpan.FromHours(distance_km / travel_avg_speed);
                }
            }
            updateOnZoomChangeNoLock(-1, -1);
            textInfo.fill_all_info(this);
        }

        /// <summary>
        /// Adds manual point in the end of the track
        /// </summary>
        /// <param name="nmea_ll"></param>
        public void addManualPoint(NMEA_LL nmea_ll)
        {
            if (trackData.Count > 0 && lastTrackPos != null)
            {
                distance_km += CommonGeo.getDistanceByLonLat2(lastTrackPos.lon,
                                                              lastTrackPos.lat,
                                                              nmea_ll.lon, nmea_ll.lat);
                travel_avg_speed = Program.opt.manual_avg_speed;
                travel_time = TimeSpan.FromHours(distance_km / travel_avg_speed);
                if (trackData.Last.Value.ptype == NMEA_LL.PointType.ENDTP)
                {
                    trackData.Last.Value.ptype = NMEA_LL.PointType.MWP;
                }
            }
            else
                nmea_ll.ptype = NMEA_LL.PointType.STARTP;
            lastTrackPos = lastPos = lastSpeedPos = nmea_ll;
            trackData.AddLast(lastTrackPos);
            reducedAddLast(trackData.Last);
            way.add(nmea_ll, distance_km);
            way.recalc_last_waypoint(mapo.geosystem);
            updateOnZoomChangeNoLock(-1, -1);
            textInfo.fill_all_info(this);
        }

        #endregion

        #region Save and Load tracks from different formats (GPX, KML, NMEA)

        /// <summary>
        /// Save the track and surrounding POI into a file with format defined in fi.fileType
        /// Different formats are supported: GPX, KML ...
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="poiFact"></param>
        /// <param name="groupFact"></param>
        /// <returns></returns>
        public GPS.TrackFileInfo save(GPS.TrackFileInfo fi, BookMarkFactory poiFact,
                                    Bookmarks.POIGroupFactory groupFact)
        {
            TrackLoader.ITrackLoader loader = TrackLoader.TrackLoaderFactory.singleton.getLoaderByName(fi.FileType)
                            as TrackLoader.ITrackLoader;

            if (loader == null)
                throw new ApplicationException("Could not find saving plugin for type: " + fi.FileType);
            loader = loader.Clone() as TrackLoader.ITrackLoader;
            loader.save(this, fi, poiFact, groupFact);
            return fi;
        }

        /// <summary>
        /// Check all out track points and return lat-lon bounding box
        /// </summary>
        /// <param name="ominlon"></param>
        /// <param name="ominlat"></param>
        /// <param name="omaxlon"></param>
        /// <param name="omaxlat"></param>
        public void getBounds(out double ominlon, out double ominlat, out double omaxlon, out double omaxlat)
        {
            double minlon = 180;
            double minlat = 180;
            double maxlon = -180;
            double maxlat = -180;
            foreach (NMEA_LL tp in trackData)
            {
                if (tp.lon < minlon)
                    minlon = tp.lon;
                if (tp.lon > maxlon)
                    maxlon = tp.lon;
                if (tp.lat < minlat)
                    minlat = tp.lat;
                if (tp.lat > maxlat)
                    maxlat = tp.lat;
            }

            ominlon = minlon;
            ominlat = minlat;
            omaxlon = maxlon;
            omaxlat = maxlat;
        }

        /// <summary>
        /// Loads track from file with GPX or GTR file format
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static GPSTrack loadFrom(GPS.TrackFileInfo fi, BookMarkFactory poiFact,
                                        Bookmarks.POIGroupFactory groupFact)
        {
            TrackLoader.ITrackLoader loader = TrackLoader.TrackLoaderFactory.singleton.getTrackLoader(fi);
            if(loader == null)
                throw new ApplicationException("Unknown file format! Could not load file: " 
                    + (fi.stype == TrackFileInfo.SourceType.FileName ? fi.fileOrBuffer : "Clipboard buffer"));
            return loader.load(fi, poiFact, groupFact);
        }

        /// <summary>
        /// Loads GPS track from file and slices it into many tracks by days.
        /// </summary>
        /// <param name="fname"></param>
        /// <returns>Return a list of tracks</returns>
        public static List<GPSTrack> loadTracks(GPS.TrackFileInfo fi)
        {
            List<GPSTrack> gtlist = new List<GPSTrack>();

            GPSTrack track = loadFrom(fi, BookMarkFactory.singleton, Bookmarks.POIGroupFactory.singleton());
            if (fi.needTrackSplitting == false 
                || track.startTime.DayOfYear == track.endTime.DayOfYear)
            {
                gtlist.Add(track);
                return gtlist;
            }
            //here we do the slicing

            int idx = 1;
            bool need_day_split = Program.opt.split_by_date;
            double distThreshold = Program.opt.split_by_distance;
            int lastday = track.startTime.DayOfYear;
            NMEA_LL lastll = track.trackData.First.Value;
            GPSTrack curtrack = new GPSTrack();
            curtrack.clone_header(track);
            curtrack.name += "-" + track.startTime.ToShortDateString() + "-" + idx.ToString(); ;
            curtrack.filename += "-" + idx.ToString();
            foreach (NMEA_LL ll in track.trackData)
            {
                NMEA_RMC rmc = ll as NMEA_RMC;
                double dist = CommonGeo.getDistanceByLonLat2(lastll.lon, lastll.lat, ll.lon, ll.lat);
                if ((need_day_split && rmc.utc_time.DayOfYear != lastday) ||
                    dist > distThreshold)
                {
                    curtrack.calculateParameters();
                    curtrack.lastSpeedPos = curtrack.lastPos;
                    gtlist.Add(curtrack);
                    lastday = rmc.utc_time.DayOfYear;
                    idx++;

                    curtrack = new GPSTrack();
                    curtrack.clone_header(track);
                    curtrack.name += "-" + rmc.utc_time.ToShortDateString() + "-" + idx.ToString();
                    curtrack.filename += "-" + idx.ToString();
                }
                curtrack.addGPSDataInternal(rmc);
                lastll = rmc;
            }
            curtrack.calculateParameters();
            curtrack.lastSpeedPos = curtrack.lastPos;
            gtlist.Add(curtrack);
            return gtlist;
        }

        public void clone_header(GPSTrack from)
        {
            name = from.name;
            way.name = from.way.name;
            filename = from.filename;
        }

        #endregion

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
            if (!shown)
                return;

            NMEA_LL startLL = null;
            Point startP = new Point();


            if (mode != TrackMode.PositionOnly && trackData.Count > 1)
            {
                NMEA_LL lastLL = trackData.First.Value;
                Point lastP;
                startLL = lastLL;
                mapo.getVisibleXYByLonLat(lastLL.lon, lastLL.lat, out lastP);
                startP = lastP;

                foreach (NMEA_LL point in trackData)
                {
                    Point curP;
                    if (!mapo.isVisibleOnMap(point.lon, point.lat) ||
                        mapo.isSameVisiblePoint(lastLL.lon, lastLL.lat, point.lon, point.lat))
                        continue;

                    mapo.getVisibleXYByLonLat(point.lon, point.lat, out curP);
                    gr.DrawLine(pen, lastP, curP);
                    lastP = curP;
                    lastLL = point;
                }
            }

            if (lastPos != null)
            {
                Point lastP;
                mapo.getVisibleXYByLonLat(lastPos.lon, lastPos.lat, out lastP);

                if (need_arrows)
                {
                    Point arrowP = lastP;
                    arrowP.X += lastPos.dir_xy.X;
                    arrowP.Y += lastPos.dir_xy.Y;
                    Point rightP = lastP;
                    rightP.X += lastPos.dir_right_xy.X;
                    rightP.Y += lastPos.dir_right_xy.Y;

                    pen_arrow.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    pen_right.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    gr.DrawLine(pen_right, lastP, rightP);
                    gr.DrawLine(pen_arrow, lastP, arrowP);
                }

                if (startLL != null)
                {
                    start_cursor.draw(gr, startP);
                }

                if (mode == TrackMode.ViewSaved)
                    end_cursor.draw(gr, lastP);
                else
                    cursor.draw(gr, lastP);
            }
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        [ XmlIgnore ]
        public int dLevel
        {
            get
            {
                return drawLevel;
            }
            set
            {
                drawLevel = value;
            }
        }

        public void show()
        {
            shown = true;
        }

        public void hide()
        {
            shown = false;
        }


        protected object cursor_tex;
        protected object end_cursor_tex;
        protected object start_cursor_tex;
        protected object arrows_tex;

        public void initGLData()
        {
            if (cursor_tex != null)
                return;
            way.initGLData();
            try
            {
                arrows = TextureFactory.singleton.getImg(TextureFactory.TexAlias.Arrows);
                cursor_tex = TextureFactory.singleton.getTex(cursor);
                end_cursor_tex = TextureFactory.singleton.getTex(end_cursor);
                start_cursor_tex = TextureFactory.singleton.getTex(start_cursor);
                arrows_tex = TextureFactory.singleton.getTex(arrows); //GL_LINEAR
            }
            catch
            {
            }
        }

        public void glDraw(int centerx, int centery)
        {
            if (!shown)
                return;

            NMEA_LL startLL = null;
            Point startP = new Point();

            //Gl.glPushAttrib(Gl.GL_TEXTURE_BIT);

            lock (this)
            {
                if (mode != TrackMode.PositionOnly && trackData.Count > 1)
                {
                    startP = mapo.start_real_xy;
                    // realX - startP.X - centerx = realX - (startP.X + centerx)
                    // centery - (realY - startp.Y) = centery - realY + startp.Y = -realY + (startp.Y + centerY)
                    startP.X += centerx;
                    startP.Y += centery;

                    GML.device.color(pen.Color);
                    GML.device.lineWidth(pen.Width);
                    GML.device.lineStipple(-1);
                    GML.device.lineDraw(drawPoints, startP, drawLevel + 2);
                    GML.device.color(Color.White);

                    startLL = trackData.First.Value;
                    mapo.getVisibleXYByLonLat(startLL.lon, startLL.lat, out startP);
                }

                if (lastPos != null)
                {
                    Point lastP;
                    mapo.getVisibleXYByLonLat(lastPos.lon, lastPos.lat, out lastP);
                    double angle = Program.opt.angle;

                    if (need_arrows)
                    {
                        GML.device.pushMatrix();
                        GML.device.translate(lastP.X - centerx, centery - lastP.Y, 0);
                        GML.device.rotateZ(-lastSpeedPos.dir_angle);
                        GML.device.texDrawBegin();
                        GML.device.texFilter(arrows_tex, TexFilter.Smooth);
                        GML.device.texDraw(arrows_tex, -arrows.delta_x, arrows.delta_y, drawLevel + 2, arrows.img.Width, arrows.img.Height);
                        GML.device.texDrawEnd();
                        GML.device.popMatrix();
                    }

                    /*
                    if (startLL != null)
                    {
                        GML.device.pushMatrix();
                        GML.device.translate(startP.X - centerx, centery - startP.Y, 0);
                        GML.device.rotateZ(-angle);
                        GML.device.texDrawBegin();
                        GML.device.texDraw(start_cursor_tex, -start_cursor.delta_x,
                            start_cursor.delta_y, drawLevel + 2, start_cursor.img.Width, start_cursor.img.Height);
                        GML.device.texDrawEnd();
                        GML.device.popMatrix();
                    }
                    */

                    if (mode != TrackMode.ViewSaved)
                    {
                        GML.device.pushMatrix();
                        GML.device.translate(lastP.X - centerx, centery - lastP.Y, 0);
                        GML.device.rotateZ(-angle);
                        GML.device.texDrawBegin();
                        GML.device.texFilter(cursor_tex, TexFilter.Pixel);
                        GML.device.texDraw(cursor_tex, -cursor.delta_x,
                            cursor.delta_y, drawLevel + 2, cursor.img.Width, cursor.img.Height);
                        GML.device.texDrawEnd();
                        GML.device.popMatrix();
                    }
                    way.glDraw(mapo, centerx, centery);
                }
            }
            //Gl.glPopAttrib();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (mapo != null)
                mapo.onZoomChanged -= updateOnZoomChange;

        }

        #endregion


        #region IGPSTrack Members

        [XmlIgnore]
        public WayBase wayObject
        {
            get { return way; }
        }

        [XmlIgnore]
        public BaseGeo geosystem
        {
            get
            {
                return mapo.geosystem;
            }
        }
        
        public double lwp_avg_speed
        {
            get
            {
                LinkedListNode<WayBase.WayPoint> point = way.wayPoints.Last;
                if (point == null)
                    return 0;
                TimeSpan dtime = lastPos.utc_time - point.Value.point.utc_time;
                if(dtime.TotalHours > 0)
                    return (distance_km - point.Value.distance_from_start)/ dtime.TotalHours;
                return 0;
            }
        }


        public double lwp_distance
        {
            get 
            {
                LinkedListNode<WayBase.WayPoint> point = way.wayPoints.Last;
                if (point == null)
                    return 0;
                return distance_km - point.Value.distance_from_start;
            }
        }
        #endregion

    }
}
