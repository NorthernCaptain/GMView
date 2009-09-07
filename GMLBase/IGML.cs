using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GMView
{
    public enum TexFilter { Pixel, Linear, Smooth };

    /// <summary>
    /// Interface for Graphics middle layer. We need abstract interface becouse we can use
    /// different types of graphic library.
    /// </summary>
    public interface IGML
    {
        event EventHandler onInitGML;
        event EventHandler onResizeGML;
        event EventHandler onRenderGML;
        event EventHandler onCurrentGML;
        event EventHandler onReinitDevice;
        event EventHandler onLostDevice;

        double angle
        {
            get;
            set;
        }

        int curColorInt
        {
            get;
        }

        Color curColor
        {
            get;
        }

        Point deltaCenter
        {
            get;
            set;
        }

        /// <summary>
        /// Return size of the half of the screen
        /// </summary>
        Point halfScreen
        {
            get;
        }

        /// <summary>
        /// Initialize internal graphics engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void initGML(object sender, EventArgs e);

        /// <summary>
        /// Do some dirty work with internal library after we resize our object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void resizeGML(object sender, EventArgs e);

        /// <summary>
        /// Ask for repaint as soon as possible
        /// </summary>
        void repaint();
        /// <summary>
        /// Pushes current world matrix into internal stack.
        /// </summary>
        void pushMatrix();
        /// <summary>
        /// Pops matrix from internal stack and make it current
        /// If there is no matrix in the stack, then current matrix is left unchanged
        /// </summary>
        void popMatrix();

        /// <summary>
        /// Sets our current position into zero position of the screen
        /// </summary>
        void zeroPosition();

        void identity();

        /// <summary>
        /// Rotate our world in Z axis to the given angle in degrees
        /// </summary>
        /// <param name="degree"></param>
        void rotateZ(double degree);

        /// <summary>
        /// Translate our current position by (ix, iy, iz)
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        void translate(int ix, int iy, int iz);

        /// <summary>
        /// Called when we need to render frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void renderFrame(object sender, EventArgs e);

        /// <summary>
        /// Sets current color
        /// </summary>
        /// <param name="col"></param>
        void color(Color col);

        /// <summary>
        /// Sets color in float values with alpha transparency
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        void color4(float r, float g, float b, float alpha);

        //=========== Texture methods ================
        /// <summary>
        /// Loads texture from given filename
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        object texFromFile(string fname);

        /// <summary>
        /// Loads texture from Bitmap image. Can modify bitmap to match texture requirements
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        object texFromBitmap(ref System.Drawing.Bitmap img);

        /// <summary>
        /// Loads texture from bitmap without any checks for proper image sizes
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        object texFromBitmapNoCheck(System.Drawing.Bitmap img);

        /// <summary>
        /// Sets texture filter to the given one. Use only if your next call would be texDraw
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="fliter"></param>
        void texFilter(object texture, TexFilter filter);

        /// <summary>
        /// Draw texture on the screen with a given length and height
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        /// <param name="il"></param>
        /// <param name="ih"></param>
        void texDraw(object tex, int ix, int iy, int iz, int il, int ih);

        /// <summary>
        /// Prepare to draw a lot of textured quads
        /// </summary>
        void texDrawBegin();

        void texDrawEnd();

        /// <summary>
        /// Dispose of a texture
        /// </summary>
        /// <param name="tex"></param>
        void texDispose(object tex);

        /// <summary>
        /// Return length of texture if we give length of our image
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        int getTextureAlignedLen(int len);

        /// <summary>
        /// Draw line on the screen with a given coordinates and color
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="col"></param>
        void lineDraw(int x1, int y1, int x2, int y2, int iz, Color col);

        void lineDraw(int x1, int y1, int x2, int y2, int iz); //draw with current color

        /// <summary>
        /// Draw multi-segment line with correction point
        /// </summary>
        /// <param name="points"></param>
        /// <param name="correct"></param>
        /// <param name="iz"></param>
        void lineDraw(List<Point> points, Point correct, int iz);

        /// <summary>
        /// Set current line width
        /// </summary>
        /// <param name="width"></param>
        void lineWidth(float width);

        /// <summary>
        /// Set current line stipple
        /// </summary>
        /// <param name="stipple"></param>
        void lineStipple(short stipple);

        /// <summary>
        /// Draw quad (filled rectangle) with given color and coordinates
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="iz"></param>
        /// <param name="col"></param>
        void quadDraw(int ix, int iy, int width, int height, int iz, Color col);

        /// <summary>
        /// Draw rectangle (not filled, just frame) with given color and coordinates
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="iz"></param>
        /// <param name="col"></param>
        void rectDraw(int ix, int iy, int width, int height, int iz, Color col);

        Point translateAbsToScene(Point pt);
        Point translateSceneToAbs(Point pt);

        /// <summary>
        /// Creates library specific font for drawing and returns IGLFont interface object
        /// </summary>
        /// <param name="from_font"></param>
        /// <returns></returns>
        IGLFont fontCreate(System.Drawing.Font from_font);
    }
}
