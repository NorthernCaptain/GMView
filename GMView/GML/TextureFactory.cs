using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class TextureFactory: IDisposable
    {
        public enum TexAlias 
        { 
            SatOK, 
            SatVI, 
            SatInfo, 
            GPSPanel, 
            GPSPoint, 
            ArrowSmall, 
            StartPos, 
            Arrows,
            ArrowsLarge,
            TilesBack,
            SemiTile64,
            SemiTile128,
            SemiTile64Empty,
            SemiTile128Empty,
            Rose,
            DashMiniTilesBg,
            DashMiniTilesWrapped,
            Zoom00,
            Zoom01,
            Zoom02,
            Zoom03,
            Zoom04,
            Zoom05,
            Zoom06,
            Zoom07,
            Zoom08,
            Zoom09,
            Zoom10,
            Zoom11,
            Zoom12,
            Zoom13,
            Zoom14,
            Zoom15,
            Zoom16,
            Zoom17,
            Zoom18,
            StayPos,
            PinYellow,
            PinGreen,
            PinRed,
            PinBlue,
            WayDot,
            WayDotSmall,
            DashTrackBg,
            DashTrackWrapped,
            DashTextBg,
            DashTextBg130,
            FinishPos,
            DashGPSBg,
            DashGPSOverlay,
            DashGPSWrapped,
            DashSpeedBarMask,
            HDOP_0_Ideal,
            HDOP_1_Excelent,
            HDOP_2_Good,
            HDOP_3_Moderate,
            HDOP_4_Fair,
            CrossHair1,
            ArrowPoint,
            PointBG
        };

        private static TextureFactory instance = new TextureFactory();
        private Dictionary<ImageDot, object> textures = new Dictionary<ImageDot, object>();
        private Dictionary<TexAlias, ImageDot> images = new Dictionary<TexAlias, ImageDot>();

        public delegate void OnInitedDelegate();
        public event OnInitedDelegate onInited;

        TextureFactory()
        {
            images.Add(TexAlias.SatOK, new ImageDot(global::GMView.Properties.Resources.satSmallInUse, 4, 4));
            images.Add(TexAlias.SatVI, new ImageDot(global::GMView.Properties.Resources.satSmallUnused, 4, 4));
            images.Add(TexAlias.SatInfo, new ImageDot(global::GMView.Properties.Resources.sat_info, 0, 0));
            images.Add(TexAlias.GPSPanel, new ImageDot(global::GMView.Properties.Resources.gps_panel, 0, 0));
            images.Add(TexAlias.GPSPoint, new ImageDot(global::GMView.Properties.Resources.gps_point, 14, 46));
            images.Add(TexAlias.ArrowSmall, new ImageDot(global::GMView.Properties.Resources.arrow_small, 7, 22));
            images.Add(TexAlias.StartPos, new ImageDot(global::GMView.Properties.Resources.startmark, 9, 33));
            images.Add(TexAlias.FinishPos, new ImageDot(global::GMView.Properties.Resources.finishmark, 9, 33));
            images.Add(TexAlias.Arrows, new ImageDot(global::GMView.Properties.Resources.arrows, 12, 83));
            images.Add(TexAlias.ArrowsLarge, new ImageDot(global::GMView.Properties.Resources.arrow_large, 11, 34));
            images.Add(TexAlias.TilesBack, new ImageDot(global::GMView.Properties.Resources.sat_info, 153, 145));
            images.Add(TexAlias.SemiTile64, new ImageDot(global::GMView.Properties.Resources.semitrans64, 0, 0));
            images.Add(TexAlias.SemiTile128, new ImageDot(global::GMView.Properties.Resources.semitrans128, 0, 0));
            images.Add(TexAlias.SemiTile64Empty, new ImageDot(global::GMView.Properties.Resources.semitrans64empty, 0, 0));
            images.Add(TexAlias.SemiTile128Empty, new ImageDot(global::GMView.Properties.Resources.semitrans128empty, 0, 0));
            images.Add(TexAlias.Rose, new ImageDot(global::GMView.Properties.Resources.rosa, 135, 135));
            images.Add(TexAlias.DashMiniTilesBg, new ImageDot(global::GMView.Properties.Resources.dashMiniTilesBg, 273, 268));
            images.Add(TexAlias.DashMiniTilesWrapped, new ImageDot(global::GMView.Properties.Resources.dashMiniTilesWrapped, 273, 20));
            images.Add(TexAlias.Zoom00, new ImageDot(global::GMView.Properties.Resources.zoom00, 8, 34));
            images.Add(TexAlias.Zoom01, new ImageDot(global::GMView.Properties.Resources.zoom01, 35, 154));
            images.Add(TexAlias.Zoom02, new ImageDot(global::GMView.Properties.Resources.zoom02, 35, 154));
            images.Add(TexAlias.Zoom03, new ImageDot(global::GMView.Properties.Resources.zoom03, 35, 154));
            images.Add(TexAlias.Zoom04, new ImageDot(global::GMView.Properties.Resources.zoom04, 35, 154));
            images.Add(TexAlias.Zoom05, new ImageDot(global::GMView.Properties.Resources.zoom05, 35, 154));
            images.Add(TexAlias.Zoom06, new ImageDot(global::GMView.Properties.Resources.zoom06, 35, 154));
            images.Add(TexAlias.Zoom07, new ImageDot(global::GMView.Properties.Resources.zoom07, 35, 154));
            images.Add(TexAlias.Zoom08, new ImageDot(global::GMView.Properties.Resources.zoom08, 35, 154));
            images.Add(TexAlias.Zoom09, new ImageDot(global::GMView.Properties.Resources.zoom09, 35, 154));
            images.Add(TexAlias.Zoom10, new ImageDot(global::GMView.Properties.Resources.zoom10, 35, 154));
            images.Add(TexAlias.Zoom11, new ImageDot(global::GMView.Properties.Resources.zoom11, 35, 154));
            images.Add(TexAlias.Zoom12, new ImageDot(global::GMView.Properties.Resources.zoom12, 35, 154));
            images.Add(TexAlias.Zoom13, new ImageDot(global::GMView.Properties.Resources.zoom13, 35, 154));
            images.Add(TexAlias.Zoom14, new ImageDot(global::GMView.Properties.Resources.zoom14, 35, 154));
            images.Add(TexAlias.Zoom15, new ImageDot(global::GMView.Properties.Resources.zoom15, 35, 154));
            images.Add(TexAlias.Zoom16, new ImageDot(global::GMView.Properties.Resources.zoom16, 35, 154));
            images.Add(TexAlias.Zoom17, new ImageDot(global::GMView.Properties.Resources.zoom17, 35, 154));
            images.Add(TexAlias.Zoom18, new ImageDot(global::GMView.Properties.Resources.zoom18, 35, 154));
            images.Add(TexAlias.StayPos, new ImageDot(global::GMView.Properties.Resources.stay, 6, 33));
            images.Add(TexAlias.PinYellow, new ImageDot(global::GMView.Properties.Resources.markwp_yellow, 11, 24));
            images.Add(TexAlias.PinGreen, new ImageDot(global::GMView.Properties.Resources.markwp_green, 11, 24));
            images.Add(TexAlias.PinRed, new ImageDot(global::GMView.Properties.Resources.markwp_red, 11, 24));
            images.Add(TexAlias.PinBlue, new ImageDot(global::GMView.Properties.Resources.markwp_blue, 11, 24));
            images.Add(TexAlias.WayDot, new ImageDot(global::GMView.Properties.Resources.dot1, 8, 8));
            images.Add(TexAlias.WayDotSmall, new ImageDot(global::GMView.Properties.Resources.dot_small, 8, 7));
            images.Add(TexAlias.DashTrackBg, new ImageDot(global::GMView.Properties.Resources.dashTrackBg, 273, 320));
            images.Add(TexAlias.DashTrackWrapped, new ImageDot(global::GMView.Properties.Resources.dashTrackWrapped, 273, 20));
            images.Add(TexAlias.DashTextBg, new ImageDot(global::GMView.Properties.Resources.dashTextBg, 90, 16));
            images.Add(TexAlias.DashTextBg130, new ImageDot(global::GMView.Properties.Resources.dashTextBg130, 130, 16));
            images.Add(TexAlias.DashGPSBg, new ImageDot(global::GMView.Properties.Resources.dashGPSBg, 273, 370));
            images.Add(TexAlias.DashGPSOverlay, new ImageDot(global::GMView.Properties.Resources.dashGPSOverlay, 273, 370));
            images.Add(TexAlias.DashGPSWrapped, new ImageDot(global::GMView.Properties.Resources.dashGPSWrapped, 273, 20));
            images.Add(TexAlias.DashSpeedBarMask, new ImageDot(global::GMView.Properties.Resources.speedBarMask, 280, 128));
            images.Add(TexAlias.HDOP_0_Ideal, new ImageDot(global::GMView.Properties.Resources.hdop_0_ideal, 80, 300));
            images.Add(TexAlias.HDOP_1_Excelent, new ImageDot(global::GMView.Properties.Resources.hdop_1_excellent, 30, 304));
            images.Add(TexAlias.HDOP_2_Good, new ImageDot(global::GMView.Properties.Resources.hdop_2_good, 30, 304));
            images.Add(TexAlias.HDOP_3_Moderate, new ImageDot(global::GMView.Properties.Resources.hdop_3_moderate, 30, 304));
            images.Add(TexAlias.HDOP_4_Fair, new ImageDot(global::GMView.Properties.Resources.hdop_4_fair, 30, 304));
            images.Add(TexAlias.CrossHair1, new ImageDot(global::GMView.Properties.Resources.crosshair1, 12, 12));
            images.Add(TexAlias.ArrowPoint, new ImageDot(global::GMView.Properties.Resources.arrow_point, 15, 48));
            images.Add(TexAlias.PointBG, new ImageDot(global::GMView.Properties.Resources.pointBG, 223, 148));
        }

        public void initGLData()
        {
            Program.Log("Loading textures...");
            foreach (KeyValuePair<TexAlias, ImageDot> pair in images)
            {
                ImageDot id = pair.Value;
                object tex = GML.device.texFromBitmap(ref id.img);
                textures.Add(pair.Value, tex);
            }
            if (onInited != null)
                onInited();
        }

        public ImageDot getImg(TexAlias tas)
        {
            return images[tas];
        }

        public object getTex(TexAlias tas)
        {
            return textures[images[tas]];
        }

        public object getTex(ImageDot imd)
        {
            return textures[imd];
        }

        public static TextureFactory singleton
        {
            get { return instance; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (KeyValuePair<ImageDot, object> pair in textures)
                GML.device.texDispose(pair.Value);
            textures.Clear();
        }

        #endregion
    }
}
