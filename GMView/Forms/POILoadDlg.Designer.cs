namespace GMView.Forms
{
    partial class POILoadDlg
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
            this.okBut = new System.Windows.Forms.Button();
            this.cancelBut = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.fileChooser = new ncFileControls.FileChooser();
            this.poiLoadControl = new GMView.Forms.POILoadControl();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Enabled = false;
            this.okBut.Location = new System.Drawing.Point(677, 508);
            this.okBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(75, 30);
            this.okBut.TabIndex = 3;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(758, 508);
            this.cancelBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(75, 30);
            this.cancelBut.TabIndex = 2;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.fileChooser);
            this.groupBox.Location = new System.Drawing.Point(2, 2);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(611, 535);
            this.groupBox.TabIndex = 5;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "POI File:";
            // 
            // fileChooser
            // 
            this.fileChooser.DirectoryPath = "";
            this.fileChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileChooser.Location = new System.Drawing.Point(3, 18);
            this.fileChooser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fileChooser.Name = "fileChooser";
            this.fileChooser.SelectedFile = null;
            this.fileChooser.Size = new System.Drawing.Size(605, 514);
            this.fileChooser.TabIndex = 0;
            // 
            // poiLoadControl
            // 
            this.poiLoadControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.poiLoadControl.FileInfo = null;
            this.poiLoadControl.Location = new System.Drawing.Point(619, 2);
            this.poiLoadControl.Name = "poiLoadControl";
            this.poiLoadControl.Size = new System.Drawing.Size(221, 260);
            this.poiLoadControl.TabIndex = 4;
            // 
            // POILoadDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(845, 549);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.poiLoadControl);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Name = "POILoadDlg";
            this.ShowIcon = false;
            this.Text = "Import POIs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.POILoadDlg_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.POILoadDlg_ResizeEnd);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Button cancelBut;
        private POILoadControl poiLoadControl;
        private System.Windows.Forms.GroupBox groupBox;
        private ncFileControls.FileChooser fileChooser;
    }
}