using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Data.Common;
using ncGeo;
using ncUtils;
using System.Data;
using GMView.Bookmarks;

namespace GMView
{
    public class Bookmark: ISprite, ncGeo.IGeoCoord, Bookmarks.IPOIBase
    {
        [XmlIgnore]
        public string sid;
        [XmlAttribute]
        public int image_idx = 0;
        [XmlAttribute]
        public MapTileType original_map_type = MapTileType.MapOnly;
        [XmlAttribute("group_name")]
        public string group = "";

        /// <summary>
        /// Unique id for this POI, from DB
        /// </summary>
        private int id = 0;

        [XmlIgnore]
        public int Id
        {
            get { return id; }
        }

        [XmlAttribute]
        public double lon, lat;
        [XmlIgnore]
        public double alt = 0.0;
        private string name;

        /// <summary>
        /// Short POI name. On set update DB
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set 
            { 
                if ( (name != null && id > 3) || name == null) 
                { 
                    name = value; 
                    updateDB(); 
                } 
            }
        }

        private string description;
        [XmlIgnore]
        public string Description
        {
            get { return description; }
            set { description = value; updateDB(); }
        }

        private string comment;

        [XmlElement("comment")]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        private int original_zoom = 10;

        [XmlAttribute("original_zoom")]
        public int Original_zoom
        {
            get { return original_zoom; }
            set { original_zoom = value; }
        }

        private DateTime created = DateTime.Now;

        [XmlAttribute("created")]
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        [XmlIgnore]
        public string CreatedS
        {
            get { return created.ToShortDateString() + " " + created.ToShortTimeString(); }
            set {}
        }

        /// <summary>
        /// Type of the POI
        /// </summary>
        private Bookmarks.POIType ptype = null;

        [XmlIgnore]
        public Bookmarks.POIType Ptype
        {
            get { return ptype; }
            set { if (value != null) { qchangeType(value); updateDB(); }; }
        }


        [XmlIgnore]
        public string PtypeS
        {
            get { return ptype.Name; }
            set 
            {
                Bookmarks.POIType pt = Bookmarks.POITypeFactory.singleton().typeByName(value);
                if (pt != null)
                {
                    qchangeType(pt);
                }
            }
        }

        /// <summary>
        /// Icon information for this POI
        /// </summary>
        private IIconInfo iconfo;

        [XmlIgnore]
        public IIconInfo Iconfo
        {
            get { return iconfo; }
            set { iconfo = value; }
        }


        [XmlIgnore]
        public bool shown = false;

        [XmlIgnore]
        public bool is_temporary = false; 

        int x, y;
        ImageDot imd = null;
        object tex = null;
        IGLFont fnt = null;

        private BookMarkFactory owner = null;

        [XmlIgnore]
        public BookMarkFactory Owner
        {
            get { return owner; }
            set { owner = value; Mapo = owner.map; }
        }

        private MapObject mapo;

        /// <summary>
        /// Map object that POI assigned to. We need it for drawing
        /// </summary>
        [XmlIgnore]
        public MapObject Mapo
        {
            get { return mapo; }
            set { mapo = value; }
        }

        /// <summary>
        /// Bitmask for POI flags (options)
        /// </summary>
        private int flags = 0;

        [XmlIgnore]
        public int Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        private bool isDbChange = true;

        /// <summary>
        /// Is changes to DB allowed?
        /// </summary>
        public bool IsDbChange
        {
            get { return isDbChange; }
            set { isDbChange = value; }
        }


        private bool isAutoShow = true;

        /// <summary>
        /// Is this POI in auto show mode or not
        /// </summary>
        public bool IsAutoShow
        {
            get { return isAutoShow; }
            set 
            { 
                isAutoShow = value;
            }
        }

        /// <summary>
        /// Icon image for display in dialogs
        /// </summary>
        protected Image icon;
        [XmlIgnore]
        public Image IconImage
        {
            get { return icon; }
            set { }
        }

        [XmlIgnore]
        public bool IsShown
        {
            get { return shown; }
            set 
            {
                if (value == shown)
                    return;

                if(value)
                {
                    mapo.addSub(this);
                    show();
                    owner.POIShown(this);
                } else
                {
                    hide();
                    mapo.delSub(this);
                    owner.POIHidden(this);
                }
                GML.repaint();
                shown = value;
            }
        }

        [XmlIgnore]
        public bool IsShownCentered
        {
            get { return shown; }
            set
            {
                if (value != shown && value)
                    mapo.CenterMapLonLat(lon, lat);
                IsShown = value;
                IsAutoShow = !value;
            }
        }


