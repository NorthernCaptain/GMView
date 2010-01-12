using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Class implements sorter for sorting POI grid on different columns
    /// </summary>
    public class POIGridSorter: IComparer
    {
        private string field;
        private SortOrder order;


        public POIGridSorter(string ifield, SortOrder iorder)
        {
            field = ifield;
            order = iorder;
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            if(x is POIGroup)
            {
                if(y is POIGroup)
                {
                    return ((POIGroup)x).Name.CompareTo(((POIGroup)y).Name);
                }
                else
                    return -1;
            } else
            {
                if(y is POIGroup)
                    return 1;
            }

            Bookmark poix = x as Bookmark;
            Bookmark poiy = y as Bookmark;

            if(poix == null || poiy == null)
                return 0;

            int ret = 0;
            switch (field)
            {
                case "Name":
                    ret = poix.Name.CompareTo(poiy.Name);
                    break;
                case "Description":
                    ret = poix.Description.CompareTo(poiy.Description);
                    break;
                case "Longitude":
                    ret = poix.longitude.CompareTo(poiy.longitude);
                    break;
                case "Latitude":
                    ret = poix.latitude.CompareTo(poiy.latitude);
                    break;
                case "Date":
                    ret = poix.Created.CompareTo(poiy.Created);
                    break;
                case "Type":
                    ret = poix.PtypeS.CompareTo(poiy.PtypeS);
                    break;
                default:
                    ret = poix.Name.CompareTo(poiy.Name);
                    break;
            }

            if(ret == 0)
                return poix.Name.CompareTo(poiy.Name);

            return (order == SortOrder.Descending ? -ret : ret);
        }

        #endregion
    }
}
