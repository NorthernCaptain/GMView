using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ncGeo;

namespace GMView
{
    public class MapObject: SpriteContainer
    {
        const double avgREarth = 6378.137; //average radius of earth in km.
        const double fastDeltaDegree = 1.0;
        static double degree2km = 0.1; //we use this for fast distance calculations

        public delegate void OnZoomDelegate(int old_zoom, int new_zoom);
        public event OnZoomDelegate onZoomChanged;

        //Start image piece number in upper left corner of the screen
        public int start_nx;
        public int start_ny;

        public Point start_real_xy = new Point();

        //Number of pieces we need to cover visible area
        public int size_nw;
        public int size_nh;
        public int max_piece = 0;

        private BaseGeo geoSystem = new GoogleGeo();

        int _zoom_lvl; //current zooming level

        int zoom_lvl
        {
            get { return _zoom_lvl; }
            set { _zoom_lvl = value; geoSystem.zoomLevel = _zoom_lvl; }
        }

        //visible area size
        Size vis_size = new Size();
        //Starting point on visible area (in screen coords)
        public Point start_p = new Point();

        Point vis_centered_xy1 = new Point(), vis_centered_xy2 = new Point();

        //point on real map in Lon/Lat degrees
        // X = Lon, Y = Lat
        double center_lon = 0.0;
        double center_lat = 0.0;
        int center_zoom_lvl = 0;

        //visible rectangle in lon/lat coords
        double visible_lon1 = 0.0;
        double visible_lat1 = 0.0;
        double visible_lon2 = 0.0;
        double visible_lat2 = 0.0;

        double visible_delta_lon = 0.0;
        double visible_delta_lat = 0.0;

        //Point in pixels of the center of the screen in global coordinates (pixels)
        Point center_p = new Point();

        Pen center_pen;

        MapObject parentMapo = null;
        bool childMode = false;

        protected double rotate_angle = 0.0;

        public delegate void onLonLatChange(double lon, double lat);
        public event onLonLatChange onCenterChanged;

        /// <summary>
        /// Cinstructor of the Map object
        /// </summary>
        public MapObject()
        {
            initObject();
            img_collector_init();
        }

        public MapObject(MapObject imapo)
        {
            parentMapo = imapo;
            img_collector = parentMapo.img_collector;
            childMode = true;
            initObject();
        }

        protected virtual void initObject()
        {
            start_nx = Program.opt.start_x;
            start_ny = Program.opt.start_y;
            zoom_lvl = Program.opt.cur_zoom_lvl;
            center_lon = Program.opt.lon;
            center_lat = Program.opt.lat;
            center_zoom_lvl = zoom_lvl;

            center_pen = new Pen(Color.FromArgb(200, 200, 200), 1);
            center_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            Program.opt.onMapTypeChanged += mapTypeChanged;
            mapTypeChanged();
        }

        #region img_collector staff, hook methods and wrappers

        ImgCollector img_collector = new ImgCollector();

        public delegate void onEndDownloadDelegate();
        public delegate void onStartDownloadDelegate(int total_pieces);
        public delegate void onProgressDownloadDelegate(ImgTile tile, double percent);

        public event onEndDownloadDelegate onEndDownloadTask;
        public event onStartDownloadDelegate onStartDownloadTask;
        public event onProgressDownloadDelegate onProgressDownloadTask;

        public event onEndDownloadDelegate onSchedEndDownloadTask;
        public event onStartDownloadDelegate onSchedStartDownloadTask;
        public event onProgressDownloadDelegate onSchedProgressDownloadTask;

        protected void img_collector_init()
        {
            img_collector.onFinishLoad += new ImgCollector.FinishedLoadTask(img_collector_onFinishLoad);
            img_collector.onStartLoad += new ImgCollector.StartLoadTask(img_collector_onStartLoad);
            img_collector.onLoadProgress += new ImgCollector.LoadTaskPartlyDone(img_collector_onLoadProgress);

            img_collector.onSchedFinishLoad += new ImgCollector.FinishedLoadTask(img_collector_onSchedFinishLoad);
            img_collector.onSchedStartLoad += new ImgCollector.StartLoadTask(img_collector_onSchedStartLoad);
            img_collector.onSchedLoadProgress += new ImgCollector.LoadTaskPartlyDone(img_collector_onSchedLoadProgress);
        }

