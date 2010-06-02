using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ncGeo;

namespace GMView.UIHelper
{
    /// <summary>
    /// Class provides selection of area on the map and zooming into it
    /// </summary>
    public class ZoomAreaMode: MouseBaseProc
    {
        protected UserSelectionArea uzoomarea;

        public ZoomAreaMode(GMViewForm form, UserControl dPane)
        {
            mainform = form;
            drawPane = dPane;
            uzoomarea = Program.opt.newUserSelectionArea(mainform.mapo);
            mainform.mapo.addSub(uzoomarea);
            uzoomarea.setColor(Color.DarkOrange);
            uzoomarea.onAreaSelection += newZoomInArea;
        }

        public override void modeEnter(MouseBaseProc oldone)
        {
            base.modeEnter(oldone);
            uzoomarea.show();
            uzoomarea.reset();
        }

        public override void modeLeave()
        {
            base.modeLeave();
            uzoomarea.hide();
        }

        protected override bool onMouseDownTranslated(System.Drawing.Point xy)
        {
            uzoomarea.setStartXY(xy);
            return true;
        }

        protected override bool onMouseUpTranslated(System.Drawing.Point xy)
        {
            uzoomarea.setEndXY(xy);
            return true;
        }

        protected override bool onMouseMoveTranslated(System.Drawing.Point xy)
        {
            uzoomarea.setDeltaXY(xy);
            return true;
        }

        protected virtual void newZoomInArea(double lon1, double lat1, double lon2, double lat2)
        {
            Options opt = Program.opt;
            double center_lon, center_lat;
            Size sz = drawPane.Size;
            sz.Width = (sz.Width + opt.image_len - 1) / opt.image_len;
            sz.Height = (sz.Height + opt.image_hei - 1) / opt.image_hei;

            center_lat = (lat1 + lat2) / 2;
            center_lon = (lon1 + lon2) / 2;

            int z_idx;
            BaseGeo geo = opt.getGeoSystem();
            for (z_idx = opt.cur_zoom_lvl + 1; z_idx <= opt.max_zoom_lvl; z_idx++)
            {
                Point xy1, xy2;
                geo.getNXNYByLonLat(lon1, lat1, z_idx, out xy1);
                geo.getNXNYByLonLat(lon2, lat2, z_idx, out xy2);

                if ((xy2.X - xy1.X) >= sz.Width ||
                    (xy2.Y - xy1.Y) >= sz.Height)
                {
                    break; //we found our zoom level
                }
            }
            if (z_idx > opt.cur_zoom_lvl && z_idx <= opt.max_zoom_lvl)
            {
                mainform.mapo.CenterMapLonLat(center_lon, center_lat);
                Program.opt.cur_zoom_lvl = z_idx;
                mainform.mapo.ZoomMapCentered(opt.cur_zoom_lvl);
            }
            uzoomarea.reset();
            GML.repaint();
        }

        public override string name()
        {
            return "Zoom area mode";
        }
    }
}
