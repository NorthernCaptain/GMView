using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using System.Runtime.InteropServices;

namespace GMView
{
    public partial class D3DUControl : UserControl, IGML
    {
        [DllImport("kernel32.dll")]
        public unsafe static extern void CopyMemory(byte* dst, byte* src, int len);

        private int delta_y = 0;
        private int delta_x = 0;
        private Device d3dDevice = null;
        private Matrix mOrtho;
        private Matrix mIdent;
        private CustomVertex.PositionColoredTextured[] vert = new CustomVertex.PositionColoredTextured[4];

        private Stack<Matrix> mtxStack = new Stack<Matrix>();

        private Line line;
        private Sprite sprite;
        private Vector3 spritevec = new Vector3();
        private Vector3[] linevert = new Vector3[2];
        private Vector3[] rectvert = new Vector3[5];
        private Matrix linemat = new Matrix();

        private Color current_color = Color.White;
        private int current_color_int;
        private double scene_angle = 0.0;

        private bool inited = false;
        private bool deviceLost = false;
        private PresentParameters d3dpp;
        private bool inTexDrawMode = false;

        private int halfWidth, halfHeight;

        public D3DUControl()
        {
            InitializeComponent();
            try
            {
                initGML(null, EventArgs.Empty);
                Resize += resizeGML;
                Paint += renderFrame;
                if (onInitGML != null)
                    onInitGML(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "EXCEPTION!");
                Environment.Exit(11);
            }
        }

        #region Test Only code

        void drawTest1(int ix, int iy, int iz, int il, int ih)
        {
            CustomVertex.PositionColored[] vert = new CustomVertex.PositionColored[4];
            float fx = (float)ix, fy = (float)iy, fz = (float)iz, fl = (float)il, fh = (float)ih;
            vert[0].Color = Color.Black.ToArgb();
            vert[0].Position = new Vector3(fx, fy, fz);

            vert[1].Color = Color.Red.ToArgb();
            vert[1].Position = new Vector3(fx + fl, fy, fz);

            vert[2].Color = Color.Green.ToArgb();
            vert[2].Position = new Vector3(fx + fl, fy - fh, fz);

            vert[3].Color = Color.Blue.ToArgb();
            vert[3].Position = new Vector3(fx, fy - fh, fz);

            d3dDevice.VertexFormat = CustomVertex.PositionColored.Format;
            d3dDevice.DrawUserPrimitives(PrimitiveType.TriangleFan, 2, vert);

        }

        void drawTest2(int ix, int iy, int iz, int il, int ih)
        {
            CustomVertex.TransformedColored[] vert = new CustomVertex.TransformedColored[4];
            float fx = (float)ix, fy = (float)iy, fz = (float)iz, fl = (float)il, fh = (float)ih;
            vert[0].Color = Color.BurlyWood.ToArgb();
            vert[0].Position = new Vector4(fx, fy, fz, 1.0f);

            vert[1].Color = Color.Red.ToArgb();
            vert[1].Position = new Vector4(fx, fy - fh, fz, 1.0f);

            vert[2].Color = Color.Red.ToArgb();
            vert[2].Position = new Vector4(fx + fl, fy - fh, fz, 1.0f);

            vert[3].Color = Color.Red.ToArgb();
            vert[3].Position = new Vector4(fx + fl, fy, fz, 1.0f);

            d3dDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            d3dDevice.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vert);

        }

        #endregion

        #region IGML Members