        void img_collector_onLoadProgress(ImgTile loaded_tile, double completed_percent)
        {
            if (onProgressDownloadTask != null)
                onProgressDownloadTask(loaded_tile, completed_percent);
        }

        void img_collector_onStartLoad(int total_pieces)
        {
            if (onStartDownloadTask != null)
                onStartDownloadTask(total_pieces);
        }

        void img_collector_onFinishLoad()
        {
            if (onEndDownloadTask != null)
                onEndDownloadTask();
        }

        void img_collector_onSchedLoadProgress(ImgTile loaded_tile, double completed_percent)
        {
            if (onSchedProgressDownloadTask != null)
                onSchedProgressDownloadTask(loaded_tile, completed_percent);
        }

        void img_collector_onSchedStartLoad(int total_pieces)
        {
            if (onSchedStartDownloadTask != null)
                onSchedStartDownloadTask(total_pieces);
        }

        void img_collector_onSchedFinishLoad()
        {
            if (onSchedEndDownloadTask != null)
                onSchedEndDownloadTask();
        }

        public void schedDownloadTask(ImgCollector.LoadTask task)
        {
            img_collector.addSchedTask(task);
        }

        public void startTileAcquisition()
        {
            ImgTile.access_id++;
            img_collector.newDownloadTask(this);
        }

        public void endTileAcquisition()
        {
            img_collector.endFeedingDownloadTask();
        }

        public void autoClear(int threshold)
        {
            img_collector.autoClear(threshold);
        }

        #endregion

        #region Get/Set staff

        public PositionStack.PositionInfo centerPos
        {
            get { return new PositionStack.PositionInfo(center_lon, center_lat, zoom_lvl, Program.opt.mapType); }
        }
        /// <summary>
        /// Sets the size of visible area of the map
        /// </summary>
        /// <param name="sz"></param>
        public void SetVisibleSize(Size sz)
        {
            vis_size = sz;
            RecalculateParams();
        }

        /// <summary>
        /// Check point for visibility to the user
        /// </summary>
        /// <param name="xy"></param>
        /// <returns>true if point is in visible area of the map</returns>
        public bool isVisibleOnMap(Point xy)
        {
            if (xy.X < 0 || xy.Y < 0 || xy.X >= vis_size.Width || xy.Y >= vis_size.Height)
                return false;
            return true;
        }

        /// <summary>
        /// Check for visibility to the user of the given point (point in absolute pixel coords)
        /// </summary>
        /// <param name="xy"></param>
        /// <returns></returns>
        public bool isVisibleOnMapRealXY(Point xy)
        {
            if (xy.X < start_real_xy.X || xy.Y < start_real_xy.Y 
                || xy.X >= start_real_xy.X + vis_size.Width || xy.Y >= start_real_xy.Y + vis_size.Height)
                return false;
            return true;
        }

        /// <summary>
        /// Check geopoint for visibility to the user on the screen according to the current view
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns>true if geopoint is visible</returns>
        public bool isVisibleOnMap(double lon, double lat)
        {
            if (lon < visible_lon1 || lat < visible_lat2 || lon > visible_lon2 || lat > visible_lat1)
                return false;
            return true;
        }

        /// <summary>
        /// Check if two geopoint has the same visible position on the screen
        /// according to our current zoom level
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public bool isSameVisiblePoint(double lon1, double lat1, double lon2, double lat2)
        {
            if (Math.Abs(lon1 - lon2) > visible_delta_lon ||
                Math.Abs(lat1 - lat2) > visible_delta_lat)
                return false;
            return true;
        }

