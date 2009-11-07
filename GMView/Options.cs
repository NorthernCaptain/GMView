using System;
using System.Collections.Generic;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ncGeo;

namespace GMView
{
    class Options
    {
        public delegate void OnChangedDelegate();
        public event OnChangedDelegate onChanged;

        public static readonly string progname = "GMView";
        private static string progdataDir;

        public BaseGeo[] geoSystem = new BaseGeo[(int)MapTileType.MaxValue];
        static Options()
        {
            progdataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    progname);
        }

        public Options()
        {
            image_path = getProgDir("Blocks");
            default_track_dir = getProgDir("Tracks_and_Waypoints");
            geoSystem[(int)MapTileType.MapOnly] = new GoogleGeo();
            geoSystem[(int)MapTileType.SatMap] = new GoogleGeo();
            geoSystem[(int)MapTileType.SatStreet] = new GoogleGeo();
            geoSystem[(int)MapTileType.OSMMapnik] = new GoogleGeo();
            geoSystem[(int)MapTileType.OSMRenderer] = new GoogleGeo();
            geoSystem[(int)MapTileType.TerMap] = new YandexGeo();
            geoSystem[(int)MapTileType.YandexMap] = new YandexGeo();
            geoSystem[(int)MapTileType.YandexSat] = new YandexGeo();
            geoSystem[(int)MapTileType.YandexSatSteet] = new YandexGeo();
            geoSystem[(int)MapTileType.YandexTraffic] = new YandexGeo();
            geoSystem[(int)MapTileType.GooTraffic] = new GoogleGeo();
            Load();
            ncUtils.DBConnPool.singleton.dbName = getDBName();
        }

        /// <summary>
        /// Return current geo coordinate system, that depends on map type
        /// </summary>
        /// <returns></returns>
        public BaseGeo getGeoSystem()
        {
            return geoSystem[(int)mapTileType];
        }

        /// <summary>
        /// return copy of coordinate system for requested map type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public BaseGeo getGeoSystem(MapTileType t)
        {
            return geoSystem[(int)t].copy();
        }


        /// <summary>
        /// Return traffic system for this type of map or null if unavailable
        /// </summary>
        /// <param name="mapT"></param>
        /// <returns></returns>
        public BaseTraffic getTrafficSystem(MapTileType mapT)
        {
            return geoSystem[(int)mapT].trafficSystem;
        }

        public static string getProgDir(string subpath)
        {
            if (subpath == null)
                return progdataDir;
            return Path.Combine(progdataDir, subpath);
        }

