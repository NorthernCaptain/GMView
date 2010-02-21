using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class AskNameDlg : Form
    {
        public AskNameDlg()
        {
            InitializeComponent();
        }

        public AskNameDlg(string caption, string label)
        {
            InitializeComponent();
            this.Text = caption;
            nameLbl.Text = label;
        }

        /// <summary>
        /// Gets or sets the text in the window dialog
        /// </summary>
        public string SelectedText
        {
            get { return nameTB.Text; }
            set
            {
            	nameTB.Text = value;
            }
        }
    }
}
