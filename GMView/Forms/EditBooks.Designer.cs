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
            this.components = new System.ComponentModel.Container();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.treeColumn1 = new Aga.Controls.Tree.TreeColumn();
            this.lonCol = new Aga.Controls.Tree.TreeColumn();
            this.latCol = new Aga.Controls.Tree.TreeColumn();
            this.altitudeCol = new Aga.Controls.Tree.TreeColumn();
            this.descrText = new Aga.Controls.Tree.TreeColumn();
            this.typeCol = new Aga.Controls.Tree.TreeColumn();
            this.idCol = new Aga.Controls.Tree.TreeColumn();
            this.commentsCol = new Aga.Controls.Tree.TreeColumn();
            this.createdCol = new Aga.Controls.Tree.TreeColumn();
            this.contextMenuStripForTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePOIOrGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeCheckBox_Shown = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nodeStateIcon1 = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.nodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeDescr = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeIntegerTextBox_Id = new Aga.Controls.Tree.NodeControls.NodeIntegerTextBox();
            this.nodeCombo_Type = new Aga.Controls.Tree.NodeControls.NodeComboBox();
            this.nodeTextBox_Created = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox_Comment = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox_Lon = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox_Lat = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeNumericUpDown_Alt = new Aga.Controls.Tree.NodeControls.NodeNumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.okBut = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PTdelTypeBut = new System.Windows.Forms.Button();
            this.newTypeBut = new System.Windows.Forms.Button();
            this.cancelChangesBut = new System.Windows.Forms.Button();
            this.PTapplyBut = new System.Windows.Forms.Button();
            this.PTminZoomLvlNum = new System.Windows.Forms.NumericUpDown();
            this.PTquickAddCB = new System.Windows.Forms.CheckBox();
            this.PTautoShowCB = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PTiconCYNum = new System.Windows.Forms.NumericUpDown();
            this.PTiconCXNum = new System.Windows.Forms.NumericUpDown();
            this.PTiconPic = new System.Windows.Forms.PictureBox();
            this.ChangeIconBut = new System.Windows.Forms.Button();
            this.PTdescrTB = new System.Windows.Forms.TextBox();
            this.PTnameTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewPType = new Aga.Controls.Tree.TreeViewAdv();
            this.PTnameCol = new Aga.Controls.Tree.TreeColumn();
            this.PTdescrCol = new Aga.Controls.Tree.TreeColumn();
            this.PTquickAddCol = new Aga.Controls.Tree.TreeColumn();
            this.PTautoShowCol = new Aga.Controls.Tree.TreeColumn();
            this.PTminZoomCol = new Aga.Controls.Tree.TreeColumn();
            this.PTnodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.PTnodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.PTnodeTextBoxDesc = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.PTnodeCheckBoxQuick = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.PTnodeCheckBoxAuto = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.PTnodeNumericUpDownMinZ = new Aga.Controls.Tree.NodeControls.NodeNumericUpDown();
            this.PTIconOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStripForTree.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PTminZoomLvlNum)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PTiconCYNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PTiconCXNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PTiconPic)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AllowColumnReorder = true;
            this.treeView.AllowDrop = true;
            this.treeView.AutoRowHeight = true;
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Columns.Add(this.treeColumn1);
            this.treeView.Columns.Add(this.lonCol);
            this.treeView.Columns.Add(this.latCol);
            this.treeView.Columns.Add(this.altitudeCol);
            this.treeView.Columns.Add(this.descrText);
            this.treeView.Columns.Add(this.typeCol);
            this.treeView.Columns.Add(this.idCol);
            this.treeView.Columns.Add(this.commentsCol);
            this.treeView.Columns.Add(this.createdCol);
            this.treeView.ContextMenuStrip = this.contextMenuStripForTree;
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.DisplayDraggingNodes = true;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Maroon;
            this.treeView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(4, 4);
            this.treeView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeCheckBox_Shown);
            this.treeView.NodeControls.Add(this.nodeStateIcon1);
            this.treeView.NodeControls.Add(this.nodeName);
            this.treeView.NodeControls.Add(this.nodeDescr);
            this.treeView.NodeControls.Add(this.nodeIntegerTextBox_Id);
            this.treeView.NodeControls.Add(this.nodeCombo_Type);
            this.treeView.NodeControls.Add(this.nodeTextBox_Created);
            this.treeView.NodeControls.Add(this.nodeTextBox_Comment);
            this.treeView.NodeControls.Add(this.nodeTextBox_Lon);
            this.treeView.NodeControls.Add(this.nodeTextBox_Lat);
            this.treeView.NodeControls.Add(this.nodeNumericUpDown_Alt);
            this.treeView.RowHeight = 19;
            this.treeView.SelectedNode = null;
            this.treeView.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.MultiSameParent;
            this.treeView.Size = new System.Drawing.Size(964, 480);
            this.treeView.TabIndex = 2;
            this.treeView.Text = "POI tree";
            this.treeView.UseColumns = true;
            this.treeView.SelectionChanged += new System.EventHandler(this.treeView_SelectionChanged);
            this.treeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            this.treeView.ColumnClicked += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this.treeView_ColumnClicked);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
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
            // altitudeCol
            // 
            this.altitudeCol.Header = "Altitude";
            this.altitudeCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.altitudeCol.TooltipText = null;
            this.altitudeCol.Width = 60;
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
            this.typeCol.Width = 100;
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
            // contextMenuStripForTree
            // 
            this.contextMenuStripForTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGroupToolStripMenuItem,
            this.deletePOIOrGroupToolStripMenuItem});
            this.contextMenuStripForTree.Name = "contextMenuStripForTree";
            this.contextMenuStripForTree.Size = new System.Drawing.Size(223, 48);
            // 
            // addGroupToolStripMenuItem
            // 
            this.addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            this.addGroupToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addGroupToolStripMenuItem.Text = "Add new group";
            this.addGroupToolStripMenuItem.Click += new System.EventHandler(this.addGroupToolStripMenuItem_Click);
            // 
            // deletePOIOrGroupToolStripMenuItem
            // 
            this.deletePOIOrGroupToolStripMenuItem.Name = "deletePOIOrGroupToolStripMenuItem";
            this.deletePOIOrGroupToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.deletePOIOrGroupToolStripMenuItem.Text = "Delete POI or group";
            this.deletePOIOrGroupToolStripMenuItem.Click += new System.EventHandler(this.deletePOIOrGroupToolStripMenuItem_Click);
            // 
            // nodeCheckBox_Shown
            // 
            this.nodeCheckBox_Shown.DataPropertyName = "IsShownCentered";
            this.nodeCheckBox_Shown.EditEnabled = true;
            this.nodeCheckBox_Shown.LeftMargin = 0;
            this.nodeCheckBox_Shown.ParentColumn = this.treeColumn1;
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
            this.nodeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
            // nodeNumericUpDown_Alt
            // 
            this.nodeNumericUpDown_Alt.DataPropertyName = "altitude";
            this.nodeNumericUpDown_Alt.DecimalPlaces = 1;
            this.nodeNumericUpDown_Alt.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nodeNumericUpDown_Alt.IncrementalSearchEnabled = true;
            this.nodeNumericUpDown_Alt.LeftMargin = 3;
            this.nodeNumericUpDown_Alt.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.nodeNumericUpDown_Alt.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.nodeNumericUpDown_Alt.ParentColumn = this.altitudeCol;
            this.nodeNumericUpDown_Alt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nodeNumericUpDown_Alt.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 540);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(197, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Auto show POI on the map";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Image = global::GMView.Properties.Resources.lamp_on;
            this.okBut.Location = new System.Drawing.Point(867, 527);
            this.okBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(99, 46);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(980, 517);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeView);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(972, 488);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Points of Interest";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.treeViewPType);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(972, 488);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Types";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.PTdelTypeBut);
            this.groupBox1.Controls.Add(this.newTypeBut);
            this.groupBox1.Controls.Add(this.cancelChangesBut);
            this.groupBox1.Controls.Add(this.PTapplyBut);
            this.groupBox1.Controls.Add(this.PTminZoomLvlNum);
            this.groupBox1.Controls.Add(this.PTquickAddCB);
            this.groupBox1.Controls.Add(this.PTautoShowCB);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.PTdescrTB);
            this.groupBox1.Controls.Add(this.PTnameTB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(655, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(304, 470);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type info:";
            // 
            // PTdelTypeBut
            // 
            this.PTdelTypeBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PTdelTypeBut.Location = new System.Drawing.Point(164, 391);
            this.PTdelTypeBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTdelTypeBut.Name = "PTdelTypeBut";
            this.PTdelTypeBut.Size = new System.Drawing.Size(132, 32);
            this.PTdelTypeBut.TabIndex = 8;
            this.PTdelTypeBut.Text = "Delete Type";
            this.PTdelTypeBut.UseVisualStyleBackColor = true;
            this.PTdelTypeBut.Click += new System.EventHandler(this.PTdelTypeBut_Click);
            // 
            // newTypeBut
            // 
            this.newTypeBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newTypeBut.Location = new System.Drawing.Point(12, 391);
            this.newTypeBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.newTypeBut.Name = "newTypeBut";
            this.newTypeBut.Size = new System.Drawing.Size(132, 32);
            this.newTypeBut.TabIndex = 8;
            this.newTypeBut.Text = "New Type";
            this.newTypeBut.UseVisualStyleBackColor = true;
            this.newTypeBut.Click += new System.EventHandler(this.newTypeBut_Click);
            // 
            // cancelChangesBut
            // 
            this.cancelChangesBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelChangesBut.Location = new System.Drawing.Point(164, 431);
            this.cancelChangesBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelChangesBut.Name = "cancelChangesBut";
            this.cancelChangesBut.Size = new System.Drawing.Size(132, 32);
            this.cancelChangesBut.TabIndex = 8;
            this.cancelChangesBut.Text = "Cancel";
            this.cancelChangesBut.UseVisualStyleBackColor = true;
            this.cancelChangesBut.Click += new System.EventHandler(this.cancelChangesBut_Click);
            // 
            // PTapplyBut
            // 
            this.PTapplyBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PTapplyBut.Enabled = false;
            this.PTapplyBut.Location = new System.Drawing.Point(12, 431);
            this.PTapplyBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTapplyBut.Name = "PTapplyBut";
            this.PTapplyBut.Size = new System.Drawing.Size(132, 32);
            this.PTapplyBut.TabIndex = 8;
            this.PTapplyBut.Text = "Apply changes";
            this.PTapplyBut.UseVisualStyleBackColor = true;
            this.PTapplyBut.Click += new System.EventHandler(this.PTapplyBut_Click);
            // 
            // PTminZoomLvlNum
            // 
            this.PTminZoomLvlNum.Location = new System.Drawing.Point(223, 302);
            this.PTminZoomLvlNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTminZoomLvlNum.Maximum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.PTminZoomLvlNum.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PTminZoomLvlNum.Name = "PTminZoomLvlNum";
            this.PTminZoomLvlNum.Size = new System.Drawing.Size(73, 22);
            this.PTminZoomLvlNum.TabIndex = 7;
            this.PTminZoomLvlNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // PTquickAddCB
            // 
            this.PTquickAddCB.AutoSize = true;
            this.PTquickAddCB.Location = new System.Drawing.Point(16, 261);
            this.PTquickAddCB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTquickAddCB.Name = "PTquickAddCB";
            this.PTquickAddCB.Size = new System.Drawing.Size(178, 21);
            this.PTquickAddCB.TabIndex = 6;
            this.PTquickAddCB.Text = "Use in Quick Add mode";
            this.PTquickAddCB.UseVisualStyleBackColor = true;
            // 
            // PTautoShowCB
            // 
            this.PTautoShowCB.AutoSize = true;
            this.PTautoShowCB.Location = new System.Drawing.Point(16, 233);
            this.PTautoShowCB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTautoShowCB.Name = "PTautoShowCB";
            this.PTautoShowCB.Size = new System.Drawing.Size(170, 21);
            this.PTautoShowCB.TabIndex = 5;
            this.PTautoShowCB.Text = "Auto show on the map";
            this.PTautoShowCB.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.PTiconCYNum);
            this.groupBox2.Controls.Add(this.PTiconCXNum);
            this.groupBox2.Controls.Add(this.PTiconPic);
            this.groupBox2.Controls.Add(this.ChangeIconBut);
            this.groupBox2.Location = new System.Drawing.Point(8, 119);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(288, 106);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Icon:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(104, 66);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "CY:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "CX:";
            // 
            // PTiconCYNum
            // 
            this.PTiconCYNum.Location = new System.Drawing.Point(144, 64);
            this.PTiconCYNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTiconCYNum.Name = "PTiconCYNum";
            this.PTiconCYNum.Size = new System.Drawing.Size(65, 22);
            this.PTiconCYNum.TabIndex = 4;
            this.PTiconCYNum.ValueChanged += new System.EventHandler(this.PTiconCXNum_ValueChanged);
            // 
            // PTiconCXNum
            // 
            this.PTiconCXNum.Location = new System.Drawing.Point(144, 32);
            this.PTiconCXNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTiconCXNum.Name = "PTiconCXNum";
            this.PTiconCXNum.Size = new System.Drawing.Size(65, 22);
            this.PTiconCXNum.TabIndex = 4;
            this.PTiconCXNum.ValueChanged += new System.EventHandler(this.PTiconCXNum_ValueChanged);
            // 
            // PTiconPic
            // 
            this.PTiconPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PTiconPic.Cursor = System.Windows.Forms.Cursors.Cross;
            this.PTiconPic.Location = new System.Drawing.Point(8, 23);
            this.PTiconPic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTiconPic.Name = "PTiconPic";
            this.PTiconPic.Size = new System.Drawing.Size(85, 71);
            this.PTiconPic.TabIndex = 2;
            this.PTiconPic.TabStop = false;
            this.PTiconPic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PTiconPic_MouseDown);
            this.PTiconPic.Paint += new System.Windows.Forms.PaintEventHandler(this.PTiconPic_Paint);
            // 
            // ChangeIconBut
            // 
            this.ChangeIconBut.Location = new System.Drawing.Point(220, 34);
            this.ChangeIconBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChangeIconBut.Name = "ChangeIconBut";
            this.ChangeIconBut.Size = new System.Drawing.Size(60, 46);
            this.ChangeIconBut.TabIndex = 3;
            this.ChangeIconBut.Text = "Load";
            this.ChangeIconBut.UseVisualStyleBackColor = true;
            this.ChangeIconBut.Click += new System.EventHandler(this.ChangeIconBut_Click);
            // 
            // PTdescrTB
            // 
            this.PTdescrTB.Location = new System.Drawing.Point(8, 87);
            this.PTdescrTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTdescrTB.Name = "PTdescrTB";
            this.PTdescrTB.Size = new System.Drawing.Size(287, 22);
            this.PTdescrTB.TabIndex = 1;
            // 
            // PTnameTB
            // 
            this.PTnameTB.Location = new System.Drawing.Point(8, 39);
            this.PTnameTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PTnameTB.Name = "PTnameTB";
            this.PTnameTB.Size = new System.Drawing.Size(287, 22);
            this.PTnameTB.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 304);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Minimum display zoom level:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Description:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Short name:";
            // 
            // treeViewPType
            // 
            this.treeViewPType.AllowColumnReorder = true;
            this.treeViewPType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewPType.AutoRowHeight = true;
            this.treeViewPType.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewPType.Columns.Add(this.PTnameCol);
            this.treeViewPType.Columns.Add(this.PTdescrCol);
            this.treeViewPType.Columns.Add(this.PTquickAddCol);
            this.treeViewPType.Columns.Add(this.PTautoShowCol);
            this.treeViewPType.Columns.Add(this.PTminZoomCol);
            this.treeViewPType.DefaultToolTipProvider = null;
            this.treeViewPType.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewPType.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewPType.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeViewPType.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewPType.Location = new System.Drawing.Point(4, 7);
            this.treeViewPType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeViewPType.Model = null;
            this.treeViewPType.Name = "treeViewPType";
            this.treeViewPType.NodeControls.Add(this.PTnodeStateIcon);
            this.treeViewPType.NodeControls.Add(this.PTnodeTextBoxName);
            this.treeViewPType.NodeControls.Add(this.PTnodeTextBoxDesc);
            this.treeViewPType.NodeControls.Add(this.PTnodeCheckBoxQuick);
            this.treeViewPType.NodeControls.Add(this.PTnodeCheckBoxAuto);
            this.treeViewPType.NodeControls.Add(this.PTnodeNumericUpDownMinZ);
            this.treeViewPType.RowHeight = 40;
            this.treeViewPType.SelectedNode = null;
            this.treeViewPType.ShowLines = false;
            this.treeViewPType.ShowPlusMinus = false;
            this.treeViewPType.Size = new System.Drawing.Size(641, 469);
            this.treeViewPType.TabIndex = 0;
            this.treeViewPType.Text = "Type view";
            this.treeViewPType.UseColumns = true;
            this.treeViewPType.SelectionChanged += new System.EventHandler(this.treeViewPType_SelectionChanged);
            // 
            // PTnameCol
            // 
            this.PTnameCol.Header = "Name";
            this.PTnameCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.PTnameCol.TooltipText = null;
            this.PTnameCol.Width = 140;
            // 
            // PTdescrCol
            // 
            this.PTdescrCol.Header = "Description";
            this.PTdescrCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.PTdescrCol.TooltipText = null;
            this.PTdescrCol.Width = 200;
            // 
            // PTquickAddCol
            // 
            this.PTquickAddCol.Header = "Quick";
            this.PTquickAddCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.PTquickAddCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PTquickAddCol.TooltipText = null;
            this.PTquickAddCol.Width = 40;
            // 
            // PTautoShowCol
            // 
            this.PTautoShowCol.Header = "Auto";
            this.PTautoShowCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.PTautoShowCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PTautoShowCol.TooltipText = null;
            this.PTautoShowCol.Width = 40;
            // 
            // PTminZoomCol
            // 
            this.PTminZoomCol.Header = "Min zoom";
            this.PTminZoomCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.PTminZoomCol.TooltipText = null;
            // 
            // PTnodeStateIcon
            // 
            this.PTnodeStateIcon.DataPropertyName = "IconImg";
            this.PTnodeStateIcon.LeftMargin = 1;
            this.PTnodeStateIcon.ParentColumn = this.PTnameCol;
            this.PTnodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // PTnodeTextBoxName
            // 
            this.PTnodeTextBoxName.DataPropertyName = "Name";
            this.PTnodeTextBoxName.EditEnabled = true;
            this.PTnodeTextBoxName.IncrementalSearchEnabled = true;
            this.PTnodeTextBoxName.LeftMargin = 3;
            this.PTnodeTextBoxName.ParentColumn = this.PTnameCol;
            this.PTnodeTextBoxName.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.PTnodeTextBoxName.UseCompatibleTextRendering = true;
            // 
            // PTnodeTextBoxDesc
            // 
            this.PTnodeTextBoxDesc.DataPropertyName = "Text";
            this.PTnodeTextBoxDesc.EditEnabled = true;
            this.PTnodeTextBoxDesc.IncrementalSearchEnabled = true;
            this.PTnodeTextBoxDesc.LeftMargin = 3;
            this.PTnodeTextBoxDesc.ParentColumn = this.PTdescrCol;
            this.PTnodeTextBoxDesc.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // PTnodeCheckBoxQuick
            // 
            this.PTnodeCheckBoxQuick.DataPropertyName = "IsQuickType";
            this.PTnodeCheckBoxQuick.EditEnabled = true;
            this.PTnodeCheckBoxQuick.LeftMargin = 0;
            this.PTnodeCheckBoxQuick.ParentColumn = this.PTquickAddCol;
            // 
            // PTnodeCheckBoxAuto
            // 
            this.PTnodeCheckBoxAuto.DataPropertyName = "IsAutoShow";
            this.PTnodeCheckBoxAuto.EditEnabled = true;
            this.PTnodeCheckBoxAuto.LeftMargin = 0;
            this.PTnodeCheckBoxAuto.ParentColumn = this.PTautoShowCol;
            // 
            // PTnodeNumericUpDownMinZ
            // 
            this.PTnodeNumericUpDownMinZ.DataPropertyName = "MinZoomLvl";
            this.PTnodeNumericUpDownMinZ.EditEnabled = true;
            this.PTnodeNumericUpDownMinZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PTnodeNumericUpDownMinZ.IncrementalSearchEnabled = true;
            this.PTnodeNumericUpDownMinZ.LeftMargin = 3;
            this.PTnodeNumericUpDownMinZ.Maximum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.PTnodeNumericUpDownMinZ.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PTnodeNumericUpDownMinZ.ParentColumn = this.PTminZoomCol;
            this.PTnodeNumericUpDownMinZ.Trimming = System.Drawing.StringTrimming.Character;
            // 
            // PTIconOpenDialog
            // 
            this.PTIconOpenDialog.DefaultExt = "png";
            this.PTIconOpenDialog.FileName = "icon.png";
            this.PTIconOpenDialog.Title = "Choose icon for the type";
            // 
            // EditBooks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 576);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.okBut);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditBooks";
            this.Text = "Points of interest";
            this.contextMenuStripForTree.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PTminZoomLvlNum)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PTiconCYNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PTiconCXNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PTiconPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private Aga.Controls.Tree.NodeControls.NodeCheckBox nodeCheckBox_Shown;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripForTree;
        private System.Windows.Forms.ToolStripMenuItem addGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePOIOrGroupToolStripMenuItem;
        private Aga.Controls.Tree.TreeColumn altitudeCol;
        private Aga.Controls.Tree.NodeControls.NodeNumericUpDown nodeNumericUpDown_Alt;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Aga.Controls.Tree.TreeViewAdv treeViewPType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PTdescrTB;
        private System.Windows.Forms.TextBox PTnameTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox PTquickAddCB;
        private System.Windows.Forms.CheckBox PTautoShowCB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox PTiconPic;
        private System.Windows.Forms.Button ChangeIconBut;
        private System.Windows.Forms.NumericUpDown PTminZoomLvlNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button PTapplyBut;
        private Aga.Controls.Tree.TreeColumn PTnameCol;
        private Aga.Controls.Tree.TreeColumn PTdescrCol;
        private Aga.Controls.Tree.TreeColumn PTquickAddCol;
        private Aga.Controls.Tree.TreeColumn PTautoShowCol;
        private Aga.Controls.Tree.TreeColumn PTminZoomCol;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon PTnodeStateIcon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox PTnodeTextBoxName;
        private Aga.Controls.Tree.NodeControls.NodeTextBox PTnodeTextBoxDesc;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox PTnodeCheckBoxQuick;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox PTnodeCheckBoxAuto;
        private Aga.Controls.Tree.NodeControls.NodeNumericUpDown PTnodeNumericUpDownMinZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown PTiconCYNum;
        private System.Windows.Forms.NumericUpDown PTiconCXNum;
        private System.Windows.Forms.Button cancelChangesBut;
        private System.Windows.Forms.Button newTypeBut;
        private System.Windows.Forms.OpenFileDialog PTIconOpenDialog;
        private System.Windows.Forms.Button PTdelTypeBut;
    }
}