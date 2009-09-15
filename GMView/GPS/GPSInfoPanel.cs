using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ncGeo;

namespace GMView
{
    /// <summary>
    /// Panel that show GPS information about satellites, position and speed
    /// </summary>
    public class GPSInfoPanel: ISprite
    {
        private bool shown = false;
        private int drawLevel = 3;

        private GPSTrack track;
        private SatelliteCollection satellites;

        private ImageDot bgImg;
        private int bgX = 5, bgY = 5, relX = 0, relY = 0;
        private Brush fntBrush, fntAlertBrush;
        private Font fnt;
        private PointF fntpt = new PointF();
        private Font fntsmall;

        public GPSInfoPanel(int ix, int iy, GPSTrack itrack, SatelliteCollection isat)
        {
            bgX = relX = ix; bgY = relY = iy;
            track = itrack;
            satellites = isat;
            bgImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.GPSPanel);
            fnt = FontFactory.singleton.getGDIFont(FontFactory.FontAlias.Big22B);
            fntsmall = FontFactory.singleton.getGDIFont(FontFactory.FontAlias.Mid14B);
            fntBrush = Brushes.Yellow;
            fntAlertBrush = Brushes.Red;
            TextureFactory.singleton.onInited += initGLData;
        }

        public void setVisibleSize(Size sz)
        {
            bgX = relX < 0 ? sz.Width + relX - bgImg.img.Width : relX;
            bgY = relY < 0 ? sz.Height + relY - bgImg.img.Height : relY;
        }

        public GPSTrack Track
        {
            set { track = value; }
        }

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
            if (!shown)
                return;

            gr.DrawImageUnscaled(bgImg.img, bgX, bgY);

            string str;
            Brush fntBrush = satellites.satellitesInUse > 0 ? this.fntBrush : this.fntAlertBrush;

            //HDOP from sats
            str = satellites.HDOP.ToString("F1");
            fntpt.X = bgX + 109;
            fntpt.Y = bgY + 14;
            gr.DrawString(str, fntsmall, fntBrush, fntpt);

            //Satellites in use (in view)
            str = satellites.satellitesInUse.ToString() + "/" + satellites.satellites.Count.ToString();
            fntpt.X = bgX + 31;
            fntpt.Y = bgY + 3;
            gr.DrawString(str, fnt, fntBrush, fntpt);


            //Lat, lon coordinates
            NMEA_LL gpos = track.lastData;
            if (gpos != null)
            {
                str = gpos.lat.ToString("F3") + " " + gpos.lon.ToString("F3");
                fntpt.X = bgX + 173;
                gr.DrawString(str, fnt, fntBrush, fntpt);

                //Speed of the vehicle
                str = gpos.speed.ToString("F1");
                fntpt.X = bgX + 391;
                gr.DrawString(str, fnt, fntBrush, fntpt);
            }
            
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

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

        protected object bg_tex;
        IGLFont glfnt;
        IGLFont glfnt_small;

        public void initGLData()
        {
            try
            {
                bg_tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.GPSPanel);
                glfnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Big22B);
                glfnt_small = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Mid14B);
            }
            catch
            {
            }
        }

        const float z_dist = -2.0f;
        public void glDraw(int centerx, int centery)
        {
            if (!shown)
                return;

            GML.device.pushMatrix();
            GML.device.identity();
            GML.device.color(Color.White);
            GML.device.texDraw(bg_tex, bgX - centerx, centery - bgY,
                    dLevel + 2, bgImg.img.Width, bgImg.img.Height);

            string str;
            if (satellites.satellitesInUse > 0)
                GML.device.color(Color.Yellow);
            else
                GML.device.color(Color.Red);

            //HDOP from sats
            str = satellites.HDOP.ToString("F1");
            fntpt.X = bgX + 111 - centerx;
            fntpt.Y = centery - bgY - 14;

            glfnt_small.draw(str, (int)fntpt.X, (int)fntpt.Y, drawLevel + 2);


            //Satellites in use (in view)
            str = satellites.satellitesInUse.ToString() + "/" + satellites.satellites.Count.ToString();
            fntpt.X = bgX + 31 - centerx;
            fntpt.Y = centery - bgY - 3;
            glfnt.draw(str, (int)fntpt.X, (int)fntpt.Y, drawLevel + 2);


            //Lat, lon coordinates
            NMEA_LL gpos = track.lastData;
            if (gpos != null)
            {
                str = gpos.lat.ToString("F3") + " " + gpos.lon.ToString("F3");
                fntpt.X = bgX + 173 - centerx;
                glfnt.draw(str, (int)fntpt.X, (int)fntpt.Y, drawLevel + 2);

                //Speed of the vehicle
                str = gpos.speed.ToString("F1");
                fntpt.X = bgX + 391 - centerx;
                glfnt.draw(str, (int)fntpt.X, (int)fntpt.Y, drawLevel + 2);
            }
            GML.device.color(Color.White);
            GML.device.popMatrix();
        }

        #endregion
    }
}
