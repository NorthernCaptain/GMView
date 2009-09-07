using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView.GPS
{
    public class TrackPositionInformer: ISprite
    {
        protected bool shown = false;
        protected ImageDot cursor_img;
        protected Object cursor_tex = null;
        protected ImageDot arrows;
        protected Object arrows_tex = null;
        protected ImageDot window_img;
        protected Object window_tex = null;
        protected IGLFont fnt;
        protected Color labelcolor;
        protected Color infocolor;

        protected double lon;
        protected double lat;
        protected MapObject mapo;
        protected GPSTrack.FindContext findctx = new GPSTrack.FindContext();
        protected GPSTrack.PointInfo pointnfo = new GPSTrack.PointInfo();

        public TrackPositionInformer(MapObject imapo)
        {
            mapo = imapo;
            TextureFactory.singleton.onInited += initGLData;
        }

        public void initGLData()
        {
            cursor_img = TextureFactory.singleton.getImg(TextureFactory.TexAlias.CrossHair1);
            cursor_tex = TextureFactory.singleton.getTex(cursor_img);
            arrows = TextureFactory.singleton.getImg(TextureFactory.TexAlias.ArrowPoint);
            arrows_tex = TextureFactory.singleton.getTex(arrows); //GL_LINEAR
            window_img = TextureFactory.singleton.getImg(TextureFactory.TexAlias.PointBG);
            window_tex = TextureFactory.singleton.getTex(window_img);
            fnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Sans10B);
            labelcolor = Color.FromArgb(200, 200, 190);
            infocolor = Color.FromArgb(255, 190, 34);
        }

        public void doAction(Point mouse_p)
        {
            mapo.getLonLatByVisibleXY(mouse_p, out lon, out lat);

            findctx.init(lon, lat);

            GPSTrackFactory.singleton.findNearestTrack(ref findctx);
            if (findctx.nearest != null)
            {

                lon = findctx.nearest.Value.lon;
                lat = findctx.nearest.Value.lat;

                Point xy;
                mapo.getVisibleXYByLonLat(lon, lat, out xy);

                int dx = xy.X - mouse_p.X;
                int dy = xy.Y - mouse_p.Y;

                int ll = (int)(Math.Sqrt(dx * dx + dy * dy));
                if (ll > 20)
                {
                    findctx.nearest = null;
                    hide();
                    GML.device.repaint();
                    return;
                }

                pointnfo.fill_all_info(findctx);
                show();
            }

            GML.device.repaint();
        }

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
            
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
            
        }

        public void glDraw(int centerx, int centery)
        {
            if (!shown || findctx.nearest == null)
                return;
            Point xy;
            mapo.getVisibleXYByLonLat(lon, lat, out xy);
            if (xy.X < 0 || xy.Y < 0 || xy.X > centerx*2 || xy.Y > centery * 2)
                return;

            if (findctx.nearest != null)
            {
                GML.device.pushMatrix();
                GML.device.translate(xy.X - centerx, centery - xy.Y, 0);
                GML.device.rotateZ(-findctx.nearest.Value.dir_angle);
                GML.device.texDrawBegin();
                GML.device.texDraw(arrows_tex, -arrows.delta_x, arrows.delta_y, 3, arrows.img.Width, arrows.img.Height);
                GML.device.texDrawEnd();
                GML.device.popMatrix();

                int capx = xy.X - centerx;
                int capy = centery - xy.Y;

                int wl = window_img.delta_x;
                int wh = window_img.delta_y;
                const int dl = 15;

                if (capx + wl + dl >= centerx)
                    capx = centerx - wl;
                else
                    capx += dl;

                if (capy - wh - dl <= -centery)
                    capy = -centery + wh;
                else
                    capy = capy - dl; 



                GML.device.pushMatrix();
                GML.device.identity();
                GML.device.texDrawBegin();
                GML.device.texDraw(window_tex, capx, capy, 3, window_img.img.Width, window_img.img.Height);
                GML.device.texDrawEnd();

                capx += 20;
                capy = capy - 20;


                GML.device.color(labelcolor);
                fnt.draw("Date:", capx, capy, 0);
                fnt.draw("Speed:", capx, capy - 20, 0);
                fnt.draw("Dist from start:", capx, capy - 40, 0);
                fnt.draw("Time from start:", capx, capy - 60, 0);
                fnt.draw("Dist from LWP:", capx, capy - 80, 0);
                fnt.draw("Time from LWP:", capx, capy - 100, 0);

                GML.device.color(infocolor);
                capx += 185;
                fnt.drawright(pointnfo.point_time, capx, capy, 0);
                fnt.drawright(pointnfo.point_speed, capx, capy - 20, 0);
                fnt.drawright(pointnfo.dist_from_start, capx, capy - 40, 0);
                fnt.drawright(pointnfo.time_from_start, capx, capy - 60, 0);
                fnt.drawright(pointnfo.dist_from_lwp, capx, capy - 80, 0);
                fnt.drawright(pointnfo.time_from_lwp, capx, capy - 100, 0);
                GML.device.color(Color.White);

                GML.device.popMatrix();


            }
/*
            GML.device.pushMatrix();
            GML.device.translate(xy.X - centerx, centery - xy.Y, 0);
            GML.device.rotateZ(-Program.opt.angle);
            GML.device.texDrawBegin();
            GML.device.texFilter(cursor_tex, TexFilter.Pixel);
            GML.device.texDraw(cursor_tex, -cursor_img.delta_x, cursor_img.delta_y, 3, cursor_img.img.Width, cursor_img.img.Height);
            GML.device.texDrawEnd();
            GML.device.popMatrix();
 */
        }

        public int dLevel
        {
            get
            {
                return 3;
            }
            set
            {
                
            }
        }

        public void show()
        {
            shown = true;
        }

        public void hide()
        {
            shown = false;
        }

        #endregion
    }
}
