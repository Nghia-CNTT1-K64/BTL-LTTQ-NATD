namespace BTL_LTTQ_BIDA.Forms.Main
{
    partial class SignUpDlg
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.txtNhapLaiMK = new System.Windows.Forms.TextBox();
            this.txtTenDN = new System.Windows.Forms.TextBox();
            this.txtCCCD = new System.Windows.Forms.TextBox();
            this.txtMK = new System.Windows.Forms.TextBox();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.btnDangKy = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.checkBoxHienMK = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtIDNV = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label1.Location = new System.Drawing.Point(75, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Họ tên:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label2.Location = new System.Drawing.Point(75, 248);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ngày sinh:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label3.Location = new System.Drawing.Point(75, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Số CCCD/CMND:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label4.Location = new System.Drawing.Point(75, 409);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tên đăng nhập";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label5.Location = new System.Drawing.Point(75, 484);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "Mật khẩu:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label6.Location = new System.Drawing.Point(75, 562);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 24);
            this.label6.TabIndex = 5;
            this.label6.Text = "Nhập lại mật khẩu:";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtHoTen.Location = new System.Drawing.Point(250, 176);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(368, 26);
            this.txtHoTen.TabIndex = 6;
            // 
            // txtNhapLaiMK
            // 
            this.txtNhapLaiMK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNhapLaiMK.Location = new System.Drawing.Point(255, 564);
            this.txtNhapLaiMK.Name = "txtNhapLaiMK";
            this.txtNhapLaiMK.Size = new System.Drawing.Size(363, 26);
            this.txtNhapLaiMK.TabIndex = 7;
            // 
            // txtTenDN
            // 
            this.txtTenDN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTenDN.Location = new System.Drawing.Point(255, 409);
            this.txtTenDN.Name = "txtTenDN";
            this.txtTenDN.Size = new System.Drawing.Size(363, 26);
            this.txtTenDN.TabIndex = 8;
            // 
            // txtCCCD
            // 
            this.txtCCCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtCCCD.Location = new System.Drawing.Point(250, 326);
            this.txtCCCD.Name = "txtCCCD";
            this.txtCCCD.Size = new System.Drawing.Size(368, 26);
            this.txtCCCD.TabIndex = 9;
            this.txtCCCD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCCCD_KeyPress);
            // 
            // txtMK
            // 
            this.txtMK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtMK.Location = new System.Drawing.Point(250, 486);
            this.txtMK.Name = "txtMK";
            this.txtMK.PasswordChar = '●';
            this.txtMK.Size = new System.Drawing.Size(368, 26);
            this.txtMK.TabIndex = 10;
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.Location = new System.Drawing.Point(250, 248);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(200, 22);
            this.dtpNgaySinh.TabIndex = 11;
            // 
            // btnDangKy
            // 
            this.btnDangKy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnDangKy.Location = new System.Drawing.Point(364, 599);
            this.btnDangKy.Name = "btnDangKy";
            this.btnDangKy.Size = new System.Drawing.Size(138, 52);
            this.btnDangKy.TabIndex = 12;
            this.btnDangKy.Text = "Đăng ký";
            this.btnDangKy.UseVisualStyleBackColor = true;
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnThoat.Location = new System.Drawing.Point(542, 599);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(138, 52);
            this.btnThoat.TabIndex = 13;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // checkBoxHienMK
            // 
            this.checkBoxHienMK.AutoSize = true;
            this.checkBoxHienMK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.checkBoxHienMK.Location = new System.Drawing.Point(645, 488);
            this.checkBoxHienMK.Name = "checkBoxHienMK";
            this.checkBoxHienMK.Size = new System.Drawing.Size(139, 24);
            this.checkBoxHienMK.TabIndex = 14;
            this.checkBoxHienMK.Text = "Hiện mật khẩu";
            this.checkBoxHienMK.UseVisualStyleBackColor = true;
            this.checkBoxHienMK.CheckedChanged += new System.EventHandler(this.checkBoxHienMK_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label8.Location = new System.Drawing.Point(74, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 25);
            this.label8.TabIndex = 16;
            this.label8.Text = "ID nhân viên:";
            // 
            // txtIDNV
            // 
            this.txtIDNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtIDNV.Location = new System.Drawing.Point(250, 27);
            this.txtIDNV.Name = "txtIDNV";
            this.txtIDNV.Size = new System.Drawing.Size(368, 26);
            this.txtIDNV.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(74, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 25);
            this.label7.TabIndex = 18;
            this.label7.Text = "Số điện thoại:";
            // 
            // txtSDT
            // 
            this.txtSDT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtSDT.Location = new System.Drawing.Point(250, 102);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(368, 26);
            this.txtSDT.TabIndex = 19;
            this.txtSDT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSDT_KeyPress);
            // 
            // SignUpDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 677);
            this.Controls.Add(this.txtSDT);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtIDNV);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBoxHienMK);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnDangKy);
            this.Controls.Add(this.dtpNgaySinh);
            this.Controls.Add(this.txtMK);
            this.Controls.Add(this.txtCCCD);
            this.Controls.Add(this.txtTenDN);
            this.Controls.Add(this.txtNhapLaiMK);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SignUpDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng kí";
            this.Load += new System.EventHandler(this.SignUpDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.TextBox txtNhapLaiMK;
        private System.Windows.Forms.TextBox txtTenDN;
        private System.Windows.Forms.TextBox txtCCCD;
        private System.Windows.Forms.TextBox txtMK;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Button btnDangKy;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.CheckBox checkBoxHienMK;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtIDNV;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSDT;
    }
}