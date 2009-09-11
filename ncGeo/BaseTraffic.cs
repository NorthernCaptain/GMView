using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ncGeo
{
    /// <summary>
    /// Base class for all trffic information systems
    /// </summary>
    public abstract class BaseTraffic
    {
        /// <summary>
        /// Return time information for requesting traffic data. If time is undefined yet,
        /// then use given webclient to retrieve it from server and then return
        /// </summary>
        /// <param name="requestFrom"></param>
        /// <returns></returns>
        public abstract string getTimetInfo(System.Net.WebClient requestFrom);

        /// <summary>
        /// Return map tile type for the current traffic system
        /// </summary>
        public abstract MapTileType trafficTileType
        {
            get;
        }

        /// <summary>
        /// Traffic areas in tile coordinates
        /// </summary>
        protected Rectangle[] trafficAreas;

        /// <summary>
        /// Traffic areas in geo coordinates
        /// </summary>
        protected RectangleF[] trafficGeoAreas;

        /// <summary>
        /// Recalculates trafficAreas from geoAreas according to the current zoom level
        /// </summary>
        /// <param name="geoSystem"></param>
        public void updateAreas(BaseGeo geoSystem)
        {
            Rectangle rec = new Rectangle();
            for (int i = 0; i < trafficGeoAreas.Length; i++)
            {
                Point xy;
                geoSystem.getNXNYByLonLat(trafficGeoAreas[i].X, trafficGeoAreas[i].Y, -1, out xy);
                rec.X = xy.X;
                rec.Y = xy.Y;
                geoSystem.getNXNYByLonLat(trafficGeoAreas[i].Width, trafficGeoAreas[i].Height, -1, out xy);
                rec.Width = xy.X - rec.X;
                rec.Height = xy.Y - rec.Y;
                trafficAreas[i] = rec;
            }
        }

        /// <summary>
        /// Return true if the given area (in tiles) has at least one tile inside a list of areas with traffic info.
        /// In other words given area and one from the list has intersection.
        /// </summary>
        /// <param name="fromnx"></param>
        /// <param name="fromny"></param>
        /// <param name="tonx"></param>
        /// <param name="tony"></param>
        /// <returns></returns>
        public bool hasTrafficNXNY(int fromnx, int fromny, int len, int hei)
        {
            Rectangle vrec = new Rectangle(fromnx, fromny, len, hei);
            foreach (Rectangle rec in trafficAreas)
            {
                if (rec.IntersectsWith(vrec))
                    return true;
            }
            return false;
        }
    }
}
