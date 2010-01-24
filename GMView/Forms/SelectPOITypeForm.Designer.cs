namespace GMView.Forms
{
    partial class SelectPOITypeForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.poiTypeListBox = new GMView.Forms.POITypeListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.poiTypeListBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 227);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "POI Types:";
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(222, 245);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(75, 27);
            this.cancelBut.TabIndex = 1;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBut.Location = new System.Drawing.Point(141, 245);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(75, 27);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            // 
            // poiTypeListBox
            // 
            this.poiTypeListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poiTypeListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.poiTypeListBox.FormattingEnabled = true;
            this.poiTypeListBox.ItemHeight = 40;
            this.poiTypeListBox.Location = new System.Drawing.Point(3, 18);
            this.poiTypeListBox.Name = "poiTypeListBox";
            this.poiTypeListBox.Size = new System.Drawing.Size(279, 204);
            this.poiTypeListBox.TabIndex = 0;
            // 
            // SelectPOITypeForm
            // 
            this.AcceptButton = this.okBut;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(309, 284);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectPOITypeForm";
            this.Text = "Select POI type";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private POITypeListBox poiTypeListBox;
    }
}