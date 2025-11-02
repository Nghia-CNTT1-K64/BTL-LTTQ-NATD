namespace BTL_LTTQ_BIDA.Forms.Main
{
    partial class LoginDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTenDN = new System.Windows.Forms.TextBox();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.checkBoxHienMK = new System.Windows.Forms.CheckBox();
            this.btnDN = new System.Windows.Forms.Button();
            this.btnDK = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.ptcLogoQuan = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ptcLogoQuan)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(55, 259);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tên đăng nhập:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(55, 321);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mật khẩu:";
            // 
            // txtTenDN
            // 
            this.txtTenDN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTenDN.Location = new System.Drawing.Point(212, 258);
            this.txtTenDN.Name = "txtTenDN";
            this.txtTenDN.Size = new System.Drawing.Size(348, 26);
            this.txtTenDN.TabIndex = 3;
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtMatKhau.Location = new System.Drawing.Point(212, 320);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.PasswordChar = '●';
            this.txtMatKhau.Size = new System.Drawing.Size(348, 26);
            this.txtMatKhau.TabIndex = 4;
            this.txtMatKhau.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMatKhau_KeyDown);
            // 
            // checkBoxHienMK
            // 
            this.checkBoxHienMK.AutoSize = true;
            this.checkBoxHienMK.Location = new System.Drawing.Point(587, 326);
            this.checkBoxHienMK.Name = "checkBoxHienMK";
            this.checkBoxHienMK.Size = new System.Drawing.Size(114, 20);
            this.checkBoxHienMK.TabIndex = 5;
            this.checkBoxHienMK.Text = "Hiện mật khẩu";
            this.checkBoxHienMK.UseVisualStyleBackColor = true;
            this.checkBoxHienMK.CheckedChanged += new System.EventHandler(this.checkBoxHienMK_CheckedChanged);
            // 
            // btnDN
            // 
            this.btnDN.Location = new System.Drawing.Point(264, 397);
            this.btnDN.Name = "btnDN";
            this.btnDN.Size = new System.Drawing.Size(108, 32);
            this.btnDN.TabIndex = 6;
            this.btnDN.Text = "Đăng nhập";
            this.btnDN.UseVisualStyleBackColor = true;
            this.btnDN.Click += new System.EventHandler(this.btnDN_Click_1);
            // 
            // btnDK
            // 
            this.btnDK.Location = new System.Drawing.Point(440, 397);
            this.btnDK.Name = "btnDK";
            this.btnDK.Size = new System.Drawing.Size(108, 32);
            this.btnDK.TabIndex = 7;
            this.btnDK.Text = "Đăng ký";
            this.btnDK.UseVisualStyleBackColor = true;
            this.btnDK.Click += new System.EventHandler(this.btnDK_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(593, 397);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(108, 32);
            this.btnThoat.TabIndex = 8;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // ptcLogoQuan
            // 
            this.ptcLogoQuan.Image = global::BTL_LTTQ_BIDA.Properties.Resources.NATD_BILLIARD;
            this.ptcLogoQuan.Location = new System.Drawing.Point(188, 3);
            this.ptcLogoQuan.Name = "ptcLogoQuan";
            this.ptcLogoQuan.Size = new System.Drawing.Size(348, 235);
            this.ptcLogoQuan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ptcLogoQuan.TabIndex = 0;
            this.ptcLogoQuan.TabStop = false;
            // 
            // LoginDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(735, 441);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnDK);
            this.Controls.Add(this.btnDN);
            this.Controls.Add(this.checkBoxHienMK);
            this.Controls.Add(this.txtMatKhau);
            this.Controls.Add(this.txtTenDN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ptcLogoQuan);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.Load += new System.EventHandler(this.LoginDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ptcLogoQuan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ptcLogoQuan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTenDN;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.CheckBox checkBoxHienMK;
        private System.Windows.Forms.Button btnDN;
        private System.Windows.Forms.Button btnDK;
        private System.Windows.Forms.Button btnThoat;
    }
}