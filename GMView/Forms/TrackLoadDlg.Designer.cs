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
            this.fileChooser = new ncFileControls.FileChooser();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.trackColorPicker = new ColorPicker.ColorPickerCombobox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitTrackCB = new System.Windows.Forms.CheckBox();
            this.trackPointsLbl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackNameLbl = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.poisLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.poiTypeComboBox = new GMView.Forms.POITypeComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(772, 550);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(691, 550);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 29);
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
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(629, 577);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose file:";
            // 
            // fileChooser
            // 
            this.fileChooser.DirectoryPath = "";
            this.fileChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileChooser.Location = new System.Drawing.Point(3, 18);
            this.fileChooser.Name = "fileChooser";
            this.fileChooser.Size = new System.Drawing.Size(623, 556);
            this.fileChooser.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.poiTypeComboBox);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.poisLbl);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(638, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(219, 143);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "POI:";
            // 
            // trackColorPicker
            // 
            this.trackColorPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackColorPicker.Location = new System.Drawing.Point(99, 93);
            this.trackColorPicker.Name = "trackColorPicker";
            this.trackColorPicker.SelectedItem = System.Drawing.Color.Wheat;
            this.trackColorPicker.Size = new System.Drawing.Size(112, 23);
            this.trackColorPicker.TabIndex = 0;
            this.trackColorPicker.Text = "colorPickerCombobox1";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.trackColorPicker);
            this.groupBox3.Controls.Add(this.trackPointsLbl);
            this.groupBox3.Controls.Add(this.splitTrackCB);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.trackNameLbl);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(638, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(219, 133);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Track:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total track points:";
            // 
            // splitTrackCB
            // 
            this.splitTrackCB.AutoSize = true;
            this.splitTrackCB.Location = new System.Drawing.Point(10, 66);
            this.splitTrackCB.Name = "splitTrackCB";
            this.splitTrackCB.Size = new System.Drawing.Size(143, 21);
            this.splitTrackCB.TabIndex = 1;
            this.splitTrackCB.Text = "Split track by date";
            this.splitTrackCB.UseVisualStyleBackColor = true;
            // 
            // trackPointsLbl
            // 
            this.trackPointsLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPointsLbl.AutoSize = true;
            this.trackPointsLbl.Location = new System.Drawing.Point(193, 39);
            this.trackPointsLbl.Name = "trackPointsLbl";
            this.trackPointsLbl.Size = new System.Drawing.Size(16, 17);
            this.trackPointsLbl.TabIndex = 2;
            this.trackPointsLbl.Text = "0";
            this.trackPointsLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Track color:";
            // 
            // trackNameLbl
            // 
            this.trackNameLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackNameLbl.AutoSize = true;
            this.trackNameLbl.Location = new System.Drawing.Point(7, 18);
            this.trackNameLbl.Name = "trackNameLbl";
            this.trackNameLbl.Size = new System.Drawing.Size(83, 17);
            this.trackNameLbl.TabIndex = 0;
            this.trackNameLbl.Text = "Track name";
            this.trackNameLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(10, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(150, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Import POI from file";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total POIs:";
            // 
            // poisLbl
            // 
            this.poisLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.poisLbl.AutoSize = true;
            this.poisLbl.Location = new System.Drawing.Point(193, 45);
            this.poisLbl.Name = "poisLbl";
            this.poisLbl.Size = new System.Drawing.Size(16, 17);
            this.poisLbl.TabIndex = 2;
            this.poisLbl.Text = "0";
            this.poisLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Default POI type:";
            // 
            // poiTypeComboBox
            // 
            this.poiTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.poiTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.poiTypeComboBox.FormattingEnabled = true;
            this.poiTypeComboBox.ItemHeight = 40;
            this.poiTypeComboBox.Location = new System.Drawing.Point(10, 86);
            this.poiTypeComboBox.MaxDropDownItems = 10;
            this.poiTypeComboBox.Name = "poiTypeComboBox";
            this.poiTypeComboBox.Size = new System.Drawing.Size(201, 46);
            this.poiTypeComboBox.TabIndex = 3;
            // 
            // TrackLoadDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 591);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Label trackNameLbl;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label poisLbl;
        private System.Windows.Forms.Label label2;
        private POITypeComboBox poiTypeComboBox;
        private System.Windows.Forms.Label label4;
    }
}