﻿namespace GMView.Forms
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
            this.startLocCB.Location = new System.Drawing.Point(113, 12);
            this.startLocCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startLocCB.Name = "startLocCB";
            this.startLocCB.Size = new System.Drawing.Size(220, 24);
            this.startLocCB.TabIndex = 1;
            this.startLocCB.SelectionChangeCommitted += new System.EventHandler(this.startLocCB_TextUpdate);
            this.startLocCB.Leave += new System.EventHandler(this.startLocCB_Leave);
            this.startLocCB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.startLocCB_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start location:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
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
            this.destLocCB.Location = new System.Drawing.Point(113, 42);
            this.destLocCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.destLocCB.Name = "destLocCB";
            this.destLocCB.Size = new System.Drawing.Size(220, 24);
            this.destLocCB.TabIndex = 3;
            this.destLocCB.SelectionChangeCommitted += new System.EventHandler(this.destLocCB_SelectionChangeCommitted);
            this.destLocCB.Leave += new System.EventHandler(this.destLocCB_Leave);
            this.destLocCB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.destLocCB_KeyUp);
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Image = global::GMView.Properties.Resources.lamp_off;
            this.cancelBut.Location = new System.Drawing.Point(245, 78);
            this.cancelBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(88, 36);
            this.cancelBut.TabIndex = 5;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Image = global::GMView.Properties.Resources.lamp_on;
            this.okBut.Location = new System.Drawing.Point(163, 78);
            this.okBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(77, 36);
            this.okBut.TabIndex = 4;
            this.okBut.Text = "OK";
            this.okBut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler(this.okBut_Click);
            // 
            // autoBut
            // 
            this.autoBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoBut.Location = new System.Drawing.Point(12, 78);
            this.autoBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.autoBut.Name = "autoBut";
            this.autoBut.Size = new System.Drawing.Size(75, 36);
            this.autoBut.TabIndex = 6;
            this.autoBut.Text = "Auto";
            this.autoBut.UseVisualStyleBackColor = true;
            this.autoBut.Click += new System.EventHandler(this.autoBut_Click);
            // 
            // TrackDestFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(345, 124);
            this.Controls.Add(this.autoBut);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Controls.Add(this.destLocCB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startLocCB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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