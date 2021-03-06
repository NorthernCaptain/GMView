using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ncGeo
{
    public enum MapTileType
    {
        MapOnly, //Only street map
        SatMap,  //Satellite view map
        OSMMapnik, // openstreetmaps.org mapnik images
        YandexMap, // maps.yandex.ru - images from yandex
        YandexSat, // satellite view from yandex
        YandexSatSteet, //satellite view with streets from yandex
        SatStreet, //Satellite view with streets on top on it
        TerMap, //Terrain map with streets
        OSMRenderer, // openstreetmaps.org OSMArenderer images
        YandexTraffic, // Yandex.Probki traffic information (Russia only)
        GooTraffic, //Google traffic information (USA only)
        MaxValue
    };


    /// <summary>
    /// Base class for different geo coordinate systems
    /// </summary>
    public abstract class BaseGeo
    {
        /// <summary>
        /// Convert coordinates given in pixels into geo coordinates (lon, lat) for the given zoom level
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public abstract void getLonLatByXY(Point xy, int zoom, out double lon, out double lat);
        /// <summary>
        /// Convert coordinates given in pixels for current zoom level into geo coordinates (lon, lat)
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public abstract void getLonLatByXY(Point xy, out double lon, out double lat);
        /// <summary>
        /// Convert geo coordinates into pixel coordinates for current zoom level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="xy"></param>
        public abstract void getXYByLonLat(double lon, double lat, out Point xy);
        /// <summary>
        /// Convert geo coordinates into pixel coordinates for the given zoom level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zoom"></param>
        /// <param name="xy"></param>
        public abstract void getXYByLonLat(double lon, double lat, int zoom, out Point xy);
        /// <summary>
        /// Return tiles xy numbers from the given lon, lat and zooming level
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zoom_lvl"></param>
        /// <param name="xy"></param>
        public abstract void getNXNYByLonLat(double lon, double lat, int zoom_lvl, out Point xy);

        /// <summary>
        /// Get/Set current zoom level
        /// </summary>
        public abstract int zoomLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Creates and returns a copy of itself
        /// </summary>
        /// <returns></returns>
        public abstract BaseGeo copy();

        /// <summary>
        /// Gets the traffic system for this geo system
        /// </summary>
        public abstract BaseTraffic trafficSystem
        {
            get;
        }

        /// <summary>
        /// Return base type of the map or lets say the type of the underlying coordinate system.
        /// Many map types uses the same coordinate geo system, so we return base (first) type.
        /// </summary>
        public abstract MapTileType baseType
        {
            get;
        }
    }
}
