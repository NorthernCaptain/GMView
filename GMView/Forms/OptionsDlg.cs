using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class OptionsDlg : Form
    {
        public OptionsDlg()
        {
            InitializeComponent();
            imagePathTxt.Text = Program.opt.image_path;
            subdirCb.Checked = Program.opt.need_zoom_dir;
            fmaskTb.Text = Program.opt.file_mask;
            httpTb1.Text = Program.opt.goohttp[0];
            httpTb2.Text = Program.opt.goohttp[1];
            httpTb3.Text = Program.opt.goohttp[2];
            proxy1Tb.Text = Program.opt.httpproxy[0];
            proxy2Tb.Text = Program.opt.httpproxy[1];
            proxy3Tb.Text = Program.opt.httpproxy[2];
            useOnlineCb.Checked = Program.opt.use_online;
            proxyCombo.SelectedIndex = Program.opt.httpproxy_idx + 2;
            terrainTb1.Text = Program.opt.terrainhttp[0];
            terrainTb2.Text = Program.opt.terrainhttp[1];
            terrainTb3.Text = Program.opt.terrainhttp[2];
            satTb1.Text = Program.opt.sathttp[0];
            satTb2.Text = Program.opt.sathttp[1];
            satTb3.Text = Program.opt.sathttp[2];
            streetTb1.Text = Program.opt.streethttp[0];
            streetTb2.Text = Program.opt.streethttp[1];
            streetTb3.Text = Program.opt.streethttp[2];
            nmeaComCB.SelectedIndex = Program.opt.nmea_com_num - 1;
            nmeaSpeedCB.SelectedItem = Program.opt.nmea_com_speed.ToString();
            nmeaLogCb.Checked = Program.opt.nmea_log;
            logErrCB.Checked = Program.opt.all_err;
            logLogCB.Checked = Program.opt.all_log;
            osmTb1.Text = Program.opt.osmmapnik[0];
            osmTb2.Text = Program.opt.osmmapnik[1];
            osmTb3.Text = Program.opt.osmmapnik[2];
            osmrendTb1.Text = Program.opt.osmarenderer[0];
            osmrendTb2.Text = Program.opt.osmarenderer[1];
            osmrendTb3.Text = Program.opt.osmarenderer[2];
            yamTb1.Text = Program.opt.yamapshttp[0];
            yamTb2.Text = Program.opt.yamapshttp[1];
            yamTb3.Text = Program.opt.yamapshttp[2];
            yasatTb1.Text = Program.opt.yasathttp[0];
            yasatTb2.Text = Program.opt.yasathttp[1];
            yasatTb3.Text = Program.opt.yasathttp[2];
            yastrTb1.Text = Program.opt.yastreethttp[0];
            yastrTb2.Text = Program.opt.yastreethttp[1];
            yastrTb3.Text = Program.opt.yastreethttp[2];
            yatrfTb1.Text = Program.opt.yatrafhttp[0];
            yatrfTb2.Text = Program.opt.yatrafhttp[1];
            yatrfTb3.Text = Program.opt.yatrafhttp[2];
            yatimeTb.Text = Program.opt.yatraftimehttp;
            gootrfTb1.Text = Program.opt.gootrafhttp[0];
            gootrfTb2.Text = Program.opt.gootrafhttp[1];
            gootrfTb3.Text = Program.opt.gootrafhttp[2];
            useGPSCb.Checked = Program.opt.use_gps;
            try
            {
                miniZoomSpin.Value = (decimal)Program.opt.mini_delta_zoom;
            }
            catch (System.Exception ex)
            {
            	
            }
            switch (Program.opt.useGML)
            {
                case GML.GMLType.simpleGDI:
                    graphicsRB1.Checked = true;
                    break;
                case GML.GMLType.openGL:
                    graphicsRB2.Checked = true;
                    break;
                case GML.GMLType.direct3D:
                    graphicsRB3.Checked = true;
                    break;
            }

            try
            {
                zeroSpeedSB.Value = (decimal)Program.opt.zero_speed;
            }
            catch (System.Exception ex)
            {
            	
            }
            try
            {
                avgSpeedSB.Value = (decimal)Program.opt.manual_avg_speed;
            }
            catch (System.Exception ex)
            {
            	
            }
            loadWithMTCB.Checked = Program.opt.load_with_mt;
            fullnameTB.Text = Program.opt.author;
            emailTB.Text = Program.opt.email;
            trackDirTB.Text = Program.opt.default_track_dir;
            autosaveCB.Checked = Program.opt.do_autosave;
            showInMilesCb.Checked = Program.opt.km_or_miles > 1.1 ? true : false;
            try
            {
                divDistSB.Value = (decimal)Program.opt.split_by_distance;
            }
            catch (System.Exception ex)
            {
            	
            }
            splitDaysCB.Checked = Program.opt.split_by_date;
            dashRightCB.Checked = Program.opt.dash_right_side;
            emulateGPSCb.Checked = Program.opt.emulate_gps;
            emuFileTB.Text = Program.opt.emu_nmea_file;
            dynCenterCB.SelectedIndex = (int)Program.opt.dynamic_center;
            nightColor = Program.opt.nightColor;
            try
            {
                colorDlg.Color = nightColor;
            }
            catch (System.Exception ex)
            {
            	
            }
            nightColorTB.Text = (nightColor.ToArgb() & 0xfffff).ToString("X6");
            try
            {
                opacitySpin.Value = (nightColor.ToArgb() >> 24) * 100 / 255;
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        private void CancelBut_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void OKBut_Click(object sender, EventArgs e)
        {
            Program.opt.image_path = imagePathTxt.Text;
            Program.opt.file_mask = fmaskTb.Text;
            Program.opt.need_zoom_dir = subdirCb.Checked;
            Program.opt.goohttp[0] = httpTb1.Text;
            Program.opt.goohttp[1] = httpTb2.Text;
            Program.opt.goohttp[2] = httpTb3.Text;
            Program.opt.httpproxy[0] = proxy1Tb.Text;
            Program.opt.httpproxy[1] = proxy2Tb.Text;
            Program.opt.httpproxy[2] = proxy3Tb.Text;
            Program.opt.httpproxy_idx = proxyCombo.SelectedIndex - 2;
            Program.opt.use_online = useOnlineCb.Checked;
            Program.opt.streethttp[0] = streetTb1.Text;
            Program.opt.streethttp[1] = streetTb2.Text;
            Program.opt.streethttp[2] = streetTb3.Text;
            Program.opt.terrainhttp[0] = terrainTb1.Text;
            Program.opt.terrainhttp[1] = terrainTb2.Text;
            Program.opt.terrainhttp[2] = terrainTb3.Text;
            Program.opt.sathttp[0] = satTb1.Text;
            Program.opt.sathttp[1] = satTb2.Text;
            Program.opt.sathttp[2] = satTb3.Text;
            Program.opt.nmea_com_speed = int.Parse(nmeaSpeedCB.SelectedItem.ToString());
            Program.opt.nmea_com_num = nmeaComCB.SelectedIndex + 1;
            Program.opt.nmea_log = nmeaLogCb.Checked;
            Program.opt.all_log = logLogCB.Checked;
            Program.opt.all_err = logErrCB.Checked;
            Program.opt.osmmapnik[0] = osmTb1.Text;
            Program.opt.osmmapnik[1] = osmTb2.Text;
            Program.opt.osmmapnik[2] = osmTb3.Text;
            Program.opt.osmarenderer[0] = osmrendTb1.Text;
            Program.opt.osmarenderer[1] = osmrendTb2.Text;
            Program.opt.osmarenderer[2] = osmrendTb3.Text;
            Program.opt.yamapshttp[0] = yamTb1.Text;
            Program.opt.yamapshttp[1] = yamTb2.Text;
            Program.opt.yamapshttp[2] = yamTb3.Text;
            Program.opt.yasathttp[0] = yasatTb1.Text;
            Program.opt.yasathttp[1] = yasatTb2.Text;
            Program.opt.yasathttp[2] = yasatTb3.Text;
            Program.opt.yastreethttp[0] = yastrTb1.Text;
            Program.opt.yastreethttp[1] = yastrTb2.Text;
            Program.opt.yastreethttp[2] = yastrTb3.Text;
            Program.opt.yatrafhttp[0] = yatrfTb1.Text;
            Program.opt.yatrafhttp[1] = yatrfTb2.Text;
            Program.opt.yatrafhttp[2] = yatrfTb3.Text;
            Program.opt.yatraftimehttp = yatimeTb.Text;
            Program.opt.gootrafhttp[0] = gootrfTb1.Text;
            Program.opt.gootrafhttp[1] = gootrfTb2.Text;
            Program.opt.gootrafhttp[2] = gootrfTb3.Text;
            Program.opt.use_gps = useGPSCb.Checked;
            Program.opt.mini_delta_zoom = (int)miniZoomSpin.Value;
            Program.opt.zero_speed = (double)zeroSpeedSB.Value;
            Program.opt.manual_avg_speed = (double)avgSpeedSB.Value;
            Program.opt.load_with_mt = loadWithMTCB.Checked;
            Program.opt.email = emailTB.Text;
            Program.opt.author = fullnameTB.Text;
            Program.opt.default_track_dir = trackDirTB.Text;
            Program.opt.do_autosave = autosaveCB.Checked;
            Program.opt.km_or_miles = showInMilesCb.Checked ? 1.6093 : 1.0;
            Program.opt.split_by_distance = (double)divDistSB.Value;
            Program.opt.split_by_date = splitDaysCB.Checked;
            Program.opt.dash_right_side = dashRightCB.Checked;
            Program.opt.emu_nmea_file = emuFileTB.Text;
            Program.opt.emulate_gps = emulateGPSCb.Checked;
            Program.opt.dynamic_center = (Options.DynCenterType)dynCenterCB.SelectedIndex;
            Program.opt.nightColor = nightColor;

            DialogResult dr = DialogResult.No;
            GML.GMLType gtype = GML.GMLType.simpleGDI;

            if (graphicsRB2.Checked)
                gtype = GML.GMLType.openGL;
            if (graphicsRB3.Checked)
                gtype = GML.GMLType.direct3D;
            if (gtype != Program.opt.useGML)
            {
                dr = MessageBox.Show("Changing of drawing mode from/to another library requires \nthat application needs to be restarted.\nPress 'Yes' for exit from the application\nPress 'No' to cancel of changing draw mode.",
                    "Restart required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    Program.opt.useGML = gtype;
                }
            }

            Program.opt.Updated();
            this.Close();
            this.Dispose();

            if (dr == DialogResult.Yes)
                Application.Exit();
        }

        public void setActiveTab(string tabName)
        {
            tabControl.SelectTab(tabName);
        }

        public delegate void OnLonLatChangeDelegate();

        private void chooseFolderBut_Click(object sender, EventArgs e)
        {
            folderBrowserDlg.SelectedPath = imagePathTxt.Text;
            if (folderBrowserDlg.ShowDialog() == DialogResult.OK)
                imagePathTxt.Text = folderBrowserDlg.SelectedPath;
        }

        private void chooseTrackFolderBut_Click(object sender, EventArgs e)
        {
            folderBrowserDlg.SelectedPath = trackDirTB.Text;
            if (folderBrowserDlg.ShowDialog() == DialogResult.OK)
                trackDirTB.Text = folderBrowserDlg.SelectedPath;
        }

        private void useGPSCb_CheckedChanged(object sender, EventArgs e)
        {
            nmeaGroupBox.Enabled = useGPSCb.Checked;
            nmeaLogCb.Enabled = useGPSCb.Checked;
        }

        private void emuFileChooserBut_Click(object sender, EventArgs e)
        {
            openFileDlg.FileName = emuFileTB.Text;
            if (openFileDlg.ShowDialog() == DialogResult.OK)
                emuFileTB.Text = openFileDlg.FileName;
        }

        private void emulateGPSCb_CheckedChanged(object sender, EventArgs e)
        {
            nmeaGroupBox.Enabled = !emulateGPSCb.Checked;
            emuFileTB.Enabled = emulateGPSCb.Checked;
            emuFileChooserBut.Enabled = emulateGPSCb.Checked;
        }

        private void pickColorBut_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                int alpha = (Decimal.ToInt32(opacitySpin.Value) * 255 / 100) << 24;
                nightColor = Color.FromArgb((colorDlg.Color.ToArgb() & 0xffffff) | alpha);
                nightColorTB.Text = (nightColor.ToArgb() & 0xfffff).ToString("X6");
            }

        }

        private Color nightColor = Color.FromArgb(0x4087cefa);

        private void opacitySpin_ValueChanged(object sender, EventArgs e)
        {
            int alpha = (Decimal.ToInt32(opacitySpin.Value) * 255 / 100) << 24;
            nightColor = Color.FromArgb((colorDlg.Color.ToArgb() & 0xffffff) | alpha);
            nightColorTB.Text = (nightColor.ToArgb() & 0xfffff).ToString("X6");
        }
    }
}
