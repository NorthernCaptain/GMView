using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GMView.UIHelper
{
    public class DownloadAreaMode: ZoomAreaMode
    {
        public DownloadAreaMode(GMViewForm form, UserControl dPane) :             
            base(form, dPane)
        {
            uzoomarea.setColor(Color.DarkRed);
        }

        protected override void newZoomInArea(double lon1, double lat1, double lon2, double lat2)
        {
            DownloadQueryForm frm = new DownloadQueryForm(mainform.mapo);
            frm.init(lon1, lat1, lon2, lat2);
            frm.Show();
        }

        public override string name()
        {
            return "Download area mode";
        }
    }
}
