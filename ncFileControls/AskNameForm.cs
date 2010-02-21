using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ncFileControls
{
    public partial class AskNameForm : Form
    {
        public AskNameForm()
        {
            InitializeComponent();
        }

        public AskNameForm(string caption, string label)
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
            get { return nameTb.Text; }
            set
            {
            	nameTb.Text = value;
                nameTb.SelectAll();
            }
        }
    }
}
