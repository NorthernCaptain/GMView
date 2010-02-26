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
            this.fileChooser = new ncFileControls.FileChooser();
            this.trackLoadControl = new GMView.Forms.TrackLoadControl();
            this.trackPoiLoadControl = new GMView.Forms.POILoadControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(913, 628);
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
            this.okBut.Location = new System.Drawing.Point(832, 628);
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
            this.groupBox1.Size = new System.Drawing.Size(770, 656);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose file:";
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
            this.fileChooser.Size = new System.Drawing.Size(764, 637);
            this.fileChooser.TabIndex = 0;
            // 
            // trackLoadControl
            // 
            this.trackLoadControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackLoadControl.FileInfo = null;
            this.trackLoadControl.Location = new System.Drawing.Point(778, 2);
            this.trackLoadControl.Name = "trackLoadControl";
            this.trackLoadControl.Size = new System.Drawing.Size(219, 246);
            this.trackLoadControl.TabIndex = 6;
            // 
            // trackPoiLoadControl
            // 
            this.trackPoiLoadControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPoiLoadControl.FileInfo = null;
            this.trackPoiLoadControl.Location = new System.Drawing.Point(778, 248);
            this.trackPoiLoadControl.Name = "trackPoiLoadControl";
            this.trackPoiLoadControl.Size = new System.Drawing.Size(219, 260);
            this.trackPoiLoadControl.TabIndex = 5;
            // 
            // TrackLoadDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(1000, 669);
            this.Controls.Add(this.trackLoadControl);
            this.Controls.Add(this.trackPoiLoadControl);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TrackLoadDlg";
            this.ShowIcon = false;
            this.Text = "Open track file";
            this.Load += new System.EventHandler(this.TrackLoadDlg_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrackLoadDlg_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.TrackLoadDlg_ResizeEnd);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.GroupBox groupBox1;
        private ncFileControls.FileChooser fileChooser;
        private POILoadControl trackPoiLoadControl;
        private TrackLoadControl trackLoadControl;
    }
}