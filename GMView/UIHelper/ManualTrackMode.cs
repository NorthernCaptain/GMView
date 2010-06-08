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
        private Color origColor = Color.DarkRed;

        private ContextMenuStrip contextMenu;

        private List<LinkedListNode<ncGeo.NMEA_LL>> selected = new List<LinkedListNode<ncGeo.NMEA_LL>>();

        private ToolStripLabel pointSelectedLabel = new ToolStripLabel();

        /// <summary>
        /// Current stage of dragging. NoDrag means that drag is not started yet
        /// </summary>
        protected DragStage dragEnabled = DragStage.NoDrag;

        /// <summary>
        /// Stages of the drag process
        /// </summary>
        protected enum DragStage {  
                                    NoDrag, 
                                    Dragging, 
                                    Ignore, 
                                    NewAreaSelection, 
                                    AddAreaSelection, 
                                    SubtractAreaSelection 
                                 };

        public GPSTrack manualTrack
        {
            get { return manual_track; }
            set 
            {
                if (manual_track != null)
                    manual_track.trackColor = origColor;

                manual_track = value;
                if (manual_track == null)
                {
                    initTrack();
                    GPSTrackFactory.singleton.addTrack(manual_track);
                    mainform.mapo.addSub(manual_track);
                }
                else
                {
                    origColor = manual_track.trackColor;
                    manual_track.trackColor = Color.Red;
                }
            }
        }

        private void initTrack()
        {
            manual_track = new GPSTrack(mainform.mapo);
            manual_track.trackMode = GPSTrack.TrackMode.ViewSaved;
            manual_track.need_arrows = false;
            manual_track.track_name = GPSTrackFactory.singleton.genUniqueName("Manual track");
        }

        public ManualTrackMode(GMViewForm form, UserControl dPane)
        {
            mainform = form;
            drawPane = dPane;

            areaselection = Program.opt.newUserSelectionArea(mainform.mapo);
            areaselection.onAreaSelection += areaselection_onAreaSelection;

            contextMenu = new ContextMenuStrip();
            contextMenu.Closed += new ToolStripDropDownClosedEventHandler(contextMenu_Closed);
            contextMenu.Opening += new System.ComponentModel.CancelEventHandler(contextMenu_Opening);

            contextMenu.Items.Add(pointSelectedLabel);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add(new ToolStripMenuItem("Delete points", null, deleteSelectedPoints));
            contextMenu.Items.Add(new ToolStripMenuItem("Extract into new track", null, extractPointsIntoNewTrack));
            contextMenu.Items.Add(new ToolStripMenuItem("Add point in between", null, addPointsInBetween));
            contextMenu.Items.Add("-");
            contextMenu.Items.Add(new ToolStripMenuItem("Reset selection", null, resetSelection));
            contextMenu.Items.Add(new ToolStripMenuItem("Continue selection", null, continueSelection));

        }

        void singleton_onCurrentTrackChanged(GPSTrack gtrack)
        {
            if (gtrack != null && gtrack != GPSTrackFactory.singleton.recordingTrack)
                manualTrack = gtrack;
            GML.repaint();
        }

        void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pointSelectedLabel.Text = selected.Count.ToString() + " points selected";
            e.Cancel = false;
        }

        void contextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            areaselection.reset();
        }

        #region ==== Selection area processing methods ====

        /// <summary>
        /// Called after we finished selecting the area and need to process this area
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        void areaselection_onAreaSelection(double lon1, double lat1, double lon2, double lat2)
        {            
            LinkedListNode<ncGeo.NMEA_LL> currentPointNode = manual_track.trackPointData.First;

            while(currentPointNode != null)
            {
                if(currentPointNode.Value.lon >= lon1 &&
                    currentPointNode.Value.lon <= lon2 &&
                    currentPointNode.Value.lat <= lat1 &&
                    currentPointNode.Value.lat >= lat2)
                {
                    if (altPressed)
                    {
                        if (currentPointNode.Value.ptype == ncGeo.NMEA_LL.PointType.MARKWP)
                        {
                            currentPointNode.Value.ptype = currentPointNode.Value.prevPtype;
                        }
                        selected.Remove(currentPointNode);
                    }
                    else
                    {

                        if (!selected.Contains(currentPointNode))
                            selected.Add(currentPointNode);
                    }
                }
                currentPointNode = currentPointNode.Next;
            }

            markSelection();

            areaselection.reset();
            if (controlPressed == false && altPressed == false)
                contextMenu.Show(drawPane, mouse_release_p);
        }

        /// <summary>
        /// Marks selected points with MARKWP for highlighting on the map
        /// </summary>
        private void markSelection()
        {
            foreach (LinkedListNode<ncGeo.NMEA_LL> currentPointNode in selected)
            {
                if (currentPointNode.Value.ptype != ncGeo.NMEA_LL.PointType.TP
                    && currentPointNode.Value.ptype != ncGeo.NMEA_LL.PointType.MARKWP)
                {
                    currentPointNode.Value.prevPtype = currentPointNode.Value.ptype;
                    currentPointNode.Value.ptype = ncGeo.NMEA_LL.PointType.MARKWP;
                }
            }
        }

        private void resetSelection(object sender, EventArgs args)
        {
            selectionReset();
            areaselection.reset();
        }

        private void continueSelection(object sender, EventArgs args)
        {
            markSelection();
            GML.repaint();
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
            dragEnabled = DragStage.NewAreaSelection;
            //           if (controlPressed == false && altPressed == false)
            //                selectionReset();
        }

        /// <summary>
        /// Clear the selection list
        /// </summary>
        private void selectionReset()
        {
            foreach (LinkedListNode<ncGeo.NMEA_LL> pt in selected)
            {
                if (pt.Value.ptype == ncGeo.NMEA_LL.PointType.MARKWP)
                    pt.Value.ptype = pt.Value.prevPtype;
            }
            selected.Clear();
            GML.repaint();
        }

        /// <summary>
        /// Called when we finished selecting the area
        /// </summary>
        /// <param name="xy"></param>
        private void finishAreaSelection(Point xy)
        {
            areaselection.setEndXY(xy);
            GML.repaint();
        }

        #endregion

        #region ==== Track selected points operations ====
        /// <summary>
        /// Delete selected points from the track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void deleteSelectedPoints(object sender, EventArgs args)
        {
            manual_track.deleteSelectedPoints(selected);
            manual_track.updateVisual();
            selectionReset();
            GML.repaint();
        }

        /// <summary>
        /// Extracts selected points into new track that is added on the fly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void extractPointsIntoNewTrack(object sender, EventArgs args)
        {
            if (selected.Count == 0)
                return;

            string secondName = GPSTrackFactory.singleton.genUniqueName(manual_track.track_name);

            GPSTrack newTrack = new GPSTrack(mainform.mapo);
            newTrack.track_name = secondName;
            newTrack.trackMode = GPSTrack.TrackMode.ViewSaved;
            newTrack.need_arrows = false;

            foreach (LinkedListNode<ncGeo.NMEA_LL> pointNode in selected)
            {
                pointNode.List.Remove(pointNode);
                if (pointNode.Value.ptype == ncGeo.NMEA_LL.PointType.MARKWP)
                    pointNode.Value.ptype = pointNode.Value.prevPtype;
                newTrack.addManualPoint(pointNode.Value);
            }


            manual_track.calculateParameters();
            manual_track.updateVisual();
            newTrack.calculateParameters();
            newTrack.initGLData();
            newTrack.updateVisual();
            newTrack.trackColor = Color.DarkRed;

            GPSTrackFactory.singleton.addTrack(newTrack);
            mainform.mapo.addSub(newTrack);
            newTrack.show();
            selectionReset();
        }

        /// <summary>
        /// Goes through selected points, find pairs (two connected points) and
        /// adds new point in between.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void addPointsInBetween(object sender, EventArgs args)
        {
            bool added = false;

            foreach (LinkedListNode<ncGeo.NMEA_LL> pointNode in selected)
            {
                if (pointNode.Next != null &&
                    selected.Contains(pointNode.Next))
                {
                    double lon = (pointNode.Value.lon + pointNode.Next.Value.lon) / 2.0;
                    double lat = (pointNode.Value.lat + pointNode.Next.Value.lat) / 2.0;
                    ncGeo.NMEA_LL nmea_ll = new NMEA_RMC(lon, lat, ncGeo.NMEA_LL.PointType.MWP);
                    pointNode.List.AddAfter(pointNode, nmea_ll);
                    added = true;
                }
            }
            if (added)
            {
                manual_track.calculateParameters();
                manual_track.updateVisual();
            }
            selectionReset();
            GML.repaint();
        }

