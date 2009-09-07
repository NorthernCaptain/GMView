﻿namespace GMView
{
    partial class DownloadQueryForm
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
            this.needTerrainCb = new System.Windows.Forms.CheckBox();
            this.needStreetCb = new System.Windows.Forms.CheckBox();
            this.needSatCb = new System.Windows.Forms.CheckBox();
            this.needMapCb = new System.Windows.Forms.CheckBox();
            this.zoomCheckList = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toLatNT = new System.Windows.Forms.NumericUpDown();
            this.toLonNT = new System.Windows.Forms.NumericUpDown();
            this.fromLatNT = new System.Windows.Forms.NumericUpDown();
            this.fromLonNT = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numTilesLb = new System.Windows.Forms.Label();
            this.sizeLb = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.antiBanSecNT = new System.Windows.Forms.NumericUpDown();
            this.antiBanTilesNT = new System.Windows.Forms.NumericUpDown();
            this.copyToBut = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toLatNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLonNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLatNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLonNT)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.antiBanSecNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.antiBanTilesNT)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.needTerrainCb);
            this.groupBox1.Controls.Add(this.needStreetCb);
            this.groupBox1.Controls.Add(this.needSatCb);
            this.groupBox1.Controls.Add(this.needMapCb);
            this.groupBox1.Location = new System.Drawing.Point(160, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Type:";
            // 
            // needTerrainCb
            // 
            this.needTerrainCb.AutoSize = true;
            this.needTerrainCb.Location = new System.Drawing.Point(6, 88);
            this.needTerrainCb.Name = "needTerrainCb";
            this.needTerrainCb.Size = new System.Drawing.Size(59, 17);
            this.needTerrainCb.TabIndex = 4;
            this.needTerrainCb.Text = "Terrain";
            this.needTerrainCb.UseVisualStyleBackColor = true;
            this.needTerrainCb.CheckedChanged += new System.EventHandler(this.needTerrainCb_CheckedChanged);
            // 
            // needStreetCb
            // 
            this.needStreetCb.AutoSize = true;
            this.needStreetCb.Location = new System.Drawing.Point(6, 65);
            this.needStreetCb.Name = "needStreetCb";
            this.needStreetCb.Size = new System.Drawing.Size(81, 17);
            this.needStreetCb.TabIndex = 3;
            this.needStreetCb.Text = "Sat. Streets";
            this.needStreetCb.UseVisualStyleBackColor = true;
            this.needStreetCb.CheckedChanged += new System.EventHandler(this.needStreetCb_CheckedChanged);
            // 
            // needSatCb
            // 
            this.needSatCb.AutoSize = true;
            this.needSatCb.Location = new System.Drawing.Point(6, 42);
            this.needSatCb.Name = "needSatCb";
            this.needSatCb.Size = new System.Drawing.Size(63, 17);
            this.needSatCb.TabIndex = 2;
            this.needSatCb.Text = "Satellite";
            this.needSatCb.UseVisualStyleBackColor = true;
            this.needSatCb.CheckedChanged += new System.EventHandler(this.needSatCb_CheckedChanged);
            // 
            // needMapCb
            // 
            this.needMapCb.AutoSize = true;
            this.needMapCb.Location = new System.Drawing.Point(6, 19);
            this.needMapCb.Name = "needMapCb";
            this.needMapCb.Size = new System.Drawing.Size(47, 17);
            this.needMapCb.TabIndex = 1;
            this.needMapCb.Text = "Map";
            this.needMapCb.UseVisualStyleBackColor = true;
            this.needMapCb.CheckedChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // zoomCheckList
            // 
            this.zoomCheckList.FormattingEnabled = true;
            this.zoomCheckList.Items.AddRange(new object[] {
            "18 (Largest)",
            "17",
            "16",
            "15",
            "14",
            "13",
            "12",
            "11",
            "10",
            "9",
            "8",
            "7",
            "6",
            "5 (smalest)"});
            this.zoomCheckList.Location = new System.Drawing.Point(6, 13);
            this.zoomCheckList.Name = "zoomCheckList";
            this.zoomCheckList.Size = new System.Drawing.Size(90, 214);
            this.zoomCheckList.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.zoomCheckList);
            this.groupBox2.Location = new System.Drawing.Point(160, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(103, 233);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zoom levels:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.toLatNT);
            this.groupBox3.Controls.Add(this.toLonNT);
            this.groupBox3.Controls.Add(this.fromLatNT);
            this.groupBox3.Controls.Add(this.fromLonNT);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 123);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Area:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "To Lat:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "To Lon:";
            // 
            // toLatNT
            // 
            this.toLatNT.DecimalPlaces = 8;
            this.toLatNT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.toLatNT.Location = new System.Drawing.Point(66, 91);
            this.toLatNT.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.toLatNT.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.toLatNT.Name = "toLatNT";
            this.toLatNT.Size = new System.Drawing.Size(78, 20);
            this.toLatNT.TabIndex = 8;
            // 
            // toLonNT
            // 
            this.toLonNT.DecimalPlaces = 8;
            this.toLonNT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.toLonNT.Location = new System.Drawing.Point(66, 65);
            this.toLonNT.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.toLonNT.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.toLonNT.Name = "toLonNT";
            this.toLonNT.Size = new System.Drawing.Size(78, 20);
            this.toLonNT.TabIndex = 7;
            // 
            // fromLatNT
            // 
            this.fromLatNT.DecimalPlaces = 8;
            this.fromLatNT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.fromLatNT.Location = new System.Drawing.Point(66, 39);
            this.fromLatNT.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.fromLatNT.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.fromLatNT.Name = "fromLatNT";
            this.fromLatNT.Size = new System.Drawing.Size(78, 20);
            this.fromLatNT.TabIndex = 6;
            // 
            // fromLonNT
            // 
            this.fromLonNT.DecimalPlaces = 8;
            this.fromLonNT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.fromLonNT.Location = new System.Drawing.Point(66, 14);
            this.fromLonNT.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.fromLonNT.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.fromLonNT.Name = "fromLonNT";
            this.fromLonNT.Size = new System.Drawing.Size(78, 20);
            this.fromLonNT.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "From Lat:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "From Lon:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numTilesLb);
            this.groupBox4.Controls.Add(this.sizeLb);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(3, 214);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(151, 76);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Result:";
            // 
            // numTilesLb
            // 
            this.numTilesLb.AutoSize = true;
            this.numTilesLb.Location = new System.Drawing.Point(95, 25);
            this.numTilesLb.Name = "numTilesLb";
            this.numTilesLb.Size = new System.Drawing.Size(13, 13);
            this.numTilesLb.TabIndex = 6;
            this.numTilesLb.Text = "0";
            // 
            // sizeLb
            // 
            this.sizeLb.AutoSize = true;
            this.sizeLb.Location = new System.Drawing.Point(95, 48);
            this.sizeLb.Name = "sizeLb";
            this.sizeLb.Size = new System.Drawing.Size(13, 13);
            this.sizeLb.TabIndex = 7;
            this.sizeLb.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Aprox Size (kb):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Number of tiles:";
            // 
            // cancelBut
            // 
            this.cancelBut.Location = new System.Drawing.Point(12, 364);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(135, 28);
            this.cancelBut.TabIndex = 4;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            this.cancelBut.Click += new System.EventHandler(this.cancelBut_Click);
            // 
            // okBut
            // 
            this.okBut.Location = new System.Drawing.Point(12, 296);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(135, 28);
            this.okBut.TabIndex = 5;
            this.okBut.Text = "Start download";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.antiBanSecNT);
            this.groupBox5.Controls.Add(this.antiBanTilesNT);
            this.groupBox5.Location = new System.Drawing.Point(3, 132);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(151, 76);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "AntiBan:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Wait sec:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Num tiles:";
            // 
            // antiBanSecNT
            // 
            this.antiBanSecNT.Location = new System.Drawing.Point(66, 40);
            this.antiBanSecNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.antiBanSecNT.Name = "antiBanSecNT";
            this.antiBanSecNT.Size = new System.Drawing.Size(78, 20);
            this.antiBanSecNT.TabIndex = 3;
            this.antiBanSecNT.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // antiBanTilesNT
            // 
            this.antiBanTilesNT.Location = new System.Drawing.Point(66, 19);
            this.antiBanTilesNT.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.antiBanTilesNT.Name = "antiBanTilesNT";
            this.antiBanTilesNT.Size = new System.Drawing.Size(78, 20);
            this.antiBanTilesNT.TabIndex = 2;
            this.antiBanTilesNT.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            // 
            // copyToBut
            // 
            this.copyToBut.Location = new System.Drawing.Point(12, 330);
            this.copyToBut.Name = "copyToBut";
            this.copyToBut.Size = new System.Drawing.Size(135, 28);
            this.copyToBut.TabIndex = 5;
            this.copyToBut.Text = "Copy to folder";
            this.copyToBut.UseVisualStyleBackColor = true;
            this.copyToBut.Click += new System.EventHandler(this.copyToBut_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Choose a folder to copy map images to.";
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // DownloadQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 403);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.copyToBut);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadQueryForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Download parameters";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toLatNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLonNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLatNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLonNT)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.antiBanSecNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.antiBanTilesNT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox needTerrainCb;
        private System.Windows.Forms.CheckBox needStreetCb;
        private System.Windows.Forms.CheckBox needSatCb;
        private System.Windows.Forms.CheckBox needMapCb;
        private System.Windows.Forms.CheckedListBox zoomCheckList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown toLatNT;
        private System.Windows.Forms.NumericUpDown toLonNT;
        private System.Windows.Forms.NumericUpDown fromLatNT;
        private System.Windows.Forms.NumericUpDown fromLonNT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Label sizeLb;
        private System.Windows.Forms.Label numTilesLb;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown antiBanTilesNT;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown antiBanSecNT;
        private System.Windows.Forms.Button copyToBut;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}