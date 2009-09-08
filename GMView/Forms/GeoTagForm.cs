using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class GeoTagForm : Form
    {
        private XnGFL.ExifViewControl exifcontrol;

        public GeoTagForm(XnGFL.ExifViewControl eview)
        {
            exifcontrol = eview;
            InitializeComponent();

            exifcontrol.Dock = DockStyle.Fill;
            exifcontrol.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            exifcontrol.Size = this.ClientSize;
            this.Controls.Add(exifcontrol);
            this.PerformLayout();

            Program.onShutdown += shutdown;
        }

        private void GeoTagForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            this.Visible = false;
        }

        private void shutdown()
        {
            exifcontrol.shutdown();
        }
    }
}
