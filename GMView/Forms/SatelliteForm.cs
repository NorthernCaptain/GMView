using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class SatelliteForm : Form, ISprite
    {
        private SatelliteCollection satellites;
        private int radius = 56;
        private Point center_xy = new Point();
        private Font fnt;
        private Brush fntbrush = new SolidBrush(Color.DarkOrange);
        private Brush fntusebrush = new SolidBrush(Color.White);
        private Pen pen;
        private Pen penuse;

        private const int MaxSatAr = 12;
        private Label[] satLabels = new Label[MaxSatAr];
        private ProgressBar[] satPBars = new ProgressBar[MaxSatAr];
        private SatelliteCollection.OnSatelliteChangeDelegate ourChangeDel;

        private ImageDot bgImg;
        private int bgX = 0, bgY = 0, relX = 0, relY = 0;
        private bool shown = false;

        public SatelliteForm(int ix, int iy, SatelliteCollection scol)
        {
            satellites = scol;
            bgX = ix; bgY = iy;
            relX = ix; relY = iy;
            bgImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.SatInfo);

            InitializeComponent();
            fnt = FontFactory.singleton.getGDIFont(FontFactory.FontAlias.Small8R);
            center_xy.X = 65;
            center_xy.Y = 65;
            pen = new Pen(fntbrush, 2);
            penuse  = new Pen(fntusebrush, 2);

            satLabels[0] = label3;
            satLabels[1] = label4;
            satLabels[2] = label5;
            satLabels[3] = label6;
            satLabels[4] = label7;
            satLabels[5] = label8;
            satLabels[6] = label9;
            satLabels[7] = label10;
            satLabels[8] = label11;
            satLabels[9] = label12;
            satLabels[10] = label13;
            satLabels[11] = label14;

            satPBars[0] = progressBar1;
            satPBars[1] = progressBar2;
            satPBars[2] = progressBar3;
            satPBars[3] = progressBar4;
            satPBars[4] = progressBar5;
            satPBars[5] = progressBar6;
            satPBars[6] = progressBar7;
            satPBars[7] = progressBar8;
            satPBars[8] = progressBar9;
            satPBars[9] = progressBar10;
            satPBars[10] = progressBar11;
            satPBars[11] = progressBar12;

            ourChangeDel = new SatelliteCollection.OnSatelliteChangeDelegate(satellites_onSatellitesChanged);
            satellites.onSatellitesChanged += ourChangeDel;
            TextureFactory.singleton.onInited += initGLData;
        }

        void satellites_onSatellitesChanged()
        {
            if (!this.Visible)
                return;

            satelliteBP.Invalidate();
            inViewLb.Text = satellites.satellites.Count.ToString();
            inUseLb.Text = satellites.satellitesInUse.ToString();
            int i = 0;
            foreach (KeyValuePair<int, Satellite> satpair in satellites.satellites)
            {
                satLabels[i].Text = satpair.Value.prn.ToString();
                satPBars[i].Value = satpair.Value.signal_strength;
                i++;
            }
            for (; i < MaxSatAr; i++)
            {
                satLabels[i].Text = "-";
                satPBars[i].Value = 0;
            }
        }

        private void satelliteBP_Paint(object sender, PaintEventArgs e)
        {
            foreach (KeyValuePair<int, Satellite> satpair in satellites.satellites)
            {
                Point xy;
                satpair.Value.getXY(radius, out xy);
                xy.X += center_xy.X;
                xy.Y = center_xy.Y - xy.Y;
                if (satpair.Value.state == Satellite.State.InUse)
                {
                    e.Graphics.DrawEllipse(penuse, xy.X - 3, xy.Y - 3, 6, 6);
                    e.Graphics.DrawString(satpair.Value.prn.ToString(), fnt, fntusebrush, new PointF(xy.X+1, xy.Y+1));
                }
                else
                {
                    e.Graphics.DrawEllipse(pen, xy.X - 3, xy.Y - 3, 6, 6);
                    e.Graphics.DrawString(satpair.Value.prn.ToString(), fnt, fntbrush, new PointF(xy.X+1, xy.Y+1));
                }
            }
        }

        private void SatelliteForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            satellites.onSatellitesChanged -= ourChangeDel;
        }

        private void SatelliteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void setVisibleSize(Size sz)
        {
            bgX = relX < 0 ? sz.Width + relX: relX;
            bgY = relY < 0 ? sz.Height + relY: relY;
        }

        #region ISprite Members

        public void draw(Graphics gr)
        {
            if (!shown)
                return;

            gr.DrawImageUnscaled(bgImg.img, bgX, bgY);

            foreach (KeyValuePair<int, Satellite> satpair in satellites.satellites)
            {
                Point xy;
                satpair.Value.getXY(radius, out xy);
                xy.X += center_xy.X + bgX + 2;
                xy.Y = center_xy.Y + bgY - xy.Y + 2;
                if (satpair.Value.state == Satellite.State.InUse)
                {
                    gr.DrawEllipse(penuse, xy.X - 3, xy.Y - 3, 6, 6);
                    gr.DrawString(satpair.Value.prn.ToString(), fnt, fntusebrush, new PointF(xy.X+1, xy.Y+1));
                }
                else
                {
                    gr.DrawEllipse(pen, xy.X - 3, xy.Y - 3, 6, 6);
                    gr.DrawString(satpair.Value.prn.ToString(), fnt, fntbrush, new PointF(xy.X+1, xy.Y+1));
                }
            }
        }

        public void draw(Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        public int dLevel
        {
            get
            {
                return 3;
            }
            set
            {
            }
         }

        public void show()
        {
            shown = true;
        }

        public void hide()
        {
            shown = false;
        }

        protected object bg_tex;
        protected object sat_ok_tex;
        protected object sat_vi_tex;
        protected ImageDot sat_ok;
        protected ImageDot sat_vi;

        public void initGLData()
        {
            try
            {
                sat_ok = TextureFactory.singleton.getImg(TextureFactory.TexAlias.SatOK);
                sat_vi = TextureFactory.singleton.getImg(TextureFactory.TexAlias.SatVI);
                bg_tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SatInfo);
                sat_ok_tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SatOK);
                sat_vi_tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SatVI);
            }
            catch
            {
            }
        }

        public void glDraw(int centerx, int centery)
        {
            if (!shown)
                return;

            GML.device.pushMatrix();
            GML.device.identity();

            GML.device.texDraw(bg_tex, bgX - centerx, centery - bgY,
                    dLevel + 2, bgImg.img.Width, bgImg.img.Height);

            lock (satellites)
            {
                foreach (KeyValuePair<int, Satellite> satpair in satellites.satellites)
                {
                    Point xy;
                    satpair.Value.getXY(radius, out xy);
                    xy.X += center_xy.X + bgX + 2 - centerx;
                    xy.Y = centery - (center_xy.Y + bgY - xy.Y + 2);
                    if (satpair.Value.state == Satellite.State.InUse)
                    {
                        GML.device.texDraw(sat_ok_tex, xy.X, xy.Y,
                                dLevel + 2, sat_ok.img.Width, sat_ok.img.Height);
                        ///TODO: draw text - satellite numbers
                        //gr.DrawString(satpair.Value.prn.ToString(), fnt, fntusebrush, new PointF(xy.X + 1, xy.Y + 1));
                    }
                    else
                    {
                        GML.device.texDraw(sat_vi_tex, xy.X, xy.Y,
                                dLevel + 2, sat_vi.img.Width, sat_ok.img.Height);
                    }
                }
            }
            GML.device.popMatrix();
        }

        #endregion
    }
}
