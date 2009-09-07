using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    public enum DashMode { Normal, Wrapped, Hidden };

    public interface IDashBoard
    {
        DashBoardContainer.Justify justify
        {
            get;
        }

        /// <summary>
        /// Width of our board
        /// </summary>
        int width
        {
            get;
        }

        /// <summary>
        /// Height of our board
        /// </summary>
        int height
        {
            get;
        }

        /// <summary>
        /// Gets/Sets visual mode of dashboard (normal, wrapped or hidden)
        /// </summary>
        DashMode mode
        {
            get;
            set;
        }

        /// <summary>
        /// Draw the board on the screen on given location
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        void draw(int ix, int iy);

        /// <summary>
        /// Receive this call when mouse left button was pressed and released in our dashboard area
        /// </summary>
        /// <param name="xy"></param>
        /// <returns>return true if this message was for our dashboard</returns>
        bool mouseUp(System.Drawing.Point xy);
        bool mouseDown(System.Drawing.Point xy);
        bool mouseClick(System.Drawing.Point xy);
        bool mouseDoubleClick(System.Drawing.Point xy);
        bool mouseMove(System.Drawing.Point xy);
    }
}
