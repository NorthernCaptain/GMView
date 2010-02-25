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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoLb
            // 
            this.infoLb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLb.BackColor = System.Drawing.Color.Transparent;
            this.infoLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoLb.ForeColor = System.Drawing.Color.Yellow;
            this.infoLb.Location = new System.Drawing.Point(4, 226);
            this.infoLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.infoLb.Name = "infoLb";
            this.infoLb.Size = new System.Drawing.Size(374, 30);
            this.infoLb.TabIndex = 0;
            this.infoLb.Text = "Initializing...";
            this.infoLb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // appNameLbl
            // 
            this.appNameLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.appNameLbl.BackColor = System.Drawing.Color.Transparent;
            this.appNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.appNameLbl.ForeColor = System.Drawing.Color.White;
            this.appNameLbl.Location = new System.Drawing.Point(4, 161);
            this.appNameLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.appNameLbl.Name = "appNameLbl";
            this.appNameLbl.Size = new System.Drawing.Size(374, 35);
            this.appNameLbl.TabIndex = 1;
            this.appNameLbl.Text = "GMView";
            this.appNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rendererLbl
            // 
            this.rendererLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rendererLbl.BackColor = System.Drawing.Color.Transparent;
            this.rendererLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rendererLbl.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.rendererLbl.Location = new System.Drawing.Point(4, 196);
            this.rendererLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rendererLbl.Name = "rendererLbl";
            this.rendererLbl.Size = new System.Drawing.Size(374, 30);
            this.rendererLbl.TabIndex = 2;
            this.rendererLbl.Text = "visual renderer";
            this.rendererLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.appNameLbl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.infoLb, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.rendererLbl, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(382, 256);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // IntroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GMView.Properties.Resources.intro;
            this.ClientSize = new System.Drawing.Size(382, 256);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "IntroForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IntroForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label infoLb;
        private System.Windows.Forms.Label appNameLbl;
        private System.Windows.Forms.Label rendererLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}