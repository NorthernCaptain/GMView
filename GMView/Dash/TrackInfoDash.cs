using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class TrackInfoDash: DashBoardBase
    {
        ImageDot textbg;
        object texttex;
        IGLFont fnt;

        GPSTrack track = null;
        GPSTrack.TextInfo info = null;
        Color labelcolor;

        public delegate void OnCenterWaypointDelegate(Way.WayPoint wp);
        public event OnCenterWaypointDelegate onCenterWaypoint;

        public TrackInfoDash()
        {
            bgImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashTrackBg);
            bgTex = TextureFactory.singleton.getTex(bgImg);
            bgImgWrapped = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashTrackWrapped);
            bgTexWrapped = TextureFactory.singleton.getTex(bgImgWrapped);
            textbg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashTextBg130);
            texttex = TextureFactory.singleton.getTex(textbg);
            fnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Sans12R);

            dheight = bgImg.delta_y;
            width = bgImg.delta_x;
            justif = DashBoardContainer.Justify.Top | DashBoardContainer.Justify.Right;
            labelcolor = Color.FromArgb(200,200,190);

            addCallers();

            GPSTrackFactory.singleton.onCurrentTrackChanged += new GPSTrackFactory.TrackChanged(Glob_onCurrentTrackChanged);
        }

        protected override void addCallers()
        {
            base.addCallers();
            mouseUpCallers.add(new DashRect(24, 24, 19, 16, new DashRect.OnMatchDelegate(clickNextTrack)));
            mouseUpCallers.add(new DashRect(225, 143, 19, 16, new DashRect.OnMatchDelegate(clickNextWayPoint)));
            mouseUpCallers.add(new DashRect(50, 143, 19, 16, new DashRect.OnMatchDelegate(clickPrevWayPoint)));
        }

        void Glob_onCurrentTrackChanged(GPSTrack gtrack)
        {
            track = gtrack;
            info = gtrack.textInfo;
        }

        public GPSTrack.TextInfo textInfo
        {
            set { info = value; }
        }

        bool clickNextTrack(Point xy)
        {
            GPSTrackFactory.singleton.stepToNextTrack();
            Way.WayPoint wp = track.way.currentWP;
            track.textInfo.fill_all_info(track);
            GML.repaint();
            if (onCenterWaypoint != null)
                onCenterWaypoint(wp);
            return true;
        }

        bool clickNextWayPoint(Point xy)
        {
            Way.WayPoint wp = track.way.stepNextPoint();
            track.textInfo.fill_all_info(track);
            GML.repaint();
            if (onCenterWaypoint != null)
                onCenterWaypoint(wp);
            return true;
        }

        bool clickPrevWayPoint(Point xy)
        {
            Way.WayPoint wp = track.way.stepPrevPoint();
            track.textInfo.fill_all_info(track);
            GML.repaint();
            if (onCenterWaypoint != null)
                onCenterWaypoint(wp);
            return true;
        }

        public override void draw(int ix, int iy)
        {
            if (dmode == DashMode.Normal)
            {
                int tx = ix + rwidth - 125;
                int ty = iy - 45;
                int rightx = ix + width - 22;
                int capx = ix + delta_border_x + 15;
                GML.device.texDrawBegin();
                GML.device.texFilter(bgTex, TexFilter.Pixel);
                GML.device.texDraw(bgTex, ix, iy, 0, bgImg.img.Width, bgImg.img.Height);
                ix += delta_border_x;
                GML.device.texDraw(texttex, tx, ty, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 25, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 50, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 75, 0, textbg.delta_x, textbg.delta_y);
                //waypoint field backgrounds
                GML.device.texDraw(texttex, tx, ty - 120, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 145, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 170, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 195, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 220, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDraw(texttex, tx, ty - 245, 0, textbg.delta_x, textbg.delta_y);
                GML.device.texDrawEnd();
                GML.device.color(labelcolor);
                fnt.draw("Distance:", capx, ty, 0);
                fnt.draw("Travel time:", capx, ty - 25, 0);
                fnt.draw("Start date:", capx, ty - 50, 0);
                fnt.draw("Avg speed:", capx, ty - 75, 0);
                fnt.draw("Lat / Lon:", capx, ty - 120, 0);
                fnt.draw("Date:", capx, ty - 145, 0);
                fnt.draw("Dist from start:", capx, ty - 170, 0);
                fnt.draw("Time from start:", capx, ty - 195, 0);
                fnt.draw("Dist from prev:", capx, ty - 220, 0);
                fnt.draw("Dist to next:", capx, ty - 245, 0);
                if (info == null || info.track_name == null)
                    return;

                GML.device.color(textcolor);
                fnt.draw(info.track_name, ix + 22, iy - 24, 0);

                fnt.drawright(info.total_distance, rightx, ty, 0);
                fnt.drawright(info.total_time, rightx, ty - 25, 0);
                fnt.drawright(info.start_time, rightx, ty - 50, 0);
                fnt.drawright(info.avg_speed, rightx, ty - 75, 0);
                fnt.draw(info.waypoint_num, ix + 96, ty - 98, 0);
                fnt.drawright(info.waypoint_lonlat, rightx, ty - 120, 0);
                fnt.drawright(info.waypoint_time, rightx, ty - 145, 0);
                fnt.drawright(info.waypoint_distance_from_start, rightx, ty - 170, 0);
                fnt.drawright(info.waypoint_time_from_start, rightx, ty - 195, 0);
                fnt.drawright(info.waypoint_distance_from_prev, rightx, ty - 220, 0);
                fnt.drawright(info.waypoint_distance_to_next, rightx, ty - 245, 0);

                GML.device.color(Color.White);

            }
            else
                base.draw(ix, iy);
        }
    }
}
