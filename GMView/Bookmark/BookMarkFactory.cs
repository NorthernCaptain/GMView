using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Data.Common;
using ncGeo;
using ncUtils;

namespace GMView
{
    [XmlRoot("BookmarkList")]
    public class BookMarkFactory
    {
        /// <summary>
        /// Instance of bookmark factory
        /// </summary>
        private static volatile BookMarkFactory instance = null;

        /// <summary>
        /// Bookmark list stored by name
        /// </summary>
        private SortedDictionary<string, Bookmark> marks = new SortedDictionary<string, Bookmark>();
        private MapObject mapo;

        public delegate void BookmarkFactoryChangedDelegate(BookMarkFactory factory);
        /// <summary>
        /// Event fired if bookmarks were changed in the factory
        /// </summary>
        public event BookmarkFactoryChangedDelegate onChanged;

        private readonly object locker = new object();

        #region Get/Set methods + XML serialization

        private Bookmarks.POIGroupFactory groupFactory;

        /// <summary>
        /// POI group factory associated with this POI factory
        /// </summary>
        [XmlIgnore]
        public Bookmarks.POIGroupFactory GroupFactory
        {
            get { return groupFactory; }
            set { groupFactory = value; }
        }

        /// <summary>
        /// Return a list of bookmarks, i.e not a list but sorted dictionary
        /// </summary>
        [XmlIgnore]
        public SortedDictionary<string, Bookmark> bookmarks
        {
            get { return marks; }
        }

        /// <summary>
        /// Return instance of the factory
        /// </summary>
        [XmlIgnore]
        public static BookMarkFactory singleton
        {
            get
            {
                if (instance == null)
                    loadXml();
                return instance;
            }
        }

        /// <summary>
        /// Factory constructor.
        /// </summary>
        public BookMarkFactory()
        {
            Bookmarks.POITypeFactory.singleton();
            groupFactory = Bookmarks.POIGroupFactory.singleton();
        }

        [XmlArray("bookmarks"),
          XmlArrayItem("bmark", typeof(Bookmark))]
        public Bookmark[] bmarks
        {
            get 
            {
                List<Bookmark> blist = new List<Bookmark>();
                foreach( KeyValuePair<string, Bookmark> pair in marks)
                {
                    if(!pair.Value.is_temporary)
                        blist.Add(pair.Value);
                }

                return blist.ToArray();
            }
            set 
            {
                for (int i = 0; i < value.Length; i++)
                {
                    value[i].makeId();
                    marks.Add(value[i].sid, value[i]);
                    value[i].Owner = this;
                }
            }
        }

        [XmlAttribute]
        public string version
        {
            get { return "1.0"; }
            set { }
        }

        [XmlIgnore]
        public MapObject map
        {
            get { return mapo; }
            set { mapo = value; mapo.onZoomChanged += updateOnZoomChange; }
        }

        public static string globalFilename
        {
            get { return Path.Combine(Options.getConfigDir(), "bookmarks.xml"); }
        }

        public static BookMarkFactory loadXml()
        {
            BookMarkFactory factory = loadXml(globalFilename);
            if (factory != null)
                instance = factory;
            else
                instance = new BookMarkFactory();
            return instance;
        }

        public static BookMarkFactory loadXml(string fname)
        {
            return null;

            FileStream fs;
            try
            {
                fs = new FileStream(fname, FileMode.Open);
            }
            catch
            {
                return null;
            }
            try
            {
                XmlSerializer xser = new XmlSerializer(typeof(BookMarkFactory));
                BookMarkFactory factory = (BookMarkFactory)xser.Deserialize(fs);
                return factory;
            }
            catch
            {
                return null;
            }
            finally
            {
                fs.Close();
            }
        }

        public void saveXml()
        {
            //saveXml(globalFilename);
        }

        public void saveXml(string fname)
        {            
            TextWriter writer = new StreamWriter(fname);
            XmlSerializer xser = new XmlSerializer(typeof(BookMarkFactory));

            xser.Serialize(writer, this);
            writer.Close();
        }

        #endregion

        public class TStripBookI : ToolStripMenuItem
        {
        }

        public class TStripBookItem : TStripBookI
        {
            internal Bookmark bmark;
            internal TStripBookItem(Bookmark imark)
            {
                bmark = imark;
                this.Text = bmark.Name;
                this.CheckOnClick = true;
                this.Checked = false;
            }
        }

        internal class TStripBookGroup : TStripBookI
        {
            internal TStripBookGroup(string groupname)
            {
                this.Text = groupname;
            }
        }

        //First level menu list with subgroups
        ToolStripItemCollection tsitems;