        /// <summary>
        /// Gets the image piece for displaying on screen.
        /// If the image is not loaded yet, then loads it
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <returns>return image piece or null</returns>
        public ImgTile getImage(int x_idx, int y_idx)
        {
            return img_collector.getImage(start_nx + x_idx, start_ny + y_idx, zoom_lvl, Program.opt.mapType);
        }
        /// <summary>
        /// Gets the image piece for displaying on screen.
        /// If the image is not loaded yet, then loads it
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="mType"></param>
        /// <returns></returns>
        public ImgTile getImage(int x_idx, int y_idx, MapTileType mType)
        {
            return img_collector.getImage(start_nx + x_idx, start_ny + y_idx, zoom_lvl, mType);
        }
        /// <summary>
        /// Gets starting point of the upper left image
        /// </summary>
        /// <returns>position of the first image</returns>
        public Point getStartPoint()
        {
            return start_p;
        }

        public int zoom
        {
            get { return zoom_lvl; }
        }

        public double angle
        {
            get { return rotate_angle; }
            set { Program.opt.angle = value; rotate_angle = Program.opt.angle; }
        }

        #endregion

        #region Reset and recalculate parameters

        /// <summary>
        /// Called when map type changed
        /// </summary>
        private void mapTypeChanged()
        {
            geoSystem = Program.opt.getGeoSystem().copy();
            geoSystem.zoomLevel = this.zoom_lvl;
        }

        public void ResetCache()
        {
            img_collector.ResetCache();
        }

        /// <summary>
        /// Calculate all parameters according to our variables
        /// </summary>
        void RecalculateParams()
        {
            max_piece = 2 << (zoom_lvl - 1);
            if (start_p.X > 0)
            {
                start_p.X -= Program.opt.image_len;
                start_nx--;
            }
            if (start_p.Y > 0)
            {
                start_p.Y -= Program.opt.image_hei;
                start_ny--;
            }

            if (start_p.X <= -Program.opt.image_len)
            {
                start_p.X += Program.opt.image_len;
                start_nx++;
            }
            else
                if (start_p.X >= Program.opt.image_len)
                {
                    start_p.X -= Program.opt.image_len;
                    start_nx--;
                }

            if (start_p.Y <= -Program.opt.image_hei)
            {
                start_p.Y += Program.opt.image_hei;
                start_ny++;
            }
            else
                if (start_p.Y >= Program.opt.image_hei)
                {
                    start_p.Y -= Program.opt.image_hei;
                    start_ny--;
                }

            if (start_nx < 0)
            {
                start_nx = 0;
                start_p.X = 0;
            }

            if (start_ny < 0)
            {
                start_ny = 0;
                start_p.Y = 0;
            }

            size_nw = (vis_size.Width + Program.opt.image_len - 1 - start_p.X) / Program.opt.image_len;
            size_nh = (vis_size.Height + Program.opt.image_hei - 1 - start_p.Y) / Program.opt.image_hei;

            if (size_nw >= max_piece)
            {
                start_nx = 0;
                start_p.X = 0;
                size_nw = max_piece;
            }

            if (size_nh >= max_piece)
            {
                start_ny = 0;
                start_p.Y = 0;
                size_nh = max_piece;
            }

            start_real_xy.X = (start_nx * Program.opt.image_len - start_p.X);
            start_real_xy.Y = (start_ny * Program.opt.image_hei - start_p.Y);
            center_p.X =  start_real_xy.X + vis_size.Width / 2;
            center_p.Y =  start_real_xy.Y + vis_size.Height / 2;

            vis_centered_xy1.X = vis_size.Width / 2 - vis_size.Width / 5;
            vis_centered_xy2.X = vis_size.Width / 2 + vis_size.Width / 5;

            vis_centered_xy1.Y = vis_size.Height / 2 - vis_size.Height / 5;
            vis_centered_xy2.Y = vis_size.Height / 2 + vis_size.Height / 5;

            { //calculate visible area
                Point p = new Point(0, 0);
                getLonLatByVisibleXY(p, out visible_lon1, out visible_lat1);

                p.X = vis_size.Width;
                p.Y = vis_size.Height;
                getLonLatByVisibleXY(p, out visible_lon2, out visible_lat2);

                visible_delta_lon = Math.Abs(visible_lon1 - visible_lon2)/(double)p.X;
                visible_delta_lat = Math.Abs(visible_lat1 - visible_lat2)/(double)p.Y;
            }

            //Program.Log("Recalc: start delta xy: " + start_p.ToString() + " piece nums: " + start_nx + "/" 
            //    + start_ny + " [" + size_nw + "x" + size_nh + "]");
        }
        #endregion

