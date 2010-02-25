namespace GMView.Forms
{
    partial class POILoadControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tuneNameCommRb = new System.Windows.Forms.RadioButton();
            this.tuneNoChangesRb = new System.Windows.Forms.RadioButton();
            this.tuneNameDescRb = new System.Windows.Forms.RadioButton();
            this.poiTypeComboBox = new GMView.Forms.POITypeComboBox();
            this.tuneDescCommCB = new System.Windows.Forms.CheckBox();
            this.needPOICB = new System.Windows.Forms.CheckBox();
            this.poisLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tuneNameCommRb);
            this.groupBox1.Controls.Add(this.tuneNoChangesRb);
            this.groupBox1.Controls.Add(this.tuneNameDescRb);
            this.groupBox1.Controls.Add(this.poiTypeComboBox);
            this.groupBox1.Controls.Add(this.tuneDescCommCB);
            this.groupBox1.Controls.Add(this.needPOICB);
            this.groupBox1.Controls.Add(this.poisLbl);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 260);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "POI:";
            // 
            // tuneNameCommRb
            // 
            this.tuneNameCommRb.AutoSize = true;
            this.tuneNameCommRb.Location = new System.Drawing.Point(9, 199);
            this.tuneNameCommRb.Name = "tuneNameCommRb";
            this.tuneNameCommRb.Size = new System.Drawing.Size(164, 21);
            this.tuneNameCommRb.TabIndex = 11;
            this.tuneNameCommRb.Text = "Name <=> Comments";
            this.tuneNameCommRb.UseVisualStyleBackColor = true;
            // 
            // tuneNoChangesRb
            // 
            this.tuneNoChangesRb.AutoSize = true;
            this.tuneNoChangesRb.Checked = true;
            this.tuneNoChangesRb.Location = new System.Drawing.Point(9, 145);
            this.tuneNoChangesRb.Name = "tuneNoChangesRb";
            this.tuneNoChangesRb.Size = new System.Drawing.Size(105, 21);
            this.tuneNoChangesRb.TabIndex = 13;
            this.tuneNoChangesRb.TabStop = true;
            this.tuneNoChangesRb.Text = "No changes";
            this.tuneNoChangesRb.UseVisualStyleBackColor = true;
            // 
            // tuneNameDescRb
            // 
            this.tuneNameDescRb.AutoSize = true;
            this.tuneNameDescRb.Location = new System.Drawing.Point(9, 172);
            this.tuneNameDescRb.Name = "tuneNameDescRb";
            this.tuneNameDescRb.Size = new System.Drawing.Size(169, 21);
            this.tuneNameDescRb.TabIndex = 12;
            this.tuneNameDescRb.Text = "Name <=> Description";
            this.tuneNameDescRb.UseVisualStyleBackColor = true;
            // 
            // poiTypeComboBox
            // 
            this.poiTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.poiTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.poiTypeComboBox.FormattingEnabled = true;
            this.poiTypeComboBox.ItemHeight = 40;
            this.poiTypeComboBox.Location = new System.Drawing.Point(9, 94);
            this.poiTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.poiTypeComboBox.MaxDropDownItems = 10;
            this.poiTypeComboBox.Name = "poiTypeComboBox";
            this.poiTypeComboBox.Size = new System.Drawing.Size(201, 46);
            this.poiTypeComboBox.TabIndex = 10;
            // 
            // tuneDescCommCB
            // 
            this.tuneDescCommCB.AutoSize = true;
            this.tuneDescCommCB.Location = new System.Drawing.Point(9, 225);
            this.tuneDescCommCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tuneDescCommCB.Name = "tuneDescCommCB";
            this.tuneDescCommCB.Size = new System.Drawing.Size(207, 21);
            this.tuneDescCommCB.TabIndex = 8;
            this.tuneDescCommCB.Text = "+Description <=> Comments";
            this.tuneDescCommCB.UseVisualStyleBackColor = true;
            // 
            // needPOICB
            // 
            this.needPOICB.AutoSize = true;
            this.needPOICB.Location = new System.Drawing.Point(9, 20);
            this.needPOICB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.needPOICB.Name = "needPOICB";
            this.needPOICB.Size = new System.Drawing.Size(150, 21);
            this.needPOICB.TabIndex = 6;
            this.needPOICB.Text = "Import POI from file";
            this.needPOICB.UseVisualStyleBackColor = true;
            // 
            // poisLbl
            // 
            this.poisLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.poisLbl.Location = new System.Drawing.Point(121, 47);
            this.poisLbl.Name = "poisLbl";
            this.poisLbl.Size = new System.Drawing.Size(92, 17);
            this.poisLbl.TabIndex = 9;
            this.poisLbl.Text = "0";
            this.poisLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Default POI type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Total POIs:";
            // 
            // POILoadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "POILoadControl";
            this.Size = new System.Drawing.Size(221, 260);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton tuneNameCommRb;
        private System.Windows.Forms.RadioButton tuneNoChangesRb;
        private System.Windows.Forms.RadioButton tuneNameDescRb;
        private POITypeComboBox poiTypeComboBox;
        private System.Windows.Forms.CheckBox tuneDescCommCB;
        private System.Windows.Forms.CheckBox needPOICB;
        private System.Windows.Forms.Label poisLbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
    }
}
