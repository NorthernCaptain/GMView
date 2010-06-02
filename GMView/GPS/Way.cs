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
            int oldwx = 99999999;
            int oldwy = 99999999;
            ImageDot imd;
            double angle = Program.opt.angle;

            foreach (WayPoint wp in points)
            {
                imd = imds[(int)wp.ptype];

                wx = wp.x - startP.X;
                wy = wp.y - startP.Y;

                if (Math.Abs(oldwx - wx) < 8 &&
                    Math.Abs(oldwy - wy) < 8 && 
                    wp.ptype != NMEA_LL.PointType.ENDTP)
                    continue;
                oldwy = wy;
                oldwx = wx;

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
    }
}