        #region Map manipulation methods (zoom, center, move)

        public void MoveMapByScreenPoint(Point mouse_delta_p)
        {
            start_p.X += mouse_delta_p.X;
            start_p.Y += mouse_delta_p.Y;
            RecalculateParams();
            getLonLatByXY(center_p, out center_lon, out center_lat);
            center_zoom_lvl = zoom_lvl;
            if (onCenterChanged != null)
                onCenterChanged(center_lon, center_lat);
        }

        /// <summary>
        /// Here we convert Lon/Lat to pixel coordinates, compute piece numbers
        /// according to our zooming level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public void CenterMapLonLat(double lon, double lat)
        {
            center_lon = lon;
            center_lat = lat;
            center_zoom_lvl = zoom_lvl;

            Point xy;

            updateFastDistance();

            getXYByLonLat(lon, lat, out xy);

            //here is a right way of calculating viewing borders
            start_p.X = (xy.X - vis_size.Width / 2);
            start_p.Y = (xy.Y - vis_size.Height / 2);
            start_nx = start_p.X/Program.opt.image_len;
            start_ny = start_p.Y/Program.opt.image_hei;
            start_p.X = - (start_p.X % Program.opt.image_len);
            start_p.Y = - (start_p.Y % Program.opt.image_hei);

            if (start_nx < 0)
                start_nx = 0;
            if (start_ny < 0)
                start_ny = 0;

            RecalculateParams();
            if (onCenterChanged != null)
                onCenterChanged(center_lon, center_lat);

        }

        /// <summary>
        /// Centering the map according to relative (screen) coordinates in pixels
        /// </summary>
        /// <param name="xy"></param>
        public void CenterMapVisibleXY(Point xy)
        {
            xy.X += (start_nx * Program.opt.image_len - start_p.X);
            xy.Y += (start_ny * Program.opt.image_hei - start_p.Y);

            CenterMapAbsXY(xy);
        }

        /// <summary>
        /// Centering the map by given absolute XY values (in pixel on current zoom level)
        /// </summary>
        /// <param name="xy"></param>
        public void CenterMapAbsXY(Point xy)
        {
            int max_x = max_piece * Program.opt.image_len;
            if (xy.X < 0 || xy.Y < 0 ||
                xy.X >= max_x || xy.Y >= max_x)
                return;
            //here is a right way of calculating viewing borders
            start_p.X = (xy.X - vis_size.Width / 2);
            start_p.Y = (xy.Y - vis_size.Height / 2);
            start_nx = start_p.X / Program.opt.image_len;
            start_ny = start_p.Y / Program.opt.image_hei;
            start_p.X = -(start_p.X % Program.opt.image_len);
            start_p.Y = -(start_p.Y % Program.opt.image_hei);

            if (start_nx < 0)
                start_nx = 0;
            if (start_ny < 0)
                start_ny = 0;

            RecalculateParams();
            getLonLatByXY(center_p, out center_lon, out center_lat);
            center_zoom_lvl = zoom_lvl;
            if (onCenterChanged != null)
                onCenterChanged(center_lon, center_lat);
        }

        /// <summary>
        /// Function check if given point inside centered region,
        /// if not then it center the map to the given point
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns>true if we did recenter and need to redraw map, false if not</returns>
        public bool ensureCentered(double lon, double lat)
        {
            double lon1, lat1;
            double lon2, lat2;

            getLonLatByVisibleXY(vis_centered_xy1, out lon1, out lat1);
            getLonLatByVisibleXY(vis_centered_xy2, out lon2, out lat2);
            if (lon >= lon1 && lon <= lon2 && lat >= lat1 && lat <= lat2)
                return false; //we are inside centered region
            CenterMapLonLat(lon, lat);
            return true;
        }

        public void recenterMap()
        {
            CenterMapLonLat(center_lon, center_lat);
            if (onZoomChanged != null)
                onZoomChanged(zoom_lvl, zoom_lvl);
        }