        //All menu items in all subgroups except groups
        private List<TStripBookItem> allMenuItems = new List<TStripBookItem>();

        /// <summary>
        /// Return list of all menu items that are bookmarks
        /// </summary>
        [XmlIgnore]
        public List<TStripBookItem> menuItems
        {
            get { return allMenuItems; }
        }

        public void fillMenuItems(ToolStripItemCollection items)
        {
            /*
            tsitems = items;
            foreach (KeyValuePair<string, Bookmark> pair in marks)
            {
                TStripBookItem tstrip = new TStripBookItem(pair.Value);
                TStripBookGroup grp = findGroup(pair.Value.group);
                allMenuItems.Add(tstrip);
                if (grp == null)
                    tsitems.Add(tstrip);
                else
                    grp.DropDown.Items.Add(tstrip);
                tstrip.Click += tstrip_Click;
            }
            */
            if (onChanged != null)
                onChanged(this);
        }

        public void tstrip_Click(object sender, EventArgs e)
        {
            TStripBookItem tstrip = sender as TStripBookItem;
            if (tstrip == null)
                return;

            if (tstrip.Checked)
            {
                tstrip.bmark.Owner = this;
                tstrip.bmark.show();
                mapo.addSub(tstrip.bmark);
                mapo.CenterMapLonLat(tstrip.bmark.lon, tstrip.bmark.lat);
            }
            else
            {
                tstrip.bmark.hide();
                mapo.delSub(tstrip.bmark);
            }
        }

        public void test()
        {
            Bookmark bmark = new Bookmark();
            bmark.lat = 30.0;
            bmark.lon = 60.0;
            bmark.Name = "Just for test 1";
            bmark.Comment = "This is a comment";
            try
            {
                marks.Add(bmark.Name, bmark);
                bmark.Owner = this;
            }
            catch
            {
            }
        }

        /// <summary>
        /// Called when zoom level of the map changes, we recompute all coordinates for
        /// shown POIs
        /// </summary>
        /// <param name="old_zoom"></param>
        /// <param name="new_zoom"></param>
        public void updateOnZoomChange(int old_zoom, int new_zoom)
        {
            foreach (KeyValuePair<int, Bookmark> pair in shownPOIs)
            {
                pair.Value.calculateXY(mapo);
            }
        }

        /// <summary>
        /// Adds new bookmark to the factory
        /// </summary>
        /// <param name="newone"></param>
        /// <returns></returns>
        public bool addBookmark(Bookmark newone)
        {
            bool ret = addBookmarkSilently(newone);
            if (ret && onChanged != null)
                onChanged(this);
            return ret;
        }
        /// <summary>
        /// Internal method for adding bookmarks
        /// </summary>
        /// <param name="newone"></param>
        /// <returns></returns>
        private bool addBookmarkSilently(Bookmark newone)
        {
            newone.makeId();

            if (marks.ContainsKey(newone.sid))
                return false;
            marks.Add(newone.sid, newone);
            newone.Owner = this;

            TStripBookGroup grp = findGroup(newone.group);
            TStripBookItem tstrip = new TStripBookItem(newone);
            allMenuItems.Add(tstrip);
            if (grp == null)
                tsitems.Add(tstrip);
            else
                grp.DropDownItems.Add(tstrip);

            tstrip.Click += tstrip_Click;
            tstrip.Checked = true;
            tstrip_Click(tstrip, EventArgs.Empty);
            return true;
        }

        internal TStripBookGroup findGroup(string groupname)
        {
            if (groupname == null || groupname.Length == 0)
                return null;

            foreach (ToolStripItem mi in tsitems)
            {
                TStripBookGroup bgrp = mi as TStripBookGroup;
                if (bgrp != null && bgrp.Text == groupname)
                    return bgrp;
            }

            TStripBookGroup bgroup = new TStripBookGroup(groupname);
            tsitems.Add(bgroup);
            return bgroup;
        }

        internal List<string> getGroupNames()
        {
            List<string> lst = new List<string>();
            foreach (ToolStripItem mi in tsitems)
            {
                TStripBookGroup bgrp = mi as TStripBookGroup;
                if (bgrp != null)
                    lst.Add(bgrp.Text);
            }
            return lst;
        }

        /// <summary>
        /// Return a list of POI that are in the given bounds and have auto show flag switched on
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public List<Bookmark> getBookmarksByBounds(double lon1, double lat1, double lon2, double lat2)
        {
            List<Bookmark> blist = new List<Bookmark>();

            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select " + BookMarkFactory.poiSelectFields
                    + "from poi, poi_type, poi_spatial where poi.id = poi_spatial.id "
                    + "and poi_type.id = poi.type and poi_type.is_auto_show=1 "
                    + "and poi_spatial.minLon>=@LON1 and poi_spatial.maxLon<=@LON2 "
                    + "and poi_spatial.minLat>=@LAT1 and poi_spatial.maxLat<=@LAT2");

