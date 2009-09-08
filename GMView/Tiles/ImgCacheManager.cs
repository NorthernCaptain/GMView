using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace GMView
{
    public class ImgCacheManager
    {
        private static ImgCacheManager instance = new ImgCacheManager();

        ImgDirInfo[,] info;
        private MapObject mapo = null;

        private ImageDot bgImg;
        private Brush noEntryPen;
        private Brush hasEntryPen;
        private Brush highlightEntryPen;
        private Brush backPen;
        private int curZoom = 15;
        private MapTileType mType = MapTileType.MapOnly;
        private int last_nx = 0, last_ny = 0;
        private int last_nw = 0, last_nh = 0;
        private int maxZoomTiles;
        internal int delta = 4;
        private Queue<Bitmap> availableImg = new Queue<Bitmap>();

        private Bitmap curImg = null;
        private Object curTex = null;
        private Control resultControl;
        private RenderInfo curNfo = new RenderInfo();

        private Object semi64Tex, semi128Tex;
        private Object empty64Tex, empty128Tex;
        private System.Windows.Forms.Timer updateTimer;

        public event MethodInvoker onVisualChanged;

        Thread visualizer = new Thread(new ParameterizedThreadStart(processVisualStub));

        internal class RenderInfo
        {
            internal int vis_nx;
            internal int vis_ny;
            internal int size_nw;
            internal int size_nh;
            internal int zoom;
            internal MapTileType mType;
            internal int start_nx;
            internal int start_ny;
            internal Bitmap img;

            internal RenderInfo()
            {
            }

            internal RenderInfo(int ix, int iy, int iw, int ih, int izoom, MapTileType imtype)
            {
                vis_nx = ix;
                vis_ny = iy;
                size_nw = iw;
                size_nh = ih;
                zoom = izoom;
                mType = imtype;
                start_nx = -1;
                start_ny = -1;
                img = null;
            }

            internal RenderInfo(int ix, int iy, int iw, int ih, int izoom, MapTileType imtype,
                            int istart_nx, int istart_ny)
            {
                vis_nx = ix;
                vis_ny = iy;
                size_nw = iw;
                size_nh = ih;
                zoom = izoom;
                mType = imtype;
                start_nx = istart_nx;
                start_ny = istart_ny;
                img = null;
            }
        }
        ncUtils.SyncQueue<RenderInfo> visTasks = new ncUtils.SyncQueue<RenderInfo>();
        private delegate void onSwitchImgDelegate(RenderInfo newImg);

        public ImgCacheManager()
        {
            info = new ImgDirInfo[(int)MapTileType.MaxValue, Program.opt.max_zoom_lvl];
            for(int i = 0;i<(int)MapTileType.MaxValue;i++)
                for (int z = 0; z < Program.opt.max_zoom_lvl; z++)
                {
                    info[i, z] = new ImgDirInfo((MapTileType)i, z + 1);
                }
            Program.opt.onChanged += saveIf;
            Program.onShutdown += onShutdown;

            bgImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.TilesBack);
            TextureFactory.singleton.onInited += initGLData;

            noEntryPen = new SolidBrush(Color.FromArgb(64,84,108));
            highlightEntryPen = Brushes.White;
            hasEntryPen = new SolidBrush(Color.FromArgb(156, 172, 195));
            backPen = new SolidBrush(Color.FromArgb(25, 45, 80));

            Program.opt.onMapTypeChanged += new Options.OnChangedDelegate(opt_onMapTypeChanged);
            visualizer.IsBackground = true;
            visualizer.Start(this);
            this.updateTimer = new System.Windows.Forms.Timer();
            this.updateTimer.Interval = 300;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
        }

        public int zoom
        {
            get { return curZoom; }
            set { curZoom = value; maxZoomTiles = 1 << (curZoom - 1); }
        }

        public MapObject map
        {
            get { return mapo; }
            set 
            { 
                mapo = value;
                mapo.onCenterChanged += mapoCenterChanged;
                mapo.onZoomChanged += new MapObject.OnZoomDelegate(mapo_onZoomChanged);
            }
        }

        public Control control
        {
            get { return resultControl; }
            set { resultControl = value; }
        }

        internal RenderInfo curRenderInfo
        {
            get { return curNfo; }
        }

        static void processVisualStub(Object o)
        {
            ImgCacheManager mgr = (ImgCacheManager)o;
            mgr.processVisualThread();
        }

        void processVisualThread()
        {
            RenderInfo nfo;
            while (true)
            {
                nfo = visTasks.DequeueAllGetLast();
                nfo = buildTileInfoImg(nfo);
                try
                {
                    if (resultControl != null)
                        resultControl.Invoke(new onSwitchImgDelegate(switchImage), new Object[] { nfo });
                }
                catch { };
            }
        }

        void opt_onMapTypeChanged()
        {
            updateVisual();
        }

        void mapo_onZoomChanged(int old_zoom, int new_zoom)
        {
            updateVisual();
        }

        public void initGLData()
        {
            semi64Tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SemiTile64);
            semi128Tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SemiTile128);
            empty64Tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SemiTile64Empty);
            empty128Tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SemiTile128Empty);
        }

        public void mapoCenterChanged(double lon, double lat)
        {
            updateVisual();
        }

        public void updateVisual()
        {
            if (mapo == null)
                return;

            if (zoom != mapo.zoom || mType != Program.opt.mapType ||
                last_nx != mapo.start_nx || last_ny != mapo.start_ny ||
                last_nw != mapo.size_nw || last_nh != mapo.size_nh)
            {
                updateTimer.Stop();
                updateTimer.Start();
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            updateTimer.Stop();
            updateVisualForced();
        }

        public void updateVisualForced()
        {
            zoom = mapo.zoom;
            mType = Program.opt.mapType;
            last_nx = mapo.start_nx;
            last_ny = mapo.start_ny;
            last_nw = mapo.size_nw;
            last_nh = mapo.size_nh;

            visTasks.Enqueue(new RenderInfo(last_nx, last_ny, last_nw, last_nh, curZoom, mType));
        }

        RenderInfo buildTileInfoImg(RenderInfo nfo)
        {
            int total_nx = bgImg.delta_x / delta;
            int total_ny = bgImg.delta_y / delta;
            int maxZoomTiles = 1 << (nfo.zoom - 1);

            int start_x = nfo.start_nx;
            int start_y = nfo.start_ny;

            if (start_x < 0)
            {
                start_x = (nfo.vis_nx + nfo.size_nw / 2) - total_nx / 2;
                start_y = (nfo.vis_ny + nfo.size_nh / 2) - total_ny / 2;
            }

            if (start_x < 0)
                start_x = 0;
            if (start_y < 0)
                start_y = 0;

            int end_x = start_x + total_nx -1;
            int end_y = start_y + total_ny -1;

            if (end_x >= maxZoomTiles)
                end_x = maxZoomTiles -1;
            if (end_y >= maxZoomTiles)
                end_y = maxZoomTiles - 1;

            int img_x = bgImg.delta_x / 2 - (end_x - start_x + 1) * delta / 2;
            int img_y = bgImg.delta_y / 2 - (end_y - start_y + 1) * delta / 2; 

            ImgDirInfo di = info[(int)nfo.mType, nfo.zoom - 1];
            lock (GML.lock2D)
            {
                Bitmap img = getAvailableImg();

                Graphics gr = Graphics.FromImage(img);
                gr.Clear(Color.Transparent);
                gr.FillRectangle(backPen, 0, 0, bgImg.delta_x, bgImg.delta_y);

                for (int y = start_y; y <= end_y; y++)
                {
                    for (int x = start_x; x <= end_x; x++)
                    {
                        Brush p1;
                        if (x >= nfo.vis_nx && x < nfo.vis_nx + nfo.size_nw &&
                            y >= nfo.vis_ny && y <= nfo.vis_ny + nfo.size_nh)
                            p1 = highlightEntryPen;
                        else
                            if (di.haveEntry(ImgTile.getHash(x, y, nfo.zoom, nfo.mType)))
                            {
                                p1 = hasEntryPen;
                            }
                            else
                                p1 = noEntryPen;
                        gr.FillRectangle(p1, img_x + (x - start_x) * delta, img_y + (y - start_y) * delta, delta - 1, delta - 1);
                    }
                }
                gr.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);
                nfo.img = img;
                nfo.start_nx = start_x;
                nfo.start_ny = start_y;
                nfo.size_nw = end_x - start_x + 1;
                nfo.size_nh = end_y - start_y + 1;
            }
            return nfo;
        }

        private Bitmap getAvailableImg()
        {
            lock (availableImg)
            {
                if (availableImg.Count > 0)
                    return availableImg.Dequeue();
            }

            Bitmap img = new Bitmap(bgImg.img.Width, bgImg.img.Height, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            return img;
        }

        internal void switchImage(RenderInfo nfo)
        {
            if (curImg != null)
            {
                lock (availableImg)
                {
                    availableImg.Enqueue(curImg);
                }
                if (curTex != null)
                    GML.device.texDispose(curTex);

            }
            curImg = nfo.img;
            curNfo = nfo;
            curTex = GML.device.texFromBitmap(ref curImg);
            if (onVisualChanged != null)
                onVisualChanged();
        }

        public static ImgCacheManager singleton
        {
            get { return instance; }
        }

        public void rebuildCacheAll()
        {
            for(int i = 0;i<(int)MapTileType.MaxValue;i++)
                for (int z = 0; z < Program.opt.max_zoom_lvl; z++)
                {
                    info[i, z].rebuildCache();
                }
        }

        public void rebuildCacheType(MapTileType mtype)
        {
            int i = (int)mtype;
            for (int z = 0; z < Program.opt.max_zoom_lvl; z++)
            {
                info[i, z].rebuildCache();
            }
        }

        /// <summary>
        /// Flush all caches that have changed to disk
        /// </summary>
        public void saveIf()
        {
            for (int i = 0; i < (int)MapTileType.MaxValue; i++)
                for (int z = 0; z < Program.opt.max_zoom_lvl; z++)
                {
                    info[i, z].saveIf(0);
                }
        }

        public void reorganizeAll()
        {
            for (int i = 0; i < (int)MapTileType.MaxValue; i++)
                for (int z = 0; z < Program.opt.max_zoom_lvl; z++)
                {
                    info[i, z].reorganizeTilesOnDisk();
                }
        }

        public void loadAll()
        {
            for (int i = 0; i < (int)MapTileType.MaxValue; i++)
                for (int z = 0; z < Program.opt.max_zoom_lvl; z++)
                {
                    if (!info[i, z].load())
                        info[i, z].rebuildCache();
                }
        }

        public ImgDirInfo cache(MapTileType mtype, int zoom)
        {
            return info[(int)mtype, zoom - 1];
        }

        public void addEntry(int ix, int iy, int zoom, MapTileType mtype)
        {
            ulong hash = ImgTile.getHash(ix, iy, zoom, mtype);
            info[(int)mtype, zoom - 1].addEntry(hash);
        }

        public void onShutdown()
        {
            saveIf();
            visualizer.Abort();
            visualizer.Join();
        }

        public void glDraw(int ix, int iy)
        {
            if (curImg == null)
                return;
            if (curTex == null)
                curTex = GML.device.texFromBitmap(ref curImg);
            GML.device.texFilter(curTex, TexFilter.Pixel);
            GML.device.texDraw(curTex, ix, iy, 0, curImg.Width, curImg.Height);
        }

        public void glDrawOnScreenGrid(int centerx, int centery)
        {
            int delta = 1;
            if (mapo == null || Program.opt.cur_zoom_lvl+delta > Program.opt.max_zoom_lvl)
                return;

            int imgSize = 256 >> delta;
            object tex = empty128Tex;
            object etex = semi128Tex;

            GML.device.texFilter(tex, TexFilter.Pixel);
            GML.device.texFilter(etex, TexFilter.Pixel);
            GML.device.texDrawBegin();

            int start_x = mapo.start_nx << delta;
            int start_y = mapo.start_ny << delta;
            int size_nw = mapo.size_nw << delta;
            int size_nh = map.size_nh << delta;
            int zoom = Program.opt.cur_zoom_lvl + delta;
            MapTileType mt = Program.opt.mapType;

            ImgDirInfo di = info[(int)Program.opt.mapType, Program.opt.cur_zoom_lvl+delta-1];

            int vy;
            for (int y = -2; y < size_nh+4; y++)
            {
                vy = centery - (mapo.start_p.Y + y * imgSize);
                for (int x = -2; x < size_nw+4; x++)
                {
                    if (di.haveEntry(ImgTile.getHash(start_x + x, start_y + y, zoom, mt)))
                        GML.device.texDraw(tex, mapo.start_p.X + x * imgSize - centerx,
                                vy, 0, imgSize, imgSize);
                    else
                        GML.device.texDraw(etex, mapo.start_p.X + x * imgSize - centerx,
                                vy, 0, imgSize, imgSize);
                }
            }

            GML.device.texDrawEnd();
        }
    }
}
