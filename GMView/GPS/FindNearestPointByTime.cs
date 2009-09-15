using System;
using System.Collections.Generic;
using System.Text;
using ncGeo;

namespace GMView.GPS
{
    public class FindNearestPointByTime: IFindPoint
    {
        //results:
        private LinkedListNode<NMEA_LL> nearest;
        private GPSTrack gtrack;
        private DateTime fromDate;

        /// <summary>
        /// Result time span between nearest point on the track and our date
        /// </summary>
        public TimeSpan timeSpan;

        /// <summary>
        /// Constructor of the context with the given date
        /// </summary>
        /// <param name="from"></param>
        public FindNearestPointByTime(DateTime from)
        {
            init(from);
        }

        /// <summary>
        /// Initialize finding context
        /// </summary>
        /// <param name="from"></param>
        public void init(DateTime from)
        {
            fromDate = from;
            nearest = null;
            timeSpan = new TimeSpan();
            gtrack = null;
        }

        #region IFindPoint Members

        public void findStart(GPSTrack track, LinkedListNode<NMEA_LL> first)
        {
            gtrack = track;
            nearest = first;
            timeSpan = first.Value.utc_time - fromDate;
        }

        public void checkPoint(LinkedListNode<NMEA_LL> pointNode)
        {
            TimeSpan ts = pointNode.Value.utc_time - fromDate;
            if (Math.Abs(ts.Ticks) < Math.Abs(timeSpan.Ticks))
            {
                timeSpan = ts;
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
            FindNearestPointByTime clone = new FindNearestPointByTime(fromDate);
            clone.gtrack = gtrack;
            clone.nearest = nearest;
            clone.timeSpan = timeSpan;
            return clone;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            FindNearestPointByTime second = obj as FindNearestPointByTime;
            if (second == null)
                throw new ArgumentException("Argument is not a FindNearestPointByTime");

            return timeSpan.CompareTo(second.timeSpan);
        }

        #endregion
    }
}
