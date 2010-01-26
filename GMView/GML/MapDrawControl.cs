using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class MapDrawControl : UserControl, IGML
    {
        public MapObject mapo;
        public SatelliteForm satForm;
        public GPS.GPSInfoPanel gpsinfo;
        private double scene_angle;

        public MapDrawControl()
        {
            InitializeComponent();
        }

        public MapDrawControl(MapObject imapo, SatelliteForm isatForm, GPS.GPSInfoPanel igpsinfo)
        {
            mapo = imapo;
            satForm = isatForm;
            gpsinfo = igpsinfo;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnResize(EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            mapo.draw(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            mapo.SetVisibleSize(this.Size);
            mapo.recenterMap();
            if (satForm != null && gpsinfo != null)
            {
                satForm.setVisibleSize(this.Size);
                gpsinfo.setVisibleSize(this.Size);
            }
            repaint();
        }

        public static Color screwColor(Color base_col)
        {
            return Color.FromArgb(base_col.R + ncUtils.Glob.rnd.Next(256 - base_col.R),
                            base_col.G + ncUtils.Glob.rnd.Next(256 - base_col.G),
                            base_col.B + ncUtils.Glob.rnd.Next(256 - base_col.B));
        }

        #region IGML Members

        public event EventHandler onInitGML;

        public void initGML(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void resizeGML(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void pushMatrix()
        {
            throw new NotImplementedException();
        }

        public void popMatrix()
        {
            throw new NotImplementedException();
        }

        public void zeroPosition()
        {
            throw new NotImplementedException();
        }

        public void rotateZ(double degree)
        {
            throw new NotImplementedException();
        }

        public void translate(int ix, int iy, int iz)
        {
            throw new NotImplementedException();
        }

        public void renderFrame(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void color(Color col)
        {
            throw new NotImplementedException();
        }

        public void color4(float r, float g, float b, float alpha)
        {
            throw new NotImplementedException();
        }

        public object texFromFile(string fname)
        {
            throw new NotImplementedException();
        }

        public object texFromBitmap(ref Bitmap img)
        {
            throw new NotImplementedException();
        }

        public object texFromBitmapUnchanged(Bitmap img)
        {
            throw new NotImplementedException();
        }

        public object texFromBitmapNoCheck(Bitmap img)
        {
            throw new NotImplementedException();
        }

        public void texFilter(object texture, TexFilter filter)
        {
            throw new NotImplementedException();
        }

        public void texDraw(object tex, int ix, int iy, int iz, int il, int ih)
        {
            throw new NotImplementedException();
        }

        public void texDispose(object tex)
        {
            throw new NotImplementedException();
        }

        public int getTextureAlignedLen(int len)
        {
            throw new NotImplementedException();
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz, Color col)
        {
            throw new NotImplementedException();
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz)
        {
            throw new NotImplementedException();
        }

        public void lineWidth(float width)
        {
            throw new NotImplementedException();
        }

        public void lineStipple(short stipple)
        {
            throw new NotImplementedException();
        }

        public void quadDraw(int ix, int iy, int width, int height, int iz, Color col)
        {
            throw new NotImplementedException();
        }

        public void rectDraw(int ix, int iy, int width, int height, int iz, Color col)
        {
            throw new NotImplementedException();
        }

        public event EventHandler onResizeGML;

        public event EventHandler onRenderGML;

        public double angle
        {
            get
            {
                return scene_angle;
            }
            set
            {
                scene_angle = value;
            }
        }

        public event EventHandler onCurrentGML;
        public event EventHandler onReinitDevice;
        public event EventHandler onLostDevice;

        public void lineDraw(List<Point> points, Point correct, int iz)
        {
            throw new NotImplementedException();
        }

        public Point translateAbsToScene(Point pt)
        {
            throw new NotImplementedException();
        }

        public Point translateSceneToAbs(Point pt)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGML Members


        public void texDrawBegin()
        {
            throw new NotImplementedException();
        }

        public void texDrawEnd()
        {
            throw new NotImplementedException();
        }

        public IGLFont fontCreate(Font from_font)
        {
            throw new NotImplementedException();
        }

        public int curColorInt
        {
            get { throw new NotImplementedException(); }
        }

        public Color curColor
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IGML Members


        public void identity()
        {
            throw new NotImplementedException();
        }

        public Point deltaCenter
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void repaint()
        {
            this.Invalidate();
        }

        public Point halfScreen
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
