using System;
using System.Collections.Generic;
using System.Text;
using ncUtils;
using System.Data.Common;
using System.Drawing;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Class that represents task data for POI Visual Worker thread
    /// </summary>
    public class POIVisualWorkerTask: IRunnable
    {
        private double lon1;

        /// <summary>
        /// Longitude for left upper corner
        /// </summary>
        public double Lon1
        {
            get { return lon1; }
            set { lon1 = value; }
        }
        private double lat1;

        /// <summary>
        /// Latitude for left upper corner
        /// </summary>
        public double Lat1
        {
            get { return lat1; }
            set { lat1 = value; }
        }

        private double lon2;

        /// <summary>
        /// Longitude for right bottom corner
        /// </summary>
        public double Lon2
        {
            get { return lon2; }
            set { lon2 = value; }
        }

        private double lat2;

        /// <summary>
        /// Latitude for right bottom corner
        /// </summary>
        public double Lat2
        {
            get { return lat2; }
            set { lat2 = value; }
        }

        /// <summary>
        /// Position in tiles and size in tiles
        /// </summary>
        private Rectangle position;
        private int zoom;
        private ncGeo.MapTileType maptype;

        /// <summary>
        /// Factory that we use for loading POIs
        /// </summary>
        private BookMarkFactory factory;

        private List<Bookmark> listToShow;
        private Dictionary<int, Bookmark> listToHide;

        /// <summary>
        /// Constructor that takes region for poi visualization
        /// </summary>
        /// <param name="ilon1"></param>
        /// <param name="ilat1"></param>
        /// <param name="ilon2"></param>
        /// <param name="ilat2"></param>
        public POIVisualWorkerTask(Rectangle ipos, int izoom,
                ncGeo.MapTileType imaptype, BookMarkFactory ifactory)
        {
            position = ipos;
            zoom = izoom;
            maptype = imaptype;
            factory = ifactory;
        }



        public POIVisualWorkerTask(BookMarkFactory ifactory)
        {
            factory = ifactory;
        }

        private static Dictionary<ncGeo.MapTileType, ncGeo.BaseGeo> geos = new Dictionary<ncGeo.MapTileType, ncGeo.BaseGeo>();

        /// <summary>
        /// Return our geosystem
        /// </summary>
        private ncGeo.BaseGeo geosystem
        {
            get
            {
                ncGeo.BaseGeo geo;
                if (!geos.TryGetValue(maptype, out geo))
                {
                    geo = Program.opt.getGeoSystem(maptype);
                    geos.Add(maptype, geo);
                }
                return geo;
            }
        }

        /// <summary>
        /// Calculates and fills lon1,lat1 and lon2,lat2 from nx, ny tile coordinates
        /// using appropriate Geosystem.
        /// </summary>
        private void calculateLonLat()
        {
            ncGeo.BaseGeo geo = geosystem;

            geo.zoomLevel = zoom;

            Point xy = new Point(position.X * Program.opt.image_len, 
                                position.Y * Program.opt.image_hei);
            Point xy2 = xy;
            xy2.Offset(position.Width * Program.opt.image_len, position.Height * Program.opt.image_hei);

            geo.getLonLatByXY(xy, out lon1, out lat1);
            geo.getLonLatByXY(xy2, out lon2, out lat2);
        }

        #region IRunnable Members

        /// <summary>
        /// Select POIs from DB for a given region and types that have auto show flag set
        /// </summary>
        public void run()
        {
            System.Threading.Thread.Sleep(1500);

            listToShow = new List<Bookmark>();

            listToHide = factory.fillListToHide();

            calculateLonLat();

            DBObj dbo = null;
            try
            {
                DateTime start = DateTime.Now;
                dbo = new DBObj(@"select " + BookMarkFactory.poiSelectFields
                    + "from poi, poi_type, poi_spatial where poi.id = poi_spatial.id "
                    + "and poi_type.id = poi.type and poi_type.is_auto_show=1 and poi_type.min_zoom_lvl<=@ZOOM "
                    + "and poi_spatial.minLon>=@LON1 and poi_spatial.maxLon<=@LON2 "
                    + "and poi_spatial.minLat>=@LAT1 and poi_spatial.maxLat<=@LAT2");

                dbo.addIntPar("@ZOOM", zoom);
                dbo.addFloatPar("@LON1", (lon1 < lon2 ? lon1 : lon2));
                dbo.addFloatPar("@LON2", (lon1 < lon2 ? lon2 : lon1));
                dbo.addFloatPar("@LAT1", (lat1 < lat2 ? lat1 : lat2));
                dbo.addFloatPar("@LAT2", (lat1 < lat2 ? lat2 : lat1));

                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
            		//DO each item processing
                    Bookmark poi = new Bookmark(reader);
                    poi.IsAutoShow = true;
                    listToShow.Add( poi );
                    listToHide.Remove(poi.Id);
                }
                TimeSpan ts = DateTime.Now - start;
                Program.Log("POIVisualTask: search took: " + ts.TotalSeconds.ToString("F3"));
            }
            catch (System.Exception e)
            {
            	Program.Log("POIVT SQLError: " + e.ToString());
            
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// The result of the completed task
        /// </summary>
        public List<Bookmark> result
        {
            get { return listToShow; }
        }

        /// <summary>
        /// The list of pois that we need to hide
        /// </summary>
        public Dictionary<int, Bookmark> hideList
        {
            get
            {
                return listToHide;
            }
        }
    }
}
