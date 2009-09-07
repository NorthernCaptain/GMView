using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class IntroForm : Form, ILog
    {
        public IntroForm()
        {
            InitializeComponent();
            Bitmap img = global::GMView.Properties.Resources.intro;
            Size sz = new Size(img.Width, img.Height);
            this.Size = sz;
            this.MinimumSize = sz;
            this.MaximumSize = sz;
        }

        public void doInit()
        {
            try
            {
                Application.DoEvents();
                Program.onShutdown += new Program.ShutdownDelegate(BookMarkFactory.singleton.saveXml);
                ImgCacheManager.singleton.reorganizeAll();
                Application.DoEvents();
                ImgCacheManager.singleton.loadAll();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "EXCEPTION at startup!");
            }
        }

        public void showIntro()
        {
            appNameLbl.Text = Options.program_full_name + " v." + Options.program_version;
            switch (Program.opt.useGML)
            {
                case GML.GMLType.direct3D:
                    rendererLbl.Text = "using MS Direct3D renderer";
                    break;
                case GML.GMLType.openGL:
                    rendererLbl.Text = "using Open GL renderer";
                    break;
                case GML.GMLType.simpleGDI:
                    rendererLbl.Text = "using plain GDI+ renderer";
                    break;
                default:
                    rendererLbl.Text = "unknown renderer, back to GDI+";
                    break;
            }

            this.Visible = true;
            Program.logger = this;
            Application.DoEvents();
        }

        public void hideIntro()
        {
            this.Visible = false;
            Program.logger = null;
        }

        #region ILog Members

        public void Log(string txt)
        {
            infoLb.Text = txt;
            Application.DoEvents();
        }

        public void NMEALog(NMEAString str)
        {
            throw new NotImplementedException();
        }

        public void Err(string txt)
        {
            infoLb.Text = "ERR: " + txt;
            Application.DoEvents();
        }

        public bool needInvoke
        {
            get { return false; }
        }

        #endregion
    }
}
