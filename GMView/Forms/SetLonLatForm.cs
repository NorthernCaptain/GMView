using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class SetLonLatForm : Form
    {
        public double longitude
        {
            get 
            {
                if (lonTBox.Text.Length == 0)
                    return 0.0;
                return ncUtils.Glob.parseLon(lonTBox.Text);
            }
        }

        public double latitude
        {
            get
            {
                if (latTBox.Text.Length == 0)
                    return 0.0;
                return ncUtils.Glob.parseLat(latTBox.Text);
            }
        }

        public SetLonLatForm()
        {
            InitializeComponent();
            lonTBox.Text = ncUtils.Glob.lonString(Program.opt.lon);
            latTBox.Text = ncUtils.Glob.latString(Program.opt.lat);
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            if (lonTBox.Text.Trim().Length == 0)
            {
                lonTBox.Focus();
                return;
            }
            if(latTBox.Text.Trim().Length == 0)
            {
                latTBox.Focus();
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
