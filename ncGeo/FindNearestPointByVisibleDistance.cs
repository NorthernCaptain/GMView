﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ncGeo
{
    /// <summary>
    /// Context for finding nearest point by distance, but we search only visible points,
    /// i.e those having draw_idx >=0
    /// </summary>
    public class FindNearestPointByVisibleDistance: IFindPoint
    {
        /// <summary>
        /// point on the surface - we need to find track point nearest to the given coords.
        /// </summary>
        public double lon;
        public double lat;

        /// <summary>
        /// distance from requested point to the found one. Distance is in km.
        /// </summary>
        public double distance;

        //results:
        private LinkedListNode<NMEA_LL> nearest;
        private IGPSTrack gtrack;

        public FindNearestPointByVisibleDistance(double ilon, double ilat)
        {
            init(ilon, ilat);
        }

        public FindNearestPointByVisibleDistance()
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

        public void findStart(IGPSTrack track, LinkedListNode<NMEA_LL> first)
        {
            gtrack = track;
            distance = CommonGeo.getDistanceByLonLat2(lon, lat,
                                            first.Value.lon,
                                            first.Value.lat);
            nearest = first;
        }

        public void checkPoint(LinkedListNode<NMEA_LL> pointNode)
        {
            if (pointNode.Value.draw_idx < 0)
                return;

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
        /// <summary>
        /// Result of the search, i.e nearest point on the track or null if not found
        /// </summary>
        public LinkedListNode<NMEA_LL> resultPoint
        {
            get { return nearest; }
        }

        public IGPSTrack track
        {
            get { return gtrack; }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            FindNearestPointByVisibleDistance clone = new FindNearestPointByVisibleDistance(lon, lat);
            clone.gtrack = gtrack;
            clone.nearest = nearest;
            clone.distance = distance;
            return clone;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            FindNearestPointByVisibleDistance second = obj as FindNearestPointByVisibleDistance;
            if (second == null)
                throw new ArgumentException("Argument is not a FindNearestPointByDistance");

            if(ncGeo.CommonGeo.almostEqual(distance, second.distance))
                return 0;
            return distance < second.distance ? -1 : 1;
        }

        #endregion
    }
}
