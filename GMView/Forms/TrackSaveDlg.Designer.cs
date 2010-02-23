namespace GMView.Forms
{
    partial class TrackSaveDlg
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
            this.groupFileChoose = new System.Windows.Forms.GroupBox();
            this.fileChooser = new ncFileControls.FileChooser();
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trackPointsNumLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackNameTb = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.savePOICb = new System.Windows.Forms.CheckBox();
            this.poiNumLbl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupFileChoose.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupFileChoose
            // 
            this.groupFileChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupFileChoose.Controls.Add(this.fileChooser);
            this.groupFileChoose.Location = new System.Drawing.Point(1, 1);
            this.groupFileChoose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupFileChoose.Name = "groupFileChoose";
            this.groupFileChoose.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupFileChoose.Size = new System.Drawing.Size(851, 614);
            this.groupFileChoose.TabIndex = 0;
            this.groupFileChoose.TabStop = false;
            this.groupFileChoose.Text = "File to save into:";
            // 
            // fileChooser
            // 
            this.fileChooser.DirectoryPath = "";
            this.fileChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileChooser.Location = new System.Drawing.Point(4, 19);
            this.fileChooser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fileChooser.Name = "fileChooser";
            this.fileChooser.SelectedFile = null;
            this.fileChooser.Size = new System.Drawing.Size(843, 591);
            this.fileChooser.TabIndex = 0;
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(995, 578);
            this.cancelBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(100, 37);
            this.cancelBut.TabIndex = 1;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Location = new System.Drawing.Point(887, 578);
            this.okBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(100, 37);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.trackPointsNumLbl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.trackNameTb);
            this.groupBox1.Location = new System.Drawing.Point(855, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(252, 129);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Track Info:";
            // 
            // trackPointsNumLbl
            // 
            this.trackPointsNumLbl.Location = new System.Drawing.Point(139, 96);
            this.trackPointsNumLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.trackPointsNumLbl.Name = "trackPointsNumLbl";
            this.trackPointsNumLbl.Size = new System.Drawing.Size(101, 18);
            this.trackPointsNumLbl.TabIndex = 2;
            this.trackPointsNumLbl.Text = "0";
            this.trackPointsNumLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 96);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Total track points:";
            // 
            // trackNameTb
            // 
            this.trackNameTb.Location = new System.Drawing.Point(8, 23);
            this.trackNameTb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trackNameTb.Multiline = true;
            this.trackNameTb.Name = "trackNameTb";
            this.trackNameTb.Size = new System.Drawing.Size(231, 62);
            this.trackNameTb.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.savePOICb);
            this.groupBox2.Controls.Add(this.poiNumLbl);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(855, 134);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(252, 92);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "POI info:";
            // 
            // savePOICb
            // 
            this.savePOICb.AutoSize = true;
            this.savePOICb.Checked = true;
            this.savePOICb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.savePOICb.Location = new System.Drawing.Point(12, 57);
            this.savePOICb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.savePOICb.Name = "savePOICb";
            this.savePOICb.Size = new System.Drawing.Size(164, 21);
            this.savePOICb.TabIndex = 3;
            this.savePOICb.Text = "Save neighbour POIs";
            this.savePOICb.UseVisualStyleBackColor = true;
            // 
            // poiNumLbl
            // 
            this.poiNumLbl.Location = new System.Drawing.Point(139, 25);
            this.poiNumLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.poiNumLbl.Name = "poiNumLbl";
            this.poiNumLbl.Size = new System.Drawing.Size(101, 18);
            this.poiNumLbl.TabIndex = 2;
            this.poiNumLbl.Text = "0";
            this.poiNumLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Total POIs:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(860, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 34);
            this.label2.TabIndex = 4;
            this.label2.Text = "Default file format is GPX.\r\nUse \'Type\' drop down to change.\r\n";
            // 
            // TrackSaveDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(1111, 622);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.groupFileChoose);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "TrackSaveDlg";
            this.ShowIcon = false;
            this.Text = "Save track";
            this.ResizeEnd += new System.EventHandler(this.TrackSaveDlg_ResizeEnd);
            this.groupFileChoose.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupFileChoose;
        private ncFileControls.FileChooser fileChooser;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label trackPointsNumLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox trackNameTb;
        private System.Windows.Forms.Label poiNumLbl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox savePOICb;
        private System.Windows.Forms.Label label2;

    }
}