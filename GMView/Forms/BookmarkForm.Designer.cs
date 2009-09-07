namespace GMView.Forms
{
    partial class BookmarkForm
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
            this.groupCB = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pinCombo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.commentTb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nameTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.okBut = new System.Windows.Forms.Button();
            this.cancelBut = new System.Windows.Forms.Button();
            this.latBox = new System.Windows.Forms.TextBox();
            this.lonBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lonBox);
            this.groupBox1.Controls.Add(this.latBox);
            this.groupBox1.Controls.Add(this.groupCB);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.pinCombo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.commentTb);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nameTb);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 238);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main info";
            // 
            // groupCB
            // 
            this.groupCB.FormattingEnabled = true;
            this.groupCB.Location = new System.Drawing.Point(50, 119);
            this.groupCB.Name = "groupCB";
            this.groupCB.Size = new System.Drawing.Size(157, 21);
            this.groupCB.Sorted = true;
            this.groupCB.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Group:";
            // 
            // pinCombo
            // 
            this.pinCombo.FormattingEnabled = true;
            this.pinCombo.Items.AddRange(new object[] {
            "Yellow",
            "Green",
            "Red",
            "Blue"});
            this.pinCombo.Location = new System.Drawing.Point(105, 92);
            this.pinCombo.Name = "pinCombo";
            this.pinCombo.Size = new System.Drawing.Size(102, 21);
            this.pinCombo.TabIndex = 3;
            this.pinCombo.Text = "Yellow";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Pin color:";
            // 
            // commentTb
            // 
            this.commentTb.Location = new System.Drawing.Point(6, 164);
            this.commentTb.Multiline = true;
            this.commentTb.Name = "commentTb";
            this.commentTb.Size = new System.Drawing.Size(198, 68);
            this.commentTb.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Description:";
            // 
            // nameTb
            // 
            this.nameTb.Location = new System.Drawing.Point(50, 16);
            this.nameTb.Name = "nameTb";
            this.nameTb.Size = new System.Drawing.Size(157, 20);
            this.nameTb.TabIndex = 0;
            this.nameTb.Text = "New location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Longitude:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Latitude:";
            // 
            // okBut
            // 
            this.okBut.Location = new System.Drawing.Point(150, 256);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(75, 25);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // cancelBut
            // 
            this.cancelBut.Location = new System.Drawing.Point(69, 256);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(75, 25);
            this.cancelBut.TabIndex = 2;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            this.cancelBut.Click += new System.EventHandler(this.cancelBut_Click);
            // 
            // latBox
            // 
            this.latBox.Location = new System.Drawing.Point(104, 40);
            this.latBox.Name = "latBox";
            this.latBox.Size = new System.Drawing.Size(103, 20);
            this.latBox.TabIndex = 12;
            // 
            // lonBox
            // 
            this.lonBox.Location = new System.Drawing.Point(104, 62);
            this.lonBox.Name = "lonBox";
            this.lonBox.Size = new System.Drawing.Size(103, 20);
            this.lonBox.TabIndex = 12;
            // 
            // BookmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 293);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.groupBox1);
            this.Name = "BookmarkForm";
            this.Text = "New point of interest";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox nameTb;
        private System.Windows.Forms.ComboBox pinCombo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox commentTb;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.ComboBox groupCB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox lonBox;
        private System.Windows.Forms.TextBox latBox;
    }
}