#endregion

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

            GPSTrackFactory.singleton.onCurrentTrackChanged += singleton_onCurrentTrackChanged;

            if (GPSTrackFactory.singleton.currentTrack != null &&
                GPSTrackFactory.singleton.currentTrack != GPSTrackFactory.singleton.recordingTrack)
                manualTrack = GPSTrackFactory.singleton.currentTrack;
            else
                manualTrack = manual_track;
        }

        public override void modeLeave()
        {
            base.modeLeave();
            mainform.mapo.delSub(areaselection);
            selectionReset();
            GPSTrackFactory.singleton.onCurrentTrackChanged -= singleton_onCurrentTrackChanged;
            if (manual_track != null)
                manual_track.trackColor = origColor;
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
            Point pxy;

            switch (dragEnabled)
            {
                case DragStage.NoDrag:
                    if(controlPressed || altPressed)
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
                        dragEnabled = DragStage.Ignore;
                        break;
                    }
                    mainform.mapo.getVisibleXYByLonLat(ctx.resultPoint.Value.lon, ctx.resultPoint.Value.lat, out pxy);
                    if (Math.Abs(lastxy.X - pxy.X) < 20 && Math.Abs(lastxy.Y - pxy.Y) <= drag_start_delta * 2)
                    {
                        dragEnabled = DragStage.Dragging;
                        dragPoint = ctx.resultPoint;
                        manual_track.updatePointPosition(dragPoint, lon, lat);
                        original_ptype = dragPoint.Value.ptype;
                        dragPoint.Value.ptype = ncGeo.NMEA_LL.PointType.MARKWP;
                        GML.repaint();
                    }
                    else
                        startSelection(xy);                        
                    break;
                case DragStage.Dragging:
                    lastxy = GML.translateAbsToScene(lastxy);
                    mainform.mapo.getLonLatByVisibleXY(lastxy, out lon, out lat);
                    manual_track.updatePointPosition(dragPoint, lon, lat);
                    GML.repaint();
                    break;
                case DragStage.NewAreaSelection:
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
                case DragStage.Dragging:
                    dragPoint.Value.ptype = original_ptype;
                    manual_track.calculateParameters();
                    manual_track.updateVisual();
                    GML.repaint();
                    return true;
                case DragStage.NewAreaSelection:
                    finishAreaSelection(xy);
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

    }
}
