using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class GDIUControl : UserControl, IGML
    {
        private Color current_color = Color.White;
        private int current_color_int;
        private double scene_angle = 0.0;

        private bool inited = false;
        private int halfWidth, halfHeight;
        private int delta_y = 0;
        private int delta_x = 0;

        private Graphics gr;
        private Point pt = new Point();
        private Stack<Matrix> mtxStack = new Stack<Matrix>();
        private Brush fntBrush = new SolidBrush(Color.Black);
        private float line_width = 1.0f;

        public GDIUControl()
        {
            InitializeComponent();
            try
            {
                Resize += resizeGML;
                Paint += renderFrame;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "EXCEPTION!");
                Environment.Exit(11);
            }
        }


        public Graphics currentGraphics
        {
            get { return gr; }
        }

        public Brush fontBrush
        {
            get { return fntBrush; }
        }

        #region IGML Members

        public event EventHandler onInitGML;

        public event EventHandler onResizeGML;

        public event EventHandler onRenderGML;

        public event EventHandler onCurrentGML;

        public event EventHandler onReinitDevice;

        public event EventHandler onLostDevice;

        public double angle
        {
            get
            {
                return scene_angle;
            }
            set
            {
                scene_angle = value;
                double ang = scene_angle * GML.deg2rad;
                GML.cosa = Math.Cos(ang);
                GML.sina = Math.Sin(ang);
            }
        }

        public int curColorInt
        {
            get { return current_color_int; }
        }

        public Color curColor
        {
            get { return current_color; }
        }

        public Point deltaCenter
        {
            get
            {
                return new Point(delta_x, delta_y);
            }
            set
            {
                delta_x = value.X;
                delta_y = value.Y;
            }
        }

        public void initGML(object sender, EventArgs e)
        {
            if (inited)
                return;
            inited = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            resizeGML(sender, e);
            if (onInitGML != null)
                onInitGML(this, EventArgs.Empty);
        }

        public void resizeGML(object sender, EventArgs e)
        {
            GML.device = this;
            lock (GML.lock3D)
            {

                halfWidth = Size.Width / 2;
                halfHeight = Size.Height / 2;

                if (onResizeGML != null)
                    onResizeGML(this, EventArgs.Empty);
            }
        }

        public void repaint()
        {
            this.Invalidate();
        }

        public void pushMatrix()
        {
            mtxStack.Push(gr.Transform);
        }

        public void popMatrix()
        {
            if (mtxStack.Count > 0)
                gr.Transform = mtxStack.Pop();
        }

        public void zeroPosition()
        {
            gr.ResetTransform();
            gr.TranslateTransform(halfWidth+delta_x, halfHeight-delta_y);
        }

        public void identity()
        {
            gr.ResetTransform();
            gr.TranslateTransform(halfWidth, halfHeight);
        }

        public void rotateZ(double degree)
        {
            gr.RotateTransform(-(float)degree);
        }

        public void translate(int ix, int iy, int iz)
        {
            gr.TranslateTransform(ix, -iy);
        }

        public void renderFrame(object sender, EventArgs e)
        {
            GML.device = this;
            GML.tranBegin();
            try
            {
                if (!inited)
                    initGML(this, EventArgs.Empty);

                if (onCurrentGML != null)
                    onCurrentGML(this, EventArgs.Empty);


                gr.Clear(Color.Gray);
                pushMatrix();
                zeroPosition();
                rotateZ(scene_angle);

                color(Color.White);

                if (onRenderGML != null)
                    onRenderGML(this, EventArgs.Empty);

                popMatrix();
            }
            finally
            {
                GML.tranEnd();
            }
        }

        public void color(Color col)
        {
            if (current_color != col)
            {
                current_color = col;
                current_color_int = col.ToArgb();
                fntBrush = new SolidBrush(current_color);
            }
        }

        public void color4(float r, float g, float b, float alpha)
        {
            current_color = Color.FromArgb((int)(alpha * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
            current_color_int = current_color.ToArgb();
            fntBrush = new SolidBrush(current_color);
        }

        public object texFromFile(string fname)
        {
            return new Bitmap(fname);
        }

        public object texFromBitmap(ref Bitmap img)
        {
            return img;
        }

        public object texFromBitmapNoCheck(Bitmap img)
        {
            return img;
        }

        public void texFilter(object texture, TexFilter filter)
        {
        }

        private Rectangle imgRect = new Rectangle(0, 0, 256, 256);
        public void texDraw(object tex, int ix, int iy, int iz, int il, int ih)
        {
            Bitmap img = tex as Bitmap;
            if (img == null)
                return;
            imgRect.X = ix;
            imgRect.Y = -iy;
            imgRect.Width = il;
            imgRect.Height = ih;
            gr.DrawImage(img, imgRect);
        }

        public void texDrawBegin()
        {
            
        }

        public void texDrawEnd()
        {
            
        }

        public void texDispose(object tex)
        {
            
        }

        public int getTextureAlignedLen(int len)
        {
            return len;
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz, Color col)
        {
            Pen p = new Pen(col, line_width);
            gr.DrawLine(p, x1, -y1, x2, -y2);
            p.Dispose();
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz)
        {
            Pen p = new Pen(current_color, line_width);
            gr.DrawLine(p, x1, -y1, x2, -y2);
            p.Dispose();
        }

        private List<Point> linePoints = new List<Point>();

        public void lineDraw(List<Point> points, Point correct, int iz)
        {
            if (points == null || points.Count <= 1)
                return;
            Pen pen = new Pen(current_color, line_width);

            int rx, ry;
            int lastrx, lastry;

            bool lastVis = true;
            bool curVis = true;

            linePoints.Clear();
            iz = 0;

            lastrx = points[0].X;
            lastry = points[0].Y;

            gr.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (Point p in points)
            {
                rx = p.X - correct.X;
                ry = correct.Y - p.Y;

                curVis = rx >= -halfWidth && rx <= halfWidth && ry >= -halfHeight && ry <= halfHeight;

                if (!curVis && !lastVis)
                {
                    lastrx = rx;
                    lastry = ry;
                    lastVis = curVis;
                    continue;
                }

                if (!curVis && lastVis) //we moved from visible area to hidden
                {
                    linePoints.Add(new Point(rx, -ry));
                    if (linePoints.Count > 1)
                        gr.DrawLines(pen, linePoints.ToArray());
                    linePoints.Clear();
                    lastrx = rx;
                    lastry = ry;
                    lastVis = curVis;
                    continue;
                }

                if (curVis && !lastVis) //we moved back from hidden area to visible one
                {
                    linePoints.Add(new Point(lastrx, -lastry));
                }

                linePoints.Add(new Point(rx, -ry));
                lastrx = rx;
                lastry = ry;
                lastVis = curVis;
            }

            if (linePoints.Count > 1)
                gr.DrawLines(pen, linePoints.ToArray());

            pen.Dispose();
            gr.SmoothingMode = SmoothingMode.Default;
        }

        public void lineWidth(float width)
        {
            line_width = width;
        }

        public void lineStipple(short stipple)
        {
            //throw new NotImplementedException();
        }

        public void quadDraw(int ix, int iy, int width, int height, int iz, Color col)
        {
            //throw new NotImplementedException();
        }

        private Point[] rectPoints = new Point[5];

        public void rectDraw(int ix, int iy, int width, int height, int iz, Color col)
        {
            Pen pen = new Pen(col, line_width);
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            rectPoints[0].X = ix;
            rectPoints[0].Y = -iy;

            rectPoints[1].X = ix + width;
            rectPoints[1].Y = -iy;

            rectPoints[2].X = ix + width;
            rectPoints[2].Y = -iy + height;

            rectPoints[3].X = ix;
            rectPoints[3].Y = -iy + height;

            rectPoints[4].X = ix;
            rectPoints[4].Y = -iy;

            gr.DrawLines(pen, rectPoints);

            pen.Dispose();
            gr.SmoothingMode = SmoothingMode.Default;
        }

        public Point translateAbsToScene(Point pt)
        {
            pt.X -= (halfWidth + delta_x);
            pt.Y -= (halfHeight - delta_y);
            return pt;
        }

        public Point translateSceneToAbs(Point pt)
        {
            pt.X += halfWidth;
            pt.Y += (halfHeight);
            return pt;
        }

        public IGLFont fontCreate(Font from_font)
        {
            return new GDIGLFont(from_font);
        }

        public Point halfScreen
        {
            get { return new Point(halfWidth, halfHeight); }
        }

        #endregion

        private void GDIUControl_Paint(object sender, PaintEventArgs e)
        {
            gr = e.Graphics;
            gr.PageUnit = GraphicsUnit.Pixel;
            gr.PageScale = 1;
            renderFrame(sender, e);
        }
    }
}
