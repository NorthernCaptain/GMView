﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    partial class GMViewForm
    {
        private GPS.FollowUpConnector followConnect;

        /// <summary>
        /// Initialize follow up engine
        /// </summary>
        private void initFollowUP()
        {
            followConnect = new GMView.GPS.FollowUpConnector();

            initFollowTrackList(GPSTrackFactory.singleton);
            GPSTrackFactory.singleton.onTrackListChanged += initFollowTrackList;
        }

        /// <summary>
        /// Initialize follow track list if it is changes.
        /// </summary>
        /// <param name="factory"></param>
        private void initFollowTrackList(GPSTrackFactory factory)
        {
            followTrackList.Items.Clear();
            followTrackList.Items.Add("None");

            if (factory.trackList.Count == 0)
            {
                return;
            }
            followTrackList.Items.AddRange(factory.trackList.ToArray());
        }

        /// <summary>
        /// Called by GUI when we select the track in Follow combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void followTrackList_SelectedIndexChanged(object sender, EventArgs e)
        {
            logWin.Log("Selected index of follow track: " + followTrackList.SelectedItem);
            ncGeo.IGPSTrack track = followTrackList.SelectedItem as ncGeo.IGPSTrack;
            if (track == GPSTrackFactory.singleton.recordingTrack)
            {
                MessageBox.Show("You can not follow track that is in recording mode.\nChoose another one, please.",
                                "Wrong track selected", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                followTrackList.SelectedIndex = 0;
                return;
            }

            try
            {
                followConnect.follower = track;
                infoMessage("Set follower to " + ((track == null) ? "None" : track.ToString()));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error setting follower", 
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                followTrackList.SelectedIndex = 0;
            }
        }
    }
}