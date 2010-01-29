using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    partial class GMViewForm
    {
        /// <summary>
        /// Shows download dialog for loading tiles along the selected track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downloadTilesAlongTheTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ncGeo.IGPSTrack track = followTrackList.SelectedItem as ncGeo.IGPSTrack;
            if(track == null)
            {
                MessageBox.Show("Select the track in the combobox first, then download tiles under it.",
                                "Wrong track selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DownloadQueryForm qryForm = new DownloadQueryForm(mapo);
            qryForm.init(track);
            qryForm.Owner = this;
            qryForm.Visible = true;
        }

        /// <summary>
        /// Zooms to the first position of the track (centers map on it)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomToStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ncGeo.IGPSTrack track = followTrackList.SelectedItem as ncGeo.IGPSTrack;
            if (track == null)
            {
                MessageBox.Show("Select the track in the combobox first, then repeat the action.",
                                "Wrong track selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if(track.trackPointData.First != null)
            {
                mapo.CenterMapLonLat(track.trackPointData.First.Value.lon,
                                     track.trackPointData.First.Value.lat);
                repaintMap();
            }
        }

        private void zoomToFinishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ncGeo.IGPSTrack track = followTrackList.SelectedItem as ncGeo.IGPSTrack;
            if (track == null)
            {
                MessageBox.Show("Select the track in the combobox first, then repeat the action.",
                                "Wrong track selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (track.trackPointData.Last != null)
            {
                mapo.CenterMapLonLat(track.trackPointData.Last.Value.lon,
                                     track.trackPointData.Last.Value.lat);
                repaintMap();
            }
        }

        /// <summary>
        /// Shows track information window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showInfoWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPSTrack track = followTrackList.SelectedItem as GPSTrack;
            if (track == null)
            {
                MessageBox.Show("Select the track in the combobox first, then repeat the action.",
                                "Wrong track selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GPSTrackInfoForm frm = GPSTrackFactory.singleton.infoForm(track);
            frm.initData();
            frm.Visible = true;
        }

        /// <summary>
        /// Saves the track selected in the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPSTrack track = followTrackList.SelectedItem as GPSTrack;
            if (track == null)
            {
                MessageBox.Show("Select the track in the combobox first, then repeat the action.",
                                "Wrong track selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            saveTrackWithDialog(track);

        }

        /// <summary>
        /// Saves the given track to disk, but first shows Save dialog to the user
        /// </summary>
        /// <param name="track"></param>
        private void saveTrackWithDialog(GPSTrack track)
        {
            if (track.countPoints == 0)
            {
                MessageBox.Show("Current track has no points. There is nothing to be saved.\nUse 'start recording track' button first.\n\nOperation cancelled.",
                    "Empty track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string fname = track.fileName;
                fname = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fname),
                    System.IO.Path.GetFileNameWithoutExtension(fname) + ".gpx");
                trackSaveFileDialog.FileName = fname;
                trackSaveFileDialog.DefaultExt = "gpx";
                trackSaveFileDialog.Title = "Save track to a file";
                trackSaveFileDialog.Filter = "GPX unified files|*.gpx|KML google earth files|*.kml|All files|*.*";
                if (trackSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    GPS.TrackFileInfo fi = new GPS.TrackFileInfo(trackSaveFileDialog.FileName,
                                                GPS.TrackFileInfo.SourceType.FileName);
                    if (!string.IsNullOrEmpty(System.IO.Path.GetExtension(fi.fileOrBuffer)))
                        fi.FileType = System.IO.Path.GetExtension(fi.fileOrBuffer).Remove(0, 1).ToLower();
                    track.save(fi, BookMarkFactory.singleton, Bookmarks.POIGroupFactory.singleton());
                }
            }
        }

    }
}
