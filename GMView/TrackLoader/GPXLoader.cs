using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using ncGeo;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Implements GPX file format for loading and saving operations.
    /// </summary>
    public class GPXLoader: IPOILoader, ITrackLoader
    {
        private Bookmarks.POIGroupFactory groupFactory;
        private BookMarkFactory poiFactory;

        /// <summary>
        /// Public constructor
        /// </summary>
        public GPXLoader()
        {

        }

        #region IPOILoader Members

        public bool isOurFormat(GMView.GPS.TrackFileInfo fi)
        {
            string first_line = string.Empty;

            System.IO.TextReader reader = fi.openReader();

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    first_line = reader.ReadLine();
                    if (string.IsNullOrEmpty(first_line))
                        continue;
                    if (first_line.Contains("<gpx"))
                        return true;
                }
            }
            catch (System.Exception)
            {
            }
            finally
            {
                reader.Close();
            }
            return false;
        }

        /// <summary>
        /// Import POI from GPX file into the given factory.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="intoFactory"></param>
        /// <param name="groupFactory"></param>
        /// <returns></returns>
        public int importPOIs(GMView.GPS.TrackFileInfo fi, BookMarkFactory intoFactory, 
                    GMView.Bookmarks.POIGroupFactory groupFactory)
        {
            XmlDocument doc = fi.openXml();

            poiFactory = intoFactory;
            this.groupFactory = groupFactory;

            if (doc.DocumentElement.Name != "gpx")
                throw new ApplicationException("Not a valid GPX file! Could not find gpx root tag.");

            XmlNamespaceManager nsm = ncUtils.XmlHelper.getNSMforDoc(doc, "gpx");

            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//gpx:wpt", nsm);
            if (nlist.Count == 0)
                return 0;

            string gname = (fi.stype == GPS.TrackFileInfo.SourceType.FileName ?
                Path.GetFileNameWithoutExtension(fi.fileOrBuffer) :
                "gpx-buffer-" + DateTime.Now.ToShortDateString() +
                "-" + DateTime.Now.ToShortTimeString());

            int count = subloadBookmarks(nlist, nsm, gname);
            if (fi.stype == GPS.TrackFileInfo.SourceType.StringBuffer)
                fi.fileOrBuffer = gname;
            return count;
        }

        /// <summary>
        /// Sub routine for loading POIs itself
        /// </summary>
        /// <param name="nlist"></param>
        /// <param name="nsm"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        private int subloadBookmarks(XmlNodeList nlist, XmlNamespaceManager nsm, string groupname)
        {
            int count = 0;
            NMEA_RMC rmc = new NMEA_RMC();
            DateTime startTime = DateTime.Now;

            if (string.IsNullOrEmpty(groupname))
                groupname = "Imported";

            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            try
            {
                Bookmarks.POIGroup parentGroup = groupFactory.rootGroup.getSubGroupByPath(groupname);
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
                        bmark.Description = xnode.InnerText.Trim();

                    xnode = node.SelectSingleNode("./gpx:cmt", nsm);
                    if (xnode != null)
                        bmark.Comment = xnode.InnerText.Trim();

                    //Empty description but non-empty comment
                    if (string.IsNullOrEmpty(bmark.Description) &&
                        !string.IsNullOrEmpty(bmark.Comment))
                        bmark.Description = bmark.Comment;

                    xnode = node.SelectSingleNode("./gpx:ele", nsm);
                    if (xnode != null)
                    {
                        bmark.altitude = NMEACommand.getDouble(xnode.InnerText.Trim());
                    }

                    xnode = node.SelectSingleNode("./gpx:extensions/knw:group", nsm);
                    if (xnode != null)
                    {
                        bmark.group = xnode.InnerText.Trim();
                    }

                    xnode = node.SelectSingleNode("./gpx:sym", nsm);
                    if (xnode != null)
                    {
                        bmark.PtypeS = xnode.InnerText.Trim();
                    }

                    bmark.IsDbChange = true;
                    bmark.updateDB();

                    bmark.addLinkDB(parentGroup.getSubGroupByPath(bmark.group));

                    count++;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                ncUtils.DBConnPool.singleton.commitThreadTransaction();
            }

            TimeSpan dTime = DateTime.Now - startTime;
            Program.Log("Loaded " + count + " POI's in seconds: " + dTime.TotalSeconds.ToString("F3"));

            return count;
        }


        public void exportPOIs(GPS.TrackFileInfo fileInfo, BookMarkFactory pFactory, 
                                LinkedList<Bookmarks.POIGroup> groups,
                                List<Bookmark> poilist, Bookmarks.POIGroup parentGroup)
        {
            poiFactory = pFactory;
            groupFactory = parentGroup.Owner;

            XmlTextWriter writer = null;
            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("");
            System.Globalization.NumberFormatInfo nf = cul.NumberFormat;

            if (fileInfo.stype == GPS.TrackFileInfo.SourceType.FileName)
                writer = new XmlTextWriter(fileInfo.fileOrBuffer, Encoding.UTF8);
            else
                writer = new XmlTextWriter(new StringWriter());

            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("gpx");
            writer.WriteAttributeString("version", "1.1");
            writer.WriteAttributeString("creator", "GMView v." + Options.program_version);
            writer.WriteAttributeString("xmlns:xsi", @"http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", @"http://www.topografix.com/GPX/1/1");
            writer.WriteAttributeString("xsi:schemaLocation", @"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
            writer.WriteAttributeString("xmlns:knw", @"http://xnc.jinr.ru/knowhere/xmlns/knw/1/0");

            // metadata section
            {
                writer.WriteStartElement("metadata");
                writer.WriteElementString("name", Path.GetFileNameWithoutExtension(fileInfo.fileOrBuffer));
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

            saveListToGPX(writer, groups, poilist, parentGroup, nf);

            // end of gpx root tag
            writer.WriteEndElement(); //gpx
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Recursively saves all pois and subgroups
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="groups"></param>
        /// <param name="pois"></param>
        private void saveListToGPX(XmlTextWriter writer,
                                    LinkedList<Bookmarks.POIGroup> groups,
                                    List<Bookmark> pois, Bookmarks.POIGroup parentGroup,
                                    System.Globalization.NumberFormatInfo nf)
        {
            string currentGroupName = null;
            if (parentGroup == null)
                parentGroup = groupFactory.rootGroup;

            if (pois != null)
            {
                foreach (Bookmark book in pois)
                {
                    if (currentGroupName == null)
                    {
                        currentGroupName = (book.Parent as Bookmarks.POIGroup).getPathTill(parentGroup);
                    }
                    writer.WriteStartElement("wpt");
                    writer.WriteAttributeString("lon", book.longitude.ToString("F6", nf));
                    writer.WriteAttributeString("lat", book.latitude.ToString("F6", nf));
                    writer.WriteElementString("name", book.Name);
                    writer.WriteElementString("ele", book.altitude.ToString("F1", nf));
                    writer.WriteElementString("desc", book.Description);
                    writer.WriteElementString("cmt", book.Comment);
                    writer.WriteElementString("sym", book.PtypeS);
                    writer.WriteElementString("time", book.Created.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                    {
                        writer.WriteStartElement("extensions");
                        writer.WriteElementString("knw:group", currentGroupName);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }

            if (groups != null)
            {
                foreach (Bookmarks.POIGroup node in groups)
                {
                    List<Bookmark> childrenPOI = poiFactory.loadByParent(node.Id, false);
                    saveListToGPX(writer, node.Children, childrenPOI,
                                    parentGroup, nf);
                }
            }
        }

        #endregion

        #region ITrackLoader Members

        public GMView.GPS.TrackFileInfo preLoad(GMView.GPS.TrackFileInfo info)
        {
            XmlDocument doc = null;

            try
            {
                doc = info.openXml();

                if (doc.DocumentElement.Name != "gpx")
                    return info;

                XmlNamespaceManager nsm = ncUtils.XmlHelper.getNSMforDoc(doc, "gpx");

                XmlNode node = doc.DocumentElement.SelectSingleNode("/gpx:gpx/gpx:metadata/gpx:name", nsm);
                if (node != null)
                {
                    info.preloadName = node.InnerText;
                } else
                    info.preloadName = (info.stype == GPS.TrackFileInfo.SourceType.FileName ?
                        Path.GetFileNameWithoutExtension(info.fileOrBuffer) : "gpx-buffer");

                XmlNodeList nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt", nsm);
                info.preloadTPointCount = nlist.Count;

                nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:rte/gpx:rtept", nsm);
                info.preloadRouteCount = nlist.Count;

                nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:wpt", nsm);
                info.preloadPOICount = nlist.Count;
            }
            catch (SystemException)
            {
            }
            return info;
        }

        public GPSTrack load(GMView.GPS.TrackFileInfo fi, BookMarkFactory poiFact, Bookmarks.POIGroupFactory igroupFact)
        {
            XmlDocument doc = null;

            poiFactory = poiFact;
            groupFactory = igroupFact;

            try
            {
                doc = fi.openXml();

                if (doc.DocumentElement.Name != "gpx")
                    throw new ApplicationException("Not a valid GPX file! Could not find gpx root tag.");

                XmlNamespaceManager nsm = ncUtils.XmlHelper.getNSMforDoc(doc, "gpx");

                GPSTrack track = new GPSTrack();

                string tname;

                XmlNode node = doc.DocumentElement.SelectSingleNode("/gpx:gpx/gpx:metadata/gpx:name", nsm);
                if (node != null)
                {
                    string nodeval = node.InnerText;
                    if (nodeval.Length > 7 && nodeval.Substring(0, 7) == "Track: ")
                        nodeval = nodeval.Substring(7);

                    track.track_name = nodeval;
                    track.wayObject.name = "Route: " + nodeval;
                    tname = nodeval;
                }
                else
                {
                    if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
                    {
                        string dirname = Path.GetDirectoryName(fi.fileOrBuffer);
                        tname = dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                              + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
                        track.track_name = tname;
                        track.wayObject.name = "Route: " + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
                    }
                    else
                    {
                        tname = "GPX buffer " + DateTime.Now.ToShortDateString()
                            + " " + DateTime.Now.ToShortTimeString();
                        track.track_name = tname;
                        track.wayObject.name = "Route: " + tname;
                    }
                }

                XmlNodeList nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt", nsm);
                foreach (XmlNode xnode in nlist)
                {
                    loadPoint(track, xnode, nsm);
                }

                if (nlist.Count == 0) //we don't have track in file, lets try route
                {
                    nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:rte/gpx:rtept", nsm);
                    foreach (XmlNode xnode in nlist)
                    {
                        loadPoint(track, xnode, nsm);
                    }
                }

                if (track.trackPointData.Count == 0)
                    throw new ApplicationException("This file does not have any tracks or routes! Check file content");

                nlist = doc.DocumentElement.SelectNodes("/gpx:gpx/gpx:wpt", nsm);

                if (fi.needPOI)
                {
                    if (nlist.Count > 0)
                    {
                        subloadBookmarks(nlist, nsm, fi.poiParentGroupName + "/" + track.track_name);
                    }
                }

                track.fileName = (fi.stype == GPS.TrackFileInfo.SourceType.FileName ? fi.fileOrBuffer
                    : tname + ".gpx");
                track.calculateParameters();
                track.lastNonZeroPos = track.lastData;

                if (nlist.Count > 0 && track.way.TotalPoints < 3)
                    findAndMarkWayPoints(track, nlist, nsm);

                return track;
            }
            finally
            {
                doc = null;
            }
        }

        /// <summary>
        /// Find any way point and try to place it on the track
        /// </summary>
        /// <param name="track"></param>
        /// <param name="nlist"></param>
        /// <param name="nsm"></param>
        private void findAndMarkWayPoints(GPSTrack track, XmlNodeList nlist, XmlNamespaceManager nsm)
        {
            double lon, lat;
            try
            {
                ncGeo.FindNearestPointByDistance findctx = new ncGeo.FindNearestPointByDistance();

                foreach (XmlNode xnode in nlist)
                {
                    lat = NMEACommand.getDouble(xnode.Attributes.GetNamedItem("lat").Value);
                    lon = NMEACommand.getDouble(xnode.Attributes.GetNamedItem("lon").Value);
                    findctx.init(lon, lat);
                    track.findNearest(findctx);

                    if (findctx.resultPoint != null && findctx.distance <= 0.01)
                        findctx.resultPoint.Value.ptype = NMEA_LL.PointType.MWP;
                }

                track.calculateParameters();
            }
            catch (System.Exception)
            {

            }
        }


        /// <summary>
        /// Loads one track point from GPX xml document
        /// </summary>
        /// <param name="xnode"></param>
        /// <param name="nsm"></param>
        private void loadPoint(GPSTrack track, XmlNode xnode, XmlNamespaceManager nsm)
        {
            NMEA_RMC rmc = new NMEA_RMC();
            rmc.lat = NMEACommand.getDouble(xnode.Attributes.GetNamedItem("lat").Value);
            rmc.lon = NMEACommand.getDouble(xnode.Attributes.GetNamedItem("lon").Value);

            string sval = "";
            if (ncUtils.XmlHelper.xmltag(xnode, "./gpx:time", nsm, ref sval))
            {
                try
                {
                    rmc.utc_time = DateTime.Parse(sval).ToUniversalTime();
                }
                catch
                {
                    rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();
                }

            }
            else
                rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();

            if (ncUtils.XmlHelper.xmltag(xnode, "./gpx:ele", nsm, ref sval))
                rmc.height = NMEACommand.getDouble(sval);
            if (ncUtils.XmlHelper.xmltag(xnode, "./gpx:sat", nsm, ref sval))
                rmc.usedSats = int.Parse(sval);
            if (ncUtils.XmlHelper.xmltag(xnode, "./gpx:hdop", nsm, ref sval))
                rmc.HDOP = NMEACommand.getDouble(sval);
            if (ncUtils.XmlHelper.xmltag(xnode, "./*/gpx:vel", nsm, ref sval))
                rmc.speed = NMEACommand.getDouble(sval);
            if (ncUtils.XmlHelper.xmltag(xnode, "./*/gpx:dir", nsm, ref sval))
                rmc.dir_angle = NMEACommand.getDouble(sval);
            if (ncUtils.XmlHelper.xmltag(xnode, "./gpx:type", nsm, ref sval))
                rmc.ptype = NMEA_LL.parsePointType(sval);

            track.lastData = track.lastNonZeroPos = track.LastTrackPos = rmc;

            track.trackPointData.AddLast(rmc);
        }

        /// <summary>
        /// Saves track and surrounding POIs into GPX file
        /// </summary>
        /// <param name="track"></param>
        /// <param name="fi"></param>
        /// <param name="poiFact"></param>
        /// <param name="igroupFact"></param>
        public void save(GPSTrack track, GMView.GPS.TrackFileInfo fi, BookMarkFactory poiFact, GMView.Bookmarks.POIGroupFactory igroupFact)
        {
            double minlon, minlat, maxlon, maxlat;
            XmlTextWriter writer = null;
            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("");
            System.Globalization.NumberFormatInfo nf = cul.NumberFormat;

            lock (track)
            {
                writer = new XmlTextWriter(fi.fileOrBuffer, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("gpx");
                writer.WriteAttributeString("version", "1.1");
                writer.WriteAttributeString("creator", track.who);
                writer.WriteAttributeString("xmlns:xsi", @"http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns", @"http://www.topografix.com/GPX/1/1");
                writer.WriteAttributeString("xsi:schemaLocation", @"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
                writer.WriteAttributeString("xmlns:knw", @"http://xnc.jinr.ru/knowhere/xmlns/knw/1/0");

                // metadata section
                {
                    writer.WriteStartElement("metadata");
                    writer.WriteElementString("name", track.track_name);
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

                    writer.WriteElementString("desc", "GPX file was generated by " + track.who + " at " + Environment.MachineName);
                    writer.WriteElementString("time", track.startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                    writer.WriteElementString("copyright", "GNU FPL");
                    {
                        writer.WriteStartElement("bounds");
                        track.getBounds(out minlon, out minlat, out maxlon, out maxlat);
                        writer.WriteAttributeString("minlat", minlat.ToString("F8", nf));
                        writer.WriteAttributeString("minlon", minlon.ToString("F8", nf));
                        writer.WriteAttributeString("maxlat", maxlat.ToString("F8", nf));
                        writer.WriteAttributeString("maxlon", maxlon.ToString("F8", nf));
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                //track section
                writer.WriteStartElement("trk");
                writer.WriteElementString("name", track.track_name);
                writer.WriteStartElement("extensions");
                writer.WriteElementString("knw:travel_time", track.travel_time_string);
                writer.WriteElementString("knw:travel_distance", track.distance.ToString("F2", nf));
                writer.WriteEndElement();
                writer.WriteStartElement("trkseg");

                foreach (NMEA_LL tp in track.trackPointData)
                {
                    writer.WriteStartElement("trkpt");
                    writer.WriteAttributeString("lon", tp.lon.ToString("F8", nf));
                    writer.WriteAttributeString("lat", tp.lat.ToString("F8", nf));
                    writer.WriteElementString("time", tp.utc_time.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                    writer.WriteElementString("type", tp.ptype.ToString());
                    writer.WriteElementString("ele", tp.height.ToString("F3", nf));
                    writer.WriteElementString("hdop", tp.HDOP.ToString("F1", nf));
                    writer.WriteElementString("sat", tp.usedSats.ToString());
                    if (tp.speed != 0 || tp.dir_angle != 0)
                    {
                        writer.WriteStartElement("extensions");
                        writer.WriteElementString("knw:vel", tp.speed.ToString("F2", nf));
                        writer.WriteElementString("knw:dir", tp.dir_angle.ToString("F1", nf));
                        writer.WriteEndElement(); //extensions
                    }
                    writer.WriteEndElement(); //trkpt

                    //if we have stay waypoint, then start new track segment
                    if (tp.ptype == NMEA_LL.PointType.SWP)
                    {
                        writer.WriteEndElement(); //trkseg
                        writer.WriteStartElement("trkseg");
                    }

                }

                writer.WriteEndElement(); //trkseg

                writer.WriteEndElement(); //trk

                saveWayPoints(track.way, writer, nf);

                { // save bookmarks if we have them in our region
                    double delta_lon = (maxlon - minlon) / 25;
                    double delta_lat = (maxlat - minlat) / 25;

                    List<Bookmark> booklist = BookMarkFactory.singleton.getBookmarksByBounds(
                        minlon - delta_lon, minlat - delta_lat, maxlon + delta_lon, maxlat + delta_lat);
                    if (booklist.Count > 0)
                    {
                        foreach (Bookmark book in booklist)
                        {
                            writer.WriteStartElement("wpt");
                            writer.WriteAttributeString("lon", book.lon.ToString("F8", nf));
                            writer.WriteAttributeString("lat", book.lat.ToString("F8", nf));
                            writer.WriteElementString("name", book.Name);
                            writer.WriteElementString("desc", book.Description);
                            writer.WriteElementString("cmt", book.Comment);
                            writer.WriteElementString("sym", book.PtypeS);
                            writer.WriteElementString("time", book.Created.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                            writer.WriteEndElement();
                        }
                    }
                }

                // end of gpx root tag
                writer.WriteEndElement(); //gpx
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Save all waypoint from our Way as route in GPX file
        /// </summary>
        /// <param name="way"></param>
        /// <param name="writer"></param>
        /// <param name="nf"></param>
        private void saveWayPoints(GPS.Way way, XmlTextWriter writer, System.Globalization.NumberFormatInfo nf)
        {
            if (way.TotalPoints <= 2)
                return;

            writer.WriteStartElement("rte");
            writer.WriteElementString("name", way.name);
            foreach (ncGeo.WayBase.WayPoint wp in way.wayPoints)
            {
                writer.WriteStartElement("rtept");
                writer.WriteAttributeString("lon", wp.point.lon.ToString("F6", nf));
                writer.WriteAttributeString("lat", wp.point.lat.ToString("F6", nf));
                writer.WriteElementString("time", wp.point.utc_time.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                writer.WriteElementString("type", wp.ptype.ToString());
                writer.WriteElementString("ele", wp.point.height.ToString("F3", nf));
                writer.WriteElementString("hdop", wp.point.HDOP.ToString("F1", nf));
                writer.WriteElementString("sat", wp.point.usedSats.ToString());

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        #endregion


        #region ICloneable Members

        public object Clone()
        {
            return new GPXLoader();
        }

        #endregion
    }
}
