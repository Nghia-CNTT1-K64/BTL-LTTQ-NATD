namespace BTL_LTTQ_BIDA.Forms.Table
{
    partial class frmDoiBan
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
            this.lblBanCu = new System.Windows.Forms.Label();
            this.lblBanMoi = new System.Windows.Forms.Label();
            this.cboBanCu = new System.Windows.Forms.ComboBox();
            this.cboBanMoi = new System.Windows.Forms.ComboBox();
            this.btnDoiBan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBanCu
            // 
            this.lblBanCu.AutoSize = true;
            this.lblBanCu.BackColor = System.Drawing.Color.Transparent;
            this.lblBanCu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBanCu.ForeColor = System.Drawing.Color.Black;
            this.lblBanCu.Location = new System.Drawing.Point(40, 105);
            this.lblBanCu.Name = "lblBanCu";
            this.lblBanCu.Size = new System.Drawing.Size(67, 20);
            this.lblBanCu.TabIndex = 0;
            this.lblBanCu.Text = "Bàn cũ:";
            this.lblBanCu.Click += new System.EventHandler(this.lblBanCu_Click);
            // 
            // lblBanMoi
            // 
            this.lblBanMoi.AutoSize = true;
            this.lblBanMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBanMoi.ForeColor = System.Drawing.Color.Black;
            this.lblBanMoi.Location = new System.Drawing.Point(263, 105);
            this.lblBanMoi.Name = "lblBanMoi";
            this.lblBanMoi.Size = new System.Drawing.Size(76, 20);
            this.lblBanMoi.TabIndex = 1;
            this.lblBanMoi.Text = "Bàn mới:";
            // 
            // cboBanCu
            // 
            this.cboBanCu.FormattingEnabled = true;
            this.cboBanCu.Location = new System.Drawing.Point(113, 101);
            this.cboBanCu.Name = "cboBanCu";
            this.cboBanCu.Size = new System.Drawing.Size(121, 24);
            this.cboBanCu.TabIndex = 2;
            this.cboBanCu.TabStop = false;
            this.cboBanCu.SelectedIndexChanged += new System.EventHandler(this.cboBanCu_SelectedIndexChanged);
            // 
            // cboBanMoi
            // 
            this.cboBanMoi.FormattingEnabled = true;
            this.cboBanMoi.Location = new System.Drawing.Point(345, 101);
            this.cboBanMoi.Name = "cboBanMoi";
            this.cboBanMoi.Size = new System.Drawing.Size(121, 24);
            this.cboBanMoi.TabIndex = 3;
            this.cboBanMoi.SelectedIndexChanged += new System.EventHandler(this.cboBanMoi_SelectedIndexChanged);
            // 
            // btnDoiBan
            // 
            this.btnDoiBan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDoiBan.Location = new System.Drawing.Point(203, 206);
            this.btnDoiBan.Name = "btnDoiBan";
            this.btnDoiBan.Size = new System.Drawing.Size(75, 30);
            this.btnDoiBan.TabIndex = 4;
            this.btnDoiBan.Text = "Đổi bàn";
            this.btnDoiBan.UseVisualStyleBackColor = true;
            this.btnDoiBan.Click += new System.EventHandler(this.btnDoiBan_Click);
            // 
            // frmDoiBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 289);
            this.Controls.Add(this.btnDoiBan);
            this.Controls.Add(this.cboBanMoi);
            this.Controls.Add(this.cboBanCu);
            this.Controls.Add(this.lblBanMoi);
            this.Controls.Add(this.lblBanCu);
            this.Name = "frmDoiBan";
            this.Text = "Đổi bàn";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBanCu;
        private System.Windows.Forms.Label lblBanMoi;
        private System.Windows.Forms.ComboBox cboBanCu;
        private System.Windows.Forms.ComboBox cboBanMoi;
        private System.Windows.Forms.Button btnDoiBan;
    }
}