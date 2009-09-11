using System;
using System.Collections.Generic;
using System.Text;
using ncGeo;

namespace GMView
{
    public class PositionStack
    {
        public struct PositionInfo
        {
            public double lon, lat;
            public int zoom;
            public MapTileType mType;

            public PositionInfo(double ilon, double ilat, int izoom, MapTileType imType)
            {
                lon = ilon;
                lat = ilat;
                zoom = izoom;
                mType = imType;
            }
        }
         
        LinkedList<PositionInfo> pstack = new LinkedList<PositionInfo>();
        int max_positions = 30;

        public PositionStack(int imax)
        {
            max_positions = imax;
        }

        public void push(PositionInfo pi)
        {
            pstack.AddFirst(pi);
            if (pstack.Count > max_positions)
                pstack.RemoveLast();
        }

        public PositionInfo pop()
        {
            PositionInfo pi = pstack.First.Value;
            pstack.RemoveFirst();
            return pi;
        }

        public bool empty()
        {
            return pstack.Count == 0;
        }
    }
}
