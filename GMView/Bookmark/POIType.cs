using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ncUtils;
using System.Data.Common;

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

        public int Id
        {
            get { return id; }
        }
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
                updateDB();
            }
        }

        public string Name
        {
            get { return shortName; }
            set { shortName = value; updateDB();}
        }

        private bool isAutoShow = false;

        /// <summary>
        /// Is this type can be used in Auto show POI mode
        /// </summary>
        public bool IsAutoShow
        {
            get { return isAutoShow; }
            set { isAutoShow = value; updateDB(); }
        }

        private bool isQuickType = true;

        /// <summary>
        /// Can this type be used in quick type list widget
        /// </summary>
        public bool IsQuickType
        {
            get { return isQuickType; }
            set { isQuickType = value; updateDB(); }
        }

        private int minZoomLvl = 10;

        /// <summary>
        /// Minimum zoom lvl for this type to show POIs on the screen
        /// </summary>
        public int MinZoomLvl
        {
            get { return minZoomLvl; }
            set { minZoomLvl = value; updateDB(); }
        }

        private int flags;

        /// <summary>
        /// Flags for this POI type
        /// </summary>
        public int Flags
        {
            get { return flags; }
            set { flags = value; }
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
            id = reader.GetInt32(reader.GetOrdinal("ID"));
            shortName = reader.GetString(reader.GetOrdinal("NAME"));
            textName = reader.GetString(reader.GetOrdinal("DESCRIPTION"));
            iconS = reader.GetString(reader.GetOrdinal("ICON"));
            icon_dx = reader.GetInt32(reader.GetOrdinal("ICON_CX"));
            icon_dy = reader.GetInt32(reader.GetOrdinal("ICON_CY"));
            isAutoShow = reader.GetInt32(reader.GetOrdinal("IS_AUTO_SHOW")) > 0;
            isQuickType = reader.GetInt32(reader.GetOrdinal("IS_QUICK_TYPE")) > 0;
            minZoomLvl = reader.GetInt32(reader.GetOrdinal("MIN_ZOOM_LVL"));
            flags = reader.GetInt32(reader.GetOrdinal("FLAGS"));
        }

        /// <summary>
        /// Return long name as a string representation of the type
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return textName;
        }

        /// <summary>
        /// Inserts or update poi type table with current data
        /// </summary>
        public void updateDB()
        {
            //new type, insert it into DB
            if (id == 0)
            {
                DBObj dbo = null;
                try
                {
                    dbo = new DBObj(@"insert into poi_type (name, description, icon, icon_cx, "
                                + "icon_cy, min_zoom_lvl, is_auto_show, is_quick_type) "
                                + "values (@NAME,@DESCR, @ICON, @ICON_CX, @ICON_CY, "
                                + "@MINZOOM, @ISAUTOSHOW, @ISQUICK)");
                    dbo.addStringPar("@NAME", shortName);
                    dbo.addStringPar("@DESCR", textName);
                    dbo.addStringPar("@ICON", iconS);
                    dbo.addIntPar("@ICON_CX", icon_dx);
                    dbo.addIntPar("@ICON_CY", icon_dy);
                    dbo.addIntPar("@MINZOOM", minZoomLvl);
                    dbo.addIntPar("@ISAUTOSHOW", (isAutoShow ? 1 : 0));
                    dbo.addIntPar("@ISQUICK", (isQuickType ? 1 : 0));

                    dbo.executeNonQuery();

                    id = dbo.seqCurval("poi_type");
                }
                catch (System.Exception e)
                {
                    Program.Log("SQLError: " + e.ToString());

                }
                finally
                {
                    if (dbo != null)
                        dbo.Dispose();
                }
            } else
            {
                DBObj dbo = null;
                try
                {
                    dbo = new DBObj(@"update poi_type set name=@NAME, description=@DESCR, "
                                + "icon=@ICON, icon_cx=@ICON_CX, min_zoom_lvl=@MINZOOM, "
                                + "is_auto_show=@ISAUTOSHOW, is_quick_type=@ISQUICK "
                                + "where id=@ID");
                    dbo.addStringPar("@NAME", shortName);
                    dbo.addStringPar("@DESCR", textName);
                    dbo.addStringPar("@ICON", iconS);
                    dbo.addIntPar("@ICON_CX", icon_dx);
                    dbo.addIntPar("@ICON_CY", icon_dy);
                    dbo.addIntPar("@MINZOOM", minZoomLvl);
                    dbo.addIntPar("@ISAUTOSHOW", (isAutoShow ? 1 : 0));
                    dbo.addIntPar("@ISQUICK", (isQuickType ? 1 : 0));
                    dbo.addIntPar("@ID", id);

                    dbo.executeNonQuery();
                }
                catch (System.Exception e)
                {
                    Program.Log("SQLError: " + e.ToString());

                }
                finally
                {
                    if (dbo != null)
                        dbo.Dispose();
                }

            }
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

        public Image IconImg
        {
            get { return IconFactory.singleton.getIcon(this).img; }
            set {}
        }
    }
}
