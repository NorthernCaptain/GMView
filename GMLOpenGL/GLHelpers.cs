using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.OpenGl;

namespace GMView
{
    using gltex = Int32;
    public class GLHelpers
    {
        public class Texture 
        {
            public Texture(gltex itex, int iwidth, int iheight) { tex = itex; width = iwidth; height = iheight; }
            public gltex tex;
            public int width, height;
            public void drawTexture(int ix, int iy, int iz)
            {
                float fx = (float)ix;
                float fy = (float)iy-2; //Dirty hack
                float fx2 = fx + (float)width;
                float fy2 = fy - (float)height;
                float fz = (float)iz;

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex);
                Gl.glBegin(Gl.GL_QUADS);
                // Front Face
                Gl.glNormal3f(0.0f, 0.0f, 1.0f);
                Gl.glTexCoord2f(0.0f, 0.0f);			// top right of texture
                Gl.glVertex3f(fx, fy, fz);		// top right of quad
                Gl.glTexCoord2f(0.0f, 1.0f);			// top left of texture
                Gl.glVertex3f(fx, fy2, fz);		// top left of quad
                Gl.glTexCoord2f(1.0f, 1.0f);			// bottom left of texture
                Gl.glVertex3f(fx2, fy2, fz);	// bottom left of quad
                Gl.glTexCoord2f(1.0f, 0.0f);			// bottom right of texture
                Gl.glVertex3f(fx2, fy, fz);		// bottom right of quad
                Gl.glEnd();
            }

            public void Dispose()
            {
                if(tex != 0)
                    Gl.glDeleteTextures(1, new int[] { tex });
                tex = 0;
            }

            public void Filter(int filter)
            {
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D,
                    Gl.GL_TEXTURE_MIN_FILTER,
                    filter);		// Linear Filtering
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D,
                    Gl.GL_TEXTURE_MAG_FILTER,
                    filter);		// Linear Filtering
            }
        }

        public static void texFilter(object texo, TexFilter flt)
        {
            Texture tex = (Texture)texo;

            if (flt == TexFilter.Smooth)
            {
                tex.Filter(Gl.GL_LINEAR);
            }
            else
                tex.Filter(Gl.GL_NEAREST);
        }

        public static int getTextureAlignedLen(int from_len)
        {
            int texlen = 2;
            while (from_len > texlen)
                texlen <<= 1;
            return texlen;
        }

        public static Texture toTexture(ref Bitmap img)
        {
            return toTexture(ref img, Gl.GL_NEAREST);
        }
        
        public static Texture toTexture(ref Bitmap img, uint filter)
        {
            int width = getTextureAlignedLen(img.Width);
            int height = getTextureAlignedLen(img.Height);

            if (width != img.Width || height != img.Height)
            { //We need to convert our image to texture recommended sizes
                Bitmap nbmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics gr = Graphics.FromImage(nbmp);
                gr.DrawImageUnscaled(img, 0, 0);
                img.Dispose();
                img = nbmp;
            }
            return toTextureNoCheck(img, filter);
        }

        public static Texture toTexture2(Bitmap img)
        {
            uint filter = Gl.GL_NEAREST;
            int width = getTextureAlignedLen(img.Width);
            int height = getTextureAlignedLen(img.Height);

            Bitmap timg = img;
            if (width != img.Width || height != img.Height)
            { //We need to convert our image to texture recommended sizes
                Bitmap nbmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics gr = Graphics.FromImage(nbmp);
                gr.DrawImageUnscaled(img, 0, 0);
                timg = nbmp;
            }
            Texture tex = toTextureNoCheck(timg, filter);
            if (img != timg)
                timg.Dispose();
            return tex;
        }

        public static Texture toTextureNoCheck(Bitmap img, uint filter)
        {
            BitmapData bitmapdata;
            gltex[] texture = new gltex[1];

            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);

            bitmapdata = img.LockBits(rect, ImageLockMode.ReadOnly,
                                      PixelFormat.Format32bppArgb);
            try
            {
                Gl.glGenTextures(1, texture);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0,
                    (int)Gl.GL_RGBA,
                    img.Width, img.Height,
                    0, Gl.GL_BGRA_EXT, Gl.GL_UNSIGNED_BYTE,
                    bitmapdata.Scan0);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D,
                    Gl.GL_TEXTURE_MIN_FILTER,
                    (int)filter);		// Linear Filtering
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D,
                    Gl.GL_TEXTURE_MAG_FILTER,
                    (int)filter);		// Linear Filtering
            }
            finally
            {
                img.UnlockBits(bitmapdata);
            }
            return new Texture(texture[0], img.Width, img.Height);
        }

        public static void drawQuad(int ix, int iy, int width, int height, int iz, Color col)
        {
            float fx = (float)ix;
            float fy = (float)iy;
            float fx2 = fx + (float)width;
            float fy2 = fy - (float)height;
            float fz = (float)iz;

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            GLHelpers.glColor4(col);
            Gl.glBegin(Gl.GL_QUADS);
            // Front Face
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3f(fx, fy, fz);		// top right of quad
            Gl.glVertex3f(fx, fy2, fz);		// top left of quad
            Gl.glVertex3f(fx2, fy2, fz);	// bottom left of quad
            Gl.glVertex3f(fx2, fy, fz);		// bottom right of quad
            Gl.glEnd();
            GLHelpers.glColor4(Color.FromArgb(255, Color.White));
        }

        public static void drawRect(int ix, int iy, int width, int height, int iz, Color col)
        {
            float fx = (float)ix;
            float fy = (float)iy;
            float fx2 = fx + (float)width;
            float fy2 = fy - (float)height;
            float fz = (float)iz;

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            GLHelpers.glColor4(col);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3f(fx, fy, fz);		// top right of quad
            Gl.glVertex3f(fx, fy2, fz);		// top left of quad
            Gl.glVertex3f(fx2, fy2, fz);	// bottom left of quad
            Gl.glVertex3f(fx2, fy, fz);		// bottom right of quad
            Gl.glVertex3f(fx, fy, fz);		// top right of quad
            Gl.glEnd();
            GLHelpers.glColor4(Color.FromArgb(255, Color.White));
        }

        public static void glClearColor(System.Drawing.Color col)
        {
            Gl.glClearColor((float)col.R / 255.0f, (float)col.G / 255.0f, (float)col.B / 255.0f, (float)col.A / 255.0f);
        }

        public static void glColor3(System.Drawing.Color col)
        {
            Gl.glColor4f((float)col.R / 255.0f, (float)col.G / 255.0f, (float)col.B / 255.0f, 1.0f);
        }

        public static void glColor4(System.Drawing.Color col)
        {
            Gl.glColor4f((float)col.R / 255.0f, (float)col.G / 255.0f, (float)col.B / 255.0f, (float)col.A / 255.0f);
        }
    }
}
