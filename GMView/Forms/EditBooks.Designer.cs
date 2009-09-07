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
            this.dataGV = new System.Windows.Forms.DataGridView();
            this.okBut = new System.Windows.Forms.Button();
            this.latCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lonCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.idname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pinColor = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGV, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.okBut, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(686, 271);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGV
            // 
            this.dataGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.latCol,
            this.lonCol,
            this.name,
            this.descr,
            this.group,
            this.idname,
            this.pinColor});
            this.dataGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGV.Location = new System.Drawing.Point(3, 3);
            this.dataGV.Name = "dataGV";
            this.dataGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGV.Size = new System.Drawing.Size(680, 235);
            this.dataGV.TabIndex = 0;
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
            // latCol
            // 
            this.latCol.HeaderText = "Latitude";
            this.latCol.Name = "latCol";
            this.latCol.Width = 60;
            // 
            // lonCol
            // 
            this.lonCol.HeaderText = "Longitude";
            this.lonCol.Name = "lonCol";
            this.lonCol.Width = 60;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.Width = 180;
            // 
            // descr
            // 
            this.descr.HeaderText = "Description";
            this.descr.Name = "descr";
            this.descr.Width = 350;
            // 
            // group
            // 
            this.group.AutoComplete = false;
            this.group.HeaderText = "Group";
            this.group.Items.AddRange(new object[] {
            "test"});
            this.group.Name = "group";
            this.group.ToolTipText = "Menu group for POI";
            this.group.Width = 150;
            // 
            // idname
            // 
            this.idname.HeaderText = "id";
            this.idname.Name = "idname";
            this.idname.ReadOnly = true;
            this.idname.Visible = false;
            // 
            // pinColor
            // 
            this.pinColor.HeaderText = "Pin color";
            this.pinColor.Items.AddRange(new object[] {
            "Yellow",
            "Green",
            "Red",
            "Blue"});
            this.pinColor.Name = "pinColor";
            this.pinColor.Width = 60;
            // 
            // EditBooks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 271);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditBooks";
            this.Text = "Edit places of interest";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGV;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.DataGridViewTextBoxColumn latCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn lonCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn descr;
        private System.Windows.Forms.DataGridViewComboBoxColumn group;
        private System.Windows.Forms.DataGridViewTextBoxColumn idname;
        private System.Windows.Forms.DataGridViewComboBoxColumn pinColor;
    }
}