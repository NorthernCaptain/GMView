using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class POILoadControl : UserControl
    {
        public POILoadControl()
        {
            InitializeComponent();
            poiTypeComboBox.loadList(false);
        }
    }
}
