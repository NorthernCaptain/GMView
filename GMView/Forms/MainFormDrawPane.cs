using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using GMView.UIHelper;

namespace GMView
{
    partial class GMViewForm
    {

        /// <summary>
        /// Event occured when we click (single) on map and have new lon,lat coordinates
        /// Works only in Navigate mode.
        /// </summary>
        public event MapObject.onLonLatChange onClickLonLat;

        #region drawPane hook methods


        void miniform_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            double lon, lat;
            Point xy = new Point();
            xy.X = e.X;
            xy.Y = e.Y;
            minimapo.getLonLatByVisibleXY(xy, out lon, out lat);
            mapo.CenterMapLonLat(lon, lat);
            repaintMap();
        }

        private void drawPane_Resize(object sender, EventArgs e)
        {
            mapo.SetVisibleSize(drawPane.Size);
            mapo.recenterMap();
            repaintMap();
        }

        #endregion

    }
}
