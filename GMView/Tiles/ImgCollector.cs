using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Net;
using System.Threading;
using System.IO;

namespace GMView
{
    public class ImgCollector
    {
        Thread downloader = new Thread(new ParameterizedThreadStart(processDownloadStub));
        Thread sched_downloader = new Thread(new ParameterizedThreadStart(processSchedDownloadStub));
        WebClient webclient = new WebClient();
        WebClient sched_webclient = new WebClient();

        /// <summary>
        /// Task for loading a list of tiles
        /// </summary>
        public class LoadTask
        {
            public enum Type { loadTask, copyTask };
            public Queue<ImgTile>   tiles;
            public MapObject mapo;
            public Type type = Type.loadTask;
            public string copyTo = null;
            public LoadTask(MapObject imapo)
            {
                mapo = imapo;
                tiles = new Queue<ImgTile>();
            }
        };

        ncUtils.SyncQueue<LoadTask> load_queue = new ncUtils.SyncQueue<LoadTask>();
        private LoadTask cur_load_buffer;

        ncUtils.SyncQueue<LoadTask> sched_queue = new ncUtils.SyncQueue<LoadTask>();


        public delegate void StartLoadTask(int total_pieces);
        public delegate void LoadTaskPartlyDone(ImgTile loaded_tile, double completed_percent);
        public delegate void FinishedLoadTask();

        public event StartLoadTask onStartLoad;
        public event LoadTaskPartlyDone onLoadProgress;
        public event FinishedLoadTask onFinishLoad;


        public event StartLoadTask onSchedStartLoad;
        public event LoadTaskPartlyDone onSchedLoadProgress;
        public event FinishedLoadTask onSchedFinishLoad;

        /// <summary>
        /// Here we store images that were already loaded into memory
        /// </summary>
        private Dictionary<ulong, ImgTile> img_map = new Dictionary<ulong, ImgTile>();


        /// <summary>
        /// Constructor of our map tiles collector
        /// </summary>
        public ImgCollector()
        {
            downloader.Start(this);
            sched_downloader.Start(this);

            diskloader1.Start(new DiskLoaderContext(diskload1, diskloadAnswer, this));
            diskloader2.Start(new DiskLoaderContext(diskload2, diskloadAnswer, this));

            Program.onShutdown += new Program.ShutdownDelegate(shutdown);
            Program.opt.onChanged += new Options.OnChangedDelegate(opt_onChanged);
            opt_onChanged();
        }

        /// <summary>
        /// Called when options changed, updates webclient proxy
        /// </summary>
        void opt_onChanged()
        {
            switch (Program.opt.httpproxy_idx)
            {
                case -2: // No proxy
                    webclient.Proxy = null;
                    sched_webclient.Proxy = null;
                    break;
                case -1: // Default IE proxy
                    webclient.Proxy = WebRequest.DefaultWebProxy;
                    sched_webclient.Proxy = WebRequest.DefaultWebProxy;
                    break;
                default: // Our own list of proxies
                    {
                        int idx = Program.opt.httpproxy_idx;
                        try
                        {
                            WebProxy prox = new WebProxy(Program.opt.httpproxy[idx], true);
                            webclient.Proxy = prox;
                            sched_webclient.Proxy = new WebProxy(Program.opt.httpproxy[idx], true);
                        }
                        catch
                        {
                        }
                    }
                    break;
            }
            InitDirCache();
        }

        /// <summary>
        /// Thread static stub for processing downloads
        /// </summary>
        /// <param name="o">ImgCollector class - owner of tiles</param>
        static void processDownloadStub(Object o)
        {
            ImgCollector ic = o as ImgCollector;

            if(ic != null)
                ic.processDownload();

        }

        static void processSchedDownloadStub(Object o)
        {
            ImgCollector ic = o as ImgCollector;

            if (ic != null)
                ic.processDownloadSched();

        }

        /// <summary>
        /// Thread task that process download operations for given queue of image tiles
        /// </summary>
        private void processDownload()
        {
            LoadTask task;
            while (true)
            {
                task = load_queue.Dequeue();

                if (onStartLoad != null)
                    onStartLoad(task.tiles.Count);

                while (task.tiles.Count != 0)
                {
                    ImgTile tile = task.tiles.Dequeue();
                    lock (tile)
                    {
                        if (tile.status == ImgStatus.InMemory)
                            continue;
                        if (task.mapo.zoom != tile.zoom)//TODO:Think how to solve || Program.opt.mapType != tile.map_type)
                        {
                            tile.status = ImgStatus.Unknown;
                            continue;
                        }
                    }

                    if (Program.opt.use_online)
                        tile.downloadOnline(webclient);
                    else
                    {
                        lock (tile)
                        {
                            tile.status = ImgStatus.Unknown;
                            continue;
                        }
                    }

                    if (onLoadProgress != null)
                        onLoadProgress(tile, 0.0);
                }

                if (onFinishLoad != null)
                    onFinishLoad();
            }
        }


