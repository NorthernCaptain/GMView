namespace GMView.Forms
{
    partial class IntroForm
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
            this.infoLb = new System.Windows.Forms.Label();
            this.appNameLbl = new System.Windows.Forms.Label();
            this.rendererLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // infoLb
            // 
            this.infoLb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLb.BackColor = System.Drawing.Color.Black;
            this.infoLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoLb.ForeColor = System.Drawing.Color.Yellow;
            this.infoLb.Location = new System.Drawing.Point(7, 230);
            this.infoLb.Name = "infoLb";
            this.infoLb.Size = new System.Drawing.Size(363, 16);
            this.infoLb.TabIndex = 0;
            this.infoLb.Text = "Initializing...";
            // 
            // appNameLbl
            // 
            this.appNameLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.appNameLbl.BackColor = System.Drawing.Color.Transparent;
            this.appNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.appNameLbl.ForeColor = System.Drawing.Color.White;
            this.appNameLbl.Location = new System.Drawing.Point(10, 157);
            this.appNameLbl.Name = "appNameLbl";
            this.appNameLbl.Size = new System.Drawing.Size(360, 24);
            this.appNameLbl.TabIndex = 1;
            this.appNameLbl.Text = "GMView";
            this.appNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rendererLbl
            // 
            this.rendererLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rendererLbl.BackColor = System.Drawing.Color.Transparent;
            this.rendererLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rendererLbl.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.rendererLbl.Location = new System.Drawing.Point(7, 181);
            this.rendererLbl.Name = "rendererLbl";
            this.rendererLbl.Size = new System.Drawing.Size(363, 16);
            this.rendererLbl.TabIndex = 2;
            this.rendererLbl.Text = "visual renderer";
            this.rendererLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IntroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GMView.Properties.Resources.intro;
            this.ClientSize = new System.Drawing.Size(382, 255);
            this.Controls.Add(this.rendererLbl);
            this.Controls.Add(this.appNameLbl);
            this.Controls.Add(this.infoLb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IntroForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IntroForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label infoLb;
        private System.Windows.Forms.Label appNameLbl;
        private System.Windows.Forms.Label rendererLbl;
    }
}