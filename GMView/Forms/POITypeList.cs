using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Drawing;
using ncUtils;
using GMView.Bookmarks;

namespace GMView.Forms
{
    /// <summary>
    /// Class loads and shows listbox with poi types.
    /// </summary>
    public class POITypeList: ListBox
    {
        /// <summary>
        /// Constructor sets owner mode for drawing items
        /// </summary>
        public POITypeList()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = heightItem;
        }

        /// <summary>
        /// Loads types from DB and fills the list
        /// </summary>
        public void loadList()
        {
            DBObj dbo = null;

            try
            {
                dbo = new DBObj("select id, name, description, icon, icon_cx, icon_cy "
                                + "from poi_type where id > 0 order by sort_order desc, description");

                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
                    POIType pi = new POIType(reader);
                    this.Items.Add(pi);
                }
                if (this.Items.Count > 0)
                    this.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                if(dbo != null)
                    dbo.Dispose();
            }
        }

        protected int delta_x = 32;
        protected int heightItem = 40;
        protected int delta_y = -1;

        /// <summary>
        /// Draw list item with an icon
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (this.DesignMode)
            {
                base.OnDrawItem(e);
                return;
            }

            if (delta_y == -1)
            {
                delta_y = (int)(heightItem - e.Font.GetHeight(e.Graphics)) / 2;
            }

            POIType pi = Items[e.Index] as POIType;

            if(pi != null)
            {
                ImageDot dot = GMView.IconFactory.singleton.getIcon(pi);
                if (dot != null)
                    e.Graphics.DrawImage(dot.img, e.Bounds.Left + (delta_x - dot.real_len) / 2,
                        e.Bounds.Top + (heightItem - dot.real_hei) / 2, dot.real_len, dot.real_hei);
            }

            e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor),
                e.Bounds.Left + delta_x, e.Bounds.Top + delta_y);

            if(this.ItemHeight != heightItem)
                this.ItemHeight = heightItem;
        }
    }
}
