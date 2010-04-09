using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;
using ncGeo;

namespace GMView
{
    public enum ImgStatus { Unknown, InMemory, NeedDiskLoad, NeedDownload, ForceDownload, NoFile, AskDownload };
    public enum ImgFlags { None, 
                           NoDirectDisplay = 1 //We don not display this tile on the screen, we use it so merging
        };

    public class ImgTile
    {
        public static int access_id = 1;

        protected const long Ticks2sec = 10000000L;
        /// <summary>
        /// Number of seconds for each map tile after which we consider tile as old and need to reload it from web
        /// Time from creation date of a file is used to compare.
        /// </summary>
        protected static long[] loadExpiredTicks = new long[] 
                {
                    15552000      * Ticks2sec, //MapOnly
                    15552000      * Ticks2sec, //SatMap
                    15552000      * Ticks2sec, //SatStreet
                    15552000      * Ticks2sec, //TerMap
                    5184000       * Ticks2sec, //OSMMapnik
                    5184000       * Ticks2sec, //OSMRender
                    15552000      * Ticks2sec, //YandexMap
                    15552000      * Ticks2sec, //YandexSat
                    15552000      * Ticks2sec, //YandexSatStreet
                    600           * Ticks2sec, //YandexTraffic
                    600           * Ticks2sec, //GoogleTraffic
                };

        /// <summary>
        /// If we failed and could not load tile from web - how often could we try again - this table tells us.
        /// </summary>
        protected static long[] failedExpiredTicks = new long[] 
                {
                    43200      * Ticks2sec, //MapOnly
                    43200      * Ticks2sec, //SatMap
                    43200      * Ticks2sec, //SatStreet
                    43200      * Ticks2sec, //TerMap
                    600        * Ticks2sec, //OSMMapnik
                    600        * Ticks2sec, //OSMRender
                    43200      * Ticks2sec, //YandexMap
                    43200      * Ticks2sec, //YandexSat
                    43200      * Ticks2sec, //YandexSatStreet
                    300        * Ticks2sec, //YandexTraffic
                    300        * Ticks2sec, //GoogleTraffic
                };

        protected static bool[] staticMap = new bool []
                {
                    true, //MapOnly
                    true, //SatMap
                    true, //SatStreet
                    true, //TerMap
                    true, //OSMMapnik
                    true, //OSMRender
                    true, //YandexMap
                    true, //YandexSat
                    true, //YandexSatStreet
                    false, //YandexTraffic
                    false, //GoogleTraffic
                };



        protected int x_idx;
        protected int y_idx;
        protected int zoom_lvl;

        protected MapTileType tile_type;

        public Bitmap img = null;

        public ImgStatus status = ImgStatus.Unknown;

        public ulong hash = 0ul;

        public int our_access_id = 0;

        public int flags = 0;

        public delegate Bitmap LoadAddons(ImgTile forMe);
        /// <summary>
        /// If this delegate is not null then we'll merge image it returns on top of our own tile
        /// </summary>
        public static LoadAddons loadAddon;

        /// <summary>
        /// Date and time when tile was loaded (in Ticks)
        /// </summary>
        public long loadedAtTicks = 0L;
        /// <summary>
        /// Date and time when we failed to load this tile (in Ticks)
        /// </summary>
        public long failToLoadTicks = 0L;

        public ImgTile(int ix_idx, int iy_idx, int izoom_lvl, MapTileType itype)
        {
            x_idx = ix_idx;
            y_idx = iy_idx;
            zoom_lvl = izoom_lvl;
            tile_type = itype;
            hash = getHash();
        }

        /// <summary>
        /// Return true if it's time to try again to download previously failed tile
        /// </summary>
        /// <returns></returns>
        public bool isDownloadTryValid()
        {
            if (Program.opt.use_online && ncUtils.Glob.lastRefreshTicks - failToLoadTicks > failedExpiredTicks[(int)map_type])
                return true;
            return false;
        }

        /// <summary>
        /// Return true if this tile is too old and we need to reload it from the web again
        /// </summary>
        /// <returns></returns>
        public bool isTimeExpired()
        {
            if (zoom_lvl > 9 && Program.opt.use_online && staticMap[(int)map_type] == false
                && ncUtils.Glob.lastRefreshTicks - loadedAtTicks > loadExpiredTicks[(int)map_type])
                return true;
            return false;
        }

