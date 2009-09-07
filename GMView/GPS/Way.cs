using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;

namespace GMView
{
    public class Way
    {
        LinkedList<WayPoint> points = new LinkedList<WayPoint>();
        double total_distance = 0.0;
        TimeSpan total_time;
        int total_points = 0;
        ImageDot[] imds = new ImageDot[(int)NMEA_LL.PointType.MaxPT];
        object[] texs = new object[(int)NMEA_LL.PointType.MaxPT];
        WayPoint currentWayPoint = null;
        string way_name = "Route: manual";

        public class WayPoint
        {
            public int num = 1;
            public int x, y;
            public NMEA_LL point;
            public NMEA_LL.PointType ptype;
            public double distance_from_prev;
            public TimeSpan time_from_prev;
            public double distance_to_next;
            public TimeSpan time_to_next;
            public double distance_from_start;
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

        public Way()
        {
            imds[(int)NMEA_LL.PointType.STARTP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.StartPos);
            imds[(int)NMEA_LL.PointType.SWP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.WayDot);
            imds[(int)NMEA_LL.PointType.MWP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.WayDotSmall);
            imds[(int)NMEA_LL.PointType.ENDTP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.FinishPos);
        }

        public void initGLData()
        {
            for(int i = 0; i<(int)NMEA_LL.PointType.MaxPT;i++)
                if(imds[i]!=null)
                    texs[i] = TextureFactory.singleton.getTex(imds[i]);
        }

        public void recalc_last_waypoint(MapObject mapo)
        {
            WayPoint lastwp = points.Last.Value;
            Point xy;
            mapo.getXYByLonLat(lastwp.point.lon, lastwp.point.lat, out xy);
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

        public string name
        {
            get { return way_name; }
            set { way_name = value; }
        }

        public void add(NMEA_LL point, double distance)
        {
            WayPoint wp = new WayPoint();
            wp.point = point;
            wp.distance_from_prev = distance;
            add(wp, point.ptype);
        }

        public void add(WayPoint wp, NMEA_LL.PointType pt)
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

        public WayPoint delLastFromWay(NMEA_LL point)
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

        public void clear()
        {
            points.Clear();
            total_distance = 0.0;
            total_points = 0;
        }

        public void updateXY(MapObject mapo)
        {
            Point xy;
            foreach (WayPoint wplnk in points)
            {
                mapo.getXYByLonLat(wplnk.point.lon, wplnk.point.lat, out xy);
                wplnk.x = xy.X;
                wplnk.y = xy.Y;
            }
        }

        public void glDraw(MapObject mapo, int centerx, int centery)
        {
            Point startP = mapo.start_real_xy;
            int wx, wy;
            ImageDot imd;
            double angle = Program.opt.angle;

            foreach (WayPoint wp in points)
            {
                imd = imds[(int)wp.ptype];

                wx = wp.x - startP.X;
                wy = wp.y - startP.Y;

                GML.device.pushMatrix();
                GML.device.translate( wx - centerx, centery - wy, 0);
                GML.device.rotateZ(-angle);
                GML.device.texDrawBegin();
                GML.device.texFilter(texs[(int)wp.ptype], TexFilter.Pixel);
                GML.device.texDraw(texs[(int)wp.ptype], -imd.delta_x,
                    imd.delta_y, 2, imd.img.Width, imd.img.Height);
                GML.device.texDrawEnd();
                GML.device.popMatrix();
            }
        }


        internal void mark_way(NMEA_LL last_ll, double distance, NMEA_LL.PointType pt)
        {
            if (points.Last == null || points.Last.Value.point != last_ll)
            {
                add(last_ll, distance);
            }

            points.Last.Value.ptype = pt;
            points.Last.Value.point.ptype = pt;
        }

        internal WayPoint currentWP
        {
            get { return currentWayPoint; }
        }

        /// <summary>
        /// Go to next waypoint in our way and make it current
        /// </summary>
        /// <returns></returns>
        internal WayPoint stepNextPoint()
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
        internal WayPoint stepPrevPoint()
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

        internal void saveGPX(XmlTextWriter writer, System.Globalization.NumberFormatInfo nf)
        {
            if (total_points <= 2)
                return;

            writer.WriteStartElement("rte");
            writer.WriteElementString("name", name);
            foreach (WayPoint wp in points)
            {
                writer.WriteStartElement("rtept");
                writer.WriteAttributeString("lon", wp.point.lon.ToString("F6", nf));
                writer.WriteAttributeString("lat", wp.point.lat.ToString("F6", nf));
                writer.WriteElementString("time", wp.point.utc_time.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                writer.WriteElementString("type", wp.ptype.ToString());
                writer.WriteElementString("ele", wp.point.height.ToString("F3", nf));
                writer.WriteElementString("hdop", wp.point.HDOP.ToString("F1", nf));
                writer.WriteElementString("sat", wp.point.usedSats.ToString());

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        internal void saveKML(XmlTextWriter writer, System.Globalization.NumberFormatInfo nf)
        {
            if (total_points <= 2)
                return;

            writer.WriteStartElement("Style");
            writer.WriteAttributeString("id", "routeStyle");
            writer.WriteStartElement("LineStyle");
            writer.WriteElementString("color", "CCFF7711");
            writer.WriteElementString("width", "3");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("Folder");
            writer.WriteElementString("name", "Routes");

            writer.WriteStartElement("Placemark");
            writer.WriteElementString("name", "Route");
            writer.WriteElementString("styleUrl", "#routeStyle");
            {//LineString - our track
                writer.WriteStartElement("MultiGeometry");
                writer.WriteStartElement("LineString");
                writer.WriteElementString("tessellate", "1");
                { //cordinates of our track
                    writer.WriteStartElement("coordinates");

                    foreach (WayPoint wp in points)
                    {
                        string slon = wp.point.lon.ToString("F8", nf);
                        string slat = wp.point.lat.ToString("F8", nf);
                        string shei = wp.point.height.ToString("F2", nf);
                        writer.WriteString(slon + "," + slat + "," + shei + " ");
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); //lineString
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); //placemark
            writer.WriteEndElement();
        }
    }
}
