using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class TrackDestFrom : Form
    {
        protected ncUtils.HistoryList historyStartLoc;
        public TrackDestFrom()
        {
            historyStartLoc = new ncUtils.HistoryList("trackLocation");
            InitializeComponent();
            startLocCB.Items.Clear();
            startLocCB.Items.AddRange(historyStartLoc.items.ToArray());
        }

        private void startLocCB_TextUpdate(object sender, EventArgs e)
        {
            historyStartLoc.add(startLocCB.Text);
        }
    }
}
