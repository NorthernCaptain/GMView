using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Represents the user position on the map
    /// Draws the sight on the map if visible
    /// </summary>
    public class UserPosition: ISprite
    {
        protected double lon, lat;
        protected MapObject mapo;
        protected ImageDot img;
        protected int drawLevel = 2;
        protected bool shown = false;
        protected string lonS = "";
        protected string latS = "";

        public UserPosition(MapObject imapo, ImageDot imd)
        {
            mapo = imapo;
            img = imd;
        }

        public UserPosition(MapObject imapo)
        {
            mapo = imapo;
            img = new ImageDot(global::GMView.Properties.Resources.arrow_large, 11, 34);
        }

        public void setVisXY(Point from)
        {
            mapo.getLonLatByVisibleXY(from, out lon, out lat);
            fillLLString();
        }

        private void fillLLString()
        {
            lonS = ncUtils.Glob.lonString(lon);
            latS = ncUtils.Glob.latString(lat);
        }

        public void setLonLat(double flon, double flat)
        {
            lon = flon;
            lat = flat;
            fillLLString();
        }

        public double Lon
        {
            get { return lon; }
        }

        public double Lat
        {
            get { return lat; }
        }

        public string LonS
        {
            get { return lonS; }
        }

        public string LatS
        {
            get { return latS; }
        }

        #region ISprite Members

        public void draw(Graphics g)
        {
            if (!shown)
                return;

            Point xy;
            mapo.getVisibleXYByLonLat(lon, lat, out xy);
            img.draw(g, xy);
        }

        public void draw(Graphics g, int x, int y)
        {
            draw(g);
        }

        public virtual void initGLData()
        {
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

        public virtual void glDraw(int centerx, int centery)
        {
        }

        #endregion
    }
}
