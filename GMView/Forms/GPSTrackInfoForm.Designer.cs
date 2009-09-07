namespace GMView
{
    partial class GPSTrackInfoForm
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
            this.travelTimeLb = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.finishTimeDTP = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.startTimeDTP = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.maxSpeedLb = new System.Windows.Forms.Label();
            this.avgSpeedLb = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.distanceLb = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.closeBut = new System.Windows.Forms.Button();
            this.removeTrackBut = new System.Windows.Forms.Button();
            this.recordBut = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.travelTimeLb);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.finishTimeDTP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.startTimeDTP);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 106);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time";
            // 
            // travelTimeLb
            // 
            this.travelTimeLb.AutoSize = true;
            this.travelTimeLb.BackColor = System.Drawing.SystemColors.Control;
            this.travelTimeLb.Location = new System.Drawing.Point(82, 81);
            this.travelTimeLb.Name = "travelTimeLb";
            this.travelTimeLb.Size = new System.Drawing.Size(49, 13);
            this.travelTimeLb.TabIndex = 6;
            this.travelTimeLb.Text = "00:00:00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Time duration:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Finish time:";
            // 
            // finishTimeDTP
            // 
            this.finishTimeDTP.CustomFormat = "dd MMM yyyy HH:mm:ss";
            this.finishTimeDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.finishTimeDTP.Location = new System.Drawing.Point(85, 45);
            this.finishTimeDTP.Name = "finishTimeDTP";
            this.finishTimeDTP.Size = new System.Drawing.Size(141, 20);
            this.finishTimeDTP.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start time:";
            // 
            // startTimeDTP
            // 
            this.startTimeDTP.CustomFormat = "dd MMM yyyy HH:mm:ss";
            this.startTimeDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTimeDTP.Location = new System.Drawing.Point(85, 19);
            this.startTimeDTP.Name = "startTimeDTP";
            this.startTimeDTP.Size = new System.Drawing.Size(141, 20);
            this.startTimeDTP.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.maxSpeedLb);
            this.groupBox2.Controls.Add(this.avgSpeedLb);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.distanceLb);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 124);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 100);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(192, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "km/h";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(192, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "km/h";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(192, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "km";
            // 
            // maxSpeedLb
            // 
            this.maxSpeedLb.BackColor = System.Drawing.SystemColors.Control;
            this.maxSpeedLb.Location = new System.Drawing.Point(140, 50);
            this.maxSpeedLb.Name = "maxSpeedLb";
            this.maxSpeedLb.Size = new System.Drawing.Size(50, 13);
            this.maxSpeedLb.TabIndex = 11;
            this.maxSpeedLb.Text = "00";
            this.maxSpeedLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // avgSpeedLb
            // 
            this.avgSpeedLb.BackColor = System.Drawing.SystemColors.Control;
            this.avgSpeedLb.Location = new System.Drawing.Point(140, 33);
            this.avgSpeedLb.Name = "avgSpeedLb";
            this.avgSpeedLb.Size = new System.Drawing.Size(50, 13);
            this.avgSpeedLb.TabIndex = 10;
            this.avgSpeedLb.Text = "00:00";
            this.avgSpeedLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(6, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Maximum speed:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(6, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Average speed:";
            // 
            // distanceLb
            // 
            this.distanceLb.BackColor = System.Drawing.SystemColors.Control;
            this.distanceLb.Location = new System.Drawing.Point(140, 16);
            this.distanceLb.Name = "distanceLb";
            this.distanceLb.Size = new System.Drawing.Size(50, 13);
            this.distanceLb.TabIndex = 7;
            this.distanceLb.Text = "00:00:00";
            this.distanceLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Travel distance:";
            // 
            // closeBut
            // 
            this.closeBut.Location = new System.Drawing.Point(173, 230);
            this.closeBut.Name = "closeBut";
            this.closeBut.Size = new System.Drawing.Size(69, 23);
            this.closeBut.TabIndex = 2;
            this.closeBut.Text = "Close";
            this.closeBut.UseVisualStyleBackColor = true;
            this.closeBut.Click += new System.EventHandler(this.closeBut_Click);
            // 
            // removeTrackBut
            // 
            this.removeTrackBut.Location = new System.Drawing.Point(102, 230);
            this.removeTrackBut.Name = "removeTrackBut";
            this.removeTrackBut.Size = new System.Drawing.Size(65, 23);
            this.removeTrackBut.TabIndex = 1;
            this.removeTrackBut.Text = "Remove";
            this.removeTrackBut.UseVisualStyleBackColor = true;
            this.removeTrackBut.Click += new System.EventHandler(this.removeTrackBut_Click);
            // 
            // recordBut
            // 
            this.recordBut.Location = new System.Drawing.Point(12, 230);
            this.recordBut.Name = "recordBut";
            this.recordBut.Size = new System.Drawing.Size(80, 23);
            this.recordBut.TabIndex = 0;
            this.recordBut.Text = "Record";
            this.recordBut.UseVisualStyleBackColor = true;
            this.recordBut.Click += new System.EventHandler(this.recordBut_Click);
            // 
            // GPSTrackInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 263);
            this.Controls.Add(this.recordBut);
            this.Controls.Add(this.removeTrackBut);
            this.Controls.Add(this.closeBut);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GPSTrackInfoForm";
            this.Text = "GPSTrackInfoForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GPSTrackInfoForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker startTimeDTP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker finishTimeDTP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label travelTimeLb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label distanceLb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label maxSpeedLb;
        private System.Windows.Forms.Label avgSpeedLb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button closeBut;
        private System.Windows.Forms.Button removeTrackBut;
        private System.Windows.Forms.Button recordBut;
    }
}