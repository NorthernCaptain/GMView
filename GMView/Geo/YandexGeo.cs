using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Yandex Maps coordinate system with all their transformation calls that we need.
    /// </summary>
    public class YandexGeo: BaseGeo
    {
        /// <summary>
        /// Earth radius in meters for mercantor projection
        /// </summary>
        protected double radius = 6378137d;
        protected double equator = 40075016.68557849d;

        protected const double _e = 0.0818191908426d;
        protected const double _e2 = 0.00669437999014d;
        protected const double _epsilon = 1e-10d;

        protected const double _halfPI = Math.PI / 2.0d;
        protected const double _twoPI = Math.PI * 2.0d;
        protected const double _180divPI = 180d / Math.PI;
        protected const double _PIdiv180 = Math.PI / 180d;

        /// <summary>
        /// Maximum zoom level for ya.maps
        /// </summary>
        protected const long maxZoom = 23;
        protected long worldSize = 1L << (int)(maxZoom + 8L);
        protected double worldDivEquator = 0d;
        protected double halfEquator = 0d;
        protected const double latRestriction = 89.3;

        protected int currentZoom = 1;

        protected long scaleFactor = 1;

        private YandexTraffic trafSystem = new YandexTraffic();

        public YandexGeo()
        {
            halfEquator = equator / 2.0d;
            worldDivEquator = (double)(worldSize) / equator;
        }

        /// <summary>
        /// Translates mercator projected coordinates to Geo-coordinates (lon, lat)
        /// </summary>
        /// <param name="mercX"></param>
        /// <param name="mercY"></param>
        /// <param name="geoLon"></param>
        /// <param name="geoLat"></param>
        public void mercatorToGeo(double mercX, double mercY, out double geoLon, out double geoLat)
        {
            double V = _halfPI - 2.0d * Math.Atan(1.0d / Math.Exp(mercY / radius));
            double b = V +  0.003356551468879694d * Math.Sin(2.0d * V) 
                +  0.00000657187271079536d * Math.Sin(4.0d * V) 
                +  1.764564338702e-8d * Math.Sin(6.0d * V) 
                +  5.328478445e-11d * Math.Sin(8.0d * V);
            double S = mercX / radius;
            geoLon = S * _180divPI;
            geoLat = b * _180divPI;
        }

        /// <summary>
        /// Translates Geo coordinates (lon, lat) into Mercator projected coordinates
        /// </summary>
        /// <param name="geoLon"></param>
        /// <param name="geoLat"></param>
        /// <param name="mercX"></param>
        /// <param name="mercY"></param>
        public void geoToMercator(double geoLon, double geoLat, out double mercX, out double mercY)
        {
            double R = geoLon * _PIdiv180;
            double a = boundaryRestrict(geoLat, -90, 90) * _PIdiv180;
            double S = _e * Math.Sin(a);
            double V = Math.Tan(Math.PI / 4.0d + a / 2.0d);
            if(V < _epsilon)
                V = _epsilon;

            double X = Math.Pow(Math.Tan(Math.PI / 4.0d + Math.Asin(S) / 2.0d), _e);
            mercX = Math.Round(radius * R);
            mercY = Math.Round(radius * Math.Log( V / X ));
        }

        /// <summary>
        /// Convert pixel coordinates to mercator coordinates
        /// </summary>
        /// <param name="piX"></param>
        /// <param name="piY"></param>
        /// <param name="mercX"></param>
        /// <param name="mercY"></param>
        public void pixelsToMercator(long piX, long piY, out double mercX, out double mercY)
        {
            mercX = Math.Round((double)(piX) / worldDivEquator - halfEquator);
            mercY = Math.Round(halfEquator - (double)(piY) / worldDivEquator);
        }

        /// <summary>
        /// Convert mercator coordinates to pixel coordinates
        /// </summary>
        /// <param name="mercX"></param>
        /// <param name="mercY"></param>
        /// <param name="piX"></param>
        /// <param name="piY"></param>
        public void mercatorToPixels(double mercX, double mercY, out long piX, out long piY)
        {
            piX = (long)Math.Round((halfEquator + mercX) * worldDivEquator);
            piY = (long)Math.Round((halfEquator - mercY) * worldDivEquator);
        }

        /// <summary>
        /// Check boundaries and restrict value to them
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        protected double boundaryRestrict(double value, double min, double max)
        {
            value = value < min ? min : (value > max ? max : value);
            return value;
        }

        /// <summary>
        /// Convert coordinates given in pixels into geo coordinates (lon, lat) for the given zoom level
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public override void getLonLatByXY(Point xy, int zoom, out double lon, out double lat)
        {
            double mercX, mercY;
            long scaleFact = this.scaleFactor;
            if (zoom > 0 && zoom != currentZoom)
            {
                scaleFact = 1L << (int)(maxZoom - zoom);
            }

            long piX = xy.X, piY = xy.Y;
            piX *= scaleFact;
            piY *= scaleFact;
            pixelsToMercator(piX, piY, out mercX, out mercY);
            mercatorToGeo(mercX, mercY, out lon, out lat);
        }

        /// <summary>
        /// Convert coordinates given in pixels for current zoom level into geo coordinates (lon, lat)
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public override void getLonLatByXY(Point xy, out double lon, out double lat)
        {
            getLonLatByXY(xy, -1, out lon, out lat);
        }

        /// <summary>
        /// Convert geo coordinates into pixel coordinates for current zoom level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="xy"></param>
        public override void getXYByLonLat(double lon, double lat, out Point xy)
        {
            getXYByLonLat(lon, lat, -1, out xy);
        }

        /// <summary>
        /// Convert geo coordinates into pixel coordinates for the given zoom level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zoom"></param>
        /// <param name="xy"></param>
        public override void getXYByLonLat(double lon, double lat, int zoom, out Point xy)
        {
            long scaleFact = this.scaleFactor;
            if (zoom > 0 && zoom != currentZoom)
            {
                scaleFact = 1L << (int)(maxZoom - zoom);
            }

            long piX, piY;
            double mercX, mercY;
            geoToMercator(lon, lat, out mercX, out mercY);
            mercatorToPixels(mercX, mercY, out piX, out piY);
            xy = new Point();
            xy.X = (int)(piX / scaleFact);
            xy.Y = (int)(piY / scaleFact);
        }

        /// <summary>
        /// Return tiles xy numbers from the given lon, lat and zooming level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zoom_lvl"></param>
        /// <param name="xy"></param>
        public override void getNXNYByLonLat(double lon, double lat, int zoom_lvl, out Point xy)
        {
            getXYByLonLat(lon, lat, zoom_lvl -1, out xy);
            xy.X /= 256;
            xy.Y /= 256;
        }

        /// <summary>
        /// Get/Set current zoom level
        /// </summary>
        public override int zoomLevel
        {
            get { return currentZoom + 1; }
            set 
            { 
                currentZoom = value - 1; 
                scaleFactor = 1L << (int)(maxZoom - currentZoom); 
                trafficSystem.updateAreas(this); 
            }
        }

        public override BaseGeo copy()
        {
            BaseGeo geo = new YandexGeo();
            geo.zoomLevel = this.zoomLevel;
            return geo;
        }

        public override BaseTraffic trafficSystem
        {
            get { return trafSystem; }
        }
    }
}