        public void initGML(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            if (inited)
                return;
            //
            // Do we support hardware vertex processing? If so, use it. 
            // If not, downgrade to software.
            //

            Caps caps = Manager.GetDeviceCaps(Manager.Adapters.Default.Adapter,
                                               DeviceType.Hardware);
            CreateFlags flags;

            if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                flags = CreateFlags.HardwareVertexProcessing;
            else
                flags = CreateFlags.SoftwareVertexProcessing;

            flags |= CreateFlags.FpuPreserve;
            //
            // Everything checks out - create a simple, windowed device.
            //

            d3dpp = new PresentParameters();

            d3dpp.BackBufferFormat = Format.Unknown;
            d3dpp.SwapEffect = SwapEffect.Discard;
            d3dpp.Windowed = true;
            d3dpp.EnableAutoDepthStencil = true;
            d3dpp.AutoDepthStencilFormat = DepthFormat.D16;
            d3dpp.PresentationInterval = PresentInterval.Immediate;

            d3dDevice = new Device(0, DeviceType.Hardware, this, flags, d3dpp);

            // Register an event-handler for DeviceReset and call it to continue
            // our setup.
//            d3dDevice.DeviceLost += new EventHandler(d3dDevice_DeviceLost);
//            d3dDevice.DeviceReset += new System.EventHandler(this.resetDevice);
//            d3dDevice.DeviceResizing += new System.ComponentModel.CancelEventHandler(cancelResize);
            resizeGML(d3dDevice, null);

            line = new Line(d3dDevice);
            line.Antialias = true;
            line.Width = 1.0f;
            line.PatternScale = 1.0f;

            lineStipple(0x3333);

            sprite = new Sprite(d3dDevice);

#if false
            vb = new VertexBuffer(typeof(CustomVertex.PositionColoredTextured), 4, 
                d3dDevice, Usage.WriteOnly | Usage.Dynamic, CustomVertex.PositionColoredTextured.Format, 
                Pool.Managed);
#endif
        }