        public static string getConfigDir()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    progname);
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
            }
            return path;
        }

        private static string getFileName()
        {
            string path = getConfigDir();
            return Path.Combine(path, "gmvOptions.cfg");
        }

        /// <summary>
        /// Return SQLite db filename (absolute)
        /// </summary>
        /// <returns></returns>
        private static string getDBName()
        {
            string path = getConfigDir();
            return Path.Combine(path, "knowhere.sq3");
        }

        /// <summary>
        /// Reads options from isolated storage. Called in constructor
        /// </summary>
        void Load()
        {
            string filename = getFileName();

            try
            {
                // Checks to see if the options.txt file exists.
                    // Opens the file because it exists.
                    System.IO.StreamReader reader = null;
                    try
                    {
                        reader = new System.IO.StreamReader(filename);

                        // Reads the values stored.
                        image_path = reader.ReadLine();
                        file_mask = reader.ReadLine();
                        need_zoom_dir = bool.Parse(reader.ReadLine());
                        start_x = int.Parse(reader.ReadLine());
                        start_y = int.Parse(reader.ReadLine());
                        cur_zoom_lvl = int.Parse(reader.ReadLine());
                        lon = double.Parse(reader.ReadLine()) % 180.0;
                        lat = double.Parse(reader.ReadLine()) % 180.0;
                        for (int i = 0; i < goohttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            goohttp[i] = str.Length == 0 ? goohttp[i] : str;
                        }
                        for (int i = 0; i < httpproxy.Length; i++)
                        {
                            string str = reader.ReadLine();
                            httpproxy[i] = str.Length == 0 ? httpproxy[i] : str;
                        }
                        use_online = bool.Parse(reader.ReadLine());
                        httpproxy_idx = int.Parse(reader.ReadLine());
                        mapType = (MapTileType)int.Parse(reader.ReadLine());
                        for (int i = 0; i < sathttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            sathttp[i] = str.Length == 0 ? sathttp[i] : str;
                        }
                        for (int i = 0; i < terrainhttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            terrainhttp[i] = str.Length == 0 ? terrainhttp[i] : str;
                        }
                        for (int i = 0; i < streethttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            streethttp[i] = str.Length == 0 ? streethttp[i] : str;
                        }
                        nmea_com_num = int.Parse(reader.ReadLine());
                        nmea_com_speed = int.Parse(reader.ReadLine());
                        nmea_log = bool.Parse(reader.ReadLine());
                        all_err = bool.Parse(reader.ReadLine());
                        all_log = bool.Parse(reader.ReadLine());
                        for (int i = 0; i < osmmapnik.Length; i++)
                        {
                            string str = reader.ReadLine();
                            osmmapnik[i] = str.Length == 0 ? osmmapnik[i] : str;
                        }
                        for (int i = 0; i < osmarenderer.Length; i++)
                        {
                            string str = reader.ReadLine();
                            osmarenderer[i] = str.Length == 0 ? osmarenderer[i] : str;
                        }
                        show_gps_info = bool.Parse(reader.ReadLine());
                        show_sat_info = bool.Parse(reader.ReadLine());
                        use_gps = bool.Parse(reader.ReadLine());
                        is_full_screen = bool.Parse(reader.ReadLine());
                        gps_follow_map = bool.Parse(reader.ReadLine());
                        mini_delta_zoom = int.Parse(reader.ReadLine());
                        mini_position.X = int.Parse(reader.ReadLine());
                        mini_position.Y = int.Parse(reader.ReadLine());
                        mini_size.Width = int.Parse(reader.ReadLine());
                        mini_size.Height = int.Parse(reader.ReadLine());
                        show_mini_map = bool.Parse(reader.ReadLine());
                        show_zoom_panel = bool.Parse(reader.ReadLine());
                        useGML = (GML.GMLType)(int.Parse(reader.ReadLine()));
                        fog_of_war = bool.Parse(reader.ReadLine());
                        manual_avg_speed = double.Parse(reader.ReadLine());
                        show_wind_rose = bool.Parse(reader.ReadLine());
                        load_with_mt = bool.Parse(reader.ReadLine());
                        authorS = reader.ReadLine();
                        emailS = reader.ReadLine();
                        default_track_dir = reader.ReadLine();
                        do_autosave = bool.Parse(reader.ReadLine());
                        km_or_miles = double.Parse(reader.ReadLine());
                        split_by_distance = double.Parse(reader.ReadLine());
                        split_by_date = bool.Parse(reader.ReadLine());
                        dash_right_side = bool.Parse(reader.ReadLine());
                        emulate_gps = bool.Parse(reader.ReadLine());
                        emu_nmea_file = reader.ReadLine();
                        dynamic_center = (DynCenterType)int.Parse(reader.ReadLine());
                        for (int i = 0; i < yamapshttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            yamapshttp[i] = str.Length == 0 ? yamapshttp[i] : str;
                        }
                        for (int i = 0; i < yasathttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            yasathttp[i] = str.Length == 0 ? yasathttp[i] : str;
                        }
                        for (int i = 0; i < yastreethttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            yastreethttp[i] = str.Length == 0 ? yastreethttp[i] : str;
                        }
                        for (int i = 0; i < yatrafhttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            yatrafhttp[i] = str.Length == 0 ? yatrafhttp[i] : str;
                        }
                        {
                            string str = reader.ReadLine();
                            yatraftimehttp = str.Length == 0 ? yatraftimehttp : str;
                        }
                        for (int i = 0; i < gootrafhttp.Length; i++)
                        {
                            string str = reader.ReadLine();
                            gootrafhttp[i] = str.Length == 0 ? gootrafhttp[i] : str;
                        }
                        {
                            string str = reader.ReadLine();
                            if (str.Length > 0)
                                nightColor = Color.FromArgb(int.Parse(str));
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.Err("Options: Cannot read options " + ex.ToString());
                    }
                    finally
                    {
                        reader.Close();
                    }
            }
            catch (Exception ex)
            {
                Program.Err("Options: Cannot read options " + ex.ToString());
            }
        }

        /// <summary>
        /// Writes our options into isolated storage
        /// </summary>
        public void Save()
        {
            string filename = getFileName();
            // Creates the options file and writes the button options to it.
            System.IO.StreamWriter writer = null;
            try
            {
                writer = new System.IO.StreamWriter(filename);

                //Here we write our options back to file

                writer.WriteLine(image_path);
                writer.WriteLine(file_mask);
                writer.WriteLine(need_zoom_dir.ToString());
                writer.WriteLine(start_x.ToString());
                writer.WriteLine(start_y.ToString());
                writer.WriteLine(cur_zoom_lvl.ToString());
                writer.WriteLine(lon.ToString());
                writer.WriteLine(lat.ToString());
                for (int i = 0; i < goohttp.Length; i++)
                {
                    writer.WriteLine(goohttp[i]);
                }
                for (int i = 0; i < httpproxy.Length; i++)
                {
                    writer.WriteLine(httpproxy[i]);
                }
                writer.WriteLine(use_online.ToString());
                writer.WriteLine(httpproxy_idx.ToString());
                writer.WriteLine(((int)(mapType)).ToString());
                for (int i = 0; i < sathttp.Length; i++)
                {
                    writer.WriteLine(sathttp[i]);
                }
                for (int i = 0; i < terrainhttp.Length; i++)
                {
                    writer.WriteLine(terrainhttp[i]);
                }
                for (int i = 0; i < streethttp.Length; i++)
                {
                    writer.WriteLine(streethttp[i]);
                }
                writer.WriteLine(nmea_com_num.ToString());
                writer.WriteLine(nmea_com_speed.ToString());
                writer.WriteLine(nmea_log.ToString());
                writer.WriteLine(all_err.ToString());
                writer.WriteLine(all_log.ToString());
                for (int i = 0; i < osmmapnik.Length; i++)
                {
                    writer.WriteLine(osmmapnik[i]);
                }
                for (int i = 0; i < osmarenderer.Length; i++)
                {
                    writer.WriteLine(osmarenderer[i]);
                }
                writer.WriteLine(show_gps_info.ToString());
                writer.WriteLine(show_sat_info.ToString());
                writer.WriteLine(use_gps.ToString());
                writer.WriteLine(is_full_screen.ToString());
                writer.WriteLine(gps_follow_map.ToString());
                writer.WriteLine(mini_delta_zoom.ToString());
                writer.WriteLine(mini_position.X.ToString());
                writer.WriteLine(mini_position.Y.ToString());
                writer.WriteLine(mini_size.Width.ToString());
                writer.WriteLine(mini_size.Height.ToString());
                writer.WriteLine(show_mini_map.ToString());
                writer.WriteLine(show_zoom_panel.ToString());
                writer.WriteLine(((int)useGML).ToString());
                writer.WriteLine(fog_of_war.ToString());
                writer.WriteLine(manual_avg_speed.ToString("F2"));
                writer.WriteLine(show_wind_rose.ToString());
                writer.WriteLine(load_with_mt.ToString());
                writer.WriteLine(authorS);
                writer.WriteLine(emailS);
                writer.WriteLine(default_track_dir);
                writer.WriteLine(do_autosave.ToString());
                writer.WriteLine(km_or_miles.ToString());
                writer.WriteLine(split_by_distance.ToString());
                writer.WriteLine(split_by_date.ToString());
                writer.WriteLine(dash_right_side.ToString());
                writer.WriteLine(emulate_gps.ToString());
                writer.WriteLine(emu_nmea_file);
                writer.WriteLine(((int)(dynamic_center)).ToString());
                for (int i = 0; i < yamapshttp.Length; i++)
                {
                    writer.WriteLine(yamapshttp[i]);
                }
                for (int i = 0; i < yasathttp.Length; i++)
                {
                    writer.WriteLine(yasathttp[i]);
                }
                for (int i = 0; i < yastreethttp.Length; i++)
                {
                    writer.WriteLine(yastreethttp[i]);
                }
                for (int i = 0; i < yatrafhttp.Length; i++)
                {
                    writer.WriteLine(yatrafhttp[i]);
                }
                writer.WriteLine(yatraftimehttp);
                for (int i = 0; i < gootrafhttp.Length; i++)
                {
                    writer.WriteLine(gootrafhttp[i]);
                }
                writer.WriteLine(nightColor.ToArgb().ToString());
            }
            catch (Exception ex)
            {
                Program.Err("Options: Cannot write options " + ex.ToString());
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// We need to call this method after update to our options.
        /// </summary>
        public void Updated()
        {
            Save();
            if (onChanged != null)
                onChanged();
        }

        /// <summary>
        /// Color for night view mode
        /// </summary>
        public Color nightColor = Color.FromArgb(0x5087cefa);

        /// <summary>
        /// Is in night view mode
        /// </summary>
        public bool isNightView = false;

        // Maximum zooming level
        public int max_zoom_lvl = 18;

        public int cur_zoom_lvl = 2;

        // Image (map piece) sizes - length
        public int image_len = 256;

        // Image (map piece) sizes - height
        public int image_hei = 256;

        //Path to cached images of maps
        public string image_path = @"C:\GMView\Blocks";

        public string file_mask = "x={X}&y={Y}&zoom={Zi}.{EXT}";
        
        /// <summary>
        /// Do we need zoom dir name in path or no
        /// Example zoom=14 -> D:\Maps\14\...
        /// </summary>
        public bool need_zoom_dir = true;

        //Just for testing
        public int start_x = 2466;
        public int start_y = 3013;

        //Longitude and Latitute of lat visited point
        public double lon = 0.0;
        public double lat = 0.0;

        //Use internet connection for online downloading of images
        public bool use_online = false;
        public string [] goohttp = 
        {
          @"http://mt0.google.com/mt?n=404&v=w2.69&hl=ru&x={X}&y={Y}&zoom={Zi}&s=G",
          @"http://mt1.google.com/mt?n=404&v=w2.69&hl=ru&x={X}&y={Y}&zoom={Zi}&s=Ga",
          @"http://mt2.google.com/mt?n=404&v=w2.69&hl=ru&x={X}&y={Y}&zoom={Zi}&s=Gal"
        };

        public string[] terrainhttp =
        {
            @"http://mt0.google.com/mt?n=404&v=w2p.64&hl=en&x={X}&y={Y}&zoom={Zi}&s=G",
            @"http://mt1.google.com/mt?n=404&v=w2p.64&hl=en&x={X}&y={Y}&zoom={Zi}&s=Ga",
            @"http://mt2.google.com/mt?n=404&v=w2p.64&hl=en&x={X}&y={Y}&zoom={Zi}&s=Gal"
        };

        public string[] sathttp =
        {
            @"http://kh0.google.com/kh?n=404&v=25&hl=en&t={TRSQ}",
            @"http://kh1.google.com/kh?n=404&v=25&hl=en&t={TRSQ}",
            @"http://kh2.google.com/kh?n=404&v=25&hl=en&t={TRSQ}"
        };

        public string[] streethttp =
        {
            @"http://mt0.google.com/mt?n=404&v=w2t.69&hl=en&x={X}&y={Y}&zoom={Zi}&s=G",
            @"http://mt1.google.com/mt?n=404&v=w2t.69&hl=en&x={X}&y={Y}&zoom={Zi}&s=Ga",
            @"http://mt2.google.com/mt?n=404&v=w2t.69&hl=en&x={X}&y={Y}&zoom={Zi}&s=Gal"
        };

        public string[] osmmapnik = 
        {
            @"http://a.tile.openstreetmap.org/{Z}/{X}/{Y}.png",
            @"http://b.tile.openstreetmap.org/{Z}/{X}/{Y}.png",
            @"http://c.tile.openstreetmap.org/{Z}/{X}/{Y}.png"
        };

        public string[] osmarenderer = 
        {
            @"http://a.tah.openstreetmap.org/Tiles/tile.php/{Z}/{X}/{Y}.png",
            @"http://b.tah.openstreetmap.org/Tiles/tile.php/{Z}/{X}/{Y}.png",
            @"http://c.tah.openstreetmap.org/Tiles/tile.php/{Z}/{X}/{Y}.png"
        };

        public string[] yamapshttp = 
        {
            @"http://vec01.maps.yandex.ru/tiles?l=map&v=2.7.1&x={X}&y={Y}&z={Z}",
            @"http://vec02.maps.yandex.ru/tiles?l=map&v=2.7.1&x={X}&y={Y}&z={Z}",
            @"http://vec03.maps.yandex.ru/tiles?l=map&v=2.7.1&x={X}&y={Y}&z={Z}"
        };

        public string[] yasathttp = 
        {
            @"http://sat01.maps.yandex.ru/tiles?l=sat&v=1.10.0&x={X}&y={Y}&z={Z}",
            @"http://sat02.maps.yandex.ru/tiles?l=sat&v=1.10.0&x={X}&y={Y}&z={Z}",
            @"http://sat03.maps.yandex.ru/tiles?l=sat&v=1.10.0&x={X}&y={Y}&z={Z}"
        };

        public string[] yastreethttp = 
        {
            @"http://vec01.maps.yandex.ru/tiles?l=skl&v=2.7.1&x={X}&y={Y}&z={Z}",
            @"http://vec02.maps.yandex.ru/tiles?l=skl&v=2.7.1&x={X}&y={Y}&z={Z}",
            @"http://vec03.maps.yandex.ru/tiles?l=skl&v=2.7.1&x={X}&y={Y}&z={Z}"
        };

        public string[] yatrafhttp = 
        {
            @"http://trf.maps.yandex.net/tiles?l=trf&x={X}&y={Y}&z={Z}&tm={Timet}",
            @"http://trf.maps.yandex.net/tiles?l=trf&x={X}&y={Y}&z={Z}&tm={Timet}",
            @"http://trf.maps.yandex.net/tiles?l=trf&x={X}&y={Y}&z={Z}&tm={Timet}"
        };

        public string yatraftimehttp = @"http://trf.maps.yandex.net/trf/stat.js?id={RND}";

        public string[] gootrafhttp = 
        {
            @"http://mt1.google.com/mapstt?zoom={Z}&x={X}&y={Y}&client=google",
            @"http://mt2.google.com/mapstt?zoom={Z}&x={X}&y={Y}&client=google",
            @"http://mt3.google.com/mapstt?zoom={Z}&x={X}&y={Y}&client=google"
        };

        public string[] httpproxy = { "http://hostname:port", "unknown", "empty" };

        enum ProxyNum { NoProxy = -2, DefaultProxy = -1, Proxy1 = 0, Proxy2 = 1, Proxy3 = 2};
        public int httpproxy_idx = (int)ProxyNum.DefaultProxy; //index of proxies: -2 = None

        /// <summary>
        /// File in plain NMEA format for our GPS emulation device
        /// </summary>
        public string emu_nmea_file = "";

        /// <summary>
        /// Do we use a real GPS receiver or we should emulate it
        /// </summary>
        public bool emulate_gps = false;

        //Type of tile map
        private MapTileType mapTileType = MapTileType.MapOnly;

        public MapTileType mapType
        {
            get { return mapTileType; }
            set { mapTileType = value; getGeoSystem().zoomLevel = cur_zoom_lvl; if (onMapTypeChanged != null) onMapTypeChanged(); }
        }

        public event OnChangedDelegate onMapTypeChanged;

        static readonly string [] MapTileNames = { "Map only", 
                                                     "Satellite", 
                                                     "OSM mapnik", 
                                                     "Yandex Map", 
                                                     "Yandex Satellite", 
                                                     "Yandex Sat streets",
                                                     "Sat streets", 
                                                     "Terrain", 
                                                     "OSM renderer",
                                                     "Yandex traffic", 
                                                     "Google traffic"
                                                 };

        public string MapTileTypeString
        {
            get
            {
                return MapTileNames[(int)mapType];
            }
        }

        static readonly string [] MapTileDirs = {"MAP", "SAT", "STREET", "TERRAIN", "OSMAPNIK", 
                                                    "OSMAREND", "YMAP", "YSAT", "YSTREET", "YTRAF", "GTRAF"};
        public string MapTileDir(MapTileType tp)
        {
                return MapTileDirs[(int)tp];
        }

        public string MapPath(MapTileType tp)
        {
                return image_path + Path.DirectorySeparatorChar + MapTileDir(tp);
        }

        private int cur_goo_ = 0;
        public int cur_goo
        {
            get
            {
                int val = cur_goo_;
                cur_goo_++;
                if(cur_goo_ >= goohttp.Length)
                    cur_goo_ = 0;
                return val;
            }
        } //current goo site we need to use for images.

        public static string program_version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string program_full_name
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public int nmea_com_num = 6;
        public int nmea_com_speed = 4800;
        public bool use_gps = true;
        public bool nmea_log = true;
        public bool all_log = true;
        public bool all_err = true;

        /// <summary>
        /// if we have two coordinates and there diff is less that gps_same_pos_delta
        /// then we think that this is the same position.
        /// </summary>
        public double gps_same_pos_delta = 0.0003; //in degress of lon/lat

        public bool show_gps_info = true;
        public bool show_sat_info = true;
        public bool is_full_screen = false;
        public bool gps_follow_map = false;
        public bool gps_rotate_map = false;

        //mini map window parameters;
        public int mini_delta_zoom = -2;
        public Point mini_position = new Point(0, 0);
        public Size mini_size = new Size(350,250);
        public bool show_mini_map = true;
        public bool show_zoom_panel = false;
        public string[] command_line_args;
        public bool dash_right_side = false;
        public object glEmptyTexture;

        public GML.GMLType useGML = GML.GMLType.openGL;

        public void initGLData()
        {
            Bitmap img = global::GMView.Properties.Resources.white;
            glEmptyTexture = GML.device.texFromBitmap(ref img);
        }

        public UserControl newMapDrawControl(MapObject mapo, SatelliteForm satForm, GPSInfoPanel gpsinfo)
        {
            switch (useGML)
            {
                case GML.GMLType.simpleGDI:
                    try
                    {
                        IGML igml = GML.loadIGML("GMLgdiPlus.dll");
                        if (igml == null)
                            throw new NullReferenceException("No IGML object");
                        return (UserControl)igml;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not load GDIPlus library! Exiting from application.\n\nException:\n" + ex.ToString() + "\nLibrary: GMLOpenGL.dll",
                            "Error loading GML library", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                case GML.GMLType.openGL:
                    try
                    {
                        IGML igml = GML.loadIGML("GMLOpenGL.dll");
                        if (igml == null)
                            throw new NullReferenceException("No IGML object");
                        return (UserControl)igml;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not load OpenGL library! Falling back to simple GDI.\n\nException:\n" + ex.ToString() + "\nLibrary: GMLOpenGL.dll",
                            "Error loading GML library", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                    /*
                case GML.GMLType.direct3D:
                    return new D3DUControl();
                     */
                    
                case GML.GMLType.direct3D:
                    try
                    {
                        IGML igml = GML.loadIGML("GMLDirect3D.dll");
                        if (igml == null)
                            throw new NullReferenceException("No IGML object");
                        return (UserControl)igml;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Can't load Direct3D library! Falling back to simple GDI.\n\nException:\n" + ex.ToString() + "\nLibrary: GMLDirect3D.dll",
                            "Error loading GML library", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
            useGML = GML.GMLType.simpleGDI;
            return newMapDrawControl(mapo, satForm, gpsinfo);    
        }

        public ImgTile newImgTile(int ix_idx, int iy_idx, int izoom_lvl, MapTileType itype)
        {
            return new GLImgTile(ix_idx, iy_idx, izoom_lvl, itype);
        }

        public UserPosition newUserPosition(MapObject mapo, ImageDot imd)
        {
            return new GLUserPosition(mapo, imd);
        }

        public UserPosition newUserPosition(MapObject mapo)
        {
            return new GLUserPosition(mapo);
        }

        public UserSelectionArea newUserSelectionArea(MapObject mapo)
        {
            return new GLUserSelectionArea(mapo);
        }

        private double rotate_angle = 0.0;
        public double angle
        {
            get { return rotate_angle; }
            set { rotate_angle = value % 360.0; }
        }

        public double zero_speed = 2.0; //zero speed in km/h: if our speed is less that zero_speed then we stay

        public bool fog_of_war = true; //show fog of non loaded areas (aka fog of war in games)
        public bool show_fname_on_image = false; //show file name on image blocks (for debug)
        public double manual_avg_speed = 40.0; //average speed for manual tracks in km/h
        public bool show_wind_rose = true;
        public bool load_with_mt = true; //load map tiles using multithreading technology

        private string authorS = "";
        public string author
        {
            get
            {
                if (authorS == "")
                {
                    string uname = Environment.UserName;
                    return uname.Substring(0, 1).ToUpper() + uname.Substring(1) + " at " + Environment.MachineName;
                }
                else
                    return authorS;
            }

            set { authorS = value; }
        }

        private string emailS = "";
        public string email
        {
            get
            {
                if (emailS == "")
                {
                    return Environment.UserName + "@" + Environment.MachineName;
                }
                else
                    return emailS;
            }

            set { emailS = value; }
        }

        public string default_track_dir = @"C:\GMView\Tracks_and_Waypoints";

        /// <summary>
        /// Name of autosave file without directory.
        /// </summary>
        private string autosavefileS = "autosave.gpx";

        /// <summary>
        /// Gets full path and name of autosave track file
        /// </summary>
        public string autosavefile
        {
            get { return default_track_dir + Path.DirectorySeparatorChar + autosavefileS; }
            set
            {
            	autosavefileS = value + ".gpx";
            }
        }

        public bool do_autosave = false;
        public double km_or_miles = 1.0; // need data in km or miles. 1.6093
        public double split_by_distance = 4.0; // divide track into separate ones when distance between points is greater than this (4.0 km)
        public bool split_by_date = true; // split one track into many if we have different dates between points
        public enum DynCenterType { StaticCenter, Forward34, Forward23, SpeedDriven };
        public DynCenterType dynamic_center = DynCenterType.SpeedDriven; //type of centering the map on the screen
        public bool showTraffic = false; //Show traffic on the roads or no
    }
}
