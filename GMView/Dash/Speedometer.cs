using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class Speedometer: ThreadImageRenderer
    {
        private const double deg2rad = 0.017453292519943295769236907684886;

        public double f0=175, fm=60;
        public double f0rad, fmrad;
        public double kf=4, kR=0.8, kr=0;
        public double R0=80, Xm=165, Vm=200;
        public int curSpeed = 67;
        private int ilastSpeed = -1;

        private double speedStep = 10.0;

        private Point[] topArc;
        private Point[] bottomArc;
        private int max_idx;
        private Brush fillbrush;
        private Brush bgbrush;
        private ImageDot imgMask;

        public Speedometer()
        {
            fillbrush = new SolidBrush(Color.FromArgb(252, 162, 27));
            bgbrush = new SolidBrush(Color.FromArgb(255, 55, 55, 255));
            imgMask = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashSpeedBarMask);
            Program.onShutdown += shutdown;
            calculate((int)R0, 120);
            visualizer.IsBackground = true;
        }

        public void startThread()
        {
            origImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashSpeedBarMask).img;
            visualizer.Start(this);
            updateVisualForced();
        }

        private void calcOne(int start_x, int start_y, int i, double v)
        {
            Point p = new Point();
            double f = f0rad + (1 - 1 / (kf * (v / Vm) + 1.0)) * (fmrad - f0rad);
            double R = R0 + (1 - 1 / (kR * (v / Vm) + 1.0)) * R0;
            double r = .7 * R0 * (Math.Sqrt((1 + (v / Vm) * kr)));
            p.X = (int)((v / Vm) * Xm + R * Math.Cos(f));
            p.Y = (int)(R * Math.Sin(f));
            p.X += start_x;
            p.Y = start_y - p.Y;

            topArc[i] = p;

            p.X = start_x + (int)((v / Vm) * Xm + r * Math.Cos(f));
            p.Y = start_y; //- (int)(r * Math.Sin(f));

            bottomArc[i] = p;
        }
        
        public void calculate(int start_x, int start_y)
        {
            topArc = new Point[(int)Vm+1];
            bottomArc = new Point[(int)Vm+1];

            f0rad = f0*deg2rad;
            fmrad = fm*deg2rad;

            int i = 0;
            for (double v = 0.0; v < Vm; v++)
            {
                calcOne(start_x, start_y, i++, v);
            }
            topArc[(int)Vm] = topArc[(int)Vm - 1];
            bottomArc[(int)Vm] = bottomArc[(int)Vm - 1];
            max_idx = i - 1;
        }

        protected override RenderInfoBase drawImage(RenderInfoBase info)
        {
            int ispeed = curSpeed;

            //Here we don't want to re-remder image if we have the same speed
            //or if our speed > 10km then we update only every 2 km/h
            if (ispeed == ilastSpeed || (ispeed / 2 == ilastSpeed / 2 && ispeed > 10))
                return null; //nothing changed

            Bitmap img = getAvailableImg();
            Graphics gr = Graphics.FromImage(img);
            gr.Clear(Color.Transparent);
            drawThreaded(gr);
            gr.Dispose();
            applyAlphaMask(img, imgMask.img);
            info.img = img;
            return info;
        }

        /// <summary>
        /// Draw speedometer on bitmap in separate thread
        /// </summary>
        /// <param name="gr"></param>
        protected void drawThreaded(Graphics gr)
        {

            Point [] poly = new Point[4];
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            int ispeed = curSpeed > 200 ? 200 : curSpeed;
            int istep = (int)speedStep;
            int max_i = ispeed;
            for(int i = istep; i<=max_i; i+=istep)
            {
                poly[0]= topArc[i-istep];
                poly[1]= topArc[i];
                poly[2]= bottomArc[i];
                poly[3]= bottomArc[i-istep];
                gr.FillPolygon(fillbrush, poly);
            }
            max_i = ispeed % istep;
            if (max_i > 0)
            {
                poly[0] = topArc[ispeed - max_i];
                poly[1] = topArc[ispeed];
                poly[2] = bottomArc[ispeed];
                poly[3] = bottomArc[ispeed - max_i];
                gr.FillPolygon(fillbrush, poly);
            }
        }

        /// <summary>
        /// Copies alpha channel from mask bitmap to dest bitmap.
        /// Copy only if aplha less 30, otherwise leave alpha unchanged
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="mask"></param>
        private void applyAlphaMask(Bitmap dest, Bitmap mask)
        {
            int destw = dest.Width;
            int desth = dest.Height;

            int maskw = mask.Width;
            int maskh = mask.Height;

            if (maskw != destw || maskh < desth) //we need the same width of the image and larger mask height
                return;

            System.Drawing.Imaging.BitmapData bdest = dest.LockBits(new System.Drawing.Rectangle(0, 0, destw, desth), 
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            System.Drawing.Imaging.BitmapData bmask = mask.LockBits(new System.Drawing.Rectangle(0, 0, maskw, maskh),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* destptr = (byte*)bdest.Scan0.ToPointer();
                byte* maskptr = (byte*)bmask.Scan0.ToPointer();

                byte* tox; byte* fromx;

                tox = destptr+3;
                fromx = maskptr+3;

                int delta = bdest.Stride - (destw << 2);
                for (int y = 0; y < desth; y++)
                {
                    for (int x = 0; x < destw; x++)
                    {
                        if(*fromx < 5)
                            *tox = *fromx;
                        tox += 4;
                        fromx += 4;
                    }
                    tox += delta;
                    fromx += delta;
                }
            }

            dest.UnlockBits(bdest);
            mask.UnlockBits(bmask);
        }

        public void draw(Graphics gr)
        {
            if(curImg != null)
                gr.DrawImageUnscaled(curImg, 0, 0);
        }

        public void glDraw(int ix, int iy)
        {
            if (curImg == null)
                return;
            GML.device.texDrawBegin();
            GML.device.texFilter(curTex, TexFilter.Pixel);
            GML.device.texDraw(curTex, ix, iy, 0, curImg.Width, curImg.Height);
            GML.device.texDrawEnd();
        }

        public void updateVisualForced()
        {
            visTasks.Enqueue(new RenderInfoBase());
        }

        private void shutdown()
        {
            resultControl = null;
            visualizer.Abort();
            visualizer.Join();
        }
    }
}
