using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    class MenuItemObject: ToolStripMenuItem
    {
        private Object obj_ref;

        public MenuItemObject(Object o)
            : base(o.ToString())
        {
            obj_ref = o;
        }

        public Object data_object
        {
            get { return obj_ref; }
            set { obj_ref = value; base.Text = obj_ref.ToString(); }
        }
    }
}
