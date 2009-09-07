using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Google coordinate system
    /// </summary>
    class GoogleGeo: BaseGeo
    {
        /// <summary>
        /// Maximum zoom level for ya.maps
        /// </summary>
        protected long maxZoom = 18;

        protected int currentZoom = 1;

        private GoogleTraffic trafSystem = new GoogleTraffic();

        public override void getLonLatByXY(System.Drawing.Point xy, int zoom_lvl, out double lon, out double lat)
        {
            if (zoom_lvl < 0)
                zoom_lvl = currentZoom;
            double center_pix = (double)(1ul << (zoom_lvl + 6));

            lon = (((double)xy.X - center_pix) / center_pix * 180.0) % 180.0;
            lat = ((2.0 *
                    Math.Atan(Math.Exp(((double)xy.Y - center_pix) / -center_pix * Math.PI))
                    - Math.PI / 2.0) * 180.0 / Math.PI) % 180.0;

        }

        public override void getLonLatByXY(System.Drawing.Point xy, out double lon, out double lat)
        {
            getLonLatByXY(xy, currentZoom, out lon, out lat);
        }

        public override void getXYByLonLat(double lon, double lat, out System.Drawing.Point xy)
        {
            getXYByLonLat(lon, lat, currentZoom, out xy);
        }

        public override void getXYByLonLat(double lon, double lat, int zoom_lvl, out System.Drawing.Point xy)
        {
            if (zoom_lvl < 0)
                zoom_lvl = currentZoom;
            ulong center_pix = 1ul << (zoom_lvl + 6);

            lat = Math.PI * lat / 180.0;
            double sin_lat = Math.Sin(lat);
            double fx = (double)center_pix * (1.0 + lon / 180.0);
            //            (1 - 0.5 * ln((1 + sin(Lat)) / (1 - sin(Lat))) / Pi);
            double fy = (double)center_pix * (1.0 - 0.5 * Math.Log((1.0 + sin_lat)
                                                                   / (1.0 - sin_lat))
                                                          / Math.PI);

            xy = new System.Drawing.Point();
            xy.X = (int)fx;
            xy.Y = (int)fy;
        }

        public override void getNXNYByLonLat(double lon, double lat, int zoom_lvl, out System.Drawing.Point xy)
        {
            if (zoom_lvl < 0)
                zoom_lvl = currentZoom;

            ulong center_pix = 1ul << (zoom_lvl + 6);

            lat = Math.PI * lat / 180.0;
            double sin_lat = Math.Sin(lat);
            double fx = (double)center_pix * (1.0 + lon / 180.0);
            //            (1 - 0.5 * ln((1 + sin(Lat)) / (1 - sin(Lat))) / Pi);
            double fy = (double)center_pix * (1.0 - 0.5 * Math.Log((1.0 + sin_lat)
                                                                   / (1.0 - sin_lat))
                                                          / Math.PI);

            xy = new System.Drawing.Point();
            xy.X = (int)(fx / 256);
            xy.Y = (int)(fy / 256);
        }

        public override int zoomLevel
        {
            get { return currentZoom; }
            set { currentZoom = value; trafSystem.updateAreas(this); }
        }

        public override BaseGeo copy()
        {
            BaseGeo geo = new GoogleGeo();
            geo.zoomLevel = this.zoomLevel;
            return geo;
        }

        public override BaseTraffic trafficSystem
        {
            get { return trafSystem; }
        }
    }
}
