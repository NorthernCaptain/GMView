namespace GMView.Forms
{
    partial class EditBooks
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.okBut = new System.Windows.Forms.Button();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.treeColumn1 = new Aga.Controls.Tree.TreeColumn();
            this.lonCol = new Aga.Controls.Tree.TreeColumn();
            this.latCol = new Aga.Controls.Tree.TreeColumn();
            this.descrText = new Aga.Controls.Tree.TreeColumn();
            this.typeCol = new Aga.Controls.Tree.TreeColumn();
            this.idCol = new Aga.Controls.Tree.TreeColumn();
            this.commentsCol = new Aga.Controls.Tree.TreeColumn();
            this.createdCol = new Aga.Controls.Tree.TreeColumn();
            this.nodeStateIcon1 = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.nodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeDescr = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeIntegerTextBox_Id = new Aga.Controls.Tree.NodeControls.NodeIntegerTextBox();
            this.nodeCombo_Type = new Aga.Controls.Tree.NodeControls.NodeComboBox();
            this.nodeTextBox_Created = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox_Comment = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox_Lon = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox_Lat = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.okBut, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.treeView, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(686, 271);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Location = new System.Drawing.Point(604, 244);
            this.okBut.Margin = new System.Windows.Forms.Padding(10, 3, 7, 3);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(75, 23);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            // 
            // treeView
            // 
            this.treeView.AllowColumnReorder = true;
            this.treeView.AllowDrop = true;
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.AutoRowHeight = true;
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Columns.Add(this.treeColumn1);
            this.treeView.Columns.Add(this.lonCol);
            this.treeView.Columns.Add(this.latCol);
            this.treeView.Columns.Add(this.descrText);
            this.treeView.Columns.Add(this.typeCol);
            this.treeView.Columns.Add(this.idCol);
            this.treeView.Columns.Add(this.commentsCol);
            this.treeView.Columns.Add(this.createdCol);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.DisplayDraggingNodes = true;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Maroon;
            this.treeView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.FullRowSelect = true;
            this.treeView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeStateIcon1);
            this.treeView.NodeControls.Add(this.nodeName);
            this.treeView.NodeControls.Add(this.nodeDescr);
            this.treeView.NodeControls.Add(this.nodeIntegerTextBox_Id);
            this.treeView.NodeControls.Add(this.nodeCombo_Type);
            this.treeView.NodeControls.Add(this.nodeTextBox_Created);
            this.treeView.NodeControls.Add(this.nodeTextBox_Comment);
            this.treeView.NodeControls.Add(this.nodeTextBox_Lon);
            this.treeView.NodeControls.Add(this.nodeTextBox_Lat);
            this.treeView.RowHeight = 19;
            this.treeView.SelectedNode = null;
            this.treeView.Size = new System.Drawing.Size(680, 235);
            this.treeView.TabIndex = 2;
            this.treeView.Text = "POI tree";
            this.treeView.UseColumns = true;
            this.treeView.ColumnClicked += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this.treeView_ColumnClicked);
            // 
            // treeColumn1
            // 
            this.treeColumn1.Header = "Name";
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeColumn1.TooltipText = "POI name";
            this.treeColumn1.Width = 200;
            // 
            // lonCol
            // 
            this.lonCol.Header = "Longitude";
            this.lonCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lonCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lonCol.TooltipText = null;
            this.lonCol.Width = 70;
            // 
            // latCol
            // 
            this.latCol.Header = "Latitude";
            this.latCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.latCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.latCol.TooltipText = null;
            this.latCol.Width = 70;
            // 
            // descrText
            // 
            this.descrText.Header = "Description";
            this.descrText.SortOrder = System.Windows.Forms.SortOrder.None;
            this.descrText.TooltipText = "POI description";
            this.descrText.Width = 350;
            // 
            // typeCol
            // 
            this.typeCol.Header = "Type";
            this.typeCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.typeCol.TooltipText = "Type of POI";
            this.typeCol.Width = 60;
            // 
            // idCol
            // 
            this.idCol.Header = "Id";
            this.idCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.idCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.idCol.TooltipText = "Record Id";
            // 
            // commentsCol
            // 
            this.commentsCol.Header = "Comments";
            this.commentsCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.commentsCol.TooltipText = null;
            this.commentsCol.Width = 300;
            // 
            // createdCol
            // 
            this.createdCol.Header = "Date";
            this.createdCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.createdCol.TooltipText = "Date created";
            this.createdCol.Width = 110;
            // 
            // nodeStateIcon1
            // 
            this.nodeStateIcon1.DataPropertyName = "IconImage";
            this.nodeStateIcon1.LeftMargin = 1;
            this.nodeStateIcon1.ParentColumn = this.treeColumn1;
            this.nodeStateIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // nodeName
            // 
            this.nodeName.DataPropertyName = "Name";
            this.nodeName.EditEnabled = true;
            this.nodeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nodeName.IncrementalSearchEnabled = true;
            this.nodeName.LeftMargin = 3;
            this.nodeName.ParentColumn = this.treeColumn1;
            this.nodeName.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeName.UseCompatibleTextRendering = true;
            // 
            // nodeDescr
            // 
            this.nodeDescr.DataPropertyName = "Description";
            this.nodeDescr.EditEnabled = true;
            this.nodeDescr.EditOnClick = true;
            this.nodeDescr.IncrementalSearchEnabled = true;
            this.nodeDescr.LeftMargin = 3;
            this.nodeDescr.ParentColumn = this.descrText;
            this.nodeDescr.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeDescr.UseCompatibleTextRendering = true;
            // 
            // nodeIntegerTextBox_Id
            // 
            this.nodeIntegerTextBox_Id.DataPropertyName = "Id";
            this.nodeIntegerTextBox_Id.IncrementalSearchEnabled = true;
            this.nodeIntegerTextBox_Id.LeftMargin = 3;
            this.nodeIntegerTextBox_Id.ParentColumn = this.idCol;
            this.nodeIntegerTextBox_Id.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nodeIntegerTextBox_Id.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeIntegerTextBox_Id.UseCompatibleTextRendering = true;
            // 
            // nodeCombo_Type
            // 
            this.nodeCombo_Type.DataPropertyName = "Ptype";
            this.nodeCombo_Type.EditEnabled = true;
            this.nodeCombo_Type.EditorHeight = 150;
            this.nodeCombo_Type.EditorWidth = 200;
            this.nodeCombo_Type.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nodeCombo_Type.IncrementalSearchEnabled = true;
            this.nodeCombo_Type.LeftMargin = 3;
            this.nodeCombo_Type.ParentColumn = this.typeCol;
            this.nodeCombo_Type.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeCombo_Type.UseCompatibleTextRendering = true;
            // 
            // nodeTextBox_Created
            // 
            this.nodeTextBox_Created.DataPropertyName = "CreatedS";
            this.nodeTextBox_Created.IncrementalSearchEnabled = true;
            this.nodeTextBox_Created.LeftMargin = 3;
            this.nodeTextBox_Created.ParentColumn = this.createdCol;
            this.nodeTextBox_Created.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBox_Created.UseCompatibleTextRendering = true;
            // 
            // nodeTextBox_Comment
            // 
            this.nodeTextBox_Comment.DataPropertyName = "Comment";
            this.nodeTextBox_Comment.EditEnabled = true;
            this.nodeTextBox_Comment.EditOnClick = true;
            this.nodeTextBox_Comment.IncrementalSearchEnabled = true;
            this.nodeTextBox_Comment.LeftMargin = 3;
            this.nodeTextBox_Comment.ParentColumn = this.commentsCol;
            this.nodeTextBox_Comment.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBox_Comment.UseCompatibleTextRendering = true;
            // 
            // nodeTextBox_Lon
            // 
            this.nodeTextBox_Lon.DataPropertyName = "LongitudeS";
            this.nodeTextBox_Lon.EditEnabled = true;
            this.nodeTextBox_Lon.IncrementalSearchEnabled = true;
            this.nodeTextBox_Lon.LeftMargin = 3;
            this.nodeTextBox_Lon.ParentColumn = this.lonCol;
            this.nodeTextBox_Lon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nodeTextBox_Lon.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBox_Lon.UseCompatibleTextRendering = true;
            // 
            // nodeTextBox_Lat
            // 
            this.nodeTextBox_Lat.DataPropertyName = "LatitudeS";
            this.nodeTextBox_Lat.EditEnabled = true;
            this.nodeTextBox_Lat.IncrementalSearchEnabled = true;
            this.nodeTextBox_Lat.LeftMargin = 3;
            this.nodeTextBox_Lat.ParentColumn = this.latCol;
            this.nodeTextBox_Lat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nodeTextBox_Lat.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBox_Lat.UseCompatibleTextRendering = true;
            // 
            // EditBooks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 271);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditBooks";
            this.Text = "Places of interest";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button okBut;
        private Aga.Controls.Tree.TreeViewAdv treeView;
        private Aga.Controls.Tree.TreeColumn treeColumn1;
        private Aga.Controls.Tree.TreeColumn descrText;
        private Aga.Controls.Tree.TreeColumn lonCol;
        private Aga.Controls.Tree.TreeColumn latCol;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeName;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeDescr;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nodeStateIcon1;
        private Aga.Controls.Tree.TreeColumn idCol;
        private Aga.Controls.Tree.NodeControls.NodeIntegerTextBox nodeIntegerTextBox_Id;
        private Aga.Controls.Tree.TreeColumn typeCol;
        private Aga.Controls.Tree.TreeColumn commentsCol;
        private Aga.Controls.Tree.TreeColumn createdCol;
        private Aga.Controls.Tree.NodeControls.NodeComboBox nodeCombo_Type;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox_Created;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox_Comment;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox_Lon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox_Lat;
    }
}