using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace GMView
{
    public partial class GLMapDrawControl : SimpleOpenGlControl, IGML
    {
        protected bool inited = false;
        private double scene_angle = 0.0;
        private int halfWidth, halfHeight;
        private Color current_color = Color.White;
        private int delta_x = 0;
        private int delta_y = 0;

        public GLMapDrawControl() : base()
        {
            InitializeComponent();
            InitializeContexts();
            Resize += resizeGML;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            renderFrame(null, e);
        }

        #region Test only
        /*
        void testInit()
        {
            Bitmap image = new Bitmap("0-0.png");
            System.Drawing.Imaging.BitmapData bitmapdata;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Gl.GenTextures(4, texture);
            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[0]);
            Gl.TexImage2D(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, 0,
                OpenTK.OpenGl.Enums.PixelInternalFormat.Rgb8,
                image.Width, image.Height,
                0, OpenTK.OpenGl.Enums.PixelFormat.Bgr, OpenTK.OpenGl.Enums.PixelType.UnsignedByte,
                bitmapdata.Scan0);
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMinFilter,
                (int)OpenTK.OpenGl.Enums.TextureMinFilter.Nearest);		// Linear Filtering
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMagFilter,
                (int)OpenTK.OpenGl.Enums.TextureMagFilter.Nearest);		// Linear Filtering

            image.UnlockBits(bitmapdata);
            image.Dispose();

            image = new Bitmap("1-0.png");
            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[1]);
            Gl.TexImage2D(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, 0, OpenTK.OpenGl.Enums.PixelInternalFormat.Rgb8,
                image.Width, image.Height,
                0, OpenTK.OpenGl.Enums.PixelFormat.Bgr, OpenTK.OpenGl.Enums.PixelType.UnsignedByte,
                bitmapdata.Scan0);
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMinFilter,
                (int)OpenTK.OpenGl.Enums.TextureMinFilter.Nearest);		// Linear Filtering
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMagFilter,
                (int)OpenTK.OpenGl.Enums.TextureMagFilter.Nearest);		// Linear Filtering

            image.UnlockBits(bitmapdata);
            image.Dispose();

            image = new Bitmap("0-1.png");
            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[2]);
            Gl.TexImage2D(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, 0, OpenTK.OpenGl.Enums.PixelInternalFormat.Rgb8,
                image.Width, image.Height,
                0, OpenTK.OpenGl.Enums.PixelFormat.Bgr, OpenTK.OpenGl.Enums.PixelType.UnsignedByte,
                bitmapdata.Scan0);
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMinFilter,
                (int)OpenTK.OpenGl.Enums.TextureMinFilter.Nearest);		// Linear Filtering
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMagFilter,
                (int)OpenTK.OpenGl.Enums.TextureMagFilter.Nearest);		// Linear Filtering

            image.UnlockBits(bitmapdata);
            image.Dispose();

            image = new Bitmap("1-1.png");
            bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[3]);
            Gl.TexImage2D(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, 0, OpenTK.OpenGl.Enums.PixelInternalFormat.Rgb8,
                image.Width, image.Height,
                0, OpenTK.OpenGl.Enums.PixelFormat.Bgr, OpenTK.OpenGl.Enums.PixelType.UnsignedByte,
                bitmapdata.Scan0);
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMinFilter,
                (int)OpenTK.OpenGl.Enums.TextureMinFilter.Nearest);		// Linear Filtering
            Gl.TexParameter(OpenTK.OpenGl.Enums.TextureTarget.Texture2d,
                OpenTK.OpenGl.Enums.TextureParameterName.TextureMagFilter,
                (int)OpenTK.OpenGl.Enums.TextureMagFilter.Nearest);		// Linear Filtering

            image.UnlockBits(bitmapdata);
            image.Dispose();
        }

        private uint[] texture = new uint[4];

        void testDraw()
        {
            float delta = 256.0f;
            Gl.Rotate(0.0f, 0.0f, 0.0f, 1.0f);

            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[0]);					// defines the texture
            Gl.Begin(OpenTK.OpenGl.Enums.BeginMode.Quads);
            // Front Face
            Gl.TexCoord2(0.0f, 0.0f);			// top right of texture
            Gl.Vertex3(-delta, delta, 0.0f);		// top right of quad
            Gl.TexCoord2(0.0f, 1.0f);			// top left of texture
            Gl.Vertex3(-delta, 0.0f, 0.0f);		// top left of quad
            Gl.TexCoord2(1.0f, 1.0f);			// bottom left of texture
            Gl.Vertex3(0.0f, 0.0f, 0.0f);	// bottom left of quad
            Gl.TexCoord2(1.0f, 0.0f);			// bottom right of texture
            Gl.Vertex3(0.0f, delta, 0.0f);		// bottom right of quad
            Gl.End();

            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[1]);					// defines the texture
            Gl.Begin(OpenTK.OpenGl.Enums.BeginMode.Quads);
            Gl.TexCoord2(0.0f, 0.0f);			// top right of texture
            Gl.Vertex3(0.0f, delta, 0.0f);		// top right of quad
            Gl.TexCoord2(0.0f, 1.0f);			// top left of texture
            Gl.Vertex3(0.0f, 0.0f, 0.0f);		// top left of quad
            Gl.TexCoord2(1.0f, 1.0f);			// bottom left of texture
            Gl.Vertex3(delta, 0.0f, 0.0f);	// bottom left of quad
            Gl.TexCoord2(1.0f, 0.0f);			// bottom right of texture
            Gl.Vertex3(delta, delta, 0.0f);		// bottom right of quad
            Gl.End();

            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[2]);					// defines the texture
            Gl.Begin(OpenTK.OpenGl.Enums.BeginMode.Quads);
            Gl.TexCoord2(0.0f, 0.0f);			// top right of texture
            Gl.Vertex3(-delta, 0.0f, 0.0f);		// top right of quad
            Gl.TexCoord2(0.0f, 1.0f);			// top left of texture
            Gl.Vertex3(-delta, -delta, 0.0f);		// top left of quad
            Gl.TexCoord2(1.0f, 1.0f);			// bottom left of texture
            Gl.Vertex3(0.0f, -delta, 0.0f);	// bottom left of quad
            Gl.TexCoord2(1.0f, 0.0f);			// bottom right of texture
            Gl.Vertex3(0.0f, 0.0f, 0.0f);		// bottom right of quad
            Gl.End();

            Gl.BindTexture(OpenTK.OpenGl.Enums.TextureTarget.Texture2d, texture[3]);					// defines the texture
            Gl.Begin(OpenTK.OpenGl.Enums.BeginMode.Quads);
            Gl.TexCoord2(0.0f, 0.0f);			// top right of texture
            Gl.Vertex3(0.0f, 0.0f, 0.0f);		// top right of quad
            Gl.TexCoord2(0.0f, 1.0f);			// top left of texture
            Gl.Vertex3(0.0f, -delta, 0.0f);		// top left of quad
            Gl.TexCoord2(1.0f, 1.0f);			// bottom left of texture
            Gl.Vertex3(delta, -delta, 0.0f);	// bottom left of quad
            Gl.TexCoord2(1.0f, 0.0f);			// bottom right of texture
            Gl.Vertex3(delta, 0.0f, 0.0f);		// bottom right of quad
            Gl.End();
        }
        */
        #endregion

        #region IGML Members

        public void initGML(object sender, EventArgs e)
        {
            if (inited)
                return;

            MakeCurrent();
            if (onCurrentGML != null)
                onCurrentGML(this, EventArgs.Empty);

            GLHelpers.glClearColor(Color.Azure);
            Gl.glClearDepth(100.0f);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_LINE_STIPPLE);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            resizeGML(null, EventArgs.Empty);
            
            inited = true;
            if (onInitGML != null)
                onInitGML(this, EventArgs.Empty);
        }

        public void resizeGML(object sender, EventArgs e)
        {
            int width = Size.Width;
            width = width == 0 ? 1 : width;

            int height = Size.Height;
            height = height == 0 ? 1 : height;

            MakeCurrent();
            GML.device = this;
            if (onCurrentGML != null)
                onCurrentGML(this, EventArgs.Empty);

            lock (GML.lock3D)
            {
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                Gl.glViewport(0, 0, width, height);
                Gl.glOrtho(-(double)(width) / 2.0, (double)(width) / 2.0,
                    -(double)(height) / 2.0, (double)(height) / 2.0, 0.0, 1000.0);
                Gl.glMatrixMode(Gl.GL_MODELVIEW);

                halfWidth = width / 2;
                halfHeight = height / 2;

                if (onResizeGML != null)
                    onResizeGML(this, EventArgs.Empty);
            }
        }

        public void pushMatrix()
        {
            Gl.glPushMatrix();
        }

        public void popMatrix()
        {
            Gl.glPopMatrix();
        }

        public void zeroPosition()
        {
            Gl.glLoadIdentity();
            Gl.glTranslatef((float)delta_x, (float)delta_y, -100.0f);            
        }

        public void identity()
        {
            Gl.glLoadIdentity();
            Gl.glTranslatef(0.0f, 0.0f, -100.0f);
        }

        public void rotateZ(double degree)
        {
            Gl.glRotated(degree, 0.0, 0.0, 1.0);
        }

        public void translate(int ix, int iy, int iz)
        {
            Gl.glTranslatef((float)ix, (float)iy, (float)iz);
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

        public void renderFrame(object sender, EventArgs e)
        {
            if (!inited)
                initGML(sender, e);

            MakeCurrent();
            GML.device = this;
            GML.tranBegin();
            try
            {
                if (onCurrentGML != null)
                    onCurrentGML(this, EventArgs.Empty);

                lock (GML.lock3D)
                {
                    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                    Gl.glMatrixMode(Gl.GL_MODELVIEW);

                    pushMatrix();
                    zeroPosition();
                    rotateZ(scene_angle);
                    color(Color.White);

                    if (onRenderGML != null)
                        onRenderGML(this, EventArgs.Empty);

                    popMatrix();
                    SwapBuffers();
                }
            }
            finally
            {
                GML.tranEnd();
            }
        }

        public object texFromFile(string fname)
        {
            throw new NotImplementedException();
        }

        public object texFromBitmap(ref Bitmap img)
        {
            return GLHelpers.toTexture(ref img);
        }

        public object texFromBitmapNoCheck(Bitmap img)
        {
            return GLHelpers.toTextureNoCheck(img, 0);
        }

        public void texFilter(object texture, TexFilter filter)
        {
            GLHelpers.texFilter(texture, filter);
        }

        public void texDraw(object tex, int ix, int iy, int iz, int il, int ih)
        {
            GLHelpers.Texture texture = (GLHelpers.Texture)tex;
            texture.drawTexture(ix, iy, iz);
        }

        public void texDrawBegin()
        {
        }

        public void texDrawEnd()
        {
        }

        public void texDispose(object tex)
        {
            if (tex == null)
                return;
            GLHelpers.Texture texture = (GLHelpers.Texture)tex;
            texture.Dispose();
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz, Color col)
        {
            GLHelpers.glColor3(col);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f((float)x1, (float)y1, (float)iz);
            Gl.glVertex3f((float)x2, (float)y2, (float)iz);
            Gl.glEnd();
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz)
        {
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f((float)x1, (float)y1, (float)iz);
            Gl.glVertex3f((float)x2, (float)y2, (float)iz);
            Gl.glEnd();
        }

        public void lineWidth(float width)
        {
            Gl.glLineWidth(width);
        }

        public void lineStipple(short stipple)
        {
            Gl.glLineStipple(1, stipple);
        }

        public void color(Color col)
        {
            current_color = col;
            GLHelpers.glColor3(col);
        }

        public void color4(float r, float g, float b, float alpha)
        {
            current_color = Color.FromArgb((int)(alpha * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
            Gl.glColor4f(r, g, b, alpha);
        }

        public void quadDraw(int ix, int iy, int width, int height, int iz, Color col)
        {
            GLHelpers.drawQuad(ix, iy, width, height, iz, col);
        }

        public void rectDraw(int ix, int iy, int width, int height, int iz, Color col)
        {
            GLHelpers.drawRect(ix, iy, width, height, iz, col);
        }

        public int getTextureAlignedLen(int from_len)
        {
            int texlen = 2;
            while (from_len > texlen)
                texlen <<= 1;
            return texlen;
        }

        public event EventHandler onInitGML;

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
                double ang = -scene_angle * GML.deg2rad;
                GML.cosa = Math.Cos(ang);
                GML.sina = Math.Sin(ang);
            }
        }

        public event EventHandler onCurrentGML;
        public event EventHandler onReinitDevice;
        public event EventHandler onLostDevice;

        public void lineDraw(List<Point> points, Point correct, int iz)
        {
            if (points == null || points.Count <= 1)
                return;
            int rx, ry;
            int lastrx = 0, lastry = 0;
            int addedrx=0, addedry=0;

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glNormal3d(0.0, 0.0, 1.0);

            foreach (Point p in points)
            {
                rx = p.X - correct.X;
                ry = correct.Y - p.Y;
                if( (rx < -halfWidth || rx > halfWidth || ry < -halfHeight || ry > halfHeight) &&
                    (lastrx < -halfWidth || lastrx > halfWidth || lastry < -halfHeight || lastry > halfHeight))
                {
                    lastrx = rx;
                    lastry = ry;
                    continue;
                }

                if( (addedrx != lastrx || addedry != lastry) &&
                    (lastrx < -halfWidth || lastrx > halfWidth || lastry < -halfHeight || lastry > halfHeight))
                    Gl.glVertex3f((float)lastrx, (float)lastry, (float)iz);

                Gl.glVertex3f((float)rx, (float)ry, (float)iz);
                lastrx = rx;
                lastry = ry;
                addedrx = rx;
                addedry = ry;
            }

            Gl.glEnd();
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
            pt.Y += halfHeight;
            return pt;
        }

        public IGLFont fontCreate(Font from_font)
        {
            return new GLTexTTFont(from_font);
        }

        public int curColorInt
        {
            get { return current_color.ToArgb(); }
        }

        public Color curColor
        {
            get { return current_color; }
        }

        public void repaint()
        {
            this.Invalidate();
        }

        public Point halfScreen
        {
            get { return new Point(halfWidth, halfHeight); }
        }

        #endregion
    }
}
