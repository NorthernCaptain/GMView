using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GMView
{
    /// <summary>
    /// This class stores cached information about tiles we have in our directory.
    /// Directory means only one zoom level for one map tile type
    /// </summary>
    public class ImgDirInfo
    {
        string dirname;
        MapTileType mtype = MapTileType.MapOnly;
        int zoom;

        Dictionary<ulong, object> onDiskSet = new Dictionary<ulong,object>();
        object locker = new object();
        int need_save = 0;

        public ImgDirInfo(MapTileType typ, int izoom)
        {
            zoom = izoom;
            mtype = typ;
            dirname = ImgTile.getPath(zoom, mtype);
        }

        /// <summary>
        /// Scans directory and build cache of image presence.
        /// Then saves this cache to disk
        /// </summary>
        public void rebuildCache()
        {
            dirname = ImgTile.getPath(zoom, mtype);

            reorganizeTilesOnDisk();

            Program.Log("Rebuilding cache for tiles: " + Program.opt.MapTileDir(mtype) + Path.DirectorySeparatorChar + zoom.ToString("D2"));
            Directory.CreateDirectory(dirname);
            string[] files = Directory.GetFiles(dirname, "*.*", SearchOption.AllDirectories);
            Dictionary<ulong, object> loadSet = new Dictionary<ulong, object>();
            string fnameonly;
            foreach (string fname in files)
            {
                int x, y, z;
                if (fname.LastIndexOf("part") == fname.Length - 4)
                    continue;
                fnameonly = Path.GetFileName(fname);
                if (ImgTile.parseName(fnameonly, 0, Program.opt.file_mask, mtype, out x, out y, out z))
                {
                    ulong hash = ImgTile.getHash(x, y, z, mtype);
                    try
                    {
                        loadSet.Add(hash, null);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Rebuild cache ERROR: can't add entry\n" + fname + "\nx/y/z="
                            + x.ToString() + "/" + y.ToString() + "/" + z.ToString() + "\nException:\n"
                            + e.ToString(), "Build cache exception");
                    }
                }
            }
            lock (locker)
            {
                onDiskSet = loadSet;
            }
            save();
        }

        /// <summary>
        /// Reorganize tiles into subdirs
        /// </summary>
        public void reorganizeTilesOnDisk()
        {
            dirname = ImgTile.getPath(zoom, mtype);
            string fidname = dirname + Path.DirectorySeparatorChar+ "gmdir.ver";
            Directory.CreateDirectory(dirname);
            if (File.Exists(fidname))
                return;
            Program.Log("Reorganizing files in directory: " + Program.opt.MapTileDir(mtype) + Path.DirectorySeparatorChar + zoom.ToString("D2"));

            string[] files = Directory.GetFiles(dirname, "*.*", SearchOption.AllDirectories);
            string fnameonly;
            foreach (string fname in files)
            {
                int x, y, z;
                if (fname.LastIndexOf("part") == fname.Length - 4)
                    continue;
                fnameonly = Path.GetFileName(fname);
                try
                {
                    if (ImgTile.parseName(fnameonly, 0, Program.opt.file_mask, mtype, out x, out y, out z))
                    {
                        string newdir = ImgTile.getPath(x, y, z, mtype);
                        try
                        {
                            if (!Directory.Exists(newdir))
                                Directory.CreateDirectory(newdir);
                            newdir += Path.DirectorySeparatorChar.ToString() + fnameonly;
                            File.Move(fname, newdir);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Reorganize tiles ERROR: can't parse entry\n" + fname + "\nException:\n"
                        + ex.ToString(), "Reorganize exception");

                }
            }

            System.IO.StreamWriter writer = null;
            try
            {
                writer = new System.IO.StreamWriter(fidname);
                writer.WriteLine("GMDir");
                writer.WriteLine("1.3");
            }
            catch
            {
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Saves cache information to disk into _tiles.idx file
        /// </summary>
        /// <returns></returns>
        public bool save()
        {
            string fname = ImgTile.getPath(zoom, mtype) + Path.DirectorySeparatorChar+ "_tiles.idx";
            System.IO.StreamWriter writer = null;
            try
            {
                writer = new System.IO.StreamWriter(fname);
                writer.WriteLine("TileInfo");
                writer.WriteLine("1.0");
                writer.WriteLine(onDiskSet.Count);
                lock (locker)
                {
                    foreach (KeyValuePair<ulong, object> pair in onDiskSet)
                        writer.WriteLine((long)pair.Key);
                }
                need_save = 0;
            }
            catch (Exception ex)
            {
                Program.Err("ImgDirInfo: Cannot write cache file to: " + fname + " ERR: " + ex.ToString());
                return false;
            }
            finally
            {
                if(writer != null)
                    writer.Close();
            }
            return true;
        }

        /// <summary>
        /// Save cache to disk if we need it
        /// </summary>
        /// <returns>true if we saved cache, false if there is no need to save</returns>
        public bool saveIf(int threshold)
        {
            if (need_save > threshold)
            {
                save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads cache contens from _tiles.idx into memory cache
        /// </summary>
        /// <returns></returns>
        public bool load()
        {
            string fname = ImgTile.getPath(zoom, mtype) + Path.DirectorySeparatorChar + "_tiles.idx";
            Program.Log("Loading cache for tiles: " + Program.opt.MapTileDir(mtype) + Path.DirectorySeparatorChar + zoom.ToString("D2"));
            System.IO.StreamReader reader = null;
            try
            {
                reader = new System.IO.StreamReader(fname);
                reader.ReadLine(); //TileInfo header
                reader.ReadLine(); //version

                int count = int.Parse(reader.ReadLine());
                ulong key = 0;

                Dictionary<ulong, object> loadSet = new Dictionary<ulong, object>();

                while (!reader.EndOfStream)
                {
                    key = ulong.Parse(reader.ReadLine());
                    loadSet.Add(key, null);
                }
                lock (locker)
                {
                    onDiskSet = loadSet;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if(reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Adds new entry from external call
        /// </summary>
        /// <param name="hash"></param>
        public void addEntry(ulong hash)
        {
            lock (locker)
            {
                if (onDiskSet.ContainsKey(hash))
                    return;
                onDiskSet.Add(hash, null);
                need_save++;
            }
        }

        public bool haveEntry(ulong hash)
        {
            lock (locker)
            {
                if (onDiskSet.ContainsKey(hash))
                    return true;
            }
            return false;
        }

        public bool haveEntryNoLock(ulong hash)
        {
            if (onDiskSet.ContainsKey(hash))
                return true;
            return false;
        }
    }
}
