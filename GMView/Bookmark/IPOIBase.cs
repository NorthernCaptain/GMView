using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Base interface for POI and for the groups of POI.
    /// </summary>
    public interface IPOIBase
    {
        /// <summary>
        /// Unique id of the group, filled when inserting group into DB
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// Group name
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Group long description
        /// </summary>
        string Description
        {
            get;
            set;
        }


        Image IconImage
        {
            get;
            set;
        }

        /// <summary>
        /// If we set this property to true, then we show all POIs in this group,
        /// if to false, then we hide all POIs
        /// </summary>
        bool IsShown
        {
            get;
            set;
        }

        /// <summary>
        /// Parent group of this POI base
        /// </summary>
        IPOIBase Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Is this a group or plain POI
        /// </summary>
        bool IsGroup
        {
            get;
        }

        /// <summary>
        /// Update or insert group data into DB table
        /// </summary>
        void updateDB();

        /// <summary>
        /// Adds child group to the group, create all necessary links
        /// </summary>
        /// <param name="child"></param>
        void addChild(IPOIBase child);

        /// <summary>
        /// Remove child from this parent
        /// </summary>
        /// <param name="child"></param>
        void delChild(IPOIBase child);

        /// <summary>
        /// Reparent group to another parent. Updates DB.
        /// </summary>
        /// <param name="parentGroup"></param>
        void reparentMeTo(IPOIBase parent);

    }
}
