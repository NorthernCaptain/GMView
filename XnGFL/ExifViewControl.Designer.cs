namespace XnGFL
{
    partial class ExifViewControl
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
            this.dirTBox = new System.Windows.Forms.TextBox();
            this.openDirBut = new System.Windows.Forms.Button();
            this.filesGBox = new System.Windows.Forms.GroupBox();
            this.dirView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.gpsTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.needGPSCb = new System.Windows.Forms.CheckBox();
            this.bookmarkCB = new System.Windows.Forms.ComboBox();
            this.assignManualBut = new System.Windows.Forms.Button();
            this.manualLonTBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.manualLatTBox = new System.Windows.Forms.TextBox();
            this.needShotDateCB = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.manualDatePicker = new System.Windows.Forms.DateTimePicker();
            this.trackPage = new System.Windows.Forms.TabPage();
            this.needDeltaTimeCB = new System.Windows.Forms.CheckBox();
            this.assignGPSBut = new System.Windows.Forms.Button();
            this.hourSetCB = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.deltaTimeTbox = new System.Windows.Forms.MaskedTextBox();
            this.shotDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.gpsDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.trackListCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.setDeltaFromGPSBut = new System.Windows.Forms.Button();
            this.batchProgBar = new System.Windows.Forms.ProgressBar();
            this.progressLbl = new System.Windows.Forms.Label();
            this.applyFilesBut = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cancelShedBut = new System.Windows.Forms.Button();
            this.filesGBox.SuspendLayout();
            this.gpsTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.trackPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // dirTBox
            // 
            this.dirTBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dirTBox.Location = new System.Drawing.Point(32, 3);
            this.dirTBox.Name = "dirTBox";
            this.dirTBox.Size = new System.Drawing.Size(355, 20);
            this.dirTBox.TabIndex = 0;
            // 
            // openDirBut
            // 
            this.openDirBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openDirBut.Location = new System.Drawing.Point(393, 3);
            this.openDirBut.Name = "openDirBut";
            this.openDirBut.Size = new System.Drawing.Size(24, 20);
            this.openDirBut.TabIndex = 1;
            this.openDirBut.Text = "...";
            this.openDirBut.UseVisualStyleBackColor = true;
            this.openDirBut.Click += new System.EventHandler(this.openDirBut_Click);
            // 
            // filesGBox
            // 
            this.filesGBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filesGBox.Controls.Add(this.dirView);
            this.filesGBox.Location = new System.Drawing.Point(3, 171);
            this.filesGBox.Name = "filesGBox";
            this.filesGBox.Size = new System.Drawing.Size(420, 338);
            this.filesGBox.TabIndex = 2;
            this.filesGBox.TabStop = false;
            this.filesGBox.Text = "Files:";
            // 
            // dirView
            // 
            this.dirView.BackColor = System.Drawing.Color.Black;
            this.dirView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dirView.ForeColor = System.Drawing.Color.White;
            this.dirView.Location = new System.Drawing.Point(3, 16);
            this.dirView.Name = "dirView";
            this.dirView.OwnerDraw = true;
            this.dirView.Size = new System.Drawing.Size(414, 319);
            this.dirView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.dirView.TabIndex = 0;
            this.dirView.TileSize = new System.Drawing.Size(400, 120);
            this.dirView.UseCompatibleStateImageBehavior = false;
            this.dirView.View = System.Windows.Forms.View.Tile;
            this.dirView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.dirView_DrawItem);
            this.dirView.DoubleClick += new System.EventHandler(this.dirView_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dir:";
            // 
            // gpsTab
            // 
            this.gpsTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gpsTab.Controls.Add(this.tabPage1);
            this.gpsTab.Controls.Add(this.trackPage);
            this.gpsTab.ItemSize = new System.Drawing.Size(58, 14);
            this.gpsTab.Location = new System.Drawing.Point(6, 29);
            this.gpsTab.Name = "gpsTab";
            this.gpsTab.SelectedIndex = 0;
            this.gpsTab.Size = new System.Drawing.Size(411, 105);
            this.gpsTab.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.needGPSCb);
            this.tabPage1.Controls.Add(this.bookmarkCB);
            this.tabPage1.Controls.Add(this.assignManualBut);
            this.tabPage1.Controls.Add(this.manualLonTBox);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.manualLatTBox);
            this.tabPage1.Controls.Add(this.needShotDateCB);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.manualDatePicker);
            this.tabPage1.Location = new System.Drawing.Point(4, 18);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(403, 83);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Manual";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // needGPSCb
            // 
            this.needGPSCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.needGPSCb.AutoSize = true;
            this.needGPSCb.Checked = true;
            this.needGPSCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.needGPSCb.Location = new System.Drawing.Point(382, 34);
            this.needGPSCb.Name = "needGPSCb";
            this.needGPSCb.Size = new System.Drawing.Size(15, 14);
            this.needGPSCb.TabIndex = 8;
            this.needGPSCb.UseVisualStyleBackColor = true;
            // 
            // bookmarkCB
            // 
            this.bookmarkCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bookmarkCB.DropDownWidth = 250;
            this.bookmarkCB.FormattingEnabled = true;
            this.bookmarkCB.Location = new System.Drawing.Point(204, 31);
            this.bookmarkCB.MaxDropDownItems = 15;
            this.bookmarkCB.Name = "bookmarkCB";
            this.bookmarkCB.Size = new System.Drawing.Size(172, 21);
            this.bookmarkCB.TabIndex = 12;
            this.bookmarkCB.SelectionChangeCommitted += new System.EventHandler(this.bookmarkCB_SelectionChangeCommitted);
            // 
            // assignManualBut
            // 
            this.assignManualBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.assignManualBut.Location = new System.Drawing.Point(322, 57);
            this.assignManualBut.Name = "assignManualBut";
            this.assignManualBut.Size = new System.Drawing.Size(75, 23);
            this.assignManualBut.TabIndex = 11;
            this.assignManualBut.Text = "Assign";
            this.assignManualBut.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.assignManualBut.UseVisualStyleBackColor = true;
            this.assignManualBut.Click += new System.EventHandler(this.assignManualBut_Click);
            // 
            // manualLonTBox
            // 
            this.manualLonTBox.Location = new System.Drawing.Point(68, 55);
            this.manualLonTBox.Name = "manualLonTBox";
            this.manualLonTBox.Size = new System.Drawing.Size(130, 20);
            this.manualLonTBox.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Longitude:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Latitude:";
            // 
            // manualLatTBox
            // 
            this.manualLatTBox.Location = new System.Drawing.Point(68, 32);
            this.manualLatTBox.Name = "manualLatTBox";
            this.manualLatTBox.Size = new System.Drawing.Size(130, 20);
            this.manualLatTBox.TabIndex = 3;
            // 
            // needShotDateCB
            // 
            this.needShotDateCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.needShotDateCB.AutoSize = true;
            this.needShotDateCB.Location = new System.Drawing.Point(382, 10);
            this.needShotDateCB.Name = "needShotDateCB";
            this.needShotDateCB.Size = new System.Drawing.Size(15, 14);
            this.needShotDateCB.TabIndex = 2;
            this.needShotDateCB.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Shot date:";
            // 
            // manualDatePicker
            // 
            this.manualDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.manualDatePicker.CustomFormat = "dd MMMM yyyy HH:mm:ss";
            this.manualDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.manualDatePicker.Location = new System.Drawing.Point(68, 6);
            this.manualDatePicker.Name = "manualDatePicker";
            this.manualDatePicker.Size = new System.Drawing.Size(308, 20);
            this.manualDatePicker.TabIndex = 0;
            // 
            // trackPage
            // 
            this.trackPage.Controls.Add(this.needDeltaTimeCB);
            this.trackPage.Controls.Add(this.assignGPSBut);
            this.trackPage.Controls.Add(this.hourSetCB);
            this.trackPage.Controls.Add(this.label9);
            this.trackPage.Controls.Add(this.deltaTimeTbox);
            this.trackPage.Controls.Add(this.shotDatePicker);
            this.trackPage.Controls.Add(this.label8);
            this.trackPage.Controls.Add(this.gpsDatePicker);
            this.trackPage.Controls.Add(this.label7);
            this.trackPage.Controls.Add(this.trackListCB);
            this.trackPage.Controls.Add(this.label2);
            this.trackPage.Controls.Add(this.setDeltaFromGPSBut);
            this.trackPage.Location = new System.Drawing.Point(4, 18);
            this.trackPage.Name = "trackPage";
            this.trackPage.Padding = new System.Windows.Forms.Padding(3);
            this.trackPage.Size = new System.Drawing.Size(403, 83);
            this.trackPage.TabIndex = 1;
            this.trackPage.Text = "Track";
            this.trackPage.UseVisualStyleBackColor = true;
            // 
            // needDeltaTimeCB
            // 
            this.needDeltaTimeCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.needDeltaTimeCB.AutoSize = true;
            this.needDeltaTimeCB.Location = new System.Drawing.Point(382, 9);
            this.needDeltaTimeCB.Name = "needDeltaTimeCB";
            this.needDeltaTimeCB.Size = new System.Drawing.Size(15, 14);
            this.needDeltaTimeCB.TabIndex = 11;
            this.needDeltaTimeCB.UseVisualStyleBackColor = true;
            // 
            // assignGPSBut
            // 
            this.assignGPSBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.assignGPSBut.Location = new System.Drawing.Point(322, 57);
            this.assignGPSBut.Name = "assignGPSBut";
            this.assignGPSBut.Size = new System.Drawing.Size(75, 23);
            this.assignGPSBut.TabIndex = 10;
            this.assignGPSBut.Text = "Assign";
            this.assignGPSBut.UseVisualStyleBackColor = true;
            this.assignGPSBut.Click += new System.EventHandler(this.assignGPSBut_Click);
            // 
            // hourSetCB
            // 
            this.hourSetCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hourSetCB.FormattingEnabled = true;
            this.hourSetCB.Items.AddRange(new object[] {
            "-8 h",
            "-7 h",
            "-6 h",
            "-5 h",
            "-4 h",
            "-3 h",
            "-2 h",
            "-1 h",
            "00 h",
            "+1 h",
            "+2 h",
            "+3 h",
            "+4 h",
            "+5 h",
            "+6 h",
            "+7 h",
            "+8 h"});
            this.hourSetCB.Location = new System.Drawing.Point(351, 30);
            this.hourSetCB.MaxDropDownItems = 12;
            this.hourSetCB.Name = "hourSetCB";
            this.hourSetCB.Size = new System.Drawing.Size(46, 21);
            this.hourSetCB.TabIndex = 9;
            this.hourSetCB.Text = "00 h";
            this.hourSetCB.SelectedIndexChanged += new System.EventHandler(this.hourSetCB_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(273, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Time delta:";
            // 
            // deltaTimeTbox
            // 
            this.deltaTimeTbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deltaTimeTbox.Culture = new System.Globalization.CultureInfo("");
            this.deltaTimeTbox.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.deltaTimeTbox.Location = new System.Drawing.Point(276, 30);
            this.deltaTimeTbox.Mask = "#99.00:00:00";
            this.deltaTimeTbox.Name = "deltaTimeTbox";
            this.deltaTimeTbox.Size = new System.Drawing.Size(71, 20);
            this.deltaTimeTbox.TabIndex = 6;
            this.deltaTimeTbox.Text = "000000000";
            this.deltaTimeTbox.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.deltaTimeTbox_TypeValidationCompleted);
            // 
            // shotDatePicker
            // 
            this.shotDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shotDatePicker.CustomFormat = "dd MMMM yyyy HH:mm:ss";
            this.shotDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.shotDatePicker.Location = new System.Drawing.Point(70, 52);
            this.shotDatePicker.Name = "shotDatePicker";
            this.shotDatePicker.Size = new System.Drawing.Size(163, 20);
            this.shotDatePicker.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Shot Date:";
            // 
            // gpsDatePicker
            // 
            this.gpsDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gpsDatePicker.CustomFormat = "dd MMMM yyyy HH:mm:ss";
            this.gpsDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.gpsDatePicker.Location = new System.Drawing.Point(70, 29);
            this.gpsDatePicker.Name = "gpsDatePicker";
            this.gpsDatePicker.Size = new System.Drawing.Size(163, 20);
            this.gpsDatePicker.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "GPS Date:";
            // 
            // trackListCB
            // 
            this.trackListCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackListCB.FormattingEnabled = true;
            this.trackListCB.Location = new System.Drawing.Point(70, 6);
            this.trackListCB.Name = "trackListCB";
            this.trackListCB.Size = new System.Drawing.Size(200, 21);
            this.trackListCB.TabIndex = 1;
            this.trackListCB.SelectionChangeCommitted += new System.EventHandler(this.trackListCB_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Track:";
            // 
            // setDeltaFromGPSBut
            // 
            this.setDeltaFromGPSBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setDeltaFromGPSBut.Image = global::XnGFL.Properties.Resources.gps_on;
            this.setDeltaFromGPSBut.Location = new System.Drawing.Point(239, 29);
            this.setDeltaFromGPSBut.Name = "setDeltaFromGPSBut";
            this.setDeltaFromGPSBut.Size = new System.Drawing.Size(31, 22);
            this.setDeltaFromGPSBut.TabIndex = 8;
            this.setDeltaFromGPSBut.UseVisualStyleBackColor = true;
            this.setDeltaFromGPSBut.Click += new System.EventHandler(this.setDeltaFromGPSBut_Click);
            // 
            // batchProgBar
            // 
            this.batchProgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.batchProgBar.Location = new System.Drawing.Point(6, 140);
            this.batchProgBar.Name = "batchProgBar";
            this.batchProgBar.Size = new System.Drawing.Size(349, 10);
            this.batchProgBar.TabIndex = 4;
            // 
            // progressLbl
            // 
            this.progressLbl.AutoSize = true;
            this.progressLbl.Location = new System.Drawing.Point(7, 148);
            this.progressLbl.Name = "progressLbl";
            this.progressLbl.Size = new System.Drawing.Size(0, 13);
            this.progressLbl.TabIndex = 6;
            // 
            // applyFilesBut
            // 
            this.applyFilesBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.applyFilesBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.applyFilesBut.Image = global::XnGFL.Properties.Resources.stock_download;
            this.applyFilesBut.Location = new System.Drawing.Point(361, 140);
            this.applyFilesBut.Name = "applyFilesBut";
            this.applyFilesBut.Size = new System.Drawing.Size(25, 25);
            this.applyFilesBut.TabIndex = 5;
            this.applyFilesBut.UseVisualStyleBackColor = true;
            this.applyFilesBut.Click += new System.EventHandler(this.applyFilesBut_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Title = "Select directory with images";
            // 
            // cancelShedBut
            // 
            this.cancelShedBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelShedBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelShedBut.Image = global::XnGFL.Properties.Resources.gtk_no;
            this.cancelShedBut.Location = new System.Drawing.Point(388, 140);
            this.cancelShedBut.Name = "cancelShedBut";
            this.cancelShedBut.Size = new System.Drawing.Size(25, 25);
            this.cancelShedBut.TabIndex = 7;
            this.cancelShedBut.UseVisualStyleBackColor = true;
            this.cancelShedBut.Click += new System.EventHandler(this.cancelShedBut_Click);
            // 
            // ExifViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancelShedBut);
            this.Controls.Add(this.progressLbl);
            this.Controls.Add(this.applyFilesBut);
            this.Controls.Add(this.batchProgBar);
            this.Controls.Add(this.gpsTab);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filesGBox);
            this.Controls.Add(this.openDirBut);
            this.Controls.Add(this.dirTBox);
            this.DoubleBuffered = true;
            this.Name = "ExifViewControl";
            this.Size = new System.Drawing.Size(423, 512);
            this.filesGBox.ResumeLayout(false);
            this.gpsTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.trackPage.ResumeLayout(false);
            this.trackPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dirTBox;
        private System.Windows.Forms.Button openDirBut;
        private System.Windows.Forms.GroupBox filesGBox;
        private System.Windows.Forms.ListView dirView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TabControl gpsTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage trackPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker manualDatePicker;
        private System.Windows.Forms.ComboBox trackListCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox needShotDateCB;
        private System.Windows.Forms.TextBox manualLonTBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox manualLatTBox;
        private System.Windows.Forms.ProgressBar batchProgBar;
        private System.Windows.Forms.Button applyFilesBut;
        private System.Windows.Forms.Label progressLbl;
        private System.Windows.Forms.DateTimePicker gpsDatePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox deltaTimeTbox;
        private System.Windows.Forms.DateTimePicker shotDatePicker;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button setDeltaFromGPSBut;
        private System.Windows.Forms.ComboBox hourSetCB;
        private System.Windows.Forms.Button assignGPSBut;
        private System.Windows.Forms.Button assignManualBut;
        private System.Windows.Forms.ComboBox bookmarkCB;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button cancelShedBut;
        private System.Windows.Forms.CheckBox needGPSCb;
        private System.Windows.Forms.CheckBox needDeltaTimeCB;
    }
}
