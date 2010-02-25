using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GMView.Bookmarks;
using System.Drawing;

namespace GMView.Forms
{
    /// <summary>
    /// ComboBox that loads and display a list of POI types with images
    /// </summary>
    public class POITypeComboBox: ComboBox
    {
        /// <summary>
        /// Constructor sets owner mode for drawing items
        /// </summary>
        public POITypeComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = heightItem;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Loads types from DB and fills the list
        /// </summary>
        public void loadList(bool quickType)
        {
            List<POIType> result = new List<POIType>();

            foreach (POIType ptype in POITypeFactory.singleton().items)
            {
                if (quickType)
                {
                    if (ptype.IsQuickType)
                        result.Add(ptype);
                }
                else
                    result.Add(ptype);
            }

            this.Items.AddRange(result.ToArray());
            if (this.Items.Count > 0)
                this.SelectedIndex = 0;
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
            if (this.DesignMode)
            {
                base.OnDrawItem(e);
                return;
            }

            int idx = e.Index;

            if (idx >= Items.Count)
                return;

            if (idx < 0)
                idx = this.SelectedIndex < 0 ? 0 : this.SelectedIndex;

            if (delta_y == -1)
            {
                delta_y = (int)(heightItem - e.Font.GetHeight(e.Graphics)) / 2;
            }

            try
            {
                POIType pi = Items[idx] as POIType;

                if (pi != null)
                {
                    ImageDot dot = GMView.IconFactory.singleton.getIcon(pi);
                    if (dot != null)
                        e.Graphics.DrawImage(dot.img, e.Bounds.Left + (delta_x - dot.real_len) / 2,
                            e.Bounds.Top + (heightItem - dot.real_hei) / 2, dot.real_len, dot.real_hei);
                }

                e.Graphics.DrawString(Items[idx].ToString(), e.Font, new SolidBrush(e.ForeColor),
                    e.Bounds.Left + delta_x, e.Bounds.Top + delta_y);

                if (this.ItemHeight != heightItem)
                    this.ItemHeight = heightItem;
            }
            catch (System.Exception)
            {

            }
            e.DrawFocusRectangle();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // POITypeComboBox
            // 
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResumeLayout(false);

        }
    }
}