                dbo.addFloatPar("@LON1", (lon1 < lon2 ? lon1 : lon2));
                dbo.addFloatPar("@LON2", (lon1 < lon2 ? lon2 : lon1));
                dbo.addFloatPar("@LAT1", (lat1 < lat2 ? lat1 : lat2));
                dbo.addFloatPar("@LAT2", (lat1 < lat2 ? lat2 : lat1));

                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
                    //DO each item processing
                    Bookmark poi = new Bookmark(reader);
                    poi.IsAutoShow = true;
                    blist.Add(poi);
                }
            }
            catch (System.Exception e)
            {
                Program.Log("POIBounds SQLError: " + e.ToString());

            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
            
            return blist;
        }


        /// <summary>
        /// Exports pois into a given format (in fileInfo.fileType)
        /// </summary>
        /// <param name="fileInfo"></param>
        public void exportTo(GPS.TrackFileInfo fileInfo)
        {
            TrackLoader.IPOILoader loader = TrackLoader.TrackLoaderFactory.singleton.getLoaderByName(fileInfo.FileType)
                        as TrackLoader.IPOILoader;
            if (loader == null)
                throw new ArgumentException("Wrong format for exporting: " + fileInfo.FileType);

            Bookmarks.POIGroup root = groupFactory.rootGroup;
            List<Bookmark> childrenPOI = this.loadByParent(0, false);
            loader.exportPOIs(fileInfo, this, root.Children, childrenPOI, root);
        }

        /// <summary>
        /// Import POIs from the given file or buffer. Recognizes file format and loads
        /// from it all POIs into the group. Group name is identified by file name or
        /// internal document name.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        internal int importFrom(GPS.TrackFileInfo fileInfo)
        {
            TrackLoader.IPOILoader loader = TrackLoader.TrackLoaderFactory.singleton.getPOILoader(fileInfo);
            if(loader == null)
                return -1;
            int count = loader.importPOIs(fileInfo, this, groupFactory);
            if (onChanged != null)
                onChanged(this);
            return count;
        }

        /// <summary>
        /// List of required fields for creating bookmark object based on Select statement
        /// </summary>
        public static readonly string poiSelectFields =
                    " poi.id, poi.name, poi.description, poi.comments, "
                  + "poi.type, poi.lon, poi.lat, poi.alt, poi.flags, poi.created ";
        /// <summary>
        /// Loads all POI from DB that has given parent
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<Bookmark> loadByParent(int parent_id, bool needRegister)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select " + poiSelectFields
                            + "from poi, poi_group_member where poi.id = poi_group_member.member_id "
                            + "and poi.is_group = 0 and poi_group_member.parent_id=@PARENT_ID");
                dbo.addIntPar("@PARENT_ID", parent_id);
                DbDataReader reader = dbo.cmd.ExecuteReader();
                List<Bookmark> pois = new List<Bookmark>();
                Bookmarks.POIGroup pgroup = groupFactory.findById(parent_id);

                while (reader.Read())
                {
            		//DO each item processing
                    int poi_id = reader.GetInt32(reader.GetOrdinal("ID"));

                    Bookmark poi;

                    if (needRegister)
                    {
                        lock (locker)
                        {
                            if (!loadedPOIs.TryGetValue(poi_id, out poi))
                            {
                                poi = new Bookmark(reader);
                                register(poi);
                            }
                            else
                                poi.Owner = this;
                        }
                    }
                    else
                        poi = new Bookmark(reader);

                    poi.Parent = pgroup;
                    pois.Add(poi);
                }
                if(needRegister)
                    pgroup.ChildrenPOIs = pois;
                return pois;
            }
            catch (System.Exception e)
            {
            	Program.Log("SQLError: " + e.ToString());
                return null;
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
        }

        /// <summary>
        /// List of all POIs that are loaded into memory
        /// </summary>
        private Dictionary<int, Bookmark> loadedPOIs = new Dictionary<int, Bookmark>();

        /// <summary>
        /// The list of POIs shown on the screen (has IsShown == true).
        /// </summary>
        private Dictionary<int, Bookmark> shownPOIs = new Dictionary<int, Bookmark>();
        
        /// <summary>
        /// Unregister POI from the factory that manages it
        /// </summary>
        /// <param name="bookmark"></param>
        internal void unregister(Bookmark bookmark)
        {
            lock (locker)
            {
                loadedPOIs.Remove(bookmark.Id);
                shownPOIs.Remove(bookmark.Id);
            }
        }

        /// <summary>
        /// Register new POI with the factory, for proper management
        /// </summary>
        /// <param name="poi"></param>
        internal void register(Bookmark poi)
        {
            lock (locker)
            {

                loadedPOIs.Add(poi.Id, poi);
                poi.Owner = this;
                if (poi.IsShown)
                    shownPOIs.Add(poi.Id, poi);
            }
        }

        /// <summary>
        /// Register POI that is shown
        /// </summary>
        /// <param name="poi"></param>
        internal void POIShown(Bookmark poi)
        {
            lock (locker)
            {
                shownPOIs.Add(poi.Id, poi);
            }
        }

        /// <summary>
        /// Unregister poi from shown list on hide
        /// </summary>
        /// <param name="poi"></param>
        internal void POIHidden(Bookmark poi)
        {
            lock (locker)
            {
                shownPOIs.Remove(poi.Id);
            }
        }

        /// <summary>
        /// Gui control for receiving Worker thread commands
        /// </summary>
        private Control guiControl;

        private GUIWorkerThread<Bookmarks.POIVisualWorkerTask> visualWorker;

        /// <summary>
        /// Register the GUI control for receiving callbacks in main thread,
        /// starts worker thread for Auto show POI processing
        /// </summary>
        /// <param name="ctrl"></param>
        public void startAutoShowWorker(Control ctrl)
        {
            guiControl = ctrl;
            visualWorker = new GUIWorkerThread<Bookmarks.POIVisualWorkerTask>(guiControl);
            visualWorker.OnlyLast = true;
            visualWorker.start();
            visualWorker.taskCompleted += visualWorker_taskCompleted;
            mapo.onZoomChanged += mapo_onZoomChanged;
            mapo.onTileCenterChanged += mapo_onTileCenterChanged;
        }

        /// <summary>
        /// Called on every change of the map position in tiles, not in pixels
        /// </summary>
        /// <param name="tilePosNXNY"></param>
        void mapo_onTileCenterChanged(System.Drawing.Rectangle tilePosNXNY)
        {
            sendVisualWorkerTask();
        }

        /// <summary>
        /// Creates a task for visualizing POI that is in auto show mode
        /// </summary>
        /// <param name="old_zoom"></param>
        /// <param name="new_zoom"></param>
        private void mapo_onZoomChanged(int old_zoom, int new_zoom)
        {
            sendVisualWorkerTask();
        }

        /// <summary>
        /// Creates a task for visualizing POI in auto show mode to be done in separate thread
        /// </summary>
        private void sendVisualWorkerTask()
        {
            if (visualWorker == null)
                return;

            Bookmarks.POIVisualWorkerTask task = new Bookmarks.POIVisualWorkerTask(
                mapo.tilePosNXNY, mapo.zoom,
                mapo.geosystem.baseType, this);
            visualWorker.addTask(task);
        }

        /// <summary>
        /// Process a list of POIs to show as the result of Auto show POI thread.
        /// Shows new POIs and hide old ones.
        /// Works in the main GUI thread
        /// </summary>
        /// <param name="completedTask"></param>
        private void visualWorker_taskCompleted(GMView.Bookmarks.POIVisualWorkerTask completedTask)
        {
            List<Bookmark> resultList = completedTask.result;
            Dictionary<int, Bookmark> listToHide = completedTask.hideList;

            GML.tranBegin();

            lock (locker)
            {
                foreach (KeyValuePair<int, Bookmark> delpoi in listToHide)
                {
                    if (shownPOIs.ContainsKey(delpoi.Value.Id))
                        delpoi.Value.IsShown = false;
                    unregister(delpoi.Value);
                }

                foreach (Bookmark poi in resultList)
                {
                    Bookmark rpoi;
                    if (!loadedPOIs.TryGetValue(poi.Id, out rpoi))
                    {
                        register(poi);
                        rpoi = poi;
                    }

                    if (!shownPOIs.ContainsKey(poi.Id))
                    {
                        rpoi.IsShown = true;
                    }
                }
            }
            GML.tranEnd();
        }

        /// <summary>
        /// Fills and returns list of poi that is in auto show mode.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Bookmark> fillListToHide()
        {
            Dictionary<int, Bookmark> listToHide = new Dictionary<int, Bookmark>();
            lock (locker)
            {
                foreach (KeyValuePair<int, Bookmark> poi in shownPOIs)
                {
                    if (poi.Value.IsAutoShow)
                        listToHide.Add(poi.Key, poi.Value);
                }
            }
            return listToHide;
        }
    }
}
