using System;
using System.Collections.Generic;
using System.Text;

namespace ncGeo
{
    public interface IFindPoint: ICloneable, IComparable
    {
        /// <summary>
        /// Starts searching for the point of the given track
        /// </summary>
        /// <param name="track"></param>
        void findStart(IGPSTrack track, LinkedListNode<NMEA_LL> first);

        /// <summary>
        /// Next point for analysing search criteria on it
        /// </summary>
        /// <param name="pointNode"></param>
        void checkPoint(LinkedListNode<NMEA_LL> pointNode);

        /// <summary>
        /// Stop the search and form the result
        /// </summary>
        void findFinish();


        /// <summary>
        /// Reset the result to null
        /// </summary>
        void reset();
        /// <summary>
        /// Return result point of the search or null if not found
        /// </summary>
        LinkedListNode<NMEA_LL> resultPoint
        {
            get;
        }

        /// <summary>
        /// Return the track used for search
        /// </summary>
        IGPSTrack track
        {
            get;
        }
    }
}