        public void ZoomMapXY(Point centerxy, int new_zoom_lvl)
        {
            double lon, lat;
            int old_zoom = zoom_lvl;

            centerxy.X += (start_nx * Program.opt.image_len - start_p.X);
            centerxy.Y += (start_ny * Program.opt.image_hei - start_p.Y);

            getLonLatByXY(centerxy, out lon, out lat);
            zoom_lvl = new_zoom_lvl;
            Program.Log("XY -> Lon/Lat: " + centerxy.ToString() + " LonLat:" + lon + "/" + lat);
            CenterMapLonLat(lon, lat);
            if (onZoomChanged != null)
                onZoomChanged(old_zoom, new_zoom_lvl);
        }

        public void ZoomMapCentered(int new_zoom_lvl)
        {
            if (zoom_lvl > center_zoom_lvl)
            {
                Point centerxy = new Point();

                int w = size_nw * Program.opt.image_len + start_p.X;
                int h = size_nh * Program.opt.image_hei + start_p.Y;

                w = w < vis_size.Width ? w : vis_size.Width;
                h = h < vis_size.Height ? h : vis_size.Height;

                centerxy.X = w / 2;
                centerxy.Y = h / 2;
                ZoomMapXY(centerxy, new_zoom_lvl);
            }
            else
            {
                int old_zoom = zoom_lvl;
                zoom_lvl = new_zoom_lvl;
                CenterMapLonLat(center_lon, center_lat);
                if (onZoomChanged != null)
                    onZoomChanged(old_zoom, new_zoom_lvl);
            }
        }


        public void syncWithParent()
        {
            if (childMode)
            {
                zoom_lvl = parentMapo.zoom_lvl + Program.opt.mini_delta_zoom;
                if (zoom_lvl < 1)
                    zoom_lvl = 1;
                if (zoom_lvl > Program.opt.max_zoom_lvl)
                    zoom_lvl = Program.opt.max_zoom_lvl;

                CenterMapLonLat(parentMapo.center_lon, parentMapo.center_lat);
            }
        }

        #endregion

        #region Lon/Lat <-> XY (visible or global) transformations
        /// <summary>
        /// Here we convert point on our huge pixel map to world Lon/Lat degrees
        /// </summary>
        /// <param name="xy">absolue coordinates in pixels of current zoom level</param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public void getLonLatByXY(Point xy, out double lon, out double lat)
        {
            geoSystem.getLonLatByXY(xy, out lon, out lat);
            //Program.Log("XY -> Lon/Lat: " + xy.ToString() + " LonLat:" + lon + "/" + lat);
        }

        /// <summary>
        /// Convert LonLat coordinates to XY pixel coordinates according to current zooming level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="xy"></param>
        public void getXYByLonLat(double lon, double lat, out Point xy)
        {
            geoSystem.getXYByLonLat(lon, lat, out xy);

            //Program.Log("Lon/Lat -> XY: " + xy.ToString() + " LonLat:" + lon + "/" + lat * 180.0 / System.Math.PI);
        }

        public void getLonLatByVisibleXY(Point xy, out double lon, out double lat)
        {
            xy.X += (start_nx * Program.opt.image_len - start_p.X);
            xy.Y += (start_ny * Program.opt.image_hei - start_p.Y);

            getLonLatByXY(xy, out lon, out lat);

        }

        /// <summary>
        /// Calculates visible on screen coordinates of given Lon/Lat
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="xy"></param>
        public void getVisibleXYByLonLat(double lon, double lat, out Point xy)
        {
            getXYByLonLat(lon, lat, out xy);
            xy.X = xy.X - start_nx * Program.opt.image_len + start_p.X;
            xy.Y = xy.Y - start_ny * Program.opt.image_hei + start_p.Y;
        }

