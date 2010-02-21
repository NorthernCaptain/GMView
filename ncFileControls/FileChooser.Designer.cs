namespace ncFileControls
{
    partial class FileChooser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dirCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fileTypeCB = new System.Windows.Forms.ComboBox();
            this.fileCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bookDirBut = new System.Windows.Forms.Button();
            this.newDirBut = new System.Windows.Forms.Button();
            this.upDirBut = new System.Windows.Forms.Button();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.fileNameCol = new Aga.Controls.Tree.TreeColumn();
            this.sizeCol = new Aga.Controls.Tree.TreeColumn();
            this.dateCol = new Aga.Controls.Tree.TreeColumn();
            this.nodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxSize = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxDate = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolBox = new Silver.UI.ToolBox();
            this.SuspendLayout();
            // 
            // dirCB
            // 
            this.dirCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dirCB.DropDownHeight = 200;
            this.dirCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirCB.DropDownWidth = 500;
            this.dirCB.FormattingEnabled = true;
            this.dirCB.IntegralHeight = false;
            this.dirCB.Location = new System.Drawing.Point(135, 2);
            this.dirCB.Margin = new System.Windows.Forms.Padding(2);
            this.dirCB.Name = "dirCB";
            this.dirCB.Size = new System.Drawing.Size(346, 21);
            this.dirCB.TabIndex = 2;
            this.dirCB.SelectionChangeCommitted += new System.EventHandler(this.dirCB_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Dir:";
            // 
            // fileTypeCB
            // 
            this.fileTypeCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypeCB.FormattingEnabled = true;
            this.fileTypeCB.Location = new System.Drawing.Point(152, 366);
            this.fileTypeCB.Margin = new System.Windows.Forms.Padding(2);
            this.fileTypeCB.Name = "fileTypeCB";
            this.fileTypeCB.Size = new System.Drawing.Size(405, 21);
            this.fileTypeCB.TabIndex = 5;
            // 
            // fileCB
            // 
            this.fileCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileCB.FormattingEnabled = true;
            this.fileCB.Location = new System.Drawing.Point(152, 341);
            this.fileCB.Margin = new System.Windows.Forms.Padding(2);
            this.fileCB.Name = "fileCB";
            this.fileCB.Size = new System.Drawing.Size(405, 21);
            this.fileCB.TabIndex = 5;
            this.fileCB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fileCB_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 344);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "File:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(111, 369);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type:";
            // 
            // bookDirBut
            // 
            this.bookDirBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bookDirBut.FlatAppearance.BorderSize = 0;
            this.bookDirBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bookDirBut.Image = global::ncFileControls.Properties.Resources.bookAdd;
            this.bookDirBut.Location = new System.Drawing.Point(512, 2);
            this.bookDirBut.Margin = new System.Windows.Forms.Padding(2);
            this.bookDirBut.Name = "bookDirBut";
            this.bookDirBut.Size = new System.Drawing.Size(20, 20);
            this.bookDirBut.TabIndex = 3;
            this.bookDirBut.UseVisualStyleBackColor = true;
            // 
            // newDirBut
            // 
            this.newDirBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newDirBut.FlatAppearance.BorderSize = 0;
            this.newDirBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newDirBut.Image = global::ncFileControls.Properties.Resources.newdir;
            this.newDirBut.Location = new System.Drawing.Point(537, 2);
            this.newDirBut.Margin = new System.Windows.Forms.Padding(2);
            this.newDirBut.Name = "newDirBut";
            this.newDirBut.Size = new System.Drawing.Size(19, 20);
            this.newDirBut.TabIndex = 3;
            this.newDirBut.UseVisualStyleBackColor = true;
            // 
            // upDirBut
            // 
            this.upDirBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upDirBut.FlatAppearance.BorderSize = 0;
            this.upDirBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upDirBut.Image = global::ncFileControls.Properties.Resources.updir2;
            this.upDirBut.Location = new System.Drawing.Point(488, 2);
            this.upDirBut.Margin = new System.Windows.Forms.Padding(2);
            this.upDirBut.Name = "upDirBut";
            this.upDirBut.Size = new System.Drawing.Size(20, 20);
            this.upDirBut.TabIndex = 3;
            this.upDirBut.UseVisualStyleBackColor = true;
            this.upDirBut.Click += new System.EventHandler(this.upDirBut_Click);
            // 
            // treeView
            // 
            this.treeView.AllowColumnReorder = true;
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Columns.Add(this.fileNameCol);
            this.treeView.Columns.Add(this.sizeCol);
            this.treeView.Columns.Add(this.dateCol);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeView.Indent = 16;
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(111, 26);
            this.treeView.Margin = new System.Windows.Forms.Padding(2);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeStateIcon);
            this.treeView.NodeControls.Add(this.nodeTextBoxName);
            this.treeView.NodeControls.Add(this.nodeTextBoxSize);
            this.treeView.NodeControls.Add(this.nodeTextBoxDate);
            this.treeView.RowHeight = 20;
            this.treeView.SelectedNode = null;
            this.treeView.ShowLines = false;
            this.treeView.ShowPlusMinus = false;
            this.treeView.Size = new System.Drawing.Size(447, 311);
            this.treeView.TabIndex = 1;
            this.treeView.Text = "treeViewAdv1";
            this.treeView.UseColumns = true;
            this.treeView.SelectionChanged += new System.EventHandler(this.treeView_SelectionChanged);
            this.treeView.ColumnClicked += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this.treeView_ColumnClicked);
            this.treeView.ColumnWidthChanged += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this.treeView_ColumnWidthChanged);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            this.treeView.ColumnReordered += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this.treeView_ColumnReordered);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            // 
            // fileNameCol
            // 
            this.fileNameCol.Header = "Name";
            this.fileNameCol.MinColumnWidth = 100;
            this.fileNameCol.Sortable = true;
            this.fileNameCol.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.fileNameCol.TooltipText = null;
            this.fileNameCol.Width = 350;
            // 
            // sizeCol
            // 
            this.sizeCol.Header = "Size";
            this.sizeCol.Sortable = true;
            this.sizeCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.sizeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeCol.TooltipText = null;
            this.sizeCol.Width = 100;
            // 
            // dateCol
            // 
            this.dateCol.Header = "Modified";
            this.dateCol.Sortable = true;
            this.dateCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.dateCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dateCol.TooltipText = null;
            this.dateCol.Width = 140;
            // 
            // nodeStateIcon
            // 
            this.nodeStateIcon.DataPropertyName = "IconImg";
            this.nodeStateIcon.LeftMargin = 1;
            this.nodeStateIcon.ParentColumn = this.fileNameCol;
            this.nodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBoxName
            // 
            this.nodeTextBoxName.DataPropertyName = "DisplayName";
            this.nodeTextBoxName.EditEnabled = true;
            this.nodeTextBoxName.IncrementalSearchEnabled = true;
            this.nodeTextBoxName.LeftMargin = 3;
            this.nodeTextBoxName.ParentColumn = this.fileNameCol;
            this.nodeTextBoxName.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBoxName.UseCompatibleTextRendering = true;
            // 
            // nodeTextBoxSize
            // 
            this.nodeTextBoxSize.DataPropertyName = "SizeS";
            this.nodeTextBoxSize.IncrementalSearchEnabled = true;
            this.nodeTextBoxSize.LeftMargin = 3;
            this.nodeTextBoxSize.ParentColumn = this.sizeCol;
            this.nodeTextBoxSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nodeTextBoxSize.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBoxSize.UseCompatibleTextRendering = true;
            // 
            // nodeTextBoxDate
            // 
            this.nodeTextBoxDate.DataPropertyName = "DateS";
            this.nodeTextBoxDate.IncrementalSearchEnabled = true;
            this.nodeTextBoxDate.LeftMargin = 3;
            this.nodeTextBoxDate.ParentColumn = this.dateCol;
            this.nodeTextBoxDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nodeTextBoxDate.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBoxDate.UseCompatibleTextRendering = true;
            // 
            // toolBox
            // 
            this.toolBox.AllowDrop = true;
            this.toolBox.AllowSwappingByDragDrop = true;
            this.toolBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.toolBox.InitialScrollDelay = 500;
            this.toolBox.ItemBackgroundColor = System.Drawing.Color.Empty;
            this.toolBox.ItemBorderColor = System.Drawing.Color.Empty;
            this.toolBox.ItemHeight = 20;
            this.toolBox.ItemHoverColor = System.Drawing.SystemColors.Control;
            this.toolBox.ItemHoverTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.ItemNormalColor = System.Drawing.SystemColors.Control;
            this.toolBox.ItemNormalTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.ItemSelectedColor = System.Drawing.Color.White;
            this.toolBox.ItemSelectedTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.ItemSpacing = 2;
            this.toolBox.LargeItemSize = new System.Drawing.Size(100, 96);
            this.toolBox.LayoutDelay = 10;
            this.toolBox.Location = new System.Drawing.Point(2, 2);
            this.toolBox.Margin = new System.Windows.Forms.Padding(2);
            this.toolBox.Name = "toolBox";
            this.toolBox.ScrollDelay = 60;
            this.toolBox.SelectAllTextWhileRenaming = true;
            this.toolBox.SelectedTabIndex = -1;
            this.toolBox.ShowOnlyOneItemPerRow = false;
            this.toolBox.Size = new System.Drawing.Size(105, 384);
            this.toolBox.SmallItemSize = new System.Drawing.Size(32, 32);
            this.toolBox.TabHeight = 18;
            this.toolBox.TabHoverTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.TabIndex = 0;
            this.toolBox.TabNormalTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.TabSelectedTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.TabSpacing = 1;
            this.toolBox.UseItemColorInRename = false;
            this.toolBox.ItemSelectionChanged += new Silver.UI.ItemSelectionChangedHandler(this.toolBox_ItemSelectionChanged);
            // 
            // FileChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fileCB);
            this.Controls.Add(this.fileTypeCB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.upDirBut);
            this.Controls.Add(this.newDirBut);
            this.Controls.Add(this.bookDirBut);
            this.Controls.Add(this.dirCB);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FileChooser";
            this.Size = new System.Drawing.Size(558, 388);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Silver.UI.ToolBox toolBox;
        private Aga.Controls.Tree.TreeViewAdv treeView;
        private System.Windows.Forms.ComboBox dirCB;
        private Aga.Controls.Tree.TreeColumn fileNameCol;
        private Aga.Controls.Tree.TreeColumn sizeCol;
        private Aga.Controls.Tree.TreeColumn dateCol;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nodeStateIcon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxName;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxSize;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxDate;
        private System.Windows.Forms.Button bookDirBut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox fileTypeCB;
        private System.Windows.Forms.ComboBox fileCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button newDirBut;
        private System.Windows.Forms.Button upDirBut;
    }
}
