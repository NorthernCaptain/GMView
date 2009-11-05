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
            historyStartLoc = new ncUtils.HistoryList("trackLocationName");
            InitializeComponent();
            startLocCB.Items.Clear();
            startLocCB.Items.AddRange(historyStartLoc.items.ToArray());
        }
    }
}
