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
            this.toolBox = new Silver.UI.ToolBox();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.dirCB = new System.Windows.Forms.ComboBox();
            this.fileNameCol = new Aga.Controls.Tree.TreeColumn();
            this.sizeCol = new Aga.Controls.Tree.TreeColumn();
            this.dateCol = new Aga.Controls.Tree.TreeColumn();
            this.nodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxSize = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxDate = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.toolBox.LargeItemSize = new System.Drawing.Size(64, 64);
            this.toolBox.LayoutDelay = 10;
            this.toolBox.Location = new System.Drawing.Point(3, 3);
            this.toolBox.Name = "toolBox";
            this.toolBox.ScrollDelay = 60;
            this.toolBox.SelectAllTextWhileRenaming = true;
            this.toolBox.SelectedTabIndex = -1;
            this.toolBox.ShowOnlyOneItemPerRow = false;
            this.toolBox.Size = new System.Drawing.Size(145, 472);
            this.toolBox.SmallItemSize = new System.Drawing.Size(32, 32);
            this.toolBox.TabHeight = 18;
            this.toolBox.TabHoverTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.TabIndex = 0;
            this.toolBox.TabNormalTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.TabSelectedTextColor = System.Drawing.SystemColors.ControlText;
            this.toolBox.TabSpacing = 1;
            this.toolBox.UseItemColorInRename = false;
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Columns.Add(this.fileNameCol);
            this.treeView.Columns.Add(this.sizeCol);
            this.treeView.Columns.Add(this.dateCol);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(154, 33);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeStateIcon);
            this.treeView.NodeControls.Add(this.nodeTextBoxName);
            this.treeView.NodeControls.Add(this.nodeTextBoxSize);
            this.treeView.NodeControls.Add(this.nodeTextBoxDate);
            this.treeView.SelectedNode = null;
            this.treeView.Size = new System.Drawing.Size(587, 382);
            this.treeView.TabIndex = 1;
            this.treeView.Text = "treeViewAdv1";
            this.treeView.UseColumns = true;
            // 
            // dirCB
            // 
            this.dirCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dirCB.FormattingEnabled = true;
            this.dirCB.Location = new System.Drawing.Point(187, 3);
            this.dirCB.Name = "dirCB";
            this.dirCB.Size = new System.Drawing.Size(373, 24);
            this.dirCB.TabIndex = 2;
            // 
            // fileNameCol
            // 
            this.fileNameCol.Header = "Name";
            this.fileNameCol.MinColumnWidth = 100;
            this.fileNameCol.Sortable = true;
            this.fileNameCol.SortOrder = System.Windows.Forms.SortOrder.None;
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
            this.nodeTextBoxName.DataPropertyName = "Name";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Dir:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = global::ncFileControls.Properties.Resources.Folder;
            this.button1.Location = new System.Drawing.Point(566, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 23);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(204, 451);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(537, 24);
            this.comboBox1.TabIndex = 5;
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(204, 421);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(537, 24);
            this.comboBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 424);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "File:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 454);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type:";
            // 
            // FileChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dirCB);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolBox);
            this.Name = "FileChooser";
            this.Size = new System.Drawing.Size(744, 478);
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
