using System;
using System.Collections.Generic;
using System.Text;
using ncUtils;
using System.Data.Common;

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
        /// Factory that we use for loading POIs
        /// </summary>
        private BookMarkFactory factory;

        private List<Bookmark> listToShow;

        /// <summary>
        /// Constructor that takes region for poi visualization
        /// </summary>
        /// <param name="ilon1"></param>
        /// <param name="ilat1"></param>
        /// <param name="ilon2"></param>
        /// <param name="ilat2"></param>
        public POIVisualWorkerTask(double ilon1, double ilat1, double ilon2, double ilat2,
                BookMarkFactory ifactory)
        {
            lon1 = ilon1;
            lat1 = ilat1;
            lon2 = ilon2;
            lat2 = ilat2;
            factory = ifactory;
        }

        public POIVisualWorkerTask(BookMarkFactory ifactory)
        {
            factory = ifactory;
        }

        #region IRunnable Members

        /// <summary>
        /// Select POIs from DB for a given region and types that have auto show flag set
        /// </summary>
        public void run()
        {
            listToShow = new List<Bookmark>();
            DBObj dbo = null;
            try
            {
                DateTime start = DateTime.Now;
                dbo = new DBObj(@"select " + BookMarkFactory.poiSelectFields
                    + "from poi, poi_type, poi_spatial where poi.id = poi_spatial.id "
                    + "and poi_type.id = poi.type and poi_type.is_auto_show=1 "
                    + "and poi_spatial.minLon>=@LON1 and poi_spatial.maxLon<=@LON2 "
                    + "and poi_spatial.minLat>=@LAT1 and poi_spatial.maxLat<=@LAT2");

                dbo.addFloatPar("@LON1", lon1);
                dbo.addFloatPar("@LON2", lon2);
                dbo.addFloatPar("@LAT1", lat1);
                dbo.addFloatPar("@LAT2", lat2);

                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
            		//DO each item processing
                    Bookmark poi = new Bookmark(reader);
                    listToShow.Add( poi );
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
    }
}
