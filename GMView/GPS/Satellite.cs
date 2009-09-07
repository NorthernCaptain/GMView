using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// GPS Satellite information class
    /// </summary>
    public class Satellite
    {
        public enum State { Unknown, Visible, InUse };
        public int prn; //satellite number
        public int height; //in degrees over horizon
        public int azimuth;

        public int signal_strength; //signal strength in Db
        public State state = State.Unknown;

        public Satellite()
        {
        }


        internal void copy(Satellite sat)
        {
            height = sat.height;
            azimuth = sat.azimuth;
            signal_strength = sat.signal_strength;
            if (state != State.InUse)
                state = sat.state;
        }

        internal void getXY(int radius, out Point xy)
        {
            double hippo = System.Math.Cos(System.Math.PI * height / 180.0) * (double)radius;
            double azRad = System.Math.PI * azimuth / 180.0;
            double dy = System.Math.Cos(azRad) * hippo;
            double dx = System.Math.Sin(azRad) * hippo;

            xy = new Point((int)dx, (int)dy);
        }
    }
}