        /// <summary>
        /// Thread task that process download operations for given queue of image tiles
        /// </summary>
        private void processDownloadSched()
        {
            LoadTask task;
            while (true)
            {
                task = sched_queue.Dequeue();
                int totals = task.tiles.Count;

                if (onSchedStartLoad != null)
                    onSchedStartLoad(task.tiles.Count);

                while (task.tiles.Count != 0)
                {
                    ImgTile tile = task.tiles.Dequeue();

                    switch (task.type)
                    {
                        case LoadTask.Type.loadTask:
                            loadTile(tile, task);
                            break;
                        case LoadTask.Type.copyTask:
                            copyTile(tile, task);
                            break;
                        default:
                            break;
                    }

                    if (onSchedLoadProgress != null)
                        onSchedLoadProgress(tile, (totals - task.tiles.Count) * 100.0 / totals);
                }

                if (onSchedFinishLoad != null)
                    onSchedFinishLoad();
            }
        }


        private void loadTile(ImgTile tile, LoadTask task)
        {
            lock (tile)
            {
                if (tile.status == ImgStatus.InMemory)
                    return;
            }

            if (Program.opt.use_online)
                tile.downloadOnlyOnline(sched_webclient);
            else
            {
                lock (tile)
                {
                    tile.status = ImgStatus.Unknown;
                    tile.failToLoadTicks = DateTime.Now.Ticks;
                    return;
                }
            }
        }

