namespace BTL_LTTQ_BIDA.Forms.Main
{
    partial class frmThemBanMoi
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
            this.lblMaBan = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblGiaTien = new System.Windows.Forms.Label();
            this.txtMaBan = new System.Windows.Forms.TextBox();
            this.txtGiaTien = new System.Windows.Forms.TextBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMaBan
            // 
            this.lblMaBan.AutoSize = true;
            this.lblMaBan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaBan.Location = new System.Drawing.Point(60, 63);
            this.lblMaBan.Name = "lblMaBan";
            this.lblMaBan.Size = new System.Drawing.Size(69, 20);
            this.lblMaBan.TabIndex = 0;
            this.lblMaBan.Text = "Mã bàn:";
            this.lblMaBan.Click += new System.EventHandler(this.lblMaBan_Click_1);
            // 
            // lblGiaTien
            // 
            this.lblGiaTien.AutoSize = true;
            this.lblGiaTien.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiaTien.Location = new System.Drawing.Point(60, 144);
            this.lblGiaTien.Name = "lblGiaTien";
            this.lblGiaTien.Size = new System.Drawing.Size(72, 20);
            this.lblGiaTien.TabIndex = 1;
            this.lblGiaTien.Text = "Giá tiền:";
            // 
            // txtMaBan
            // 
            this.txtMaBan.Location = new System.Drawing.Point(155, 56);
            this.txtMaBan.Multiline = true;
            this.txtMaBan.Name = "txtMaBan";
            this.txtMaBan.Size = new System.Drawing.Size(168, 27);
            this.txtMaBan.TabIndex = 2;
            this.txtMaBan.TextChanged += new System.EventHandler(this.txtMaBan_TextChanged_1);
            // 
            // txtGiaTien
            // 
            this.txtGiaTien.Location = new System.Drawing.Point(155, 135);
            this.txtGiaTien.Multiline = true;
            this.txtGiaTien.Name = "txtGiaTien";
            this.txtGiaTien.Size = new System.Drawing.Size(168, 29);
            this.txtGiaTien.TabIndex = 3;
            this.txtGiaTien.TextChanged += new System.EventHandler(this.txtGiaTien_TextChanged);
            // 
            // btnLuu
            // 
            this.btnLuu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.Location = new System.Drawing.Point(274, 258);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(75, 31);
            this.btnLuu.TabIndex = 4;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.Location = new System.Drawing.Point(420, 258);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(75, 31);
            this.btnHuy.TabIndex = 5;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // frmThemBanMoi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 316);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.txtGiaTien);
            this.Controls.Add(this.txtMaBan);
            this.Controls.Add(this.lblGiaTien);
            this.Controls.Add(this.lblMaBan);
            this.Name = "frmThemBanMoi";
            this.Text = "Thêm bàn mới";
            this.Load += new System.EventHandler(this.frmThemBanMoi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMaBan;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblGiaTien;
        private System.Windows.Forms.TextBox txtMaBan;
        private System.Windows.Forms.TextBox txtGiaTien;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}