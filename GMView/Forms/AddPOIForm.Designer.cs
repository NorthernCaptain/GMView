namespace GMView.Forms
{
    partial class AddPOIForm
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
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.groupBoxPOI = new System.Windows.Forms.GroupBox();
            this.okBut = new System.Windows.Forms.Button();
            this.cancelBut = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTb = new System.Windows.Forms.TextBox();
            this.descrTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lonTb = new System.Windows.Forms.TextBox();
            this.latTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.treeColumnName = new Aga.Controls.Tree.TreeColumn();
            this.nodeStateIcon1 = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.nodeTextBox_Name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.commentTB = new System.Windows.Forms.TextBox();
            this.poiTypeListBox = new GMView.Forms.POITypeListBox();
            this.altTB = new System.Windows.Forms.NumericUpDown();
            this.groupBoxPOI.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.altTB)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Columns.Add(this.treeColumnName);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.Location = new System.Drawing.Point(6, 19);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeStateIcon1);
            this.treeView.NodeControls.Add(this.nodeTextBox_Name);
            this.treeView.RowHeight = 19;
            this.treeView.SelectedNode = null;
            this.treeView.Size = new System.Drawing.Size(216, 377);
            this.treeView.TabIndex = 0;
            this.treeView.Text = "treeView";
            // 
            // groupBoxPOI
            // 
            this.groupBoxPOI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxPOI.Controls.Add(this.treeView);
            this.groupBoxPOI.Location = new System.Drawing.Point(12, 12);
            this.groupBoxPOI.Name = "groupBoxPOI";
            this.groupBoxPOI.Size = new System.Drawing.Size(230, 402);
            this.groupBoxPOI.TabIndex = 1;
            this.groupBoxPOI.TabStop = false;
            this.groupBoxPOI.Text = "POI Group:";
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Location = new System.Drawing.Point(344, 420);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(75, 31);
            this.okBut.TabIndex = 20;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(424, 420);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(75, 31);
            this.cancelBut.TabIndex = 21;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.altTB);
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.latTB);
            this.groupBox1.Controls.Add(this.lonTb);
            this.groupBox1.Controls.Add(this.nameTb);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.poiTypeListBox);
            this.groupBox1.Location = new System.Drawing.Point(248, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 402);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "POI data:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // nameTb
            // 
            this.nameTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTb.Location = new System.Drawing.Point(9, 35);
            this.nameTb.Name = "nameTb";
            this.nameTb.Size = new System.Drawing.Size(236, 20);
            this.nameTb.TabIndex = 2;
            // 
            // descrTb
            // 
            this.descrTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descrTb.Location = new System.Drawing.Point(3, 3);
            this.descrTb.Multiline = true;
            this.descrTb.Name = "descrTb";
            this.descrTb.Size = new System.Drawing.Size(225, 68);
            this.descrTb.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Lat:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Lon:";
            // 
            // lonTb
            // 
            this.lonTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lonTb.Location = new System.Drawing.Point(40, 87);
            this.lonTb.Name = "lonTb";
            this.lonTb.Size = new System.Drawing.Size(98, 20);
            this.lonTb.TabIndex = 4;
            // 
            // latTB
            // 
            this.latTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.latTB.Location = new System.Drawing.Point(40, 61);
            this.latTB.Name = "latTB";
            this.latTB.Size = new System.Drawing.Size(98, 20);
            this.latTB.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(146, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Alt:";
            // 
            // treeColumnName
            // 
            this.treeColumnName.Header = "Name";
            this.treeColumnName.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeColumnName.TooltipText = null;
            this.treeColumnName.Width = 300;
            // 
            // nodeStateIcon1
            // 
            this.nodeStateIcon1.LeftMargin = 1;
            this.nodeStateIcon1.ParentColumn = this.treeColumnName;
            this.nodeStateIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBox_Name
            // 
            this.nodeTextBox_Name.DataPropertyName = "Name";
            this.nodeTextBox_Name.IncrementalSearchEnabled = true;
            this.nodeTextBox_Name.LeftMargin = 3;
            this.nodeTextBox_Name.ParentColumn = this.treeColumnName;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 113);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(239, 100);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.commentTB);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(231, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Comments";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.descrTb);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(231, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Description";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // commentTB
            // 
            this.commentTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commentTB.Location = new System.Drawing.Point(3, 3);
            this.commentTB.Multiline = true;
            this.commentTB.Name = "commentTB";
            this.commentTB.Size = new System.Drawing.Size(225, 68);
            this.commentTB.TabIndex = 5;
            this.commentTB.TabStop = false;
            // 
            // poiTypeListBox
            // 
            this.poiTypeListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.poiTypeListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.poiTypeListBox.FormattingEnabled = true;
            this.poiTypeListBox.ItemHeight = 40;
            this.poiTypeListBox.Location = new System.Drawing.Point(6, 232);
            this.poiTypeListBox.Name = "poiTypeListBox";
            this.poiTypeListBox.Size = new System.Drawing.Size(239, 164);
            this.poiTypeListBox.TabIndex = 7;
            // 
            // altTB
            // 
            this.altTB.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.altTB.Location = new System.Drawing.Point(174, 62);
            this.altTB.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.altTB.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.altTB.Name = "altTB";
            this.altTB.Size = new System.Drawing.Size(71, 20);
            this.altTB.TabIndex = 5;
            // 
            // AddPOIForm
            // 
            this.AcceptButton = this.okBut;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(511, 455);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.groupBoxPOI);
            this.Name = "AddPOIForm";
            this.Text = "Create new POI";
            this.Load += new System.EventHandler(this.AddPOIForm_Load);
            this.groupBoxPOI.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.altTB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv treeView;
        private System.Windows.Forms.GroupBox groupBoxPOI;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox nameTb;
        private System.Windows.Forms.Label label1;
        private POITypeListBox poiTypeListBox;
        private System.Windows.Forms.TextBox descrTb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox latTB;
        private System.Windows.Forms.TextBox lonTb;
        private System.Windows.Forms.Label label6;
        private Aga.Controls.Tree.TreeColumn treeColumnName;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nodeStateIcon1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox_Name;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox commentTB;
        private System.Windows.Forms.NumericUpDown altTB;
    }
}