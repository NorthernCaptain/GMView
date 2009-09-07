using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class MapForm : Form
    {
        private MapObject mapo;
        private bool inSync = true;

        public event MethodInvoker onHide;
        internal UserControl drawPane;

        public MapForm(MapObject imapo)
        {
            mapo = imapo;
            InitializeComponent();
            drawPane = new MapDrawControl(mapo, null, null);
            drawPane.Dock = DockStyle.Fill;
            this.Controls.Add(drawPane);
        }

        public void repaintMap()
        {
            if(inSync && Program.opt.show_mini_map)
                drawPane.Invalidate();
        }

        #region drawPane hook methods
        //===========================================================================

        /// <summary>
        /// Main redraw cycle. Redraw map and all objects on it.
        /// Currently it can do only full redraw (whole screen).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawPane_Paint(object sender, PaintEventArgs e)
        {
            mapo.SetVisibleSize(drawPane.Size);
            mapo.draw(e.Graphics);
        }

        private void drawPane_Resize(object sender, EventArgs e)
        {
            mapo.SetVisibleSize(drawPane.Size);
            mapo.recenterMap();
            repaintMap();
        }
        #endregion

        private void MapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Visible = false;
                if (onHide != null)
                    onHide();
            }
            else
            {
                Program.opt.mini_position = this.Location;
                Program.opt.mini_size = this.Size;
            }
        }

        public void recenterMap(double lon, double lat)
        {
            repaintMap();
        }

        public bool doSync
        {
            get { return inSync; }
            set { inSync = value; }
        }

    }
}
