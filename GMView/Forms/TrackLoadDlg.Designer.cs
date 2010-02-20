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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.poisLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackColorPicker = new ColorPicker.ColorPickerCombobox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.trackPointsLbl = new System.Windows.Forms.Label();
            this.splitTrackCB = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.poiTypeComboBox = new GMView.Forms.POITypeComboBox();
            this.fileChooser = new ncFileControls.FileChooser();
            this.trackNameTb = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(675, 468);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(614, 468);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileChooser);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(568, 490);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose file:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.poiTypeComboBox);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.poisLbl);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(574, 161);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(164, 131);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "POI:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 17);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(115, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Import POI from file";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // poisLbl
            // 
            this.poisLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.poisLbl.AutoSize = true;
            this.poisLbl.Location = new System.Drawing.Point(145, 39);
            this.poisLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.poisLbl.Name = "poisLbl";
            this.poisLbl.Size = new System.Drawing.Size(13, 13);
            this.poisLbl.TabIndex = 2;
            this.poisLbl.Text = "0";
            this.poisLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Default POI type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total POIs:";
            // 
            // trackColorPicker
            // 
            this.trackColorPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackColorPicker.Location = new System.Drawing.Point(74, 104);
            this.trackColorPicker.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trackColorPicker.Name = "trackColorPicker";
            this.trackColorPicker.SelectedItem = System.Drawing.Color.Wheat;
            this.trackColorPicker.Size = new System.Drawing.Size(84, 19);
            this.trackColorPicker.TabIndex = 0;
            this.trackColorPicker.Text = "colorPickerCombobox1";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.trackNameTb);
            this.groupBox3.Controls.Add(this.trackColorPicker);
            this.groupBox3.Controls.Add(this.trackPointsLbl);
            this.groupBox3.Controls.Add(this.splitTrackCB);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(574, 2);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(164, 155);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Track:";
            // 
            // trackPointsLbl
            // 
            this.trackPointsLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPointsLbl.AutoSize = true;
            this.trackPointsLbl.Location = new System.Drawing.Point(145, 80);
            this.trackPointsLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.trackPointsLbl.Name = "trackPointsLbl";
            this.trackPointsLbl.Size = new System.Drawing.Size(13, 13);
            this.trackPointsLbl.TabIndex = 2;
            this.trackPointsLbl.Text = "0";
            this.trackPointsLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // splitTrackCB
            // 
            this.splitTrackCB.AutoSize = true;
            this.splitTrackCB.Location = new System.Drawing.Point(8, 130);
            this.splitTrackCB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitTrackCB.Name = "splitTrackCB";
            this.splitTrackCB.Size = new System.Drawing.Size(111, 17);
            this.splitTrackCB.TabIndex = 1;
            this.splitTrackCB.Text = "Split track by date";
            this.splitTrackCB.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 106);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Track color:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 80);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total track points:";
            // 
            // poiTypeComboBox
            // 
            this.poiTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.poiTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.poiTypeComboBox.FormattingEnabled = true;
            this.poiTypeComboBox.ItemHeight = 40;
            this.poiTypeComboBox.Location = new System.Drawing.Point(8, 77);
            this.poiTypeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.poiTypeComboBox.MaxDropDownItems = 10;
            this.poiTypeComboBox.Name = "poiTypeComboBox";
            this.poiTypeComboBox.Size = new System.Drawing.Size(152, 46);
            this.poiTypeComboBox.TabIndex = 3;
            // 
            // fileChooser
            // 
            this.fileChooser.DirectoryPath = "";
            this.fileChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileChooser.Location = new System.Drawing.Point(2, 15);
            this.fileChooser.Margin = new System.Windows.Forms.Padding(2);
            this.fileChooser.Name = "fileChooser";
            this.fileChooser.Size = new System.Drawing.Size(564, 473);
            this.fileChooser.TabIndex = 0;
            // 
            // trackNameTb
            // 
            this.trackNameTb.Location = new System.Drawing.Point(8, 18);
            this.trackNameTb.Multiline = true;
            this.trackNameTb.Name = "trackNameTb";
            this.trackNameTb.ReadOnly = true;
            this.trackNameTb.Size = new System.Drawing.Size(149, 51);
            this.trackNameTb.TabIndex = 3;
            // 
            // TrackLoadDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 501);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TrackLoadDlg";
            this.Text = "Open track file";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrackLoadDlg_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private ncFileControls.FileChooser fileChooser;
        private System.Windows.Forms.GroupBox groupBox2;
        private ColorPicker.ColorPickerCombobox trackColorPicker;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label trackPointsLbl;
        private System.Windows.Forms.CheckBox splitTrackCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label poisLbl;
        private System.Windows.Forms.Label label2;
        private POITypeComboBox poiTypeComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox trackNameTb;
    }
}