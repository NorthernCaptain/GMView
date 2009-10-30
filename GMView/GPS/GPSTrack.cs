using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ncGeo;

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
        protected string filename = "track1.gtr";

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
        protected NMEA_LL lastTrackPos;
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
        public Way way = new Way();

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
        }

        public NMEA_LL lastNonZeroPos
        {
            get { return lastSpeedPos; }
        }

        public override string ToString()
        {
            return name;
        }

        [XmlIgnore]
        public string track_name
        {
            get { return name; }
            set { name = "Track: " + value; textInfo.fill_all_info(this); }
        }


        private void reducedReset()
        {
            reducedTrackData.Clear();
            reduced_running_count = reduced_max_step;
        }

        private void reducedAdd(LinkedListNode<NMEA_LL> point)
        {
            if (reduced_running_count >= reduced_max_step)
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

        /// <summary>
        /// Saves track data into xml file using xml serialization
        /// </summary>
        /// <param name="fname"></param>
        public void saveXml(string fname)
        {
            //generate_test_track();

            TextWriter writer = new StreamWriter(fname);
            XmlSerializer xser = new XmlSerializer(typeof(GPSTrack));

            xser.Serialize(writer, this);
            writer.Close();
            filename = fname;
        }

        /// <summary>
        /// Save our track data into GPX unified XML format
        /// </summary>
        /// <param name="fname"></param>
        public void saveGPX(string fname)
        {
            double minlon, minlat, maxlon, maxlat;
            XmlTextWriter writer = null;
            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("");
            System.Globalization.NumberFormatInfo nf = cul.NumberFormat;

            writer = new XmlTextWriter(fname, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("gpx");
            writer.WriteAttributeString("version", "1.1");
            writer.WriteAttributeString("creator", who);
            writer.WriteAttributeString("xmlns:xsi", @"http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", @"http://www.topografix.com/GPX/1/1");
            writer.WriteAttributeString("xsi:schemaLocation", @"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");

            // metadata section
            {
                writer.WriteStartElement("metadata");
                writer.WriteElementString("name", Path.GetFileNameWithoutExtension(fname));
                {
                    writer.WriteStartElement("author");
                    writer.WriteElementString("name", Program.opt.author);
                    {
                        writer.WriteStartElement("email");
                        string email = Program.opt.email;
                        writer.WriteAttributeString("id", email.Substring(0, email.IndexOf('@')));
                        writer.WriteAttributeString("domain", email.Substring(email.IndexOf('@') + 1));
                        writer.WriteEndElement(); // email
                    }
                    writer.WriteEndElement(); //author
                }

                writer.WriteElementString("desc", "GPX file was generated by " + who + " at " + Environment.MachineName);
                writer.WriteElementString("time", startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                writer.WriteElementString("copyright", "GNU FPL");
                {
                    writer.WriteStartElement("bounds");
                    this.getBounds(out minlon, out minlat, out maxlon, out maxlat);
                    writer.WriteAttributeString("minlat", minlat.ToString("F8", nf));
                    writer.WriteAttributeString("minlon", minlon.ToString("F8", nf));
                    writer.WriteAttributeString("maxlat", maxlat.ToString("F8", nf));
                    writer.WriteAttributeString("maxlon", maxlon.ToString("F8", nf));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            //track section
            writer.WriteStartElement("trk");
            writer.WriteElementString("name", name);
            writer.WriteStartElement("extensions");
            writer.WriteElementString("travel_time", travel_time_string);
            writer.WriteElementString("travel_distance", distance_km.ToString("F2", nf));
            writer.WriteEndElement();
            writer.WriteStartElement("trkseg");

            foreach (NMEA_LL tp in trackData)
            {
                writer.WriteStartElement("trkpt");
                writer.WriteAttributeString("lon", tp.lon.ToString("F8", nf));
                writer.WriteAttributeString("lat", tp.lat.ToString("F8", nf));
                writer.WriteElementString("time", tp.utc_time.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                writer.WriteElementString("type", tp.ptype.ToString());
                writer.WriteElementString("ele", tp.height.ToString("F3", nf));
                writer.WriteElementString("hdop", tp.HDOP.ToString("F1", nf));
                writer.WriteElementString("sat", tp.usedSats.ToString());
                if (tp.speed != 0 || tp.dir_angle != 0)
                {
                    writer.WriteStartElement("extensions");
                    writer.WriteElementString("vel", tp.speed.ToString("F2", nf));
                    writer.WriteElementString("dir", tp.dir_angle.ToString("F1", nf));
                    writer.WriteEndElement(); //extensions
                }
                writer.WriteEndElement(); //trkpt

                //if we have stay waypoint, then start new track segment
                if (tp.ptype == NMEA_LL.PointType.SWP)
                {
                    writer.WriteEndElement(); //trkseg
                    writer.WriteStartElement("trkseg");
                }

            }

            writer.WriteEndElement(); //trkseg

            writer.WriteEndElement(); //trk

            way.saveGPX(writer, nf);

            { // save bookmarks if we have them in our region
                double delta_lon = (maxlon - minlon) / 5;
                double delta_lat = (maxlat - minlat) / 5;

                List<Bookmark> booklist = BookMarkFactory.singleton.getBookmarksByBounds(
                    minlon - delta_lon, minlat - delta_lat, maxlon + delta_lon, maxlat + delta_lat);
                if (booklist.Count > 0)
                {
                    foreach (Bookmark book in booklist)
                    {
                        writer.WriteStartElement("wpt");
                        writer.WriteAttributeString("lon", book.lon.ToString("F8", nf));
                        writer.WriteAttributeString("lat", book.lat.ToString("F8", nf));
                        writer.WriteElementString("name", book.name);
                        writer.WriteElementString("desc", book.comment);
                        writer.WriteElementString("time", book.created.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                        writer.WriteEndElement();
                    }
                }
            }

            // end of gpx root tag
            writer.WriteEndElement(); //gpx
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Saves our track data, waypoint and places of interest into Google Earth KML file.
        /// </summary>
        /// <param name="fname"></param>
        public void saveKML(string fname)
        {
            double minlon, minlat, maxlon, maxlat;
            XmlTextWriter writer = null;
            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("");
            System.Globalization.NumberFormatInfo nf = cul.NumberFormat;

            writer = new XmlTextWriter(fname, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("kml");
            writer.WriteAttributeString("xmlns", @"http://earth.google.com/kml/2.0");

            { //document
                writer.WriteStartElement("Document");
                writer.WriteElementString("name", Path.GetFileNameWithoutExtension(fname));

                writer.WriteStartElement("Style");
                writer.WriteAttributeString("id", "trackStyle");
                writer.WriteStartElement("LineStyle");
                writer.WriteElementString("color", "F03399FF");
                writer.WriteElementString("width", "4");
                writer.WriteEndElement();
                writer.WriteEndElement();

                { //placemark - our track header
                    writer.WriteStartElement("Folder");
                    writer.WriteElementString("name", "Tracks");
                    writer.WriteStartElement("Placemark");
                    writer.WriteElementString("name", name);
                    writer.WriteElementString("styleUrl", "#trackStyle");
                    {//LineString - our track
                        writer.WriteStartElement("MultiGeometry");
                        writer.WriteStartElement("LineString");
                        writer.WriteElementString("tessellate", "1");
                        { //cordinates of our track
                            writer.WriteStartElement("coordinates");

                            foreach (NMEA_LL tp in trackData)
                            {
                                string slon = tp.lon.ToString("F8", nf);
                                string slat = tp.lat.ToString("F8", nf);
                                string shei = tp.height.ToString("F2", nf);
                                writer.WriteString(slon + "," + slat + "," + shei + " ");
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement(); //lineString
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement(); //placemark
                    writer.WriteEndElement(); //folder
                }

                way.saveKML(writer, nf);

                getBounds(out minlon, out minlat, out maxlon, out maxlat);

                { // save bookmarks if we have them in our region
                    double delta_lon = (maxlon - minlon) / 5;
                    double delta_lat = (maxlat - minlat) / 5;

                    List<Bookmark> booklist = BookMarkFactory.singleton.getBookmarksByBounds(
                        minlon - delta_lon, minlat - delta_lat, maxlon + delta_lon, maxlat + delta_lat);
                    if (booklist.Count > 0)
                    {
                        writer.WriteStartElement("Folder");
                        writer.WriteElementString("name", "Places");


                        foreach (Bookmark book in booklist)
                        {
                            writer.WriteStartElement("Placemark");
                            writer.WriteElementString("name", book.name);
                            writer.WriteElementString("description", book.comment);
                            writer.WriteStartElement("Point");
                            writer.WriteElementString("coordinates", book.lon.ToString("F8", nf)+","+book.lat.ToString("F8", nf)+",0.0");
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                }


                writer.WriteStartElement("LookAt");

                writer.WriteElementString("latitude", ((minlat + maxlat)/2.0).ToString("F8", nf));
                writer.WriteElementString("longitude", ((minlon + maxlon) / 2.0).ToString("F8", nf));
                writer.WriteElementString("altitude", "0");
                writer.WriteElementString("range", "10000");
                writer.WriteElementString("tilt", "0");
                writer.WriteElementString("heading", "0");
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            // end of kml root tag
            writer.WriteEndElement(); //kml
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
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
        public static GPSTrack loadFromFile(string fname)
        {
            System.IO.StreamReader reader;
            reader = new System.IO.StreamReader(fname);
            reader.ReadLine(); //skip ?xml? definition
            string first_line = reader.ReadLine();
            reader.Close();
            if (first_line.Contains("gpx"))
                return loadGPX(fname);
            if (first_line.Contains("kml"))
                return loadKML(fname);
            if (first_line.Contains("$GP") || first_line.Contains("$PS"))
                return loadNMEA(fname);
            if (first_line.Contains("gps_track"))
                return loadXml(fname);
            throw new ApplicationException("Unknown file format! Could not load file: " + fname);
        }

        /// <summary>
        /// Loads GPS track from file and slices it into many tracks by days.
        /// </summary>
        /// <param name="fname"></param>
        /// <returns>Return a list of tracks</returns>
        public static List<GPSTrack> loadTracks(string fname)
        {
            List<GPSTrack> gtlist = new List<GPSTrack>();

            GPSTrack track = loadFromFile(fname);
            if (track.startTime.DayOfYear == track.endTime.DayOfYear)
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
            curtrack.name += "-" + track.startTime.ToShortDateString() + "-" + idx.ToString();;
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

        /// <summary>
        /// Loads GPS track from NMEA log file, uses only RMC sentences from it.
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static GPSTrack loadNMEA(string fname)
        {
            System.IO.StreamReader reader = null;
            reader = new System.IO.StreamReader(fname);

            GPSTrack track = new GPSTrack();

            string dirname = Path.GetDirectoryName(fname);
            track.name = "Track: " + dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar) + 1) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname);
            track.way.name = "Route: " + Path.GetFileNameWithoutExtension(fname);

            string buf;

            try
            {
                while ((buf = reader.ReadLine()) != null)
                {
                    if (buf.Substring(0, 3) == "$GP" || buf.Substring(0, 6) == "$GPGSA")
                    {
                        NMEACommand cmd = NMEAThread.parse_command(new NMEAString(buf));
                        if (cmd.state == NMEACommand.Status.DataOK && cmd is NMEA_RMC)
                        {
                            NMEA_RMC rmc = cmd as NMEA_RMC;
                            track.addGPSDataInternal(rmc);
                        }
                    }
                }
            }
            finally
            {
                reader.Close();
            }

            if (track.trackData.Count == 0)
                throw new ApplicationException("This file does not have any tracks or routes! Check file content");

            track.filename = fname;
            track.calculateParameters();
            track.lastSpeedPos = track.lastPos;

            return track;

        }

        /// <summary>
        /// Loads GPSTrack from Google Earth KML file. Throws an ApplicationException if something goes wrong
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static GPSTrack loadKML(string fname)
        {
            XmlDocument doc = new XmlDocument();

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
            nsm.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

            doc.Load(fname);

            if (doc.DocumentElement.Name != "kml")
                throw new ApplicationException("Not a valid KML file! Could not find kml root tag.");

            { //retrieve xmlns
                XmlNode xnsnode = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                if (xnsnode != null)
                    nsm.AddNamespace("kml", xnsnode.Value);
                else
                {
                    nsm.AddNamespace("kml", "");
                }
            }


            GPSTrack track = new GPSTrack();

            string dirname = Path.GetDirectoryName(fname);
            track.name = "Track: " + dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar) + 1) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname);
            track.way.name = "Route: " + Path.GetFileNameWithoutExtension(fname);

            XmlNode folder = selectKMLFolders(doc, nsm, track);
            XmlNode titleNode = null;

            if(folder == null)
            {
                throw new ApplicationException("This file does not have any tracks or routes! Check file content");
            }

            XmlNodeList nlist = folder.SelectNodes("./kml:Placemark", nsm);
            string nodeval="";
            foreach (XmlNode xnode in nlist)
            {
                if (track.xmltag(xnode, "./kml:LineString/kml:coordinates", nsm, ref nodeval) ||
                    track.xmltag(xnode, "./*/kml:LineString/kml:coordinates", nsm, ref nodeval))
                {
                    track.loadKMLPoints(nodeval);
                    if (titleNode == null)
                        titleNode = xnode.SelectSingleNode("./kml:name", nsm);
                }

            }

            if (track.trackData.Count == 0)
                throw new ApplicationException("This file does not have any tracks or routes! Check file content");

            if (titleNode == null)
                titleNode = folder.SelectSingleNode("./kml:name", nsm);

            if (titleNode != null)
            {
                nodeval = titleNode.InnerText;
                if (nodeval.Length > 7 && nodeval.Substring(0, 7) == "Track: ")
                    nodeval = nodeval.Substring(7);
                track.name = "Track: " + nodeval;
                track.way.name = "Route: " + nodeval;
            }

            BookMarkFactory.singleton.loadTemporaryBookmarks(track.track_name, nlist, nsm);

            track.filename = fname;
            track.calculateParameters();
            track.lastSpeedPos = track.lastPos;

            return track;
        }

        /// <summary>
        /// Searches for Folder tags and return first folder that contains coordinates
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="nsm"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        private static XmlNode selectKMLFolders(XmlDocument doc, XmlNamespaceManager nsm, GPSTrack track)
        {
            string nodeval="";
            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//kml:Folder", nsm);
            foreach (XmlNode xnode in nlist)
            {
                if (track.xmltag(xnode, "./kml:Placemark/kml:LineString/kml:coordinates", nsm, ref nodeval)
                    || track.xmltag(xnode, "./kml:Placemark/*/kml:LineString/kml:coordinates", nsm, ref nodeval))
                    return xnode;
            }
            return null;
        }

        private void loadKMLPoints(string nodeval)
        {
            double lon, lat, hei;
            char[] sep = new char[] { ' ', '\n', '\r', '\t' };
            string[] tuples = nodeval.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            DateTime now = DateTime.Now.ToUniversalTime();
            foreach (string tuple in tuples)
            {
                if (splitKMLCoordTuple(tuple, out lon, out lat, out hei))
                {
                    NMEA_RMC rmc = new NMEA_RMC();
                    rmc.lat = lat;
                    rmc.lon = lon;
                    rmc.height = hei;
                    rmc.state = NMEACommand.Status.DataOK;
                    rmc.utc_time = now;
                    rmc.ptype = NMEA_LL.PointType.TP;

                    trackData.AddLast(rmc);
                    lastPos = lastTrackPos = lastSpeedPos = rmc;
                }
            }
        }

        public static bool splitKMLCoordTuple(string tuple, out double lon, out double lat, out double hei)
        {
            char[] sep = new char[] { ',' };

            string[] elements = tuple.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            lon = lat = hei = 0.0;

            if (elements.Length < 2)
                return false;
            lon = NMEACommand.getDouble(elements[0]);
            lat = NMEACommand.getDouble(elements[1]);
            if (elements.Length > 2)
                hei = NMEACommand.getDouble(elements[2]);
            return true;
        }

        /// <summary>
        /// Loads track or route from GPX file. Throws an ApplicationException if something goes wrong
        /// </summary>
        /// <param name="fname"></param>
        /// <returns>GPSTrack loaded into memory</returns>
        public static GPSTrack loadGPX(string fname)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(fname);

                if (doc.DocumentElement.Name != "gpx")
                    throw new ApplicationException("Not a valid GPX file! Could not find gpx root tag.");

                XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
                
                { //retrieve xmlns
                    XmlNode xnsnode = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                    if (xnsnode != null)
                        nsm.AddNamespace("gpx", xnsnode.Value);
                    else
                    {
                        nsm.AddNamespace("gpx", "");
                    }
                }


                GPSTrack track = new GPSTrack();

                XmlNode node = doc.DocumentElement.SelectSingleNode("/gpx:gpx/gpx:metadata/gpx:name", nsm);
                if (node != null)
                {
                    string nodeval = node.InnerText;
                    if (nodeval.Length>7 && nodeval.Substring(0, 7) == "Track: ")
                        nodeval = nodeval.Substring(7);

                    track.name = "Track: " + nodeval;
                    track.way.name = "Route: " + nodeval;
                }
                else
                {
                    string dirname = Path.GetDirectoryName(fname);
                    track.name = "Track: " + dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar) + 1) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname);
                    track.way.name = "Route: " + Path.GetFileNameWithoutExtension(fname); 
                }

                XmlNodeList nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt", nsm);
                foreach(XmlNode xnode in nlist)
                {
                    track.loadPoint(xnode, nsm);
                }

                if (nlist.Count == 0) //we don't have track in file, lets try route
                {
                    nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:rte/gpx:rtept", nsm);
                    foreach(XmlNode xnode in nlist)
                    {
                        track.loadPoint(xnode, nsm);
                    }
                }

                if (track.trackData.Count == 0)
                    throw new ApplicationException("This file does not have any tracks or routes! Check file content");

                nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:wpt", nsm);

                if (nlist.Count > 0)
                {
                    BookMarkFactory.singleton.loadTemporaryBookmarks(track.track_name, nlist, nsm);
                }

                track.filename = fname;
                track.calculateParameters();
                track.lastSpeedPos = track.lastPos;

                return track;
            }
            finally
            {
                doc = null;
            }
        }

        /// <summary>
        /// Loads one track point from GPX xml document
        /// </summary>
        /// <param name="xnode"></param>
        /// <param name="nsm"></param>
        private void loadPoint(XmlNode xnode, XmlNamespaceManager nsm)
        {
            NMEA_RMC rmc = new NMEA_RMC();
            rmc.lat = NMEACommand.getDouble(xnode.Attributes.GetNamedItem("lat").Value);
            rmc.lon = NMEACommand.getDouble(xnode.Attributes.GetNamedItem("lon").Value);

            string sval = "";
            if (xmltag(xnode, "./gpx:time", nsm, ref sval))
            {
                try
                {
                    rmc.utc_time = DateTime.Parse(sval).ToUniversalTime();
                }
                catch
                {
                    rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();
                }

            } else
                rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();

            if(xmltag(xnode, "./gpx:ele", nsm, ref sval))
                rmc.height = NMEACommand.getDouble(sval);
            if (xmltag(xnode, "./gpx:sat", nsm, ref sval))
                rmc.usedSats = int.Parse(sval);
            if (xmltag(xnode, "./gpx:hdop", nsm, ref sval))
                rmc.HDOP = NMEACommand.getDouble(sval);
            if (xmltag(xnode, "./*/gpx:vel", nsm, ref sval))
                rmc.speed = NMEACommand.getDouble(sval);
            if (xmltag(xnode, "./*/gpx:dir", nsm, ref sval))
                rmc.dir_angle = NMEACommand.getDouble(sval);
            if (xmltag(xnode, "./gpx:type", nsm, ref sval))
                rmc.ptype = NMEA_LL.parsePointType(sval);

            lastPos = lastTrackPos = lastSpeedPos = rmc;
            trackData.AddLast(lastPos);
        }

        /// <summary>
        /// Helper for retreiving xml data
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <param name="nsm"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool xmltag(XmlNode node, string xpath, XmlNamespaceManager nsm, ref string val)
        {
            XmlNode result = node.SelectSingleNode(xpath, nsm);
            if (result != null)
            {
                val = result.InnerText;
                return true;
            }
            val = "";
            return false;
        }

        /// <summary>
        /// Loads GPSTrack from xml file written by saveXML (using serialization)
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static GPSTrack loadXml(string fname)
        {
            FileStream fs;
            try
            {
                fs = new FileStream(fname, FileMode.Open);
            }
            catch
            {
                return null;
            }
            try
            {
                XmlSerializer xser = new XmlSerializer(typeof(GPSTrack));
                GPSTrack track = (GPSTrack)xser.Deserialize(fs);
                string dirname = Path.GetDirectoryName(fname);
                track.name = "Track: " + dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar)+1) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fname);
                track.way.name = "Route: " + Path.GetFileNameWithoutExtension(fname); 
                track.filename = fname;
                track.calculateParameters();
                track.lastSpeedPos = track.lastPos;
                return track;
            }
            catch
            {
                return null;
            }
            finally
            {
                fs.Close();
            }
        }
        #endregion

        const double stopMinutes = 2.0; //Number of minutes between two points that we consider as stop point
        /// <summary>
        /// Goes through all track points and calculate parameters of the track
        /// </summary>
        public void calculateParameters()
        {
            if (trackData.Count == 0)
                return;

            way.clear();

            trav_time = endTime - startTime;
            if (trav_time.TotalHours < 0.0)
                trav_time = startTime - endTime;

            distance = 0.0;
            travel_max_speed = 0.0;
            NMEA_LL first_nm = trackData.First.Value;
            first_nm.ptype = NMEA_LL.PointType.STARTP;
            way.add(first_nm, 0.0);
            reducedReset();
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

        #region Manual track operations
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
            if (trackData.Count == 0)
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
        /// Adds monual point in the end of the track
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
            }
            else
                nmea_ll.ptype = NMEA_LL.PointType.STARTP;
            lastTrackPos = lastPos = lastSpeedPos = nmea_ll;
            trackData.AddLast(lastTrackPos);
            way.add(nmea_ll, distance_km);
            way.recalc_last_waypoint(mapo.geosystem);
            updateOnZoomChangeNoLock(-1, -1);
            textInfo.fill_all_info(this);
        }

        #endregion

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
                    saveGPX(Program.opt.autosavefile);
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

            foreach (NMEA_LL point in trackData)
            {
                mapo.getXYByLonLat(point.lon, point.lat, out curP);
                if ((Math.Abs(curP.X - lastP.X) < delta_inv  && Math.Abs(curP.Y - lastP.Y) < delta_inv))
                    continue;

                drawPoints.Add(curP);
                lastP = curP;
            }
            mapo.getXYByLonLat(trackData.Last.Value.lon, trackData.Last.Value.lat, out lastP);
            drawPoints.Add(lastP);

            way.updateXY(mapo.geosystem);
        }

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
        #endregion
    }
}
