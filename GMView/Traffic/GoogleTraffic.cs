using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Google traffic system
    /// </summary>
    public class GoogleTraffic: BaseTraffic
    {
        public GoogleTraffic()
        {
            trafficAreas = new Rectangle[1];
            trafficGeoAreas = new RectangleF[1];
            trafficGeoAreas[0] = new RectangleF(-125.41f, 49.15f, -61.69f, 25.33f);
        }

        public override string getTimetInfo(System.Net.WebClient requestFrom)
        {
            return "-";
        }

        public override MapTileType trafficTileType
        {
            get { return MapTileType.GooTraffic; }
        }
    }
}
