namespace GMView.Forms
{
    partial class QuickPOIForm
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
            this.poiTypeList = new GMView.Forms.POITypeList();
            this.SuspendLayout();
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBut.Image = global::GMView.Properties.Resources.lamp_on;
            this.okBut.Location = new System.Drawing.Point(62, 390);
            this.okBut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(70, 27);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Image = global::GMView.Properties.Resources.lamp_off;
            this.cancelBut.Location = new System.Drawing.Point(136, 390);
            this.cancelBut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(74, 27);
            this.cancelBut.TabIndex = 2;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // poiTypeList
            // 
            this.poiTypeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.poiTypeList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.poiTypeList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.poiTypeList.FormattingEnabled = true;
            this.poiTypeList.ItemHeight = 40;
            this.poiTypeList.Location = new System.Drawing.Point(9, 10);
            this.poiTypeList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.poiTypeList.Name = "poiTypeList";
            this.poiTypeList.Size = new System.Drawing.Size(202, 364);
            this.poiTypeList.TabIndex = 0;
            // 
            // QuickPOIForm
            // 
            this.AcceptButton = this.okBut;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(219, 427);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.poiTypeList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickPOIForm";
            this.ShowIcon = false;
            this.Text = "Choose POI Type";
            this.ResumeLayout(false);

        }

        #endregion

        private POITypeList poiTypeList;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Button cancelBut;
    }
}