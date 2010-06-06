using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.UIHelper
{
    /// <summary>
    /// Class organizes processing of Navigation mouse mode
    /// </summary>
    public class NavigateMode: MouseBaseProc
    {
        internal PositionStack pos_stack = new PositionStack(50);
        internal PositionStack pos_stack_fwd = new PositionStack(50);
        internal UserPosition upos;
        internal UserPosition upos_mini;

        public NavigateMode(GMViewForm form, UserControl dPane)
        {
            mainform = form;
            drawPane = dPane;
            upos = Program.opt.newUserPosition(mainform.mapo, TextureFactory.singleton.getImg(TextureFactory.TexAlias.ArrowsLarge));
            upos_mini = Program.opt.newUserPosition(mainform.minimapo, TextureFactory.singleton.getImg(TextureFactory.TexAlias.ArrowSmall));
            mainform.mapo.addSub(upos);
            mainform.minimapo.addSub(upos_mini);
        }

        public override void initGL()
        {
            upos.initGLData();
            upos_mini.initGLData();
        }

        public override void modeEnter(MouseBaseProc oldone)
        {
            base.modeEnter(oldone);
            upos.show();
            upos_mini.show();
            GPSTrackFactory.singleton.currentTrack = mainform.gtrack;
        }

        public override void modeLeave()
        {
            base.modeLeave();
            upos.hide();
        }

        protected override bool onMouseMoveTranslated(Point delta_xy)
        {
            mainform.mapo.MoveMapByScreenPoint(delta_xy);
            GML.repaint();
            return true;
        }

        protected override bool onMouseMove(Point xy)
        {
            return false; //we do not need autoscrolling
        }

        protected override bool onMouseMoveNoBut(Point xy)
        {
            mainform.mouseOverTimer.Stop();
            mainform.mouseOverTimer.Start();
            return true;
        }

        protected override bool onMouseDoubleClickTranslated(Point xy)
        {
            pos_stack.push(new PositionStack.PositionInfo(upos.Lon, upos.Lat, mainform.mapo.zoom, Program.opt.mapType));
            upos.setVisXY(xy);
            mainform.centerMapLonLat(upos.Lon, upos.Lat);
            return true;
        }

        protected override bool onMouseUpTranslated(Point xy)
        {
            if(eargs.Button == MouseButtons.Right)
            {
                mainform.trackinformer.doAction(xy);
                return true;
            }
            return false;
        }

        protected override bool onMouseClickTranslated(Point xy)
        {
            double lon, lat;
            mainform.mapo.getLonLatByVisibleXY(xy, out lon, out lat);
            mainform.setOnClickLonLat(lon, lat);
            return true;
        }

        public void backHistoryPosition()
        {
            if (!pos_stack.empty())
            {
                PositionStack.PositionInfo pinfo = pos_stack.pop();
                pos_stack_fwd.push(new PositionStack.PositionInfo(upos.Lon, upos.Lat, mainform.mapo.zoom, Program.opt.mapType));
                mainform.centerMapLonLat(pinfo.lon, pinfo.lat);
            }
        }

        public void fwdHistoryPosition()
        {
            if (!pos_stack_fwd.empty())
            {
                pos_stack.push(new PositionStack.PositionInfo(upos.Lon, upos.Lat, mainform.mapo.zoom, Program.opt.mapType));
                PositionStack.PositionInfo pinfo = pos_stack_fwd.pop();
                mainform.centerMapLonLat(pinfo.lon, pinfo.lat);
            }
        }

        public void backPush(PositionStack.PositionInfo nfo)
        {
            pos_stack.push(nfo);
        }

        public override string name()
        {
            return "Navigate mode";
        }
    }
}