        public int x
        {
            get { return x_idx; }
        }

        public int y
        {
            get { return y_idx; }
        }

        /// <summary>
        /// Constructs long hash code from given x, y, and zoom of image
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl">zoom level of the image</param>
        /// <returns></returns>
        static public ulong getHash(int x_idx, int y_idx, int zoom_lvl, MapTileType type)
        {
            return ((ulong)(zoom_lvl) << 57) | ((ulong)(type) << 52) | ((ulong)(x_idx) << 22) | (ulong)(y_idx);
        }

        public ulong getHash()
        {
            return ((ulong)(zoom_lvl) << 57) | ((ulong)(tile_type) << 52) | ((ulong)(x_idx) << 22) | (ulong)(y_idx);
        }

        /// <summary>
        /// Loads Image information from disk (from cached directory)
        /// </summary>
        /// <returns>return false if no file was found or other error</returns>
        public bool loadFromDisk()
        {
            return loadFromDisk(true);
        }

        /// <summary>
        /// Loads image tile from disk. If call_bitmap_loaded = true then loads into texture
        /// </summary>
        /// <param name="call_bitmap_loaded"></param>
        /// <returns></returns>
        public bool loadFromDisk(bool call_bitmap_loaded)
        {
            string fname = getFileName();
            //Program.Log("Loading image, filename: " + fname);
            try
            {
                img = new Bitmap(fname);
                if (img == null)
                    throw new ApplicationException("Null image loaded");

                if (call_bitmap_loaded)
                {

                    if (loadAddon != null)
                    {
                        Bitmap addonImg = loadAddon(this);
                        if (addonImg != null)
                        {
                            Graphics gr = Graphics.FromImage(img);
                            gr.DrawImage(addonImg, 0, 0, Program.opt.image_len, Program.opt.image_hei);
                            gr.Dispose();
                            addonImg.Dispose();
                        }
                    }

                    if (Program.opt.show_fname_on_image)
                    {
                        img = addFileInfo(img);
                    }

                    bitmap_loaded();

                }
                else
                    if (Program.opt.show_fname_on_image)
                        img = addFileInfo(img);

                lock (this)
                {
                    status = ImgStatus.InMemory;
                }
                try
                {
                    DateTime created = File.GetCreationTime(fname);
                    loadedAtTicks = created.Ticks;
                }
                catch
                {
                }
            }
            catch (Exception)
            {
                //Program.Err("Error loading " + fname + ", exc:" + ex.Message);

                lock (this)
                {
                    if (status != ImgStatus.NoFile)
                        status = ImgStatus.ForceDownload;
                    else
                        failToLoadTicks = DateTime.Now.Ticks;
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// Draw file info on the tile
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private Bitmap addFileInfo(Bitmap img)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bmp);
            Font fnt = FontFactory.singleton.getGDIFont(FontFactory.FontAlias.Sans10B);
            gr.DrawImage(img, 0, 0, 256, 256);
            gr.DrawLine(Pens.DarkRed, 0, 0, 75, 0);
            gr.DrawLine(Pens.DarkRed, 0, 0, 0, 30);
            gr.DrawString(getFileNameOnly(), fnt, Brushes.DarkRed, 1, 16);
            gr.DrawString(getPathPart(), fnt, Brushes.DarkRed, 1, 1);
            gr.Dispose();
            img.Dispose();
            return bmp;
        }

        /// <summary>
        /// Virtual method that calls after image loading, we need it for loading textures into the video card
        /// </summary>
        public virtual void bitmap_loaded()
        {
        }

        /// <summary>
        /// Do we have this tile on disk in our cached directories
        /// </summary>
        /// <returns></returns>
        public bool haveOnDisk()
        {
            string fname = getFileName();
            return System.IO.File.Exists(fname);
        }

        /// <summary>
        /// Download required image from internet using http and proxy,
        /// stores this file in our directory structure and loads it into memory.
        /// </summary>
        /// <param name="webclient"></param>
        /// <returns>Downloaded image</returns>
        public bool downloadOnline(WebClient webclient)
        {
            return downloadOnlyOnline(webclient);
        }

        /// <summary>
        /// Only downloads file from inet to the disk cache, but do not load this file into memory.
        /// </summary>
        /// <param name="webclient"></param>
        /// <returns></returns>
        public bool downloadOnlyOnline(WebClient webclient)
        {
            string goohttp;

            switch (tile_type)
            {
                case MapTileType.MapOnly:
                    goohttp = Program.opt.goohttp[Program.opt.cur_goo];
                    break;
                case MapTileType.SatMap:
                    goohttp = Program.opt.sathttp[Program.opt.cur_goo];
                    break;
                case MapTileType.TerMap:
                    goohttp = Program.opt.terrainhttp[Program.opt.cur_goo];
                    break;
                case MapTileType.SatStreet:
                    goohttp = Program.opt.streethttp[Program.opt.cur_goo];
                    break;
                case MapTileType.OSMMapnik:
                    goohttp = Program.opt.osmmapnik[Program.opt.cur_goo];
                    break;
                case MapTileType.OSMRenderer:
                    goohttp = Program.opt.osmarenderer[Program.opt.cur_goo];
                    break;
                case MapTileType.YandexMap:
                    goohttp = Program.opt.yamapshttp[Program.opt.cur_goo];
                    break;
                case MapTileType.YandexSat:
                    goohttp = Program.opt.yasathttp[Program.opt.cur_goo];
                    break;
                case MapTileType.YandexSatSteet:
                    goohttp = Program.opt.yastreethttp[Program.opt.cur_goo];
                    break;
                case MapTileType.YandexTraffic:
                    goohttp = Program.opt.yatrafhttp[Program.opt.cur_goo];
                    break;
                case MapTileType.GooTraffic:
                    goohttp = Program.opt.gootrafhttp[Program.opt.cur_goo];
                    break;

                default:
                    goohttp = Program.opt.goohttp[Program.opt.cur_goo];
                    break;
            }


            string fname = getFileName();

            string dirname = getPath(x_idx, y_idx, zoom_lvl, tile_type);
            if (!Directory.Exists(dirname))
                try
                {
                    Directory.CreateDirectory(dirname);
                }
                catch (DirectoryNotFoundException)
                {
                }

            goohttp = goohttp.Replace("{X}", x_idx.ToString()).Replace("{Y}", y_idx.ToString()).Replace("{Z}", (zoom_lvl - 1).ToString());
            goohttp = goohttp.Replace("{Zi}", (Program.opt.max_zoom_lvl - zoom_lvl).ToString());
            goohttp = goohttp.Replace("{Yi}", ((1 << (zoom_lvl-1)) - 1 - y_idx).ToString());
            goohttp = goohttp.Replace("{TRSQ}", getTRSQ(x_idx, y_idx, zoom_lvl));
            
            if (goohttp.IndexOf("{Timet}") > 0)
            {
                BaseTraffic traf = Program.opt.getTrafficSystem(tile_type);
                if (traf != null)
                    goohttp = goohttp.Replace("{Timet}", traf.getTimetInfo(webclient));
            }

            webclient.Headers.Add("User-Agent", @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en; rv:1.8.1.12) Gecko/20080201 Firefox/2.0.0.12.l");
            webclient.Headers.Add("Accept", @"text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5");

            try
            {
                bool exists = System.IO.File.Exists(fname);
                if (!exists || status == ImgStatus.ForceDownload)
                {
                    Program.Log("INET Downloading file: " + goohttp);
                    webclient.DownloadFile(goohttp, fname + "-part");
                    if(exists)
                    {
                        try
                        {
                            System.IO.File.Delete(fname);
                        }
                        catch
                        {
                        }
                    }
                    System.IO.File.Move(fname + "-part", fname);
                    System.IO.File.SetCreationTime(fname, DateTime.Now);
                }
                lock (this)
                {
                    status = ImgStatus.NeedDiskLoad;
                }
                ImgCacheManager.singleton.addEntry(x_idx, y_idx, zoom_lvl, tile_type);
                ImgCacheManager.singleton.cache(tile_type, zoom_lvl).saveIf(20); //Save to disk after every 20 new entries
                return true;
            }
            catch (Exception ex)
            {
                Program.Err("WebClient got error: " + ex.ToString());
                lock (this)
                {
                    status = ImgStatus.NoFile;
                    failToLoadTicks = DateTime.Now.Ticks;
                }
                return false;
            }
        }

        /// <summary>
        /// Constructs file name from given x, y, and zoom of image
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl"></param>
        /// <returns>full path to the image including its name</returns>
        public static string getFileName(int x_idx, int y_idx, int zoom_lvl, MapTileType type)
        {
            string fname = Program.opt.file_mask;
            fname = fname.Replace("{X}", x_idx.ToString()).Replace("{Y}", y_idx.ToString()).Replace("{Z}", zoom_lvl.ToString());
            fname = fname.Replace("{Zi}", (Program.opt.max_zoom_lvl - zoom_lvl).ToString());
            fname = fname.Replace("{TRSQ}", getTRSQ(x_idx, y_idx, zoom_lvl));
            fname = fname.Replace("{Yi}", ((1 << zoom_lvl) - y_idx).ToString());
            if (type == MapTileType.SatMap || type == MapTileType.YandexSat)
                fname = fname.Replace("{EXT}", "jpg");
            else
                fname = fname.Replace("{EXT}", "png");

            fname = getPath(x_idx, y_idx, zoom_lvl, type) + Path.DirectorySeparatorChar + fname;

            //Program.Log("TRSQ for: " + x_idx + ", " + y_idx + "/" + zoom_lvl + "=" + getTRSQ(x_idx, y_idx, zoom_lvl));
            return fname;
        }

        public static string getPath(int zoom_lvl, MapTileType type)
        {
            if (Program.opt.need_zoom_dir)
                return Program.opt.MapPath(type) + Path.DirectorySeparatorChar + zoom_lvl.ToString("D2");
            else
                return Program.opt.MapPath(type) + Path.DirectorySeparatorChar + Program.opt.MapTileDir(type);
        }

        public static string getPath(int x, int y, int zoom_lvl, MapTileType type)
        {
            string s;
            if (Program.opt.need_zoom_dir)
                s = Program.opt.MapPath(type) + Path.DirectorySeparatorChar + zoom_lvl.ToString("D2");
            else
                s = Program.opt.MapPath(type) + Path.DirectorySeparatorChar + Program.opt.MapTileDir(type);
            s += Path.DirectorySeparatorChar + (x / 10).ToString("D5") + Path.DirectorySeparatorChar + (y / 100).ToString("D4");
            return s;
        }

        public string getPathPart()
        {
            return zoom_lvl.ToString("D2") + Path.DirectorySeparatorChar + (x_idx / 10).ToString("D5") + Path.DirectorySeparatorChar + (y_idx / 100).ToString("D4");
        }

        public string getFileName()
        {
            string fname = Program.opt.file_mask;
            fname = fname.Replace("{X}", x_idx.ToString()).Replace("{Y}", y_idx.ToString()).Replace("{Z}", zoom_lvl.ToString());
            fname = fname.Replace("{Zi}", (Program.opt.max_zoom_lvl - zoom_lvl).ToString());
            fname = fname.Replace("{TRSQ}", getTRSQ(x_idx, y_idx, zoom_lvl));
            fname = fname.Replace("{Yi}", ((1 << zoom_lvl) - y_idx).ToString());
            if (tile_type == MapTileType.SatMap || tile_type == MapTileType.YandexSat)
                fname = fname.Replace("{EXT}", "jpg");
            else
                fname = fname.Replace("{EXT}", "png");

            fname = getPath(x_idx, y_idx, zoom_lvl, tile_type) + Path.DirectorySeparatorChar + fname;

            //Program.Log("TRSQ for: " + x_idx + ", " + y_idx + "/" + zoom_lvl + "=" + getTRSQ(x_idx, y_idx, zoom_lvl));
            return fname;
        }

        public string getFileNameOnly()
        {
            string fname = Program.opt.file_mask;
            fname = fname.Replace("{X}", x_idx.ToString()).Replace("{Y}", y_idx.ToString()).Replace("{Z}", zoom_lvl.ToString());
            fname = fname.Replace("{Zi}", (Program.opt.max_zoom_lvl - zoom_lvl).ToString());
            fname = fname.Replace("{TRSQ}", getTRSQ(x_idx, y_idx, zoom_lvl));
            fname = fname.Replace("{Yi}", ((1 << zoom_lvl) - y_idx).ToString());
            if (tile_type == MapTileType.SatMap || tile_type == MapTileType.YandexSat)
                fname = fname.Replace("{EXT}", "jpg");
            else
                fname = fname.Replace("{EXT}", "png");
            return fname;
        }

        /// <summary>
        /// Generate trsq name from given x, y tile number and zooming level
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl"></param>
        /// <returns></returns>
        static string getTRSQ(int x_idx, int y_idx, int zoom_lvl)
        {
            int mid = 1 << (zoom_lvl - 1);
            if (x_idx < 0 || x_idx > mid - 1)
            {
                x_idx %= mid;
                if (x_idx < 0)
                    x_idx += mid;
            }

            string result = "t";
            for (int i = 2; i <= zoom_lvl; i++)
            {
                mid >>= 1;
                if (y_idx < mid)
                {
                    if (x_idx < mid)
                    {
                        result += "q";
                    }
                    else
                    {
                        result += "r";
                        x_idx -= mid;
                    }
                }
                else
                {
                    if (x_idx < mid)
                    {
                        result += "t";
                    }
                    else
                    {
                        result += "s";
                        x_idx -= mid;
                    }
                    y_idx -= mid;
                }
            }
            return result;
        }

        private static int parseInt(string name, ref int start_idx)
        {
            string tmp="";
            while (start_idx < name.Length && Char.IsDigit(name[start_idx]))
                tmp += name[start_idx++];
            start_idx--;
            return int.Parse(tmp);
        }

        public static void parseTRSQ(string name, MapTileType mtype, ref int start_idx, 
            out int x, out int y, out int zoom)
        {
            string valids = "trsq";
            x = 0;
            y = 0;
            zoom = 1;
            start_idx++;
            while (start_idx < name.Length && valids.Contains(name[start_idx].ToString()))
            {
                char c = name[start_idx++];
                x <<= 1;
                y <<= 1;
                zoom++;
                switch (c)
                {
                    case 't':
                        y += 1;
                        break;
                    case 'r':
                        x += 1;
                        break;
                    case 's':
                        x += 1;
                        y += 1;
                        break;
                    case 'q':
                        break;
                    default:
                        start_idx--;
                        return;
                }
            }
            start_idx--;
        }

        public static bool parseName(string name, int start_idx, string fmt, MapTileType mtype, 
            out int ix, out int iy, out int izoom)
        {
            ix = 0;
            iy = 0;
            izoom = 0;

            int ifmt = 0;

            for (; start_idx < name.Length; start_idx++)
            {
                if (fmt[ifmt] == '{') //template found
                {
                    string tmp = "";
                    ifmt++;
                    while (fmt[ifmt] != '}')
                        tmp += fmt[ifmt++];
                    ifmt++;
                    switch (tmp)
                    {
                        case "X":
                            ix = parseInt(name, ref start_idx);
                            break;
                        case "Y":
                            iy = parseInt(name, ref start_idx);
                            break;
                        case "Z":
                            izoom = parseInt(name, ref start_idx);
                            break;
                        case "Zi":
                            izoom = Program.opt.max_zoom_lvl - parseInt(name, ref start_idx);
                            break;
                        case "TRSQ":
                            parseTRSQ(name, mtype, ref start_idx, out ix, out iy, out izoom);
                            break;
                        case "EXT":
                            return true;
                    }
                }
                else
                    if (fmt[ifmt++] != name[start_idx])
                        return false;

            }
            return true;
        }

        public virtual void draw(Graphics gr, int ix, int iy)
        {
            gr.DrawImageUnscaled(img, ix, iy);
        }

        public int zoom
        {
            get { return zoom_lvl; }
        }

        public MapTileType map_type
        {
            get { return tile_type; }
        }

        public virtual void dispose()
        {
            if (img != null)
            {
                img.Dispose();
                img = null;
                status = ImgStatus.Unknown;
            }
        }


        internal void forceDownload()
        {
            string fname = getFileName();
            try
            {
                dispose();
                System.IO.File.Delete(fname);
            }
            catch
            {
            }
            status = ImgStatus.ForceDownload;
        }
    };

}