        /// <summary>
        /// Calculates distance (in km) between two points with coordinates in lon/lat.
        /// </summary>
        /// <param name="lon1">point 1 lon</param>
        /// <param name="lat1">point 1 lat</param>
        /// <param name="lon2">point 2 lon</param>
        /// <param name="lat2">point 2 lat</param>
        /// <returns>distance in km</returns>
        public static double getDistanceByLonLat(double lon1, double lat1, double lon2, double lat2)
        {
            //convert degrees to radians
            lat1 = Math.PI * lat1 / 180.0;
            lat2 = Math.PI * lat2 / 180.0;
            lon1 = Math.PI * lon1 / 180.0;
            lon2 = Math.PI * lon2 / 180.0;

            double v1 = Math.Sin(lat1) * Math.Sin(lat2);
            double v2 = Math.Cos(lat1)*Math.Cos(lat2)*Math.Cos(lon1 - lon2);
            double d = Math.Acos(v1 + v2);
            return d * avgREarth;
        }

        /// <summary>
        /// More accurate method for distance calculation between two geo coordinates (lon, lat)
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double getDistanceByLonLat2(double lon1, double lat1, double lon2, double lat2)
        {
            //convert degrees to radians
            lat1 = Math.PI * lat1 / 180.0;
            lat2 = Math.PI * lat2 / 180.0;
            lon1 = Math.PI * lon1 / 180.0;
            lon2 = Math.PI * lon2 / 180.0;

            double sinlat = Math.Sin((lat2 - lat1) / 2.0);
            double sinlon = Math.Sin((lon2 - lon1) / 2.0);
            double v1 = sinlat * sinlat + Math.Cos(lat1) * Math.Cos(lat2) * sinlon * sinlon;
            return 2.0 * Math.Asin(Math.Sqrt(v1))* avgREarth;
        }

        public static double getDistanceByLonLatFast(double lon1, double lat1, double lon2, double lat2)
        {
            double dlon = lon1 - lon2;
            double dlat = lat1 - lat2;
            double degdist = dlon * dlon + dlat * dlat;
            return Math.Sqrt(degdist) * degree2km;
        }

        protected void updateFastDistance()
        {
            degree2km = MapObject.getDistanceByLonLat2(center_lon, center_lat,
                                                center_lon + fastDeltaDegree, center_lat + fastDeltaDegree)/1.4142/fastDeltaDegree;
        }

        #endregion

        #region ISprite Members

        public override void draw(Graphics gr)
        {
            if (childMode)
                syncWithParent();

            //draw our map from tiles
            startTileAcquisition();
            for (int x = 0; x < size_nw; x++)
            {
                for (int y = 0; y < size_nh; y++)
                {
                    ImgTile img = getImage(x, y);
                    if (img != null && img.status == ImgStatus.InMemory)
                        img.draw(gr, start_p.X + x * Program.opt.image_len,
                                start_p.Y + y * Program.opt.image_hei);
                }
            }
            endTileAcquisition();

            int cx = vis_size.Width / 2;
            int cy = vis_size.Height / 2;

            gr.DrawLine(center_pen, 0, cy, vis_size.Width, cy);
            gr.DrawLine(center_pen, cx, 0, cx, vis_size.Height);

            //draw objects from the container on top of the map
            base.draw(gr);
        }

        public override void draw(Graphics gr, int x, int y)
        {
            draw(gr);
        }

        protected void drawTiles(int centerx, int centery, int sx, int sy, int size_nx, int size_ny, MapTileType mType)
        {
            for (int x = sx; x < size_nx; x++)
            {
                for (int y = sy; y < size_ny; y++)
                {
                    ImgTile img = getImage(x, y, mType);
                    if (img != null && img.status == ImgStatus.InMemory)
                        img.draw(null, start_p.X + x * Program.opt.image_len - centerx,
                                centery - (start_p.Y + y * Program.opt.image_hei));
                }
            }

        }

        protected void drawTilesMT(int centerx, int centery, int sx, int sy, int size_nx, int size_ny, MapTileType mType)
        {
            img_collector.startLoadThreaded();
            for (int x = sx; x < size_nx; x++)
            {
                for (int y = sy; y < size_ny; y++)
                {
                    ImgTile img = img_collector.getImageThreaded(start_nx + x, start_ny + y, zoom_lvl, mType);
                    if (img != null && img.status == ImgStatus.InMemory)
                        img.draw(null, start_p.X + x * Program.opt.image_len - centerx,
                                centery - (start_p.Y + y * Program.opt.image_hei));
                }
            }
            img_collector.endLoadThreaded();
            ImgTile img_loaded;
            while ((img_loaded = img_collector.popLoaded()) != null)
            {
                if (img_loaded.status == ImgStatus.InMemory)
                    img_loaded.draw(null, start_p.X + (img_loaded.x - start_nx) * Program.opt.image_len - centerx,
                            centery - (start_p.Y + (img_loaded.y - start_ny) * Program.opt.image_hei));
            }

        }

