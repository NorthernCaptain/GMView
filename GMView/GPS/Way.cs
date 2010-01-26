using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using ncGeo;

namespace GMView.GPS
{
    /// <summary>
    /// Real way (route) class with waypoints
    /// </summary>
    public class Way: ncGeo.WayBase
    {
        ImageDot[] imds = new ImageDot[(int)NMEA_LL.PointType.MaxPT];
        object[] texs = new object[(int)NMEA_LL.PointType.MaxPT];

        public Way(): base()
        {
            imds[(int)NMEA_LL.PointType.STARTP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.StartPos);
            imds[(int)NMEA_LL.PointType.SWP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.WayDot);
            imds[(int)NMEA_LL.PointType.MWP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.WayDotSmall);
            imds[(int)NMEA_LL.PointType.AWP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.SatOK);
            imds[(int)NMEA_LL.PointType.MARKWP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.WayDotGreen);
            imds[(int)NMEA_LL.PointType.ENDTP] = TextureFactory.singleton.getImg(TextureFactory.TexAlias.FinishPos);
        }

        public void initGLData()
        {
            for(int i = 0; i<(int)NMEA_LL.PointType.MaxPT;i++)
                if(imds[i]!=null)
                    texs[i] = TextureFactory.singleton.getTex(imds[i]);
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
