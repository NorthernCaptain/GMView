using System;
using System.Collections.Generic;
using System.Text;
using ncGeo;
using System.IO;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Implements loading and saving files in OziExplorer format
    /// WPT - is files for POI information
    /// </summary>
    public class OZIWptLoader: IPOILoader
    {
        private static readonly ncFileControls.FileFilter wptFilter =
                    new ncFileControls.FileFilter("Ozi Explorer WPT file format (*.wpt)", "*.wpt");

        #region IPOILoader Members

        public ncFileControls.FileFilter poiLoadFileFilter()
        {
            return wptFilter;
        }

        public ncFileControls.FileFilter poiSaveFileFilter()
        {
            return wptFilter;
        }

        public int importPOIs(GMView.GPS.TrackFileInfo fi, BookMarkFactory poiFactory, GMView.Bookmarks.POIGroupFactory groupFactory)
        {
            string gname = (fi.stype == GPS.TrackFileInfo.SourceType.FileName ?
                System.IO.Path.GetFileNameWithoutExtension(fi.fileOrBuffer) :
                "ozi-wpt-buffer-" + DateTime.Now.ToShortDateString() +
                "-" + DateTime.Now.ToShortTimeString());

            Bookmarks.POIGroup parentGroup = groupFactory.rootGroup.getSubGroupByPath(gname);

            int count = 0;

            using (System.IO.TextReader reader = fi.openReader())
            {
                string buf = reader.ReadLine();
                if (!buf.StartsWith("OziExplorer Waypoint File Version 1"))
                    return 0;
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();

                try
                {
                    ncUtils.DBConnPool.singleton.beginThreadTransaction();
                    while ((buf = reader.ReadLine()) != null)
                    {
                        string[] parts = OZIPltLoader.splitToParts(buf);
                        if (parts.Length < 18)
                            continue;
                        //Empty name? then skip this POI
                        if (string.IsNullOrEmpty(parts[1]))
                            continue;

                        Bookmark bmark = new Bookmark();
                        bmark.IsDbChange = false;

                        bmark.lat = NMEACommand.getDouble(parts[2]);
                        bmark.lon = NMEACommand.getDouble(parts[3]);
                        bmark.Name = parts[1];
                        bmark.Description = parts[10];

                        if (!string.IsNullOrEmpty(parts[4]))
                        {
                            bmark.Created = OZIPltLoader.fromDelphiTime(parts[4]);
                        }
                        else
                            bmark.Created = DateTime.UtcNow;

                        if (!string.IsNullOrEmpty(parts[14]))
                        {
                            double altf = NMEACommand.getDouble(parts[14]);
                            if (altf > -776.9 || altf < -777.1)
                                bmark.alt = altf * OZIPltLoader.feet2m;
                        }

                        if (fi.defaultPOIType != null)
                            bmark.Ptype = fi.defaultPOIType;

                        bmark.swapFields(fi);
                        bmark.IsDbChange = true;
                        bmark.updateDB();

                        bmark.addLinkDB(parentGroup);

                        count++;

                    }
                }
                catch (System.Exception)
                {

                }
                finally
                {
                    ncUtils.DBConnPool.singleton.commitThreadTransaction();
                }

            }

            if (fi.stype == GPS.TrackFileInfo.SourceType.StringBuffer)
                fi.fileOrBuffer = gname;
            return count;
        }

        public void exportPOIs(GMView.GPS.TrackFileInfo fi, BookMarkFactory pFactory, LinkedList<GMView.Bookmarks.POIGroup> groups, List<Bookmark> poilist, GMView.Bookmarks.POIGroup parentGroup)
        {
            count = 1;
            using (StreamWriter writer = new StreamWriter(fi.fileOrBuffer, false, Encoding.Default))
            {
                writer.WriteLine("OziExplorer Waypoint File Version 1.1");
                writer.WriteLine("WGS 84");
                writer.WriteLine("Reserved 2");
                writer.WriteLine("Reserved 3");

                saveListToWPT(writer, groups, poilist, pFactory, ncUtils.Glob.numformat);

                writer.Close();
            }

        }

        private int count = 0;
        /// <summary>
        /// Recursively saves all pois and subgroups
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="groups"></param>
        /// <param name="pois"></param>
        private void saveListToWPT(TextWriter writer,
                                   LinkedList<Bookmarks.POIGroup> groups,
                                   List<Bookmark> pois, BookMarkFactory poiFactory,
                                   System.Globalization.NumberFormatInfo nf)
        {
            if (pois != null)
            {
                foreach (Bookmark book in pois)
                {
                    StringBuilder buf = new StringBuilder(count.ToString());

                    buf.Append(',');
                    buf.Append(book.Name.Replace(',', ' '));
                    buf.Append(',');
                    buf.Append(book.latitude.ToString("F6", nf));
                    buf.Append(',');
                    buf.Append(book.longitude.ToString("F6", nf));
                    buf.Append(',');
                    buf.Append(OZIPltLoader.toDelphiTime(book.Created, nf));
                    buf.Append(",0,1,3,0,65535,");
                    if(!string.IsNullOrEmpty(book.Description))                        
                        buf.Append(book.Description.Replace(',', ' ').Replace('\n','.'));
                    buf.Append(",0,0,0,");
                    buf.Append((book.altitude / OZIPltLoader.feet2m).ToString("F3", nf));
                    buf.Append(",6,0,17");

                    writer.WriteLine(buf.ToString());
                    count++;
                }
            }

            if (groups != null)
            {
                foreach (Bookmarks.POIGroup node in groups)
                {
                    List<Bookmark> childrenPOI = poiFactory.loadByParent(node.Id, false);
                    saveListToWPT(writer, node.Children, childrenPOI, poiFactory, nf);
                }
            }
        }


        public bool isOurFormat(GMView.GPS.TrackFileInfo info)
        {
            string first_line = string.Empty;

            System.IO.TextReader reader = info.openReader();

            try
            {
                first_line = reader.ReadLine();
                if (string.IsNullOrEmpty(first_line))
                    return false;
                if (first_line.StartsWith("OziExplorer Waypoint File"))
                    return true;
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


        public GMView.GPS.TrackFileInfo preLoad(GMView.GPS.TrackFileInfo fi)
        {
            using (System.IO.TextReader reader = fi.openReader())
            {
                string buf = reader.ReadLine();
                if (!buf.StartsWith("OziExplorer Waypoint File Version 1"))
                    return fi;
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();

                fi.preloadPOICount = 0;
                while ((buf = reader.ReadLine()) != null)
                {
                    fi.preloadPOICount++;
                }
                reader.Close();
            }
            return fi;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new OZIWptLoader();
        }

        #endregion

    }
}