        /// <summary>
        /// Calls from ImgTile on loading tile, return additional layered bitmap or null
        /// </summary>
        /// <param name="forMe"></param>
        /// <returns></returns>
        private Bitmap loadTrafficTile(ImgTile forMe)
        {
            return img_collector.getBitmapForLayer(forMe.x, forMe.y, forMe.zoom,
                    geoSystem.trafficSystem.trafficTileType);
        }

        public override void glDraw(int centerx, int centery)
        {
            ncUtils.Glob.lastRefreshTicks = DateTime.Now.Ticks;

            if (childMode)
                syncWithParent();

            int sy = -2, size_ny = size_nh+3;
            int sx = -1, size_nx = size_nw+2;

            bool needTrafficLayer = zoom_lvl > 9 && Program.opt.showTraffic && geoSystem.trafficSystem != null
                    && geoSystem.trafficSystem.hasTrafficNXNY(start_nx, start_ny, size_nx, size_ny);

            if (rotate_angle > 0.01 || rotate_angle < -0.01)
            {
                sy = -2;
                size_ny += 1;
            }


            //draw our map from tiles
            startTileAcquisition();
            GML.device.texDrawBegin();

            if(Program.opt.load_with_mt)
                drawTilesMT(centerx, centery, sx, sy, size_nx, size_ny, Program.opt.mapType);
            else
                drawTiles(centerx, centery, sx, sy, size_nx, size_ny, Program.opt.mapType);

            GML.device.texDrawEnd();

            if (needTrafficLayer)
            {
                GML.device.texDrawBegin();

                if (Program.opt.load_with_mt)
                    drawTilesMT(centerx, centery, sx, sy, size_nx, size_ny, geoSystem.trafficSystem.trafficTileType);
                else
                    drawTiles(centerx, centery, sx, sy, size_nx, size_ny, geoSystem.trafficSystem.trafficTileType);

                GML.device.texDrawEnd();

            }

            endTileAcquisition();


            {
                GML.device.lineWidth(1.0f);
                GML.device.lineStipple((short)0x18FF);
                GML.device.lineDraw(-centerx, 0, centerx, 0, 0, center_pen.Color);
                GML.device.lineDraw(0, -centery, 0, centery, 0, center_pen.Color);
            }

            if(Program.opt.fog_of_war)
                ImgCacheManager.singleton.glDrawOnScreenGrid(centerx, centery);

            if(Program.opt.show_wind_rose)
            { //rose
                ImageDot imd = TextureFactory.singleton.getImg(TextureFactory.TexAlias.Rose);
                object tex = TextureFactory.singleton.getTex(imd);
                GML.device.texDrawBegin();
                GML.device.texFilter(tex, TexFilter.Pixel);
                GML.device.texDraw(tex, -imd.delta_x, imd.delta_y, 0, imd.img.Width, imd.img.Height);
                GML.device.texDrawEnd();
            }

            //test
            //GML.device.rectDraw(0, 0, 100, 100, 0, Color.FromArgb(155, Color.DarkRed));

            base.glDraw(centerx, centery);
        }
        #endregion

        internal void reloadScreen()
        {
            int sy = -1, size_ny = size_nh+2;
            int sx = -1, size_nx = size_nw+2;

            if (rotate_angle > 0.01 || rotate_angle < -0.01)
            {
                sy = -2;
                size_ny += 1;
            }

            startTileAcquisition();
            for (int x = sx; x < size_nx; x++)
            {
                for (int y = sy; y < size_ny; y++)
                {
                    img_collector.reloadImage(start_nx + x, start_ny + y, zoom_lvl, Program.opt.mapType);
                }
            }
            endTileAcquisition();
        }
    }
}
