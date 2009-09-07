using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GMView
{
    public class MiniTilesDash: DashBoardBase
    {
        public event MethodInvoker onZoomIn;
        public event MethodInvoker onZoomOut;

        public delegate void onCenterMapXYDelegate(int x, int y, int zoom);
        public event onCenterMapXYDelegate onCenterMapXY;


        const int minimap_x = 92;
        const int minimap_y = 92;
        const int minimap_w = 153;
        const int minimap_h = 145;

        private MapObject mapo;
        private UserPosition upos;
        private Point start_p = new Point();
        private Point end_p = new Point();
        private IGLFont zoomfnt;
        private IGLFont llfnt;

        private object[] zoomtex;
        private ImageDot zoomimg;
        private Color textcol1;

        public MiniTilesDash(MapObject imapo, UserPosition iupos)
        {
            mapo = imapo;
            upos = iupos;
            zoomtex = new object[Program.opt.max_zoom_lvl];
            for (int i = 0; i < Program.opt.max_zoom_lvl; i++)
                zoomtex[i] = TextureFactory.singleton.getTex((TextureFactory.TexAlias)(TextureFactory.TexAlias.Zoom00 + i));
            zoomimg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.Zoom00);
            justif = DashBoardContainer.Justify.Top | DashBoardContainer.Justify.Right;
            bgImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashMiniTilesBg);
            bgTex = TextureFactory.singleton.getTex(bgImg);
            bgImgWrapped = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashMiniTilesWrapped);
            bgTexWrapped = TextureFactory.singleton.getTex(bgImgWrapped);
            zoomfnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Sans18R);
            llfnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Sans12R);
            dheight = bgImg.delta_y;
            dwidth = bgImg.delta_x;
            textcol1 = Color.FromArgb(74, 254, 44);
            addCallers();
        }

        protected override void addCallers()
        {
            base.addCallers();
            // + zoomIn key
            mouseUpCallers.add(new DashRect(107, 36, 18, 18, new DashRect.OnMatchDelegate(clickZoomIn)));
            // - zoomOut key
            mouseUpCallers.add(new DashRect(28, 200, 15, 15, new DashRect.OnMatchDelegate(clickZoomOut)));
            mouseUpCallers.add(new DashRect(minimap_x,minimap_y,minimap_w,minimap_h, new DashRect.OnMatchDelegate(clickMiniMap)));
            mouseDownCallers.add(new DashRect(minimap_x-10, minimap_y-10, minimap_w+20, minimap_h+20, new DashRect.OnMatchDelegate(downMiniMap)));
            mouseDoubleClickCallers.add(new DashRect(minimap_x, minimap_y, minimap_w, minimap_h, new DashRect.OnMatchDelegate(doubleMiniMap)));
            mouseMoveCallers.add(new DashRect(minimap_x, minimap_y, minimap_w, minimap_h, new DashRect.OnMatchDelegate(moveMiniMap)));
        }

        #region IDashBoard Members

        public override void draw(int ix, int iy)
        {
            if (dmode == DashMode.Normal)
            {
                GML.device.texDrawBegin();
                GML.device.texFilter(bgTex, TexFilter.Pixel);
                GML.device.texDraw(bgTex, ix, iy, 0, bgImg.img.Width, bgImg.img.Height);
                GML.device.texDrawEnd();
                GML.device.texDrawBegin();
                GML.device.texFilter(bgTex, TexFilter.Pixel);
                ImgCacheManager.singleton.glDraw(ix + minimap_x, iy - minimap_y);
                GML.device.texDraw(zoomtex[mapo.zoom-1], ix  + zoomimg.delta_x, iy - zoomimg.delta_y, 0, zoomimg.img.Width, zoomimg.img.Height);
                GML.device.texDrawEnd();

                GML.device.color(textcol1);
                zoomfnt.draw(mapo.zoom.ToString("D2"), ix + 61, iy - 25, 0);
                GML.device.color(textcolor);
                llfnt.draw(upos.LatS, ix + 189, iy - 31, 0);
                llfnt.draw(upos.LonS, ix + 189, iy - 49, 0);
                {
                    int width = end_p.X - start_p.X;
                    int height = end_p.Y - start_p.Y;
                    int x = start_p.X, y = start_p.Y;

                    if (width == 0 || height == 0)
                    {
                        GML.device.color(Color.White);
                        return;
                    }

                    if (width < 0)
                    {
                        width = -width;
                        x = end_p.X;
                    }

                    if (height < 0)
                    {
                        height = -height;
                        y = end_p.Y;
                    }

                    GML.device.rectDraw(ix + x, iy - y, width, height, 0, Color.FromArgb(205, Color.Yellow));
                }
                GML.device.color(Color.White);
            }
            else
                base.draw(ix, iy);
        }

        #endregion

        protected bool clickZoomIn(Point xy)
        {
            if (onZoomIn != null)
                onZoomIn();
            return true;
        }

        protected bool clickZoomOut(Point xy)
        {
            if (onZoomOut != null)
                onZoomOut();
            return true;
        }

        protected bool doubleMiniMap(Point xy)
        {
            ImgCacheManager.RenderInfo nfo = ImgCacheManager.singleton.curRenderInfo;

            int nx = (xy.X - minimap_x) / ImgCacheManager.singleton.delta;
            int ny = (xy.Y - minimap_y) / ImgCacheManager.singleton.delta;

            int kx = (nfo.start_nx + nx) * Program.opt.image_len + Program.opt.image_len / 2;
            int ky = (nfo.start_ny + ny) * Program.opt.image_hei + Program.opt.image_hei / 2;

            if (onCenterMapXY != null)
                onCenterMapXY(kx, ky, nfo.zoom);

            return true;
        }

        protected bool clickMiniMap(Point xy)
        {
            ImgCacheManager.RenderInfo nfo = ImgCacheManager.singleton.curRenderInfo;
            if (nfo == null)
                return true;

            int width = end_p.X - start_p.X;
            int height = end_p.Y - start_p.Y;
            int x = start_p.X, y = start_p.Y;

            if (width == 0 || height == 0)
            {
                return true;
            }

            if (width < 0)
            {
                width = -width;
                x = end_p.X;
            }

            if (height < 0)
            {
                height = -height;
                y = end_p.Y;
            }

            xy.X = minimap_x;
            xy.Y = minimap_y;
            //xy = GML.translateAbsToScene(xy);

            int nnx = (start_p.X - xy.X) / ImgCacheManager.singleton.delta + nfo.start_nx;
            int nny = (start_p.Y - xy.Y) / ImgCacheManager.singleton.delta + nfo.start_ny;

            int nw = width / ImgCacheManager.singleton.delta;
            int nh = height / ImgCacheManager.singleton.delta;
            
            ImgCollector.LoadTask loadqueue = new ImgCollector.LoadTask(mapo);

            for (int iy = 0; iy < nh; iy++)
            {
                for (int ix = 0; ix < nw; ix++)
                {
                    loadqueue.tiles.Enqueue(Program.opt.newImgTile(nnx + ix, nny +iy, nfo.zoom, nfo.mType));
                }
            }

            mapo.schedDownloadTask(loadqueue);
            start_p = end_p;
            return true;
        }

        protected bool downMiniMap(Point xy)
        {
            start_p = GML.translateAbsToScene(xy);
            start_p = xy;
            end_p = start_p;
            return true;
        }

        protected bool moveMiniMap(Point dxy)
        {
            end_p=GML.translateAbsToScene(dxy);
            end_p = dxy;
            return true;
        }
    }
}
