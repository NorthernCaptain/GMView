using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class BookmarkForm : Form
    {
        public BookmarkForm()
        {
            InitializeComponent();
            List<string> groups = BookMarkFactory.singleton.getGroupNames();
            groupCB.Items.AddRange(groups.ToArray());
        }

        public void setLonLat(double lon, double lat)
        {
            lonBox.Text = ncUtils.Glob.lonlatDigitString(lon);
            latBox.Text = ncUtils.Glob.lonlatDigitString(lat);
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            if (nameTb.Text == "")
            {
                nameTb.Focus();
                return;
            }

            Bookmark bmark = new Bookmark();
            bmark.lon = ncUtils.Glob.parseLonLat(lonBox.Text);
            bmark.lat = ncUtils.Glob.parseLonLat(latBox.Text);
            bmark.Name = nameTb.Text;
            bmark.Comment = commentTb.Text;
            bmark.image_idx = pinCombo.SelectedIndex;
            bmark.original_map_type = Program.opt.mapType;
            bmark.Original_zoom = Program.opt.cur_zoom_lvl;
            bmark.group = groupCB.Text;

            if (!BookMarkFactory.singleton.addBookmark(bmark))
            {
                MessageBox.Show("Bookmark with the same name already exists!\nPlease, enter original name for this location.",
                    "Error in bookmark", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.nameTb.Focus();
                return;
            }
            cancelBut_Click(sender, e);
        }

        private void cancelBut_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Dispose();
        }
    }
}
