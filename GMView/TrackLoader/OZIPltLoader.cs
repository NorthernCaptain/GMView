using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ncGeo;
using System.Threading;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Implements loading and saving files in OziExplorer format
    /// PLT - is files for track information
    /// </summary>
    public class OZIPltLoader: ITrackLoader
    {
        private static readonly ncFileControls.FileFilter pltFilter =
                new ncFileControls.FileFilter("Ozi Explorer PLT file format (*.plt)", "*.plt");

        /// <summary>
        /// Feet to meters
        /// </summary>
        internal const double feet2m = 0.3048;

        #region ITrackLoader Members

        public ncFileControls.FileFilter trackLoadFileFilter()
        {
            return pltFilter;
        }

        public ncFileControls.FileFilter trackSaveFileFilter()
        {
            return pltFilter;
        }

        public GMView.GPS.TrackFileInfo preLoad(GMView.GPS.TrackFileInfo info)
        {
            TextReader reader = info.openReader();

            if (reader == null)
                return info;

            string buf;

            buf = reader.ReadLine();

            //PLT track file or not?
            if (buf.StartsWith("OziExplorer Track Point File"))
            {
                try
                {
                    buf = reader.ReadLine();
                    buf = reader.ReadLine();
                    buf = reader.ReadLine();

                    buf = reader.ReadLine();

                    string[] parts = splitToParts(buf);
                    if (parts.Length < 8)
                        return info;

                    if (string.IsNullOrEmpty(parts[3]))
                        info.preloadName = "Ozi track file, unknown name";
                    else
                        info.preloadName = parts[3];

                    buf = reader.ReadLine();
                    while ((buf = reader.ReadLine()) != null)
                    {
                        info.preloadTPointCount++;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return info;
        }

        public GPSTrack load(GMView.GPS.TrackFileInfo fi, BookMarkFactory poiFact, GMView.Bookmarks.POIGroupFactory igroupFact)
        {
            GPSTrack track = new GPSTrack();

            string tname;
            System.IO.TextReader reader;
            if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
            {
                reader = new System.IO.StreamReader(fi.fileOrBuffer, Encoding.Default);
                string dirname = Path.GetDirectoryName(fi.fileOrBuffer);
                tname = dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                      + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
                track.track_name = "Track: " + tname;
                track.wayObject.name = "Route: " + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
            }
            else
            {
                reader = new System.IO.StringReader(fi.fileOrBuffer);
                tname = "Ozi track " + DateTime.Now.ToShortDateString()
                    + " " + DateTime.Now.ToShortTimeString();
                track.track_name = "Track: " + tname;
                track.wayObject.name = "Route: " + tname;
                tname += ".plt";
            }

            string buf;

            try
            {
                buf = reader.ReadLine(); // header signature
                buf = reader.ReadLine();
                buf = reader.ReadLine();
                buf = reader.ReadLine();
                buf = reader.ReadLine(); //part3 = track name

                string[] parts = splitToParts(buf);
                if (parts.Length > 4)
                {

                    if (!string.IsNullOrEmpty(parts[3]))
                        track.track_name = parts[3];
                }

                buf = reader.ReadLine(); //skip line

                //read points, one point on each line
                while ((buf = reader.ReadLine()) != null)
                {
                    parts = splitToParts(buf);
                    if (parts.Length < 5)
                        continue;

                    NMEA_RMC rmc = new NMEA_RMC();
                    rmc.lat = NMEACommand.getDouble(parts[0]);
                    rmc.lon = NMEACommand.getDouble(parts[1]);

                    double alt = NMEACommand.getDouble(parts[3]);
                    if (alt > -776.9 || alt < -777.9)
                        rmc.height = alt * 	feet2m; // feet -> meters

                    rmc.utc_time = fromDelphiTime(parts[4]);
                    track.addGPSDataInternal(rmc);
                }
            }
            finally
            {
                reader.Close();
            }

            if (track.trackPointData.Count == 0)
                throw new ApplicationException("This file does not have any tracks or routes! Check file content");

            track.calculateParameters();
            track.lastNonZeroPos = track.lastData;

            fi.fileOrBuffer = tname;

            return track;
        }

        public void save(GPSTrack track, GMView.GPS.TrackFileInfo fi, BookMarkFactory poiFact, GMView.Bookmarks.POIGroupFactory igroupFact)
        {
            System.Globalization.NumberFormatInfo nf = ncUtils.Glob.numformat;

            lock (track)
            {
                using ( StreamWriter writer = new StreamWriter(fi.fileOrBuffer, false, Encoding.Default) )
                {
                    writer.WriteLine("OziExplorer Track Point File Version 2.1");
                    writer.WriteLine("WGS 84");
                    writer.WriteLine("Altitude is in Feet");
                    writer.WriteLine("Reserved 3");
                    string name = track.track_name.Replace(',', ' ');
                    writer.WriteLine("0,2,16711680," + name + ",1,0,0,8421376");
                    writer.WriteLine("0");
                    
                    foreach (NMEA_LL rmc in track.trackPointData)
                    {
                        StringBuilder buf = new StringBuilder();

                        buf.Append(rmc.lat.ToString("F6", nf));
                        buf.Append(',');
                        buf.Append(rmc.lon.ToString("F6", nf));
                        buf.Append(",0,");
                        buf.Append((rmc.height / feet2m).ToString("F3", nf));
                        buf.Append(',');
                        buf.Append(toDelphiTime(rmc.utc_time, nf));
                        buf.Append(',');
                        buf.Append(rmc.utc_time.ToShortDateString());
                        buf.Append(',');
                        buf.Append(rmc.utc_time.ToShortTimeString());
                        writer.WriteLine(buf.ToString());
                    }

                    writer.Close();
                } 
            }
        }

        internal static string[] splitToParts(string source)
        {
            string[] parts = source.Split(',');
            for(int idx=0;idx < parts.Length;idx++)
            {
                parts[idx] = parts[idx].Trim();
            }
            return parts;
        }

        /// <summary>
        /// Convert Delphi Date and time string to the DateTime object
        /// </summary>
        /// <param name="dTime"></param>
        /// <returns></returns>
        internal static DateTime fromDelphiTime(string dTime)
        {
            DateTime date = new DateTime(1899, 12, 30, 0, 0, 0, DateTimeKind.Utc);
            
            string[] parts = dTime.Split('.');
            if (parts.Length < 2)
                return DateTime.UtcNow;

            date = date.AddDays(int.Parse(parts[0]));
            date = date.AddSeconds(86400.0 * NMEACommand.getDouble("0." + parts[1]));
            return date;
        }

        internal static string toDelphiTime(DateTime date, System.Globalization.NumberFormatInfo nf)
        {
            DateTime start = new DateTime(1899, 12, 30, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan delta = date - start;
            return delta.TotalDays.ToString("F7", nf);
        }

        #endregion

        #region IFormatLoader Members

        public bool isOurFormat(GMView.GPS.TrackFileInfo info)
        {
            string first_line = string.Empty;

            System.IO.TextReader reader = info.openReader();

            try
            {
                    first_line = reader.ReadLine();
                    if (string.IsNullOrEmpty(first_line))
                        return false;
                    if (first_line.StartsWith("OziExplorer Track Point File"))
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

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new OZIPltLoader();
        }

        #endregion

    }
}
