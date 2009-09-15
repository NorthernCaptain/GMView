using System;
using System.Collections.Generic;
using System.Text;
using ncGeo;

namespace GMView.GPS
{
    public class FindNearestPointByDistance : IFindPoint
    {
        /// <summary>
        /// point on the surface - we need to find track point nearest to the given coords.
        /// </summary>
        public double lon;
        public double lat;

        /// <summary>
        /// distance from requested point
        /// </summary>
        public double distance;

        //results:
        private LinkedListNode<NMEA_LL> nearest;
        private GPSTrack gtrack;

        public FindNearestPointByDistance(double ilon, double ilat)
        {
            init(ilon, ilat);
        }

        public FindNearestPointByDistance()
        {
            init(0, 0);
        }

        public void init(double ilon, double ilat)
        {
            lon = ilon;
            lat = ilat;
            nearest = null;
            distance = 0.0;
            gtrack = null;
        }

        #region IFindPoint Members

        public void findStart(GPSTrack track, LinkedListNode<NMEA_LL> first)
        {
            gtrack = track;
            distance = CommonGeo.getDistanceByLonLat2(lon, lat,
                                            first.Value.lon,
                                            first.Value.lat);
            nearest = first;
        }

        public void checkPoint(LinkedListNode<NMEA_LL> pointNode)
        {
            double dist = CommonGeo.getDistanceByLonLat2(lon, lat,
                            pointNode.Value.lon,
                            pointNode.Value.lat);
            if (distance > dist)
            {
                distance = dist;
                nearest = pointNode;
            }

        }

        public void findFinish()
        {
        }

        public void reset()
        {
            nearest = null;
        }

        public LinkedListNode<NMEA_LL> resultPoint
        {
            get { return nearest; }
        }

        public GPSTrack track
        {
            get { return gtrack; }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            FindNearestPointByDistance clone = new FindNearestPointByDistance(lon, lat);
            clone.gtrack = gtrack;
            clone.nearest = nearest;
            clone.distance = distance;
            return clone;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            FindNearestPointByDistance second = obj as FindNearestPointByDistance;
            if (second == null)
                throw new ArgumentException("Argument is not a FindNearestPointByDistance");

            if(ncGeo.CommonGeo.almostEqual(distance, second.distance))
                return 0;
            return distance < second.distance ? -1 : 1;
        }

        #endregion
    }
}
