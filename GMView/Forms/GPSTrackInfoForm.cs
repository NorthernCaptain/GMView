using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class GPSTrackInfoForm : Form
    {
        public GPSTrack track;

        public delegate void actionTrackDelegate(GPSTrackInfoForm to_remove);
        public event actionTrackDelegate onRemove;
        public event actionTrackDelegate onRecord;

        public GPSTrackInfoForm(GPSTrack gtr)
        {
            track = gtr;
            InitializeComponent();

            initData();

            this.Text = track.ToString();
        }

        public void initData()
        {
            distanceLb.Text = track.distance.ToString("F2");
            travelTimeLb.Text = track.travel_time.ToString();
            startTimeDTP.Value = track.startTime;
            finishTimeDTP.Value = track.endTime;
            avgSpeedLb.Text = track.avg_speed.ToString("F1");
            maxSpeedLb.Text = track.max_speed.ToString("F1");
        }

        private void closeBut_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void GPSTrackInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = false;
                this.Visible = false;
            }
        }

        private void removeTrackBut_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (onRemove != null)
                onRemove(this);
        }

        public override string ToString()
        {
            return track.ToString();
        }

        private void recordBut_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (onRecord != null)
                onRecord(this);
        }
    }
}
