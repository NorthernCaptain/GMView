namespace GMView.Forms
{
    partial class TrackDestFrom
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
            this.startLocCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.destLocCB = new System.Windows.Forms.ComboBox();
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.autoBut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startLocCB
            // 
            this.startLocCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.startLocCB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.startLocCB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.startLocCB.FormattingEnabled = true;
            this.startLocCB.Location = new System.Drawing.Point(85, 10);
            this.startLocCB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.startLocCB.Name = "startLocCB";
            this.startLocCB.Size = new System.Drawing.Size(166, 21);
            this.startLocCB.TabIndex = 1;
            this.startLocCB.TextUpdate += new System.EventHandler(this.startLocCB_TextUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start location:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Destination:";
            // 
            // destLocCB
            // 
            this.destLocCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.destLocCB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.destLocCB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.destLocCB.FormattingEnabled = true;
            this.destLocCB.Location = new System.Drawing.Point(85, 34);
            this.destLocCB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.destLocCB.Name = "destLocCB";
            this.destLocCB.Size = new System.Drawing.Size(166, 21);
            this.destLocCB.TabIndex = 3;
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(194, 63);
            this.cancelBut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(56, 21);
            this.cancelBut.TabIndex = 5;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBut.Location = new System.Drawing.Point(133, 63);
            this.okBut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(56, 21);
            this.okBut.TabIndex = 4;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            // 
            // autoBut
            // 
            this.autoBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoBut.Location = new System.Drawing.Point(9, 63);
            this.autoBut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.autoBut.Name = "autoBut";
            this.autoBut.Size = new System.Drawing.Size(56, 21);
            this.autoBut.TabIndex = 6;
            this.autoBut.Text = "Auto";
            this.autoBut.UseVisualStyleBackColor = true;
            // 
            // TrackDestFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(259, 93);
            this.Controls.Add(this.autoBut);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.destLocCB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startLocCB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrackDestFrom";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Recording track";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox startLocCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox destLocCB;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Button autoBut;
    }
}