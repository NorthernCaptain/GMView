using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Type of the POI object
    /// </summary>
    public class POIType: IIconInfo
    {
        /// <summary>
        /// ID of this type in DB
        /// </summary>
        int id = 0;
        /// <summary>
        /// Full type name, shown to the user
        /// </summary>
        string textName;
        /// <summary>
        /// Icon name from our iconset
        /// </summary>
        string iconS;
        /// <summary>
        /// Short type name, used for definitions in files
        /// </summary>
        string shortName;
        /// <summary>
        /// Icon hot point delta x
        /// </summary>
        int icon_dx;
        /// <summary>
        /// Icon hot point delta y
        /// </summary>
        int icon_dy;

        public string Text
        {
            get
            {
                return textName;
            }
            set
            {
                textName = value;
            }
        }

        public POIType() { textName = string.Empty; }

        /// <summary>
        /// Constructor reads data from DB opened cursor (table poi_type)
        /// Fields must be in the following order:
        ///   id, name, description, icon, icon_dx, icon_dy
        /// </summary>
        /// <param name="reader"></param>
        public POIType(System.Data.Common.DbDataReader reader)
        {
            id = reader.GetInt32(0);
            shortName = reader.GetString(1);
            textName = reader.GetString(2);
            iconS = reader.GetString(3);
            icon_dx = reader.GetInt32(4);
            icon_dy = reader.GetInt32(5);
        }

        public override string ToString()
        {
            return textName;
        }


        #region IIconInfo Members

        public string iconName
        {
            get { return iconS; }
        }

        public int iconDeltaX
        {
            get { return icon_dx; }
        }

        public int iconDeltaY
        {
            get { return icon_dy; }
        }

        #endregion
    }
}
