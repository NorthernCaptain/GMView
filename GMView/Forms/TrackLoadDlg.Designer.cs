namespace GMView.Forms
{
    partial class TrackLoadDlg
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
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.POIGroupBox = new System.Windows.Forms.GroupBox();
            this.needPOICB = new System.Windows.Forms.CheckBox();
            this.poisLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackGroupBox = new System.Windows.Forms.GroupBox();
            this.trackNameTb = new System.Windows.Forms.TextBox();
            this.trackColorPicker = new ColorPicker.ColorPickerCombobox();
            this.routePointsLbl = new System.Windows.Forms.Label();
            this.trackPointsLbl = new System.Windows.Forms.Label();
            this.splitTrackCB = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fileChooser = new ncFileControls.FileChooser();
            this.showTrackInfoCB = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.poiTypeComboBox = new GMView.Forms.POITypeComboBox();
            this.groupBox1.SuspendLayout();
            this.POIGroupBox.SuspendLayout();
            this.trackGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(901, 808);
            this.cancelBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(75, 30);
            this.cancelBut.TabIndex = 0;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Enabled = false;
            this.okBut.Location = new System.Drawing.Point(820, 808);
            this.okBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(75, 30);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileChooser);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(758, 835);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose file:";
            // 
            // POIGroupBox
            // 
            this.POIGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.POIGroupBox.Controls.Add(this.radioButton2);
            this.POIGroupBox.Controls.Add(this.radioButton3);
            this.POIGroupBox.Controls.Add(this.radioButton1);
            this.POIGroupBox.Controls.Add(this.poiTypeComboBox);
            this.POIGroupBox.Controls.Add(this.checkBox1);
            this.POIGroupBox.Controls.Add(this.needPOICB);
            this.POIGroupBox.Controls.Add(this.poisLbl);
            this.POIGroupBox.Controls.Add(this.label4);
            this.POIGroupBox.Controls.Add(this.label2);
            this.POIGroupBox.Location = new System.Drawing.Point(766, 247);
            this.POIGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.POIGroupBox.Name = "POIGroupBox";
            this.POIGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.POIGroupBox.Size = new System.Drawing.Size(219, 258);
            this.POIGroupBox.TabIndex = 3;
            this.POIGroupBox.TabStop = false;
            this.POIGroupBox.Text = "POI:";
            // 
            // needPOICB
            // 
            this.needPOICB.AutoSize = true;
            this.needPOICB.Location = new System.Drawing.Point(11, 21);
            this.needPOICB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.needPOICB.Name = "needPOICB";
            this.needPOICB.Size = new System.Drawing.Size(150, 21);
            this.needPOICB.TabIndex = 0;
            this.needPOICB.Text = "Import POI from file";
            this.needPOICB.UseVisualStyleBackColor = true;
            // 
            // poisLbl
            // 
            this.poisLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.poisLbl.Location = new System.Drawing.Point(139, 48);
            this.poisLbl.Name = "poisLbl";
            this.poisLbl.Size = new System.Drawing.Size(69, 17);
            this.poisLbl.TabIndex = 2;
            this.poisLbl.Text = "0";
            this.poisLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Default POI type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total POIs:";
            // 
            // trackGroupBox
            // 
            this.trackGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackGroupBox.Controls.Add(this.trackNameTb);
            this.trackGroupBox.Controls.Add(this.trackColorPicker);
            this.trackGroupBox.Controls.Add(this.routePointsLbl);
            this.trackGroupBox.Controls.Add(this.trackPointsLbl);
            this.trackGroupBox.Controls.Add(this.showTrackInfoCB);
            this.trackGroupBox.Controls.Add(this.splitTrackCB);
            this.trackGroupBox.Controls.Add(this.label3);
            this.trackGroupBox.Controls.Add(this.label5);
            this.trackGroupBox.Controls.Add(this.label1);
            this.trackGroupBox.Location = new System.Drawing.Point(766, 2);
            this.trackGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackGroupBox.Name = "trackGroupBox";
            this.trackGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackGroupBox.Size = new System.Drawing.Size(219, 241);
            this.trackGroupBox.TabIndex = 4;
            this.trackGroupBox.TabStop = false;
            this.trackGroupBox.Text = "Track:";
            // 
            // trackNameTb
            // 
            this.trackNameTb.Location = new System.Drawing.Point(11, 22);
            this.trackNameTb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trackNameTb.Multiline = true;
            this.trackNameTb.Name = "trackNameTb";
            this.trackNameTb.ReadOnly = true;
            this.trackNameTb.Size = new System.Drawing.Size(197, 62);
            this.trackNameTb.TabIndex = 3;
            // 
            // trackColorPicker
            // 
            this.trackColorPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackColorPicker.Location = new System.Drawing.Point(99, 153);
            this.trackColorPicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackColorPicker.Name = "trackColorPicker";
            this.trackColorPicker.SelectedItem = System.Drawing.Color.Wheat;
            this.trackColorPicker.Size = new System.Drawing.Size(112, 23);
            this.trackColorPicker.TabIndex = 0;
            this.trackColorPicker.Text = "colorPickerCombobox1";
            // 
            // routePointsLbl
            // 
            this.routePointsLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.routePointsLbl.Location = new System.Drawing.Point(139, 121);
            this.routePointsLbl.Name = "routePointsLbl";
            this.routePointsLbl.Size = new System.Drawing.Size(69, 17);
            this.routePointsLbl.TabIndex = 2;
            this.routePointsLbl.Text = "0";
            this.routePointsLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackPointsLbl
            // 
            this.trackPointsLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPointsLbl.Location = new System.Drawing.Point(135, 98);
            this.trackPointsLbl.Name = "trackPointsLbl";
            this.trackPointsLbl.Size = new System.Drawing.Size(73, 17);
            this.trackPointsLbl.TabIndex = 2;
            this.trackPointsLbl.Text = "0";
            this.trackPointsLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // splitTrackCB
            // 
            this.splitTrackCB.AutoSize = true;
            this.splitTrackCB.Location = new System.Drawing.Point(11, 185);
            this.splitTrackCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitTrackCB.Name = "splitTrackCB";
            this.splitTrackCB.Size = new System.Drawing.Size(143, 21);
            this.splitTrackCB.TabIndex = 1;
            this.splitTrackCB.Text = "Split track by date";
            this.splitTrackCB.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Track color:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Total route points:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total track points:";
            // 
            // fileChooser
            // 
            this.fileChooser.AutoSize = true;
            this.fileChooser.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fileChooser.DirectoryPath = "";
            this.fileChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileChooser.Location = new System.Drawing.Point(3, 17);
            this.fileChooser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fileChooser.Name = "fileChooser";
            this.fileChooser.SelectedFile = null;
            this.fileChooser.Size = new System.Drawing.Size(752, 816);
            this.fileChooser.TabIndex = 0;
            // 
            // showTrackInfoCB
            // 
            this.showTrackInfoCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showTrackInfoCB.AutoSize = true;
            this.showTrackInfoCB.Checked = true;
            this.showTrackInfoCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTrackInfoCB.Location = new System.Drawing.Point(11, 210);
            this.showTrackInfoCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.showTrackInfoCB.Name = "showTrackInfoCB";
            this.showTrackInfoCB.Size = new System.Drawing.Size(175, 21);
            this.showTrackInfoCB.TabIndex = 1;
            this.showTrackInfoCB.Text = "Show track info window";
            this.showTrackInfoCB.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(10, 173);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(169, 21);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.Text = "Name <=> Description";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(10, 200);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(164, 21);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.Text = "Name <=> Comments";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(10, 226);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(207, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "+Description <=> Comments";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(10, 146);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(105, 21);
            this.radioButton3.TabIndex = 4;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "No changes";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // poiTypeComboBox
            // 
            this.poiTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.poiTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.poiTypeComboBox.FormattingEnabled = true;
            this.poiTypeComboBox.ItemHeight = 40;
            this.poiTypeComboBox.Location = new System.Drawing.Point(11, 95);
            this.poiTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.poiTypeComboBox.MaxDropDownItems = 10;
            this.poiTypeComboBox.Name = "poiTypeComboBox";
            this.poiTypeComboBox.Size = new System.Drawing.Size(201, 46);
            this.poiTypeComboBox.TabIndex = 3;
            // 
            // TrackLoadDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(988, 849);
            this.Controls.Add(this.trackGroupBox);
            this.Controls.Add(this.POIGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TrackLoadDlg";
            this.Text = "Open track file";
            this.Load += new System.EventHandler(this.TrackLoadDlg_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrackLoadDlg_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.TrackLoadDlg_ResizeEnd);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.POIGroupBox.ResumeLayout(false);
            this.POIGroupBox.PerformLayout();
            this.trackGroupBox.ResumeLayout(false);
            this.trackGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.GroupBox groupBox1;
        private ncFileControls.FileChooser fileChooser;
        private System.Windows.Forms.GroupBox POIGroupBox;
        private ColorPicker.ColorPickerCombobox trackColorPicker;
        private System.Windows.Forms.GroupBox trackGroupBox;
        private System.Windows.Forms.Label trackPointsLbl;
        private System.Windows.Forms.CheckBox splitTrackCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox needPOICB;
        private System.Windows.Forms.Label poisLbl;
        private System.Windows.Forms.Label label2;
        private POITypeComboBox poiTypeComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox trackNameTb;
        private System.Windows.Forms.Label routePointsLbl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox showTrackInfoCB;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton radioButton3;
    }
}