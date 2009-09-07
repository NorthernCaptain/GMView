using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Basic Sprite interface for drawing graphical objects
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Calls when we need to draw this object using Graphics context
        /// </summary>
        /// <param name="gr"></param>
        void draw(Graphics gr);

        /// <summary>
        /// Calls to draw the object on specific position
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void draw(Graphics gr, int x, int y);

        /// <summary>
        /// Draw object using OpenGL library (OpenTK wrapper)
        /// </summary>
        void glDraw(int centerx, int centery);

        /// <summary>
        /// Property for get/set drawing level
        /// </summary>
        int dLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Shows object on screen, and start to interact
        /// </summary>
        void show();

        /// <summary>
        /// Hide object from screen and stop to redraw itself
        /// </summary>
        void hide();
    }
}
