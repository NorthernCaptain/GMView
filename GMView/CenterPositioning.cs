using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Class represents the system that moves center of the map around the center of the window
    /// according to the GPS direction and speed of movement.
    /// </summary>
    public class CenterPositioning
    {
        /// <summary>
        /// Here we define directions of our center movements
        /// </summary>
        private static Point[] dirCenters = new Point[] 
        { 
            new Point( 0, 1), //N
            new Point(-1, 1), //NE
            new Point(-1, 0), //E
            new Point(-1,-1), //SE
            new Point( 0,-1), //S
            new Point( 1,-1), //SW
            new Point( 1, 0), //W
            new Point( 1, 1), //NW
            new Point( 0, 1)  //N
        };

        private Point start_pos = new Point();
        private Point current_pos = new Point();
        private Point finish_pos = new Point();
        private Point distance = new Point();
        private int steps_to_finish = 0;

        /// <summary>
        /// static center position that does not depend on the GPS information
        /// </summary>
        private Point static_pos;

        /// <summary>
        /// Return static delta center position
        /// </summary>
        public Point deltaCenter
        {
            get { return static_pos; }
            set { static_pos = value; current_track_onTrackChanged(); }
        }

        /// <summary>
        /// Maximum steps for movement from current position till finish position
        /// </summary>
        private const int max_steps = 20;

        private GPSTrack current_track = null;

        /// <summary>
        /// Timer that will do smooth animation movement of our center position
        /// </summary>
        private System.Windows.Forms.Timer timer;

        public CenterPositioning(System.Windows.Forms.Timer itimer)
        {
            timer = itimer;
            timer.Tick += doNextStep;
            GPSTrackFactory.singleton.onRecordingTrackChanged += Glob_onCurrentTrackChanged;
            Glob_onCurrentTrackChanged(GPSTrackFactory.singleton.recordingTrack);
            current_track_onTrackChanged();
        }

        /// <summary>
        /// This method will be called when the current recording track changes to a new one.
        /// We change our current track too and assign new handler
        /// </summary>
        /// <param name="gtrack"></param>
        void Glob_onCurrentTrackChanged(GPSTrack gtrack)
        {
            if(current_track != null)
                current_track.onTrackChanged -= current_track_onTrackChanged;
            current_track = gtrack;
            if(current_track != null)
                current_track.onTrackChanged += current_track_onTrackChanged;
        }

        /// <summary>
        /// This method is called every time the position in the current track changes
        /// </summary>
        void current_track_onTrackChanged()
        {
            Point newpos;
            if(current_track == null || current_track.lastNonZeroPos == null)
                newpos = static_pos;
            else
                newpos = getDeltaCenter(current_track.lastNonZeroPos);

            if (finish_pos.X != newpos.X || finish_pos.Y != newpos.Y)
            {
                finish_pos = newpos;
                start_pos = current_pos;
                steps_to_finish = max_steps;
                distance.X = System.Math.Abs(start_pos.X - finish_pos.X);
                distance.Y = System.Math.Abs(start_pos.Y - finish_pos.Y);
                ((System.Windows.Forms.Control)(GML.device)).Invoke(new System.Windows.Forms.MethodInvoker(timer.Start));
            }
        }

        /// <summary>
        /// return speed factor that affects delta movement of the center
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        private int getSpeedFactor(double speed)
        {
            int sp;
            if (Program.opt.gps_follow_map == false)
                return 0;

            switch (Program.opt.dynamic_center)
            {
                case Options.DynCenterType.SpeedDriven:
                    if (speed < 10.0)
                        return 0;
                    sp = (int)speed + 30;
                    sp = sp / 40;
                    sp = sp * 60;
                    return sp;
                case Options.DynCenterType.Forward23:
                    sp = GML.device.halfScreen.Y * 2 / 3;
                    return sp;
                case Options.DynCenterType.Forward34:
                    sp = GML.device.halfScreen.Y * 3 / 4;
                    return sp;
            }
            return 0;
        }

        /// <summary>
        /// return new position on the center in relative coordinates based on gps info and
        /// static center position
        /// </summary>
        /// <param name="gpos"></param>
        /// <returns></returns>
        private Point getDeltaCenter(ncGeo.NMEA_LL gpos)
        {
            Point cur_pos = static_pos;
            int speed_factor = getSpeedFactor(gpos.speed);
            if (Program.opt.gps_rotate_map)
            {
                cur_pos.Y -= speed_factor;
                return cur_pos;
            }

            int idx = (int)((gpos.dir_angle + 22.5) / 45.0);

            cur_pos.X += dirCenters[idx].X * speed_factor;
            cur_pos.Y -= dirCenters[idx].Y * speed_factor;
            return cur_pos;
        }

        /// <summary>
        /// Called each time when timer ticks, calculate new center position according to animation step
        /// and finish_pos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doNextStep(object sender, EventArgs e)
        {
            if (steps_to_finish < 0)
            {
                timer.Stop();
                return;
            }
            current_pos = start_pos;
            if (start_pos.X < finish_pos.X)
                current_pos.X += distance.X * (max_steps - steps_to_finish) / max_steps;
            else
                current_pos.X -= distance.X * (max_steps - steps_to_finish) / max_steps;

            if (start_pos.Y < finish_pos.Y)
                current_pos.Y += distance.Y * (max_steps - steps_to_finish) / max_steps;
            else
                current_pos.Y -= distance.Y * (max_steps - steps_to_finish) / max_steps;
            
            steps_to_finish--;

            GML.device.deltaCenter = current_pos;
            GML.device.repaint();
        }
    }
}