        private bool isDisabled = false;

        /// <summary>
        /// Is this POI disabled or not
        /// </summary>
        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; updateDB(); }
        }

        public Bookmark()
        {
            ptype = Bookmarks.POITypeFactory.singleton().typeByName("unknown");
            iconfo = ptype;
        }

        /// <summary>
        /// Constructor for quick creation of the POI, only by given coordinates and type
        /// </summary>
        /// <param name="ilon"></param>
        /// <param name="ilat"></param>
        /// <param name="ialt"></param>
        /// <param name="itype"></param>
        public Bookmark(double ilon, double ilat, double ialt, Bookmarks.POIType itype)
        {
            lon = ilon;
            lat = ilat;
            alt = ialt;

            name = "??";
            qchangeType(itype);
        }

        /// <summary>
        /// Quick POI constructor only with coordinates. Type will be set to unknown. Use qchangeType method later
        /// </summary>
        /// <param name="ilon"></param>
        /// <param name="ilat"></param>
        /// <param name="ialt"></param>
        public Bookmark(double ilon, double ilat, double ialt)
        {
            iconfo = ptype;
            lon = ilon;
            lat = ilat;
            alt = ialt;
            name = "??";
            qchangeType(Bookmarks.POITypeFactory.singleton().typeByName("unknown"));
        }

        /// <summary>
        /// Create a bookmark by reading data from opened DB cursor.
        /// Cursor must provide the following fields:
        ///    id, name, description, type, comments,
        ///    lon, lat, alt, flags
        /// </summary>
        /// <param name="reader"></param>
        public Bookmark(DbDataReader reader)
        {
            id = reader.GetInt32(reader.GetOrdinal("ID"));
            name = reader.GetString(reader.GetOrdinal("NAME"));
            {
                int idx = reader.GetOrdinal("DESCRIPTION");
                if(!reader.IsDBNull(idx))
                    description = reader.GetString(idx);
            }
            {
                int idx = reader.GetOrdinal("COMMENTS");
                if (!reader.IsDBNull(idx))
                    comment = reader.GetString(idx);
            }

            int type_id = reader.GetInt32(reader.GetOrdinal("TYPE"));
            ptype = Bookmarks.POITypeFactory.singleton().typeById(type_id);
            iconfo = ptype;
            icon = IconFactory.singleton.getIcon(iconfo).img;
            lon = reader.GetDouble(reader.GetOrdinal("LON"));
            lat = reader.GetDouble(reader.GetOrdinal("LAT"));
            alt = reader.GetDouble(reader.GetOrdinal("ALT"));
            flags = reader.GetInt32(reader.GetOrdinal("FLAGS"));
            created = reader.GetDateTime(reader.GetOrdinal("CREATED"));
            isDisabled = (reader.GetInt32(reader.GetOrdinal("IS_DISABLED")) == 1);
        }

        /// <summary>
        /// Quick change type of POI. Also changes the name of the POI and description
        /// </summary>
        /// <param name="newType"></param>
        public void qchangeType(Bookmarks.POIType newType)
        {
            ptype = newType;
            iconfo = ptype;
            icon = IconFactory.singleton.getIcon(iconfo).img; 
            if (tex != null)
                initGLData();
            if(name[0] == '?')
            {
                name = "??" + ptype.Name + "??";
                description = ptype.Text + "??";
                comment = "quickly added, no name supplied";
                is_temporary = true;
            }
        }

        /// <summary>
        /// Updates or inserts POI in the DB poi and poi_spartial tables
        /// </summary>
        public void updateDB()
        {
            if (!isDbChange)
                return;

            if(id == 0)
            {
                //Do insert statement (new record)
                DBObj dbo = null;

                try
                {
                    dbo = new DBObj("insert into poi (name, description, type, comments, "
                                    + "lon, lat, alt, flags, icon, icon_cx, icon_cy, created, is_disabled) "
                                    + "values (@NAME, @DESCRIPTION, @TYPE, @COMMENTS, "
                                    + "@LON, @LAT, @ALT, @FLAGS, @ICON, @ICON_CX, @ICON_CY, @CREATED, "
                                    + "@IS_DISABLED)");

                    dbo.addStringPar("@NAME", name);
                    dbo.addStringPar("@DESCRIPTION", description);
                    dbo.addIntPar("@TYPE", ptype.Id);
                    dbo.addStringPar("@COMMENTS", comment);
                    dbo.addFloatPar("@LON", lon);
                    dbo.addFloatPar("@LAT", lat);
                    dbo.addFloatPar("@ALT", alt);
                    dbo.addIntPar("@FLAGS", flags);
                    dbo.addStringPar("@ICON", iconfo.iconName);
                    dbo.addIntPar("@ICON_CX", iconfo.iconDeltaX);
                    dbo.addIntPar("@ICON_CY", iconfo.iconDeltaY);
                    dbo.addPar("@CREATED", DbType.DateTime, created);
                    dbo.addIntPar("@IS_DISABLED", (isDisabled ? 1 : 0));

                    dbo.executeNonQuery();

                    id = dbo.seqCurval("poi");
                    
                    dbo.commandText = "insert into poi_spatial (id, minLon, maxLon, minLat, maxLat) "
                        + "values (@ID, @MINLON, @MAXLON, @MINLAT, @MAXLAT)";
                    dbo.addIntPar("@ID", id);
                    dbo.addFloatPar("@MINLON", lon);
                    dbo.addFloatPar("@MAXLON", lon);
                    dbo.addFloatPar("@MINLAT", lat);
                    dbo.addFloatPar("@MAXLAT", lat);
                    dbo.executeNonQuery();
                    
                    is_temporary = false;
                }
                catch (System.Exception ex)
                {
                    Program.Log("SQLError: " + ex.ToString());
                }
                finally
                {
                    if (dbo != null)
                        dbo.Dispose();
                }
            } else
            {
                //Do real update statement.
                DBObj dbo = null;

                try
                {
                    dbo = new DBObj("update poi set name=@NAME, description=@DESCRIPTION,"
                                    + "type=@TYPE, comments=@COMMENTS, is_disabled=@IS_DISABLED, "
                                    + "lon=@LON, lat=@LAT, alt=@ALT, flags=@FLAGS, "
                                    + "icon=@ICON, icon_cx=@ICON_CX, icon_cy=@ICON_CY "
                                    + "where id=@ID");

                    dbo.addStringPar("@NAME", name);
                    dbo.addStringPar("@DESCRIPTION", description);
                    dbo.addIntPar("@TYPE", ptype.Id);
                    dbo.addStringPar("@COMMENTS", comment);
                    dbo.addFloatPar("@LON", lon);
                    dbo.addFloatPar("@LAT", lat);
                    dbo.addFloatPar("@ALT", alt);
                    dbo.addIntPar("@FLAGS", flags);
                    dbo.addStringPar("@ICON", iconfo.iconName);
                    dbo.addIntPar("@ICON_CX", iconfo.iconDeltaX);
                    dbo.addIntPar("@ICON_CY", iconfo.iconDeltaY);
                    dbo.addIntPar("@IS_DISABLED", (isDisabled ? 1 : 0));

                    dbo.addIntPar("@ID", id);

                    dbo.executeNonQuery();
                    
                    dbo.commandText = "update poi_spatial set minLon=@MINLON, maxLon=@MAXLON, "
                            + "minLat=@MINLAT, maxLat=@MAXLAT where id=@ID";
                    dbo.addIntPar("@ID", id);
                    dbo.addFloatPar("@MINLON", lon);
                    dbo.addFloatPar("@MAXLON", lon);
                    dbo.addFloatPar("@MINLAT", lat);
                    dbo.addFloatPar("@MAXLAT", lat);
                    dbo.executeNonQuery();
                     
                    is_temporary = false;
                }
                catch (System.Exception ex)
                {
                    Program.Log("SQLError: " + ex.ToString());
                }
                finally
                {
                    if (dbo != null)
                        dbo.Dispose();
                }
            }

        }

        /// <summary>
        /// Insert group member link for this poi into the given group
        /// </summary>
        /// <param name="parentGroup"></param>
        public void addLinkDB(Bookmarks.POIGroup parentGroup)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"insert into poi_group_member (parent_id, member_id) values (@PARENT_ID, @MEMBER_ID)");
                dbo.addIntPar("@PARENT_ID", parentGroup.Id);
                dbo.addIntPar("@MEMBER_ID", id);
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

        public void initGLData()
        {
            if(iconfo != null)
                imd = IconFactory.singleton.getIcon(iconfo);
            else
                imd = TextureFactory.singleton.getImg((TextureFactory.TexAlias)((int)TextureFactory.TexAlias.PinYellow + image_idx));
            tex = TextureFactory.singleton.getTex(imd);
            fnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Sans10B);
        }

        public void makeId()
        {
            if (group.Length == 0)
                sid = name;
            else
                sid = "___/" + group + "/" + name;
        }

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
            
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void glDraw(int centerx, int centery)
        {
            if (!shown)
                return;
            Point xy = mapo.start_real_xy;
            xy.X = x - xy.X;
            xy.Y = y - xy.Y;

            if (xy.Y < 0 || xy.X < 0)
                return;

            xy.X -= centerx;
            xy.Y = centery - xy.Y;

            GML.device.pushMatrix();
            GML.device.color(Color.White);
            GML.device.translate(xy.X, xy.Y, 0);
            GML.device.rotateZ(-mapo.angle);
            GML.device.texDrawBegin();
            GML.device.texFilter(tex, TexFilter.Pixel);
            GML.device.texDraw(tex, -imd.delta_x,
                imd.delta_y, dLevel, imd.img.Width, imd.img.Height);
            GML.device.texDrawEnd();
            GML.device.color(Color.Black);
//            xy = GML.translateToScene(xy);
            fnt.drawscene(name, xy.X, xy.Y, 0);
            GML.device.color(Color.White);
            GML.device.popMatrix();
        }

        [XmlIgnore]
        public int dLevel
        {
            get
            {
                return 1;
            }
            set
            {
                
            }
        }

        public void show()
        {
            shown = true;
            if (tex == null)
                initGLData();
            calculateXY(mapo);
        }

        public void hide()
        {
            shown = false;
        }

        #endregion

        public void calculateXY(MapObject mapo)
        {
            Point xy;
            mapo.getXYByLonLat(lon, lat, out xy);
            x = xy.X;
            y = xy.Y;
        }

        #region IGeoCoord Members

        [XmlIgnore]
        public double longitude
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
            }
        }

        /// <summary>
        /// Return string representation of the longitude
        /// </summary>
        [XmlIgnore]
        public string LongitudeS
        {
            get { return ncUtils.Glob.lonString(lon); }
            set { lon = ncUtils.Glob.parseLon(value); updateDB(); }
        }

        [XmlIgnore]
        public double latitude
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }

        /// <summary>
        /// Return string representation of the latitude
        /// </summary>
        [XmlIgnore]
        public string LatitudeS
        {
            get { return ncUtils.Glob.latString(lat); }
            set { lat = ncUtils.Glob.parseLat(value); updateDB(); }
        }

        /// <summary>
        /// We do not use altitude in bookmarks, it's wrong, but ....
        /// </summary>
        [XmlIgnore]
        public double altitude
        {
            get
            {
                return alt;
            }
            set
            {
                alt = value;
                updateDB();
            }
        }

        #endregion

        public override string ToString()
        {
            return sid;
        }

        #region IPOIBase Members

        [XmlIgnore]
        public bool IsGroup
        {
            get { return false; }
        }

        public void addChild(GMView.Bookmarks.IPOIBase child)
        {
        }

        public void addChildAfter(IPOIBase after, IPOIBase child)
        {
        }

        public void delChild(GMView.Bookmarks.IPOIBase child)
        {
        }

        public void reparentMeTo(IPOIBase parentGroup, IPOIBase after)
        {
            if (parentGroup == null)
                return;

            POIGroup oldParent = parent;
            if (oldParent != null)
                oldParent.delChild(this);
            parentGroup.addChildAfter(after, this);

            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"update poi_group_member set parent_id=@NEWPARENT where "
                      + "parent_id=@OLDPARENT and member_id=@CURRENT");
                dbo.addIntPar("@NEWPARENT", parentGroup.Id);
                dbo.addIntPar("@OLDPARENT", oldParent.Id);
                dbo.addIntPar("@CURRENT", id);
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

        /// <summary>
        /// Deletes POI from DB.
        /// </summary>
        public void deleteFromDB()
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"delete from poi_spatial where id=@ID");
                dbo.addIntPar("@ID", id);
                dbo.executeNonQuery();

                dbo.commandText = "delete from poi_group_member where member_id=@ID";
                dbo.addIntPar("@ID", id);
                dbo.executeNonQuery();

                dbo.commandText = "delete from poi where id=@ID";
                dbo.addIntPar("@ID", id);
                dbo.executeNonQuery();

                unregisterMe();
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

        /// <summary>
        /// Unregisters from the owner (factory).
        /// </summary>
        public void unregisterMe()
        {
            IsShown = false;
            if(parent != null)
                parent.delChild(this);
            owner.unregister(this);
        }

        private POIGroup parent;

        [XmlIgnore]
        public IPOIBase Parent
        {
            get { return parent; }
            set { parent = value as POIGroup; }
        }

        #endregion
    }
}
