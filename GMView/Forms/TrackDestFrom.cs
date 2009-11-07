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

        /// <summary>
        /// Construct dialog for location of the track
        /// </summary>
        public TrackDestFrom()
        {
            historyStartLoc = new ncUtils.HistoryList("trackLocation");
            InitializeComponent();
            historyUpdated(historyStartLoc);
            historyStartLoc.historyChanged += historyUpdated;
        }

        /// <summary>
        /// Updates our history comboboxes when history has been changed
        /// </summary>
        /// <param name="hist"></param>
        private void historyUpdated(ncUtils.HistoryList hist)
        {
            startLocCB.Items.Clear();
            startLocCB.Items.AddRange(hist.items.ToArray());
            destLocCB.Items.Clear();
            destLocCB.Items.AddRange(hist.items.ToArray());
        }

        private void startLocCB_TextUpdate(object sender, EventArgs e)
        {
            startLocCB.Text = startLocCB.SelectedItem.ToString();
            historyStartLoc.add(startLocCB.Text);
            destLocCB.Focus();
        }

        private void startLocCB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                historyStartLoc.add(startLocCB.Text);
                destLocCB.Focus();
            }
        }

        private void destLocCB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                historyStartLoc.add(destLocCB.Text);
                makeName();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void destLocCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            destLocCB.Text = destLocCB.SelectedItem.ToString();
            historyStartLoc.add(destLocCB.Text);
            makeName();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Make track name from start and dest locations entered by user
        /// </summary>
        private void makeName()
        {
            trackNameS = startLocCB.Text + "-" + destLocCB.Text + "-" + DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Generate auto filename with current date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoBut_Click(object sender, EventArgs e)
        {
            trackNameS = "auto-" + DateTime.Now.ToString("yyyy-MM-dd");
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Result file name of the track
        /// </summary>
        private string trackNameS = string.Empty;

        /// <summary>
        /// Return track name constructed from dialog data
        /// </summary>
        public string trackName
        {
            get
            {
                return trackNameS;
            }
        }
    }
}
