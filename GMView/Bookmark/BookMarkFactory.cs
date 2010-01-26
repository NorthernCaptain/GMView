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
            saveXml(globalFilename);
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

        public List<Bookmark> getBookmarksByBounds(double minlon, double minlat, double maxlon, double maxlat)
        {
            List<Bookmark> blist = new List<Bookmark>();
            foreach (KeyValuePair<string, Bookmark> pair in marks)
            {
                Bookmark bmark = pair.Value;
                if (bmark.lon >= minlon && bmark.lat >= minlat
                    && bmark.lon <= maxlon && bmark.lat <= maxlat)
                {
                    bool need_to_add = true;
                    foreach (Bookmark blistbook in blist)
                    {
                        if (bmark.lon == blistbook.lon && bmark.lat == blistbook.lat)
                        {
                            need_to_add = false;
                            break;
                        }
                    }
                    if(need_to_add)
                        blist.Add(bmark);
                }
            }
            return blist;
        }

        /// <summary>
        /// Exports all our bookmarks in GPX format as waypoint
        /// </summary>
        /// <param name="fname"></param>
        public void exportGPX(string fname)
        {
            XmlTextWriter writer = null;
            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("");
            System.Globalization.NumberFormatInfo nf = cul.NumberFormat;

            writer = new XmlTextWriter(fname, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("gpx");
            writer.WriteAttributeString("version", "1.1");
            writer.WriteAttributeString("creator", "GMView v." + Options.program_version);
            writer.WriteAttributeString("xmlns:xsi", @"http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", @"http://www.topografix.com/GPX/1/1");
            writer.WriteAttributeString("xsi:schemaLocation", @"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");

            // metadata section
            {
                writer.WriteStartElement("metadata");
                writer.WriteElementString("name", Path.GetFileNameWithoutExtension(fname));
                {
                    writer.WriteStartElement("author");
                    writer.WriteElementString("name", Program.opt.author);
                    {
                        writer.WriteStartElement("email");
                        string email = Program.opt.email;
                        writer.WriteAttributeString("id", email.Substring(0, email.IndexOf('@')));
                        writer.WriteAttributeString("domain", email.Substring(email.IndexOf('@') + 1));
                        writer.WriteEndElement(); // email
                    }
                    writer.WriteEndElement(); //author
                }

                writer.WriteElementString("desc", "GPX file was generated by " + "GMView v." + Options.program_version + " at " + Environment.MachineName);
                writer.WriteElementString("time", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                writer.WriteElementString("copyright", "GNU FPL");
                writer.WriteEndElement();
            }

            foreach (KeyValuePair<string, Bookmark> pair in marks)
            {
                Bookmark book = pair.Value;
                writer.WriteStartElement("wpt");
                writer.WriteAttributeString("lon", book.lon.ToString("F6", nf));
                writer.WriteAttributeString("lat", book.lat.ToString("F6", nf));
                writer.WriteElementString("name", book.Name);
                writer.WriteElementString("desc", book.Comment);
                writer.WriteElementString("time", book.Created.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                {
                    writer.WriteStartElement("extensions");
                    writer.WriteElementString("group", book.Parent.Name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            // end of gpx root tag
            writer.WriteEndElement(); //gpx
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Import all waypoints (wpt) from GPX file into our POI 
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        internal int importGPX(string fname)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(fname);

            if (doc.DocumentElement.Name != "gpx")
                throw new ApplicationException("Not a valid GPX file! Could not find gpx root tag.");

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);

            { //retreive xmlns
                XmlNode xnsnode = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                if (xnsnode != null)
                    nsm.AddNamespace("gpx", xnsnode.Value);
                else
                {
                    nsm.AddNamespace("gpx", "");
                }
            }

            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//gpx:wpt", nsm);
            if (nlist.Count == 0)
                return 0;

            return subloadBookmarksGPX(nlist, nsm, Path.GetFileNameWithoutExtension(fname));
        }

        private int subloadBookmarksGPX(XmlNodeList nlist, XmlNamespaceManager nsm, string groupname)
        {
            int count = 0;
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            NMEA_RMC rmc = new NMEA_RMC();
            foreach (XmlNode node in nlist)
            {
                Bookmark bmark = new Bookmark();
                bmark.IsDbChange = false;

                bmark.lat = NMEACommand.getDouble(node.Attributes.GetNamedItem("lat").Value);
                bmark.lon = NMEACommand.getDouble(node.Attributes.GetNamedItem("lon").Value);

                XmlNode xnode = node.SelectSingleNode("./gpx:time", nsm);
                if (xnode != null)
                {
                    try
                    {
                        rmc.utc_time = DateTime.Parse(xnode.InnerText).ToUniversalTime();
                    }
                    catch
                    {
                        rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();
                    }

                }
                else
                    rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();
                bmark.Created = rmc.utc_time.ToLocalTime();

                xnode = node.SelectSingleNode("./gpx:name", nsm);
                if (xnode == null)
                    continue;
                bmark.Name = xnode.InnerText.Trim();

                xnode = node.SelectSingleNode("./gpx:desc", nsm);
                if (xnode != null)
                    bmark.Comment = xnode.InnerText.Trim();
                if (groupname == null)
                {
                    bmark.group = "Imported";

                    xnode = node.SelectSingleNode("./gpx:extensions/gpx:group", nsm);
                    if (xnode != null)
                        bmark.group = xnode.InnerText.Trim();
                }
                else
                {
                    bmark.group = groupname;
                    bmark.is_temporary = true;
                }

                xnode = node.SelectSingleNode("./gpx:extensions/gpx:color", nsm);
                if (xnode != null)
                    bmark.image_idx = int.Parse(xnode.InnerText);

                xnode = node.SelectSingleNode("./gpx:extensions/gpx:zoom", nsm);
                if (xnode != null)
                    bmark.Original_zoom = int.Parse(xnode.InnerText);

                bmark.IsDbChange = true;
                bmark.updateDB();

                Bookmarks.POIGroup pgroup = Bookmarks.POIGroupFactory.singleton().findByName(bmark.group);
                if(pgroup == null)
                {
                    pgroup = Bookmarks.POIGroupFactory.singleton().createSimpleGroup(bmark.group);
                }
                bmark.addLinkDB(pgroup);

                //if (addBookmarkSilently(bmark))
                    count++;
            }

            ncUtils.DBConnPool.singleton.commitThreadTransaction();

            if (onChanged != null)
                onChanged(this);
            return count;
        }

        /// <summary>
        /// Import all waypoints (placemark) from KML file into our POI 
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        internal int importKML(string fname)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(fname);

            if (doc.DocumentElement.Name != "kml")
                throw new ApplicationException("Not a valid KML file! Could not find kml root tag.");

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);

            { //retreive xmlns
                XmlNode xnsnode = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                if (xnsnode != null)
                    nsm.AddNamespace("kml", xnsnode.Value);
                else
                {
                    nsm.AddNamespace("kml", "");
                }
            }

            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//kml:Placemark", nsm);
            if (nlist.Count == 0)
                return 0;

            return subloadBookmarksKML(nlist, nsm, Path.GetFileNameWithoutExtension(fname));
        }

        private int subloadBookmarksKML(XmlNodeList nlist, XmlNamespaceManager nsm, string groupname)
        {
            int count = 0;
            NMEA_RMC rmc = new NMEA_RMC();
            double lon, lat, hei;
            DateTime startTime = DateTime.Now;

            ncUtils.DBConnPool.singleton.beginThreadTransaction();

            foreach (XmlNode node in nlist)
            {
                XmlNode xnode = node.SelectSingleNode("./kml:Point/kml:coordinates", nsm);
                if(xnode != null)
                { //we have POI here, lets add it to our bookmarks
                    if (!GPSTrack.splitKMLCoordTuple(xnode.InnerText, out lon, out lat, out hei))
                        continue;
                    Bookmark bmark = new Bookmark();
                    bmark.IsDbChange = false;

                    bmark.lon = lon;
                    bmark.lat = lat;

                    xnode = node.SelectSingleNode("./kml:name", nsm);
                    if (xnode == null)
                        continue;
                    bmark.Name = xnode.InnerText;
                    xnode = node.SelectSingleNode("./kml:description", nsm);
                    if (xnode != null)
                        bmark.Description = xnode.InnerText;

                    if (groupname == null)
                    {
                        bmark.group = "Imported";
                    }
                    else
                    {
                        bmark.group = groupname;
                        bmark.is_temporary = true;
                    }

                    bmark.IsDbChange = true;
                    bmark.updateDB();

                    Bookmarks.POIGroup pgroup = Bookmarks.POIGroupFactory.singleton().findByName(bmark.group);
                    if (pgroup == null)
                    {
                        pgroup = Bookmarks.POIGroupFactory.singleton().createSimpleGroup(bmark.group);
                    }
                    bmark.addLinkDB(pgroup);

                    count++;
                }
            }

            ncUtils.DBConnPool.singleton.commitThreadTransaction();

            TimeSpan dTime = DateTime.Now - startTime;
            Program.Log("Loaded " + count + " POI's in seconds: " + dTime.TotalSeconds.ToString("F3"));
            if (onChanged != null)
                onChanged(this);
            return count;
        }

        internal void loadTemporaryBookmarks(string groupname, XmlNodeList nlist, XmlNamespaceManager nsm)
        {
            if (nlist.Count == 0)
                return;
            if (nlist.Item(0).LocalName == "Placemark")
                subloadBookmarksKML(nlist, nsm, groupname);
            else
                subloadBookmarksGPX(nlist, nsm, groupname);
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
        public List<Bookmark> loadByParent(int parent_id)
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
                    poi.Parent = pgroup;
                    pois.Add(poi);
                }
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
