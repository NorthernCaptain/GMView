using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Generic interface for fonts in OpenGL
    /// </summary>
    public interface IGLFont: IDisposable
    {
        /// <summary>
        /// Draw left justified text
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        void draw(string str, int ix, int iy, int iz);
        /// <summary>
        /// Draw right justified text. XYZ coordinates are of the right upper corner
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        void drawright(string str, int ix, int iy, int iz);

        /// <summary>
        /// Draw text on scene, not on dashboards
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        void drawscene(string str, int ix, int iy, int iz);
    }
}
