namespace GMView
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
            this.needOSMRendCB = new System.Windows.Forms.CheckBox();
            this.needOSMCB = new System.Windows.Forms.CheckBox();
            this.needYamapCB = new System.Windows.Forms.CheckBox();
            this.needTerrainCb = new System.Windows.Forms.CheckBox();
            this.needStreetCb = new System.Windows.Forms.CheckBox();
            this.needSatCb = new System.Windows.Forms.CheckBox();
            this.needMapCb = new System.Windows.Forms.CheckBox();
            this.zoomCheckList = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.allNoneCB = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.followTrackCB = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nearbyNT = new System.Windows.Forms.NumericUpDown();
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
            this.copyToBut = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.forceDownloadCB = new System.Windows.Forms.CheckBox();
            this.modeLbl = new System.Windows.Forms.Label();
            this.createOziBut = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportTypeCB = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nearbyNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLatNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLonNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLatNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLonNT)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.needOSMRendCB);
            this.groupBox1.Controls.Add(this.needOSMCB);
            this.groupBox1.Controls.Add(this.needYamapCB);
            this.groupBox1.Controls.Add(this.needTerrainCb);
            this.groupBox1.Controls.Add(this.needStreetCb);
            this.groupBox1.Controls.Add(this.needSatCb);
            this.groupBox1.Controls.Add(this.needMapCb);
            this.groupBox1.Location = new System.Drawing.Point(213, 31);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(137, 231);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Type:";
            // 
            // needOSMRendCB
            // 
            this.needOSMRendCB.AutoSize = true;
            this.needOSMRendCB.Location = new System.Drawing.Point(8, 194);
            this.needOSMRendCB.Margin = new System.Windows.Forms.Padding(4);
            this.needOSMRendCB.Name = "needOSMRendCB";
            this.needOSMRendCB.Size = new System.Drawing.Size(125, 21);
            this.needOSMRendCB.TabIndex = 4;
            this.needOSMRendCB.Text = "OSM Renderer";
            this.needOSMRendCB.UseVisualStyleBackColor = true;
            this.needOSMRendCB.CheckedChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // needOSMCB
            // 
            this.needOSMCB.AutoSize = true;
            this.needOSMCB.Location = new System.Drawing.Point(8, 165);
            this.needOSMCB.Margin = new System.Windows.Forms.Padding(4);
            this.needOSMCB.Name = "needOSMCB";
            this.needOSMCB.Size = new System.Drawing.Size(61, 21);
            this.needOSMCB.TabIndex = 4;
            this.needOSMCB.Text = "OSM";
            this.needOSMCB.UseVisualStyleBackColor = true;
            this.needOSMCB.CheckedChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // needYamapCB
            // 
            this.needYamapCB.AutoSize = true;
            this.needYamapCB.Location = new System.Drawing.Point(8, 137);
            this.needYamapCB.Margin = new System.Windows.Forms.Padding(4);
            this.needYamapCB.Name = "needYamapCB";
            this.needYamapCB.Size = new System.Drawing.Size(77, 21);
            this.needYamapCB.TabIndex = 4;
            this.needYamapCB.Text = "Yandex";
            this.needYamapCB.UseVisualStyleBackColor = true;
            this.needYamapCB.CheckedChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // needTerrainCb
            // 
            this.needTerrainCb.AutoSize = true;
            this.needTerrainCb.Location = new System.Drawing.Point(8, 108);
            this.needTerrainCb.Margin = new System.Windows.Forms.Padding(4);
            this.needTerrainCb.Name = "needTerrainCb";
            this.needTerrainCb.Size = new System.Drawing.Size(76, 21);
            this.needTerrainCb.TabIndex = 4;
            this.needTerrainCb.Text = "Terrain";
            this.needTerrainCb.UseVisualStyleBackColor = true;
            this.needTerrainCb.CheckedChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // needStreetCb
            // 
            this.needStreetCb.AutoSize = true;
            this.needStreetCb.Location = new System.Drawing.Point(8, 80);
            this.needStreetCb.Margin = new System.Windows.Forms.Padding(4);
            this.needStreetCb.Name = "needStreetCb";
            this.needStreetCb.Size = new System.Drawing.Size(104, 21);
            this.needStreetCb.TabIndex = 3;
            this.needStreetCb.Text = "Sat. Streets";
            this.needStreetCb.UseVisualStyleBackColor = true;
            this.needStreetCb.CheckedChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // needSatCb
            // 
            this.needSatCb.AutoSize = true;
            this.needSatCb.Location = new System.Drawing.Point(8, 52);
            this.needSatCb.Margin = new System.Windows.Forms.Padding(4);
            this.needSatCb.Name = "needSatCb";
            this.needSatCb.Size = new System.Drawing.Size(80, 21);
            this.needSatCb.TabIndex = 2;
            this.needSatCb.Text = "Satellite";
            this.needSatCb.UseVisualStyleBackColor = true;
            this.needSatCb.CheckedChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // needMapCb
            // 
            this.needMapCb.AutoSize = true;
            this.needMapCb.Location = new System.Drawing.Point(8, 23);
            this.needMapCb.Margin = new System.Windows.Forms.Padding(4);
            this.needMapCb.Name = "needMapCb";
            this.needMapCb.Size = new System.Drawing.Size(57, 21);
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
            "8 (least)"});
            this.zoomCheckList.Location = new System.Drawing.Point(8, 16);
            this.zoomCheckList.Margin = new System.Windows.Forms.Padding(4);
            this.zoomCheckList.Name = "zoomCheckList";
            this.zoomCheckList.Size = new System.Drawing.Size(119, 191);
            this.zoomCheckList.TabIndex = 1;
            this.zoomCheckList.SelectedIndexChanged += new System.EventHandler(this.needMapCb_CheckedChanged);
            this.zoomCheckList.DoubleClick += new System.EventHandler(this.needMapCb_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.allNoneCB);
            this.groupBox2.Controls.Add(this.zoomCheckList);
            this.groupBox2.Location = new System.Drawing.Point(213, 270);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(137, 268);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zoom levels:";
            // 
            // allNoneCB
            // 
            this.allNoneCB.AutoSize = true;
            this.allNoneCB.Checked = true;
            this.allNoneCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allNoneCB.Location = new System.Drawing.Point(9, 234);
            this.allNoneCB.Margin = new System.Windows.Forms.Padding(4);
            this.allNoneCB.Name = "allNoneCB";
            this.allNoneCB.Size = new System.Drawing.Size(83, 21);
            this.allNoneCB.TabIndex = 2;
            this.allNoneCB.Text = "All/None";
            this.allNoneCB.UseVisualStyleBackColor = true;
            this.allNoneCB.CheckedChanged += new System.EventHandler(this.allNoneCB_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.followTrackCB);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.nearbyNT);
            this.groupBox3.Controls.Add(this.toLatNT);
            this.groupBox3.Controls.Add(this.toLonNT);
            this.groupBox3.Controls.Add(this.fromLatNT);
            this.groupBox3.Controls.Add(this.fromLonNT);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(4, 31);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(201, 210);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Area:";
            // 
            // followTrackCB
            // 
            this.followTrackCB.AutoSize = true;
            this.followTrackCB.Enabled = false;
            this.followTrackCB.Location = new System.Drawing.Point(12, 144);
            this.followTrackCB.Margin = new System.Windows.Forms.Padding(4);
            this.followTrackCB.Name = "followTrackCB";
            this.followTrackCB.Size = new System.Drawing.Size(104, 21);
            this.followTrackCB.TabIndex = 11;
            this.followTrackCB.Text = "Follow track";
            this.followTrackCB.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 175);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "Nearby area, km:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 114);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "To Lat:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "To Lon:";
            // 
            // nearbyNT
            // 
            this.nearbyNT.Location = new System.Drawing.Point(131, 172);
            this.nearbyNT.Margin = new System.Windows.Forms.Padding(4);
            this.nearbyNT.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nearbyNT.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nearbyNT.Name = "nearbyNT";
            this.nearbyNT.Size = new System.Drawing.Size(63, 22);
            this.nearbyNT.TabIndex = 8;
            this.nearbyNT.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // toLatNT
            // 
            this.toLatNT.DecimalPlaces = 8;
            this.toLatNT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.toLatNT.Location = new System.Drawing.Point(88, 112);
            this.toLatNT.Margin = new System.Windows.Forms.Padding(4);
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
            this.toLatNT.Size = new System.Drawing.Size(104, 22);
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
            this.toLonNT.Location = new System.Drawing.Point(88, 80);
            this.toLonNT.Margin = new System.Windows.Forms.Padding(4);
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
            this.toLonNT.Size = new System.Drawing.Size(104, 22);
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
            this.fromLatNT.Location = new System.Drawing.Point(88, 48);
            this.fromLatNT.Margin = new System.Windows.Forms.Padding(4);
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
            this.fromLatNT.Size = new System.Drawing.Size(104, 22);
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
            this.fromLonNT.Location = new System.Drawing.Point(88, 17);
            this.fromLonNT.Margin = new System.Windows.Forms.Padding(4);
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
            this.fromLonNT.Size = new System.Drawing.Size(104, 22);
            this.fromLonNT.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "From Lat:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "From Lon:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numTilesLb);
            this.groupBox4.Controls.Add(this.sizeLb);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(4, 247);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(201, 94);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Result:";
            // 
            // numTilesLb
            // 
            this.numTilesLb.AutoSize = true;
            this.numTilesLb.Location = new System.Drawing.Point(127, 31);
            this.numTilesLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.numTilesLb.Name = "numTilesLb";
            this.numTilesLb.Size = new System.Drawing.Size(16, 17);
            this.numTilesLb.TabIndex = 6;
            this.numTilesLb.Text = "0";
            // 
            // sizeLb
            // 
            this.sizeLb.AutoSize = true;
            this.sizeLb.Location = new System.Drawing.Point(127, 59);
            this.sizeLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sizeLb.Name = "sizeLb";
            this.sizeLb.Size = new System.Drawing.Size(16, 17);
            this.sizeLb.TabIndex = 7;
            this.sizeLb.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 59);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Aprox Size (kb):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 31);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Number of tiles:";
            // 
            // cancelBut
            // 
            this.cancelBut.Location = new System.Drawing.Point(15, 504);
            this.cancelBut.Margin = new System.Windows.Forms.Padding(4);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(180, 34);
            this.cancelBut.TabIndex = 4;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            this.cancelBut.Click += new System.EventHandler(this.cancelBut_Click);
            // 
            // okBut
            // 
            this.okBut.Location = new System.Drawing.Point(15, 378);
            this.okBut.Margin = new System.Windows.Forms.Padding(4);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(180, 34);
            this.okBut.TabIndex = 5;
            this.okBut.Text = "Start download";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // copyToBut
            // 
            this.copyToBut.Location = new System.Drawing.Point(15, 420);
            this.copyToBut.Margin = new System.Windows.Forms.Padding(4);
            this.copyToBut.Name = "copyToBut";
            this.copyToBut.Size = new System.Drawing.Size(180, 34);
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
            // forceDownloadCB
            // 
            this.forceDownloadCB.AutoSize = true;
            this.forceDownloadCB.Location = new System.Drawing.Point(19, 349);
            this.forceDownloadCB.Margin = new System.Windows.Forms.Padding(4);
            this.forceDownloadCB.Name = "forceDownloadCB";
            this.forceDownloadCB.Size = new System.Drawing.Size(130, 21);
            this.forceDownloadCB.TabIndex = 6;
            this.forceDownloadCB.Text = "Force download";
            this.forceDownloadCB.UseVisualStyleBackColor = true;
            // 
            // modeLbl
            // 
            this.modeLbl.AutoSize = true;
            this.modeLbl.Location = new System.Drawing.Point(12, 10);
            this.modeLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.modeLbl.Name = "modeLbl";
            this.modeLbl.Size = new System.Drawing.Size(151, 17);
            this.modeLbl.TabIndex = 7;
            this.modeLbl.Text = "Square area download";
            // 
            // createOziBut
            // 
            this.createOziBut.Location = new System.Drawing.Point(15, 462);
            this.createOziBut.Margin = new System.Windows.Forms.Padding(4);
            this.createOziBut.Name = "createOziBut";
            this.createOziBut.Size = new System.Drawing.Size(105, 34);
            this.createOziBut.TabIndex = 5;
            this.createOziBut.Text = "Export to:";
            this.createOziBut.UseVisualStyleBackColor = true;
            this.createOziBut.Click += new System.EventHandler(this.oziImageBut_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "png";
            this.saveFileDialog.FileName = "ozimap.png";
            this.saveFileDialog.Title = "Create OziExplorer Image map";
            // 
            // exportTypeCB
            // 
            this.exportTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.exportTypeCB.DropDownWidth = 130;
            this.exportTypeCB.FormattingEnabled = true;
            this.exportTypeCB.ItemHeight = 16;
            this.exportTypeCB.Items.AddRange(new object[] {
            "Ozi Explorer map",
            "Orux maps (Android)"});
            this.exportTypeCB.Location = new System.Drawing.Point(127, 468);
            this.exportTypeCB.Name = "exportTypeCB";
            this.exportTypeCB.Size = new System.Drawing.Size(68, 24);
            this.exportTypeCB.TabIndex = 3;
            // 
            // DownloadQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 551);
            this.Controls.Add(this.exportTypeCB);
            this.Controls.Add(this.modeLbl);
            this.Controls.Add(this.forceDownloadCB);
            this.Controls.Add(this.createOziBut);
            this.Controls.Add(this.copyToBut);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadQueryForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Download parameters";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nearbyNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLatNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLonNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLatNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromLonNT)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Button copyToBut;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.CheckBox forceDownloadCB;
        private System.Windows.Forms.CheckBox followTrackCB;
        private System.Windows.Forms.CheckBox needYamapCB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nearbyNT;
        private System.Windows.Forms.CheckBox needOSMCB;
        private System.Windows.Forms.Label modeLbl;
        private System.Windows.Forms.CheckBox allNoneCB;
        private System.Windows.Forms.CheckBox needOSMRendCB;
        private System.Windows.Forms.Button createOziBut;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ComboBox exportTypeCB;
    }
}