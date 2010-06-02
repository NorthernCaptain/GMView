using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GMView
{
    partial class GMViewForm
    {
        private UIHelper.ManualTrackMode manualMode;

        private void initManualTrackMode()
        {
            manualMode = new UIHelper.ManualTrackMode(this, drawPane);
            modes[(int)UserAction.ManualTrack] = manualMode;
            manualMode.manualTrack = null; //creates new manual track
        }

    }
}
