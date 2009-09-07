using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Wrapper for graphics middle layer
    /// </summary>
    public class GML
    {
        public const double deg2rad = 0.017453292519943295769236907684886;  //Math.PI / 180.0;
        public enum GMLType { simpleGDI, openGL, direct3D };
        public static IGML device;
        public static object lock2D = new object(); //object for locking during 2D GDI operations
        public static object lock3D = new object(); // object for locking during 3D operations
        private static bool inTran = false;
        private static bool needRepaint = false;
        public static double cosa=1.0; //cosinus of our angle
        public static double sina=0.0; //sinus of our angle

        /// <summary>
        /// Loads IGML interface from dynamic library
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns>return null if no IGML found</returns>
        public static IGML loadIGML(string assemblyName)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyName);
            foreach (Type type in assembly.GetTypes())
            {
                // Pick up a class
                if (type.IsClass == true)
                {
                    // If it does not implement the IBase Interface, skip it
                    if (type.GetInterface("GMView.IGML") == null)
                    {
                        continue;
                    }

                    // If however, it does implement the IBase Interface,
                    // create an instance of the object
                    object ibaseObject = Activator.CreateInstance(type, true);
                    return (IGML)ibaseObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Convert to rotated xy coordinates according to device angle
        /// xnew = xold*cos(a) + yold*sin(a)
        /// ynew = yold*cos(a) - xold*sin(a)
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Point translateToScene(Point pt)
        {
            double angle = -device.angle;
            if (angle <= 0.01 && angle >= -0.01)
                return pt;
            /*
            angle *= deg2rad;
            double cosa = Math.Cos(angle);
            double sina = Math.Sin(angle);
            */
            double xold = (double)pt.X;
            double yold = (double)pt.Y;
            pt.X = (int)(xold * cosa - yold * sina);
            pt.Y = (int)(yold * cosa + xold * sina);
            return pt;
        }

        /// <summary>
        /// Translates absolute screen coordinates to scene ones
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Point translateAbsToScene(Point pt)
        {
            pt = device.translateAbsToScene(pt);
            pt = translateToScene(pt);
            return device.translateSceneToAbs(pt);
        }
        /// <summary>
        /// Starts repainting transaction
        /// </summary>
        public static void tranBegin()
        {
            inTran = true;
        }

        /// <summary>
        /// Closes repainting transaction and call repaint if we asked for it
        /// </summary>
        public static void tranEnd()
        {
            inTran = false;
            if (needRepaint)
                device.repaint();
            needRepaint = false;
        }

        /// <summary>
        /// Ask for repaint screen
        /// </summary>
        public static void repaint()
        {
            if (inTran)
                needRepaint = true;
            else
                device.repaint();
        }
    }
}