        protected void cancelResize(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        void d3dDevice_DeviceLost(object sender, EventArgs e)
        {
            if (onLostDevice != null)
                onLostDevice(this, e);
        }

        private void resetDevice(object sender, EventArgs e)
        {
            resizeGML(sender, e);
        }

        public void resizeGML(object sender, EventArgs e)
        {
            //Setup orthographic projection matrix

            GML.device = this;
            if (onCurrentGML != null)
                onCurrentGML(this, EventArgs.Empty);

            lock (GML.lock3D)
            {
                d3dDevice.VertexShader = null;
                d3dDevice.VertexFormat = CustomVertex.PositionColoredTextured.Format;

                mOrtho = Matrix.OrthoLH((float)this.Size.Width, (float)this.Size.Height, 1.0f, 100.0f);
                mIdent = Matrix.Identity;
                d3dDevice.SetTransform(TransformType.Projection, mOrtho);
                d3dDevice.SetTransform(TransformType.World, mIdent);

                Matrix mView = mIdent;
                //mView.Translate(0.0f, 0.0f, 0.0f);
                d3dDevice.SetTransform(TransformType.View, mView);

                d3dDevice.SetRenderState(RenderStates.Lighting, false);
                d3dDevice.SetRenderState(RenderStates.AlphaBlendEnable, true);
                d3dDevice.SetRenderState(RenderStates.SourceBlend, (int)Blend.SourceAlpha);
                d3dDevice.SetRenderState(RenderStates.DestinationBlend, (int)Blend.InvSourceAlpha);
                d3dDevice.SetTextureStageState(0, TextureStageStates.AlphaOperation, (int)TextureOperation.Modulate);

                Microsoft.DirectX.Direct3D.Viewport ViewPort = new Viewport();
                ViewPort.X = 0;
                ViewPort.Y = 0;
                ViewPort.Width = this.Size.Width;
                ViewPort.Height = this.Size.Height;
                ViewPort.MinZ = 0.0f;
                ViewPort.MaxZ = 1000.0f;

                d3dDevice.Viewport = ViewPort;
                texFilter(null, TexFilter.Pixel);

                halfWidth = Size.Width / 2;
                halfHeight = Size.Height / 2;

                if (onResizeGML != null)
                    onResizeGML(this, EventArgs.Empty);
            }
        }

        public void renderFrame(object sender, EventArgs e)
        {
            GML.device = this;
            GML.tranBegin();
            try
            {

                if (onCurrentGML != null)
                    onCurrentGML(this, EventArgs.Empty);

                if (deviceLost && !handleLostDevice())
                    return;

                lock (GML.lock3D)
                {

                    // Now we can clear just view-port's portion of the buffer to red...
                    d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer,
                                     Color.FromArgb(255, 255, 255, 0), 1.0f, 0);
                    d3dDevice.BeginScene();
                    d3dDevice.VertexFormat = CustomVertex.PositionColoredTextured.Format;

                    pushMatrix();
                    zeroPosition();
                    rotateZ(scene_angle);
                    linemat = d3dDevice.Transform.World * d3dDevice.Transform.View * d3dDevice.Transform.Projection;

                    color(Color.White);

                    if (onRenderGML != null)
                        onRenderGML(this, EventArgs.Empty);

                    popMatrix();

                    d3dDevice.EndScene();
                    try
                    {
                        d3dDevice.Present();
                    }
                    catch (DeviceLostException)
                    {
                        deviceLost = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            finally
            {
                GML.tranEnd();
            }
        }

        private bool handleLostDevice()
        {
            try
            {
                d3dDevice.TestCooperativeLevel();
            }
            catch (DeviceLostException)
            {
            }
            catch (DeviceNotResetException)
            {
                try
                {
                    d3dDevice.Reset(d3dpp);
                    deviceLost = false;
                    if (onReinitDevice != null)
                        onReinitDevice(this, EventArgs.Empty);
                    return true;
                }
                catch (DeviceLostException)
                {
                    // If it's still lost or lost again, just do 
                    // nothing
                }
            }
            return false;
        }

        public object texFromFile(string fname)
        {
            Texture tex = TextureLoader.FromFile(d3dDevice, fname);
            return tex;
        }

        public object texFromBitmap(ref Bitmap img)
        {
            int width = getTextureAlignedLen(img.Width);
            int height = getTextureAlignedLen(img.Height);

            if (width != img.Width || height != img.Height)
            { //We need to convert our image to texture recommended sizes
                Bitmap nbmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics gr = Graphics.FromImage(nbmp);
                gr.DrawImageUnscaled(img, 0, 0);
                img = nbmp;
            }
            return texFromBitmapNoCheck(img);
        }

        public object texFromBitmapUnchanged(System.Drawing.Bitmap img)
        {
            int width = getTextureAlignedLen(img.Width);
            int height = getTextureAlignedLen(img.Height);

            System.Drawing.Bitmap timg = img;
            if (width != img.Width || height != img.Height)
            { //We need to convert our image to texture recommended sizes
                Bitmap nbmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics gr = Graphics.FromImage(nbmp);
                gr.DrawImageUnscaled(img, 0, 0);
                timg = nbmp;
            }
            object tex = texFromBitmapNoCheck(img);
            if (img != timg)
                timg.Dispose();
            return tex;
        }

        public object texFromBitmapNoCheck(Bitmap img)
        {
            try
            {
                Texture texture = new Texture(d3dDevice, img.Width, img.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
                int pitch, deltas, deltap;
                Microsoft.DirectX.GraphicsStream a = texture.LockRectangle(0, LockFlags.None, out pitch);
                System.Drawing.Imaging.BitmapData bd = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                int w = bd.Width * 4;
                int h = bd.Height;
                unsafe
                {
                    byte* to = (byte*)a.InternalDataPointer;
                    byte* from = (byte*)bd.Scan0.ToPointer();
                    for (int y = 0; y < h; ++y)
                    {
                        deltap = pitch * y;
                        deltas = (h - y - 1) * bd.Stride;
                        CopyMemory(to + deltap, from + deltas, w);
                        /* use CopyMemory call instead of this code, much faster
                        for (int x = 0; x < w; ++x)
                            to[deltap + x] = from[deltas + x];
                         */
                    }
                }
                texture.UnlockRectangle(0);
                img.UnlockBits(bd);
                return texture;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "D3D IGML EXCEPTION");
            }
            return Texture.FromBitmap(d3dDevice, img, 0, Pool.Managed);
        }

        public void texDispose(object tex)
        {
            Texture t = (Texture)tex;
            if(t != null)
                t.Dispose();
        }

        public void texFilter(object texture, TexFilter filter)
        {
            if (filter == TexFilter.Smooth)
            {
                d3dDevice.SamplerState[0].MagFilter = TextureFilter.Linear;
                d3dDevice.SamplerState[0].MinFilter = TextureFilter.Linear;
            }
            else
            {
                d3dDevice.SamplerState[0].MagFilter = TextureFilter.Point;
                d3dDevice.SamplerState[0].MinFilter = TextureFilter.Point;
            }
        }

        public void texDrawBegin()
        {
            sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.ObjectSpace | SpriteFlags.SortTexture);
            inTexDrawMode = true;
        }

        public void texDrawEnd()
        {
            sprite.End();
            inTexDrawMode = false;
        }

        public void texDraw(object tex, int ix, int iy, int iz, int il, int ih)
        {
            spritevec.X = (float)ix;
            spritevec.Y = (float)iy-ih;
            spritevec.Z = (float)0;

            if (!inTexDrawMode)
            {
                texDrawBegin();
                sprite.Draw((Texture)tex, Vector3.Empty, spritevec, current_color_int);
                texDrawEnd();
            } else
                sprite.Draw((Texture)tex, Vector3.Empty, spritevec, current_color_int);
        }

        public void texDraw2(object tex, int ix, int iy, int iz, int il, int ih)
        {
            float fx = (float)ix, fy = (float)iy, fz = (float)0, fl = (float)il, fh = (float)ih;
            //fx = 0.0f; fy = 0.0f;
            vert[0].Color = current_color_int;
            vert[0].Position = new Vector3(fx, fy, fz);
            vert[0].Tu = 0.0f;
            vert[0].Tv = 0.0f;

            vert[1].Color = current_color_int;
            vert[1].Position = new Vector3(fx + fl, fy, fz);
            vert[1].Tu = 1.0f;
            vert[1].Tv = 0.0f;

            vert[2].Color = current_color_int;
            vert[2].Position = new Vector3(fx + fl, fy - fh, fz);
            vert[2].Tu = 1.0f;
            vert[2].Tv = 1.0f;

            vert[3].Color = current_color_int;
            vert[3].Position = new Vector3(fx, fy - fh, fz);
            vert[3].Tu = 0.0f;
            vert[3].Tv = 1.0f;
            d3dDevice.SetTexture(0, (Texture)tex);
            d3dDevice.DrawUserPrimitives(PrimitiveType.TriangleFan, 2, vert);
        }

        public int getTextureAlignedLen(int from_len)
        {
            int texlen = 2;
            while (from_len > texlen)
                texlen <<= 1;
            return texlen;
        }


        // ================== Line drawing methods
        public void lineWidth(float width)
        {
            line.Width = width;
        }

        public void lineStipple(short stipple)
        {
            int istipple = (int)stipple;
            line.Pattern = (istipple << 16) | istipple;
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz, Color col)
        {
            iz = 0; //hack for direct3d
            linevert[0].X = (float)x1;
            linevert[0].Y = (float)y1;
            linevert[0].Z = (float)iz;
            linevert[1].X = (float)x2;
            linevert[1].Y = (float)y2;
            linevert[1].Z = (float)iz;
            line.DrawTransform(linevert, linemat, col);
        }

        public void lineDraw(int x1, int y1, int x2, int y2, int iz)
        {
            iz = 0; //hack for direct3d
            linevert[0].X = (float)x1;
            linevert[0].Y = (float)y1;
            linevert[0].Z = (float)iz;
            linevert[1].X = (float)x2;
            linevert[1].Y = (float)y2;
            linevert[1].Z = (float)iz;
            line.DrawTransform(linevert, linemat, current_color);
        }

        public void quadDraw(int ix, int iy, int width, int height, int iz, Color col)
        {

        }

        public void rectDraw(int ix, int iy, int width, int height, int iz, Color col)
        {
            iz = 0; //hack for direct3d

            float x1 = (float)ix;
            float y1 = (float)iy;
            float x2 = (float)(ix + width);
            float y2 = (float)(iy - height);
            float z1 = (float)iz;

            rectvert[0].X = (float)x1;
            rectvert[0].Y = (float)y1;
            rectvert[0].Z = (float)iz;

            rectvert[1].X = (float)x2;
            rectvert[1].Y = (float)y1;
            rectvert[1].Z = (float)iz;

            rectvert[2].X = (float)x2;
            rectvert[2].Y = (float)y2;
            rectvert[2].Z = (float)iz;

            rectvert[3].X = (float)x1;
            rectvert[3].Y = (float)y2;
            rectvert[3].Z = (float)iz;

            rectvert[4].X = (float)x1;
            rectvert[4].Y = (float)y1;
            rectvert[4].Z = (float)iz;

            line.DrawTransform(rectvert, linemat, col);
        }

        public void pushMatrix()
        {
            mtxStack.Push(d3dDevice.Transform.World);
        }

        public void popMatrix()
        {
            if (mtxStack.Count > 0)
                d3dDevice.Transform.World = mtxStack.Pop();
        }

        public void rotateZ(double degrees)
        {
            d3dDevice.Transform.World = Matrix.RotationZ(Geometry.DegreeToRadian((float)degrees)) * d3dDevice.Transform.World;
        }

        public void translate(int x, int y, int z)
        {
            d3dDevice.Transform.World = Matrix.Translation((float)x, (float)y, (float)z) * d3dDevice.Transform.World;
        }

        public void zeroPosition()
        {
            d3dDevice.Transform.World = mIdent;
            translate(delta_x, delta_y, 1);
            //rotateZ(180.0);
        }


        public void identity()
        {
            d3dDevice.Transform.World = mIdent;
            translate(0, 0, 1);
        }

        public void color(Color col)
        {
            current_color = col;
            current_color_int = col.ToArgb();
        }

        public void color4(float r, float g, float b, float alpha)
        {
            current_color = Color.FromArgb((int)(alpha * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
            current_color_int = current_color.ToArgb();
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
                double ang = scene_angle * GML.deg2rad;
                GML.cosa = Math.Cos(ang);
                GML.sina = Math.Sin(ang);
            }
        }

        public event EventHandler onCurrentGML;
        public event EventHandler onReinitDevice;
        public event EventHandler onLostDevice;

        private List<Vector3> linePoints = new List<Vector3>();

        public void lineDraw(List<Point> points, Point correct, int iz)
        {
            if (points == null || points.Count <= 1)
                return;
            int rx, ry;
            int lastrx, lastry;

            bool lastVis = true;
            bool curVis = true;

            linePoints.Clear();
            iz = 0;

            lastrx = points[0].X;
            lastry = points[0].Y;

            line.Begin();

            foreach (Point p in points)
            {
                rx = p.X - correct.X;
                ry = correct.Y - p.Y;

                curVis = rx >= -halfWidth && rx <= halfWidth && ry >= -halfHeight && ry <= halfHeight;                

                if( !curVis && !lastVis )
                {
                    lastrx = rx;
                    lastry = ry;
                    lastVis = curVis;
                    continue;
                }

                if (!curVis && lastVis) //we moved from visible area to hidden
                {
                    linePoints.Add(new Vector3((float)rx, (float)ry, (float)iz));
                    if (linePoints.Count > 1)
                        line.DrawTransform(linePoints.ToArray(), linemat, current_color);
                    linePoints.Clear();
                    lastrx = rx;
                    lastry = ry;
                    lastVis = curVis;
                    continue;
                }

                if (curVis && !lastVis) //we moved back from hidden area to visible one
                {
                    linePoints.Add(new Vector3((float)lastrx, (float)lastry, (float)iz));
                }

                linePoints.Add(new Vector3((float)rx, (float)ry, (float)iz));
                lastrx = rx;
                lastry = ry;
                lastVis = curVis;                
            }

            if(linePoints.Count > 1)
                line.DrawTransform(linePoints.ToArray(), linemat, current_color);

            line.End();
        }

        public Point translateAbsToScene(Point pt)
        {
            pt.X -= (halfWidth+delta_x);
            pt.Y -= (halfHeight-delta_y);
            return pt;
        }

        public Point translateSceneToAbs(Point pt)
        {
            pt.X += halfWidth;
            pt.Y += (halfHeight);
            return pt;
        }

        public IGLFont fontCreate(System.Drawing.Font from_font)
        {
            return new D3DGLFont(from_font, d3dDevice, sprite);
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

        public Point halfScreen
        {
            get { return new Point(halfWidth, halfHeight); }
        }

        public void repaint()
        {
            this.Invalidate();
        }

        #endregion
    }
}
