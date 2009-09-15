using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class LogWin : Form, ILog
    {
        private int total_lines = 0;

        public LogWin()
        {
            InitializeComponent();
        }

        #region Logging methods
        public void Log(string txt)
        {
            Font oldfnt = this.logTb.SelectionFont;
            this.logTb.SelectionColor = Color.Blue;
            this.logTb.AppendText("TimeStamp: ");
            this.logTb.SelectionColor = Color.Black;
            this.logTb.SelectionFont = oldfnt;
            this.logTb.SelectionFont = new Font(this.logTb.SelectionFont.FontFamily, this.logTb.SelectionFont.Size, FontStyle.Bold);
            this.logTb.AppendText(txt + "\n");
            this.logTb.SelectionFont = oldfnt;
            total_lines++;
        }

        public void NMEALog(ncGeo.NMEAString str)
        {
            if (!Program.opt.nmea_log)
                return;

            Font oldfnt = this.logTb.SelectionFont;
            if (str.error == null)
            {
                this.logTb.SelectionColor = Color.Blue;
                this.logTb.AppendText("NMEA data: ");
                this.logTb.SelectionColor = Color.Black;
                this.logTb.SelectionFont = oldfnt;
                this.logTb.SelectionFont = new Font(this.logTb.SelectionFont.FontFamily, this.logTb.SelectionFont.Size, FontStyle.Bold);
                this.logTb.AppendText(str.sentence + "\n");
            }
            else
            {
                this.logTb.SelectionColor = Color.DarkRed;
                this.logTb.AppendText("NMEA ERR: ");
                this.logTb.SelectionFont = oldfnt;
                this.logTb.SelectionFont = new Font(this.logTb.SelectionFont.FontFamily, this.logTb.SelectionFont.Size, FontStyle.Bold);
                this.logTb.AppendText(str.error + "\n");
                this.logTb.SelectionColor = Color.Black;
            }
            this.logTb.SelectionFont = oldfnt;
            total_lines++;
        }

        public void Err(string txt)
        {
            Font oldfnt = this.logTb.SelectionFont;
            this.logTb.SelectionColor = Color.Blue;
            this.logTb.AppendText("TimeStamp: ");
            this.logTb.SelectionColor = Color.DarkRed;
            this.logTb.SelectionFont = oldfnt;
            this.logTb.SelectionFont = new Font(this.logTb.SelectionFont.FontFamily, this.logTb.SelectionFont.Size, FontStyle.Bold);
            this.logTb.AppendText(txt + "\n");
            this.logTb.SelectionFont = oldfnt;
            this.logTb.SelectionColor = Color.Black;
            total_lines++;
        }

        public void autoClear(int threshold)
        {
            if (total_lines > threshold)
            {
                total_lines = 0;
                logTb.Text = "";
            }
        }

        #endregion

        private void LogWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }


        internal void reset()
        {
            logTb.Text = "";
        }

        #region ILog Members


        public bool needInvoke
        {
            get { return true; }
        }

        #endregion
    }
}
