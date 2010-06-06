using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GMView.UIHelper
{
    /// <summary>
    /// Create new track by manually defining waypoints
    /// </summary>
    public class ManualTrackMode: MouseBaseProc
    {
        private GPSTrack manual_track;

        public GPSTrack manualTrack
        {
            get { return manual_track; }
            set 
            {
                if (manual_track != null)
                {
                    GPSTrackFactory.singleton.delTrack(manual_track);
                    mainform.mapo.delSub(manual_track);
                }

                manual_track = value;
                if (manual_track == null)
                    initTrack();
                GPSTrackFactory.singleton.addTrack(manual_track);
                mainform.mapo.addSub(manual_track);
            }
        }

        private void initTrack()
        {
            manual_track = new GPSTrack(mainform.mapo);
            manual_track.trackMode = GPSTrack.TrackMode.ViewSaved;
            manual_track.need_arrows = false;
            manual_track.track_name = "Manual track";
        }

        public ManualTrackMode(GMViewForm form, UserControl dPane)
        {
            mainform = form;
            drawPane = dPane;
            areaselection = Program.opt.newUserSelectionArea(mainform.mapo);
            areaselection.onAreaSelection += areaselection_onAreaSelection;
        }

        void areaselection_onAreaSelection(double lon1, double lat1, double lon2, double lat2)
        {
            List<LinkedListNode<ncGeo.NMEA_LL>> selected = new List<LinkedListNode<ncGeo.NMEA_LL>>();

            LinkedListNode<ncGeo.NMEA_LL> currentPointNode = manual_track.trackPointData.First;

            while(currentPointNode != null)
            {
                if(currentPointNode.Value.lon >= lon1 &&
                    currentPointNode.Value.lon <= lon2 &&
                    currentPointNode.Value.lat <= lat1 &&
                    currentPointNode.Value.lat >= lat2)
                {
                    selected.Add(currentPointNode);
                    if(currentPointNode.Value.ptype != ncGeo.NMEA_LL.PointType.TP)
                    {
                        currentPointNode.Value.prevPtype = currentPointNode.Value.ptype;
                        currentPointNode.Value.ptype = ncGeo.NMEA_LL.PointType.MARKWP;
                    }
                }
                currentPointNode = currentPointNode.Next;
            }

            if(selected.Count == 0)
            {
                areaselection.reset();
            }
            else
            {
                if(MessageBox.Show("You have selected " + selected.Count + " points on the track.\nDo you want to DELETE them?",
                    "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    manual_track.deleteSelectedPoints(selected);
                    manual_track.updateVisual();
                    selected.Clear();
                }
                else
                {
                    foreach (LinkedListNode<ncGeo.NMEA_LL> pt in selected)
                    {
                        if(pt.Value.ptype == ncGeo.NMEA_LL.PointType.MARKWP)
                            pt.Value.ptype = pt.Value.prevPtype;
                    }
                }
                areaselection.reset();
                GML.repaint();
            }
        }

        public override void initGL()
        {
            if (manual_track != null)
                manual_track.initGLData();
        }

        public override void modeEnter(MouseBaseProc oldone)
        {
            base.modeEnter(oldone);
            mainform.mapo.addSub(areaselection);
            areaselection.reset();
            areaselection.show();
        }

        public override void modeLeave()
        {
            base.modeLeave();
            mainform.mapo.delSub(areaselection);
        }

        /// <summary>
        /// The point on the track we are dragging
        /// </summary>
        private LinkedListNode<ncGeo.NMEA_LL> dragPoint = null;
        private ncGeo.NMEA_LL.PointType original_ptype;

        private UserSelectionArea areaselection;

        protected override bool onMouseMoveTranslated(System.Drawing.Point xy)
        {
            double lon, lat;
            Point lastxy = mouse_last_p;

            switch (dragEnabled)
            {
                case DragStage.NoDrag:
                    if(controlPressed)
                    {
                        startSelection(xy);
                        break;
                    }
                    lastxy = GML.translateAbsToScene(lastxy);
                    mainform.mapo.getLonLatByVisibleXY(lastxy, out lon, out lat);
                    ncGeo.FindNearestPointByVisibleDistance ctx = new ncGeo.FindNearestPointByVisibleDistance(lon, lat);
                    manual_track.findNearest(ctx);
                    if (ctx.resultPoint == null)
                    {
                        dragEnabled = MouseBaseProc.DragStage.Ignore;
                        break;
                    }
                    mainform.mapo.getVisibleXYByLonLat(ctx.resultPoint.Value.lon, ctx.resultPoint.Value.lat, out xy);
                    if (Math.Abs(lastxy.X - xy.X) < 20 && Math.Abs(lastxy.Y - xy.Y) <= drag_start_delta * 2)
                    {
                        dragEnabled = DragStage.Dragging;
                        dragPoint = ctx.resultPoint;
                        manual_track.updatePointPosition(dragPoint, lon, lat);
                        original_ptype = dragPoint.Value.ptype;
                        dragPoint.Value.ptype = ncGeo.NMEA_LL.PointType.MARKWP;
                        GML.repaint();
                    }
                    else
                        dragEnabled = MouseBaseProc.DragStage.Ignore;
                    break;
                case DragStage.Dragging:
                    lastxy = GML.translateAbsToScene(lastxy);
                    mainform.mapo.getLonLatByVisibleXY(lastxy, out lon, out lat);
                    manual_track.updatePointPosition(dragPoint, lon, lat);
                    GML.repaint();
                    break;
                case MouseBaseProc.DragStage.Second:
                    areaselection.setDeltaXY(xy);
                    GML.repaint();
                    break;
                default:
                    break;
            }
            return true;
        }

        protected override bool onMouseUpTranslated(System.Drawing.Point xy)
        {
            DragStage drag = dragEnabled;

            dragEnabled = DragStage.NoDrag;

            if (manual_track == null)
                return false;
            
            switch(drag)
            {
                case MouseBaseProc.DragStage.Dragging:
                    dragPoint.Value.ptype = original_ptype;
                    manual_track.calculateParameters();
                    manual_track.updateVisual();
                    GML.repaint();
                    return true;
                case MouseBaseProc.DragStage.Second:
                    areaselection.setEndXY(xy);
                    GML.repaint();
                    break;
                default:
                    if (Math.Abs(mouse_press_p.X - mouse_release_p.X) > drag_start_delta ||
                        Math.Abs(mouse_press_p.Y - mouse_release_p.Y) > drag_start_delta)
                        return true;

                    if (eargs.Button == MouseButtons.Right)
                    {
                        removeLastPoint();
                    }
                    else
                    {
                        double lon, lat;
                        mainform.mapo.getLonLatByVisibleXY(xy, out lon, out lat);
                        addNewPoint(lon, lat);
                    }
                    GML.repaint();
                    break;
            }

            return true;
        }

        public void resetTrack()
        {
            manual_track.resetTrackData();
        }

        /// <summary>
        /// Remove last point from track
        /// </summary>
        private void removeLastPoint()
        {
            manual_track.delLastPoint();
        }

        /// <summary>
        /// Add new waypoint to the track with given coordinates
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        private void addNewPoint(double lon, double lat)
        {
            ncGeo.NMEA_LL nmea_ll = new NMEA_RMC(lon, lat, ncGeo.NMEA_LL.PointType.ENDTP);
            manual_track.addManualPoint(nmea_ll);
            manual_track.show();
        }

        /// <summary>
        /// Starts the selection of area on the map
        /// </summary>
        /// <param name="xy"></param>
        private void startSelection(Point xy)
        {
            Point lastxy = GML.translateAbsToScene(mouse_press_p);
            areaselection.reset();
            areaselection.setStartXY(lastxy);
            areaselection.setDeltaXY(xy);
            GML.repaint();
            dragEnabled = MouseBaseProc.DragStage.Second;
        }

    }
}