        /// <summary>
        /// Copy tile to the destination folder (task.copyTo)
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="task"></param>
        private void copyTile(ImgTile tile, LoadTask task)
        {
            string curname = tile.getFileName();
            if (!File.Exists(curname))
                return; //nothing to copy

            string outname = task.copyTo + Path.DirectorySeparatorChar + tile.getPathPart();
            try
            {
                if(!Directory.Exists(outname))
                    Directory.CreateDirectory(outname);
            }
            catch
            {
            }

            outname += Path.DirectorySeparatorChar + tile.getFileNameOnly();
            try
            {
                File.Copy(curname, outname, true);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Initialize directory structure
        /// </summary>
        static public void InitDirCache()
        {
            string image_path;
            try
            {
                if (!System.IO.Directory.Exists(Program.opt.image_path))
                {
                    System.IO.Directory.CreateDirectory(Program.opt.image_path);
                }

                image_path = Program.opt.MapPath(Program.opt.mapType);
                for (int i = 1; i <= Program.opt.max_zoom_lvl; i++)
                {
                    string new_dir = image_path + Path.DirectorySeparatorChar + i.ToString("D2") + Path.DirectorySeparatorChar;
                    System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(new_dir);
                    //Program.Log("MKdir: " + di.FullName + " : " + di.Exists);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Checks cache and return tile from it or creates a new tile, adds it to the cache and return.
        /// Do not load any image, can return tile with null image.
        /// Return NULL tile on error.
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ImgTile getCachedTileEntry(int x_idx, int y_idx, int zoom_lvl, MapTileType type)
        {
            ImgTile img;
            int max_piece = 1 << (zoom_lvl - 1);
            //check if we are in range
            if (x_idx >= max_piece || y_idx >= max_piece
                || x_idx < 0 || y_idx < 0)
                return null; //out of range

            ulong key = ImgTile.getHash(x_idx, y_idx, zoom_lvl, type);

            lock (img_map)
            {
                if (!img_map.TryGetValue(key, out img) || img == null)
                {
                    img = Program.opt.newImgTile(x_idx, y_idx, zoom_lvl, type);
                    img_map.Add(img.hash, img);
                }
            }

            img.our_access_id = ImgTile.access_id;
            return img;
        }

        /// <summary>
        /// Loads and return the image by given coordinates. 
        /// Cached images are not reloaded again.
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl"></param>
        /// <returns></returns>
        public ImgTile getImage(int x_idx, int y_idx, int zoom_lvl, MapTileType type)
        {
            ImgTile img = getCachedTileEntry(x_idx, y_idx, zoom_lvl, type);
            if (img == null)
                return null;

            ImgStatus istatus;
            lock(img)
            {
                istatus = img.status;
            }

            if(istatus == ImgStatus.InMemory && img.isTimeExpired())
            {
                lock(img)
                {
                    img.dispose();
                    img.status = ImgStatus.ForceDownload;
                    
                }
                scheduleDownload(img);
                return img;
            }

            if(istatus == ImgStatus.InMemory ||
               istatus == ImgStatus.NeedDownload ||
               istatus == ImgStatus.ForceDownload) //we schedule it for downloading, can't do anything better
                  return img;

            if(istatus == ImgStatus.NoFile)
            {
                if (img.isDownloadTryValid())
                {
                    lock (img)
                    {
                        img.status = ImgStatus.ForceDownload;
                    }
                    scheduleDownload(img);
                }
                return img;
            }

            if (!img.loadFromDisk() && Program.opt.use_online)
            {
                scheduleDownload(img);
            }

            return img;
        }

        /// <summary>
        /// Extract ImgTile from memory cache, or use threads to load it from disk
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ImgTile getImageThreaded(int x_idx, int y_idx, int zoom_lvl, MapTileType type)
        {
            ImgTile img = getCachedTileEntry(x_idx, y_idx, zoom_lvl, type);

            if (img == null)
                return null;

            ImgStatus istatus;
            lock (img)
            {
                istatus = img.status;
            }

            if (istatus == ImgStatus.InMemory && img.isTimeExpired())
            {
                lock (img)
                {
                    img.dispose();
                    img.status = ImgStatus.ForceDownload;

                }
                scheduleDownload(img);
                return img;
            }

            if (istatus == ImgStatus.InMemory ||
               istatus == ImgStatus.NeedDownload ||
               istatus == ImgStatus.ForceDownload) //we schedule it for downloading, can't do anything better
                return img;

            if (istatus == ImgStatus.NoFile)
            {
                if (img.isDownloadTryValid())
                {
                    lock (img)
                    {
                        img.status = ImgStatus.ForceDownload;
                    }
                    scheduleDownload(img);
                }
                return img;
            }

            loadFromDiskThreaded(img);
            return null;
        }


        ncUtils.SyncQueue<ImgTile> diskload1 = new ncUtils.SyncQueue<ImgTile>();
        ncUtils.SyncQueue<ImgTile> diskload2 = new ncUtils.SyncQueue<ImgTile>();
        ncUtils.SyncQueue<ImgTile> diskloadAnswer = new ncUtils.SyncQueue<ImgTile>();
        ncUtils.SyncQueue<ImgTile> diskloadCurrent;
        int diskloadThreadFinished;
        Thread diskloader1 = new Thread(new ParameterizedThreadStart(processDiskLoadStub));
        Thread diskloader2 = new Thread(new ParameterizedThreadStart(processDiskLoadStub));

        class DiskLoaderContext
        {
            internal ncUtils.SyncQueue<ImgTile> task_queue;
            internal ncUtils.SyncQueue<ImgTile> answer_queue;
            internal ImgCollector collector;

            internal DiskLoaderContext(ncUtils.SyncQueue<ImgTile> itask_queue,
                            ncUtils.SyncQueue<ImgTile> ianswer_queue,
                            ImgCollector icollector)
            {
                task_queue = itask_queue;
                answer_queue = ianswer_queue;
                collector = icollector;
            }
        }

        /// <summary>
        /// Thread processor: Reads ImgTiles from queue and loads them from disk
        /// </summary>
        /// <param name="o"></param>
        static void processDiskLoadStub(Object o)
        {
            DiskLoaderContext ctx = o as DiskLoaderContext;
            if (o == null)
                return;

            while (true)
            {
                ImgTile img = ctx.task_queue.Dequeue();
                if (img == null)
                {
                    ctx.answer_queue.Enqueue(null);
                    continue;
                }
                img.loadFromDisk(false);
                ctx.answer_queue.Enqueue(img);
            }
        }

        /// <summary>
        /// Push task for loading from disk to the threads
        /// </summary>
        /// <param name="img"></param>
        void loadFromDiskThreaded(ImgTile img)
        {
            diskloadCurrent = diskloadCurrent == diskload1 ? diskload2 : diskload1;
            diskloadCurrent.Enqueue(img);
        }

        /// <summary>
        /// Calls before we start threaded loading images from disk
        /// </summary>
        public void startLoadThreaded()
        {
            diskloadThreadFinished = 0;
        }

        /// <summary>
        /// Send finish signal to our loading threads
        /// </summary>
        public void endLoadThreaded()
        {
            diskload1.Enqueue(null);
            diskload2.Enqueue(null);
        }

        /// <summary>
        /// Pops loaded image from back-queue, calls bitma_loaded()
        /// or return null if no more images loaded
        /// </summary>
        /// <returns></returns>
        public ImgTile popLoaded()
        {
            ImgTile img = diskloadAnswer.Dequeue();
            if (img == null)
            {
                diskloadThreadFinished++;
                if (diskloadThreadFinished > 1)
                    return null; //we are threadDone completely
                return popLoaded();
            }

            lock (img)
            {
                if (img.status == ImgStatus.InMemory)
                {
                    img.bitmap_loaded();
                    return img;
                }
                if (img.status == ImgStatus.NeedDownload || img.status == ImgStatus.ForceDownload)
                    scheduleDownload(img);
                return img;
            }
        }

        /// <summary>
        /// deletes image from cache and reloads it from inet
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl"></param>
        /// <param name="mapTileType"></param>
        internal void reloadImage(int x_idx, int y_idx, int zoom_lvl, MapTileType mapTileType)
        {
            ImgTile img = getCachedTileEntry(x_idx, y_idx, zoom_lvl, mapTileType);
            if (img == null)
                return;

            lock (img)
            {
                img.forceDownload();
            }

            scheduleDownload(img);
        }

        /// <summary>
        /// Gets bitmap file for layers construction. Loads it from tile from disk or from the web.
        /// Also checks all timeouts and can reschedule downloading again if file is too old.
        /// </summary>
        /// <param name="x_idx"></param>
        /// <param name="y_idx"></param>
        /// <param name="zoom_lvl"></param>
        /// <param name="mapTileType"></param>
        /// <returns></returns>
        public Bitmap getBitmapForLayer(int x_idx, int y_idx, int zoom_lvl, MapTileType mapTileType)
        {
            ImgTile trImg = this.getCachedTileEntry(x_idx, y_idx, zoom_lvl, mapTileType);

            if (trImg == null || trImg.status == ImgStatus.NeedDownload
                || trImg.status == ImgStatus.ForceDownload)
                return null;

            if (trImg.status == ImgStatus.NoFile && !trImg.isDownloadTryValid())
            {
                return null;
            }

            Bitmap img = null;
            if (trImg.loadFromDisk(false))
            {
                img = trImg.img;
                trImg.img = null;
                trImg.status = ImgStatus.NeedDiskLoad;
                if(!trImg.isTimeExpired())
                    return img;
            }

            if (Program.opt.use_online)
            {
                trImg.status = ImgStatus.ForceDownload;
                scheduleDownload(trImg);
            }
            else
            {
                trImg.status = ImgStatus.NoFile;
                trImg.failToLoadTicks = ncUtils.Glob.lastRefreshTicks;
            }
            return img;
        }

        /// <summary>
        /// Adds map tiles to download queue for later loading
        /// </summary>
        /// <param name="img"></param>
        private void scheduleDownload(ImgTile img)
        {
            //if we don't have buffer, then download in this thread
            if (cur_load_buffer == null)
            {
                img.downloadOnline(Program.webclient);
                return;
            }
            ImgStatus istatus;
            lock (img)
            {
                istatus = img.status;
            }

            lock(cur_load_buffer)
            {
                if(istatus == ImgStatus.NeedDownload || istatus == ImgStatus.ForceDownload)
                    cur_load_buffer.tiles.Enqueue(img);
            }
        }

        /// <summary>
        /// Creates new download task to fill download tiles info in it
        /// </summary>
        public void newDownloadTask(MapObject mapo)
        {
            cur_load_buffer = new LoadTask(mapo);
        }

        /// <summary>
        /// End of feeding to download task, let's send it to our thread
        /// </summary>
        public void endFeedingDownloadTask()
        {
            if (cur_load_buffer != null && cur_load_buffer.tiles.Count != 0)
            {
                load_queue.Enqueue(cur_load_buffer);
            }
            cur_load_buffer = null;
        }

        /// <summary>
        /// Clears all cached entries
        /// </summary>
        public void ResetCache()
        {
            Program.Log("Resetting image memory cache");
            foreach (KeyValuePair<ulong, ImgTile> pair in img_map)
                pair.Value.dispose();
            img_map.Clear();
            InitDirCache();
        }

        public void autoClear(int threshold)
        {
            threshold = ImgTile.access_id - threshold;
            foreach (KeyValuePair<ulong, ImgTile> pair in img_map)
            {
                if (pair.Value.our_access_id < threshold)
                    pair.Value.dispose();
            }
        }

        /// <summary>
        /// Callback, that calls on shutdown. Just stop our threads.
        /// </summary>
        public void shutdown()
        {
            downloader.Abort();
            downloader.Join();
            sched_downloader.Abort();
            sched_downloader.Join();
            diskloader1.Abort();
            diskloader1.Join();
            diskloader2.Abort();
            diskloader2.Join();
        }

        /// <summary>
        /// Adds new download task with a list of tiles to the scheduler downloader.
        /// Schedule downloader is our second downloader that we use for background tasks,
        /// not for downloading display units.
        /// </summary>
        /// <param name="task"></param>
        public void addSchedTask(LoadTask task)
        {
            sched_queue.Enqueue(task);
        }

    }
}
