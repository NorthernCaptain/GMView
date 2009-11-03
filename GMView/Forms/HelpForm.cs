using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GMView.Forms
{
    /// <summary>
    /// Simple web browser based form for displaying html help
    /// </summary>
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();

            webBrowser.DocumentText = global::GMView.Properties.Resources.helpKeys;
        }

        private void HelpForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.Visible = false;
        }
    }
}
