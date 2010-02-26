namespace GMView.Forms
{
    partial class TrackLoadControl
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
            this.trackGroupBox = new System.Windows.Forms.GroupBox();
            this.trackNameTb = new System.Windows.Forms.TextBox();
            this.trackColorPicker = new ColorPicker.ColorPickerCombobox();
            this.routePointsLbl = new System.Windows.Forms.Label();
            this.trackPointsLbl = new System.Windows.Forms.Label();
            this.showTrackInfoCB = new System.Windows.Forms.CheckBox();
            this.splitTrackCB = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackGroupBox
            // 
            this.trackGroupBox.Controls.Add(this.trackNameTb);
            this.trackGroupBox.Controls.Add(this.trackColorPicker);
            this.trackGroupBox.Controls.Add(this.routePointsLbl);
            this.trackGroupBox.Controls.Add(this.trackPointsLbl);
            this.trackGroupBox.Controls.Add(this.showTrackInfoCB);
            this.trackGroupBox.Controls.Add(this.splitTrackCB);
            this.trackGroupBox.Controls.Add(this.label3);
            this.trackGroupBox.Controls.Add(this.label5);
            this.trackGroupBox.Controls.Add(this.label1);
            this.trackGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackGroupBox.Location = new System.Drawing.Point(0, 0);
            this.trackGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackGroupBox.Name = "trackGroupBox";
            this.trackGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackGroupBox.Size = new System.Drawing.Size(217, 246);
            this.trackGroupBox.TabIndex = 5;
            this.trackGroupBox.TabStop = false;
            this.trackGroupBox.Text = "Track:";
            // 
            // trackNameTb
            // 
            this.trackNameTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackNameTb.Location = new System.Drawing.Point(11, 22);
            this.trackNameTb.Margin = new System.Windows.Forms.Padding(4);
            this.trackNameTb.Multiline = true;
            this.trackNameTb.Name = "trackNameTb";
            this.trackNameTb.ReadOnly = true;
            this.trackNameTb.Size = new System.Drawing.Size(195, 62);
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
            this.trackColorPicker.Size = new System.Drawing.Size(110, 23);
            this.trackColorPicker.TabIndex = 0;
            this.trackColorPicker.Text = "colorPickerCombobox1";
            // 
            // routePointsLbl
            // 
            this.routePointsLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.routePointsLbl.Location = new System.Drawing.Point(137, 121);
            this.routePointsLbl.Name = "routePointsLbl";
            this.routePointsLbl.Size = new System.Drawing.Size(69, 17);
            this.routePointsLbl.TabIndex = 2;
            this.routePointsLbl.Text = "0";
            this.routePointsLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackPointsLbl
            // 
            this.trackPointsLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPointsLbl.Location = new System.Drawing.Point(133, 98);
            this.trackPointsLbl.Name = "trackPointsLbl";
            this.trackPointsLbl.Size = new System.Drawing.Size(73, 17);
            this.trackPointsLbl.TabIndex = 2;
            this.trackPointsLbl.Text = "0";
            this.trackPointsLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // showTrackInfoCB
            // 
            this.showTrackInfoCB.AutoSize = true;
            this.showTrackInfoCB.Checked = true;
            this.showTrackInfoCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTrackInfoCB.Location = new System.Drawing.Point(10, 210);
            this.showTrackInfoCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.showTrackInfoCB.Name = "showTrackInfoCB";
            this.showTrackInfoCB.Size = new System.Drawing.Size(175, 21);
            this.showTrackInfoCB.TabIndex = 1;
            this.showTrackInfoCB.Text = "Show track info window";
            this.showTrackInfoCB.UseVisualStyleBackColor = true;
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
            // TrackLoadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trackGroupBox);
            this.Name = "TrackLoadControl";
            this.Size = new System.Drawing.Size(217, 246);
            this.trackGroupBox.ResumeLayout(false);
            this.trackGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox trackGroupBox;
        private System.Windows.Forms.TextBox trackNameTb;
        private ColorPicker.ColorPickerCombobox trackColorPicker;
        private System.Windows.Forms.Label routePointsLbl;
        private System.Windows.Forms.Label trackPointsLbl;
        private System.Windows.Forms.CheckBox showTrackInfoCB;
        private System.Windows.Forms.CheckBox splitTrackCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
    }
}
