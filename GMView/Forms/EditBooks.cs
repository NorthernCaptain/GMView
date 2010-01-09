using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class EditBooks : Form
    {
        public EditBooks()
        {
            InitializeComponent();
            group.Items.Clear();
            group.Items.AddRange(BookMarkFactory.singleton.getGroupNames().ToArray());
            fillGrid();
        }

        private void fillGrid()
        {
            Bookmark[] bmarks = BookMarkFactory.singleton.bmarks;
            foreach (Bookmark book in bmarks)
            {
                dataGV.Rows.Add(new object[] {ncUtils.Glob.latString(book.lat), ncUtils.Glob.lonString(book.lon),
                    book.Name, book.Comment, book.group, book.Name, null});
            }
        }
    }
}
