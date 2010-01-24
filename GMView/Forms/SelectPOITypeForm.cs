using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class SelectPOITypeForm : Form
    {
        public SelectPOITypeForm()
        {
            InitializeComponent();
            poiTypeListBox.loadList(false);
        }

        public Bookmarks.POIType result
        {
            get
            {
                return poiTypeListBox.SelectedItem as Bookmarks.POIType;
            }
            set
            {
                poiTypeListBox.SelectedItem = value;
            }
        }
    }
}
