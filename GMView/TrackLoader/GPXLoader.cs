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

            System.IO.TextReader reader;
            if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
            {
                reader = new System.IO.StreamReader(fi.fileOrBuffer);
            }
            else
                reader = new System.IO.StringReader(fi.fileOrBuffer);

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
            string gname = (fi.stype == GPS.TrackFileInfo.SourceType.FileName ?
                Path.GetFileNameWithoutExtension(fi.fileOrBuffer) :
                "gpx-buffer-" + DateTime.Now.ToShortDateString());
            return subloadBookmarks(nlist, nsm, gname);
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

                    xnode = node.SelectSingleNode("./gpx:extensions/gpx:group", nsm);
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
                        writer.WriteElementString("group", currentGroupName);
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

        public void preLoad(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
        }

        public GPSTrack load(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
