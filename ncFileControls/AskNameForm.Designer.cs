namespace ncFileControls
{
    partial class AskNameForm
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
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.nameLbl = new System.Windows.Forms.Label();
            this.nameTb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBut.Location = new System.Drawing.Point(244, 70);
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size(75, 29);
            this.cancelBut.TabIndex = 2;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBut.Location = new System.Drawing.Point(163, 70);
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size(75, 29);
            this.okBut.TabIndex = 1;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            // 
            // nameLbl
            // 
            this.nameLbl.AutoSize = true;
            this.nameLbl.Location = new System.Drawing.Point(12, 9);
            this.nameLbl.Name = "nameLbl";
            this.nameLbl.Size = new System.Drawing.Size(49, 17);
            this.nameLbl.TabIndex = 6;
            this.nameLbl.Text = "Name:";
            // 
            // nameTb
            // 
            this.nameTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTb.Location = new System.Drawing.Point(15, 29);
            this.nameTb.Name = "nameTb";
            this.nameTb.Size = new System.Drawing.Size(304, 22);
            this.nameTb.TabIndex = 0;
            // 
            // AskNameForm
            // 
            this.AcceptButton = this.okBut;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBut;
            this.ClientSize = new System.Drawing.Size(331, 111);
            this.Controls.Add(this.nameTb);
            this.Controls.Add(this.nameLbl);
            this.Controls.Add(this.okBut);
            this.Controls.Add(this.cancelBut);
            this.Name = "AskNameForm";
            this.Text = "Enter name";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Label nameLbl;
        private System.Windows.Forms.TextBox nameTb;
    }
}