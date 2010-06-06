namespace GMView.Forms
{
    partial class SetLonLatForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.latTBox = new System.Windows.Forms.TextBox();
            this.lonTBox = new System.Windows.Forms.TextBox();
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Latitude [NS]:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Longitude [WE]:";
            // 
            // latTBox
            // 
            this.latTBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.latTBox.Location = new System.Drawing.Point(102, 6);
            this.latTBox.Name = "latTBox";
            this.latTBox.Size = new System.Drawing.Size(127, 20);
            this.latTBox.TabIndex = 1;
            // 
            // lonTBox
            // 
            this.lonTBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lonTBox.Location = new System.Drawing.Point(102, 40);
            this.lonTBox.Name = "lonTBox";
            this.lonTBox.Size = new System.Drawing.Size(127, 20);
            this.lonTBox.TabIndex = 2;
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(148, 149);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(81, 30);
            this.cancelBut.TabIndex = 4;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okBut.Location = new System.Drawing.Point(12, 149);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(84, 30);
            this.okBut.TabIndex = 3;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(12, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 52);
            this.label3.TabIndex = 0;
            this.label3.Text = "Format:\r\n  Deg,Min\'Sec.Frac [NSWE] or minus sign\r\n  Deg,Min.Frac [NSWE] or minus " +
                "sign\r\n  Deg.Frac [NSWE] or minus sign";
            // 
            // SetLonLatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(241, 187);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.lonTBox);
            this.Controls.Add(this.latTBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetLonLatForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Set location by lon, lat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox latTBox;
        private System.Windows.Forms.TextBox lonTBox;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Label label3;
    }
}