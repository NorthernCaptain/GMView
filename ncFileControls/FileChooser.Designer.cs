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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.dirCB.Location = new System.Drawing.Point(173, 2);
            this.dirCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dirCB.Name = "dirCB";
            this.dirCB.Size = new System.Drawing.Size(573, 24);
            this.dirCB.TabIndex = 2;
            this.dirCB.SelectionChangeCommitted += new System.EventHandler(this.dirCB_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "Dir:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileTypeCB
            // 
            this.fileTypeCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.fileTypeCB, 4);
            this.fileTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileTypeCB.FormattingEnabled = true;
            this.fileTypeCB.Location = new System.Drawing.Point(173, 561);
            this.fileTypeCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fileTypeCB.Name = "fileTypeCB";
            this.fileTypeCB.Size = new System.Drawing.Size(657, 24);
            this.fileTypeCB.TabIndex = 5;
            this.fileTypeCB.SelectedIndexChanged += new System.EventHandler(this.fileTypeCB_SelectedIndexChanged);
            // 
            // fileCB
            // 
            this.fileCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.fileCB, 4);
            this.fileCB.FormattingEnabled = true;
            this.fileCB.Location = new System.Drawing.Point(173, 535);
            this.fileCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fileCB.Name = "fileCB";
            this.fileCB.Size = new System.Drawing.Size(657, 24);
            this.fileCB.TabIndex = 5;
            this.fileCB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fileCB_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 533);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label2.Size = new System.Drawing.Size(34, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "File:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 559);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label3.Size = new System.Drawing.Size(44, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bookDirBut
            // 
            this.bookDirBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bookDirBut.FlatAppearance.BorderSize = 0;
            this.bookDirBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bookDirBut.Image = global::ncFileControls.Properties.Resources.bookAdd;
            this.bookDirBut.Location = new System.Drawing.Point(780, 2);
            this.bookDirBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bookDirBut.Name = "bookDirBut";
            this.bookDirBut.Size = new System.Drawing.Size(22, 22);
            this.bookDirBut.TabIndex = 3;
            this.bookDirBut.UseVisualStyleBackColor = true;
            this.bookDirBut.Click += new System.EventHandler(this.bookDirBut_Click);
            // 
            // newDirBut
            // 
            this.newDirBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newDirBut.FlatAppearance.BorderSize = 0;
            this.newDirBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newDirBut.Image = global::ncFileControls.Properties.Resources.newdir;
            this.newDirBut.Location = new System.Drawing.Point(808, 2);
            this.newDirBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.newDirBut.Name = "newDirBut";
            this.newDirBut.Size = new System.Drawing.Size(22, 22);
            this.newDirBut.TabIndex = 3;
            this.newDirBut.UseVisualStyleBackColor = true;
            this.newDirBut.Click += new System.EventHandler(this.newDirBut_Click);
            // 
            // upDirBut
            // 
            this.upDirBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upDirBut.FlatAppearance.BorderSize = 0;
            this.upDirBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upDirBut.Image = global::ncFileControls.Properties.Resources.updir2;
            this.upDirBut.Location = new System.Drawing.Point(752, 2);
            this.upDirBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.upDirBut.Name = "upDirBut";
            this.upDirBut.Size = new System.Drawing.Size(22, 22);
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
            this.tableLayoutPanel1.SetColumnSpan(this.treeView, 5);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeView.Indent = 16;
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(123, 28);
            this.treeView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.treeView.Size = new System.Drawing.Size(707, 503);
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
            this.toolBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.toolBox.Location = new System.Drawing.Point(3, 2);
            this.toolBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolBox.Name = "toolBox";
            this.tableLayoutPanel1.SetRowSpan(this.toolBox, 4);
            this.toolBox.ScrollDelay = 60;
            this.toolBox.SelectAllTextWhileRenaming = true;
            this.toolBox.SelectedTabIndex = -1;
            this.toolBox.ShowOnlyOneItemPerRow = false;
            this.toolBox.Size = new System.Drawing.Size(114, 581);
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Controls.Add(this.toolBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fileTypeCB, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.fileCB, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.newDirBut, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.bookDirBut, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.upDirBut, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dirCB, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.treeView, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(833, 585);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // FileChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FileChooser";
            this.Size = new System.Drawing.Size(833, 585);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
