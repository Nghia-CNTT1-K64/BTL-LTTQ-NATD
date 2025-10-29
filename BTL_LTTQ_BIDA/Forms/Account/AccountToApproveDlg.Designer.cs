namespace BTL_LTTQ_BIDA.Forms.Account
{
    partial class AccountToApproveDlg
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
            this.dgvNhanVien = new System.Windows.Forms.DataGridView();
            this.btnDuyet = new System.Windows.Forms.Button();
            this.btnDuyetTatCa = new System.Windows.Forms.Button();
            this.btnXoaTatCa = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvNhanVien
            // 
            this.dgvNhanVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNhanVien.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvNhanVien.Location = new System.Drawing.Point(0, 0);
            this.dgvNhanVien.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvNhanVien.Name = "dgvNhanVien";
            this.dgvNhanVien.RowHeadersWidth = 51;
            this.dgvNhanVien.RowTemplate.Height = 24;
            this.dgvNhanVien.Size = new System.Drawing.Size(791, 350);
            this.dgvNhanVien.TabIndex = 0;
            // 
            // btnDuyet
            // 
            this.btnDuyet.Enabled = false;
            this.btnDuyet.Location = new System.Drawing.Point(112, 381);
            this.btnDuyet.Name = "btnDuyet";
            this.btnDuyet.Size = new System.Drawing.Size(115, 45);
            this.btnDuyet.TabIndex = 1;
            this.btnDuyet.Text = "Duyệt";
            this.btnDuyet.UseVisualStyleBackColor = true;
            // 
            // btnDuyetTatCa
            // 
            this.btnDuyetTatCa.Location = new System.Drawing.Point(552, 381);
            this.btnDuyetTatCa.Name = "btnDuyetTatCa";
            this.btnDuyetTatCa.Size = new System.Drawing.Size(115, 45);
            this.btnDuyetTatCa.TabIndex = 2;
            this.btnDuyetTatCa.Text = "Duyệt tất cả";
            this.btnDuyetTatCa.UseVisualStyleBackColor = true;
            // 
            // btnXoaTatCa
            // 
            this.btnXoaTatCa.Location = new System.Drawing.Point(552, 442);
            this.btnXoaTatCa.Name = "btnXoaTatCa";
            this.btnXoaTatCa.Size = new System.Drawing.Size(115, 45);
            this.btnXoaTatCa.TabIndex = 4;
            this.btnXoaTatCa.Text = "Xóa tất cả";
            this.btnXoaTatCa.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            this.btnXoa.Enabled = false;
            this.btnXoa.Location = new System.Drawing.Point(112, 442);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(115, 45);
            this.btnXoa.TabIndex = 3;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            // 
            // AccountToApproveDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 499);
            this.Controls.Add(this.btnXoaTatCa);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnDuyetTatCa);
            this.Controls.Add(this.btnDuyet);
            this.Controls.Add(this.dgvNhanVien);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AccountToApproveDlg";
            this.Text = "Danh sách tài khoản chờ được duyệt";
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvNhanVien;
        private System.Windows.Forms.Button btnDuyet;
        private System.Windows.Forms.Button btnDuyetTatCa;
        private System.Windows.Forms.Button btnXoaTatCa;
        private System.Windows.Forms.Button btnXoa;
    }
}