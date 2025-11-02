using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Classes;
using BTL_LTTQ_BIDA.Data;

namespace BTL_LTTQ_BIDA.Forms.Account
{
    public partial class ChangePassword : Form
    {
        Data.DataConnect dtBase = new Data.DataConnect();
        private NhanVien nv;
        public ChangePassword(NhanVien nv)
        {
            InitializeComponent();
            this.nv = nv;
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            txtMKcu.ReadOnly = true;
            txtMKmoi.Focus();
            txtMKmoi.PasswordChar = '●';
            txtNhapLaiMK.PasswordChar = '●';
            txtMKcu.Text = nv.MatKhau;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) 
            {
                txtMKmoi.PasswordChar = '\0';
                txtNhapLaiMK.PasswordChar = '\0';
            }
            else
            {
                txtMKmoi.PasswordChar = '●';
                txtNhapLaiMK.PasswordChar = '●';
            }
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            //ktre rỗng
            if (string.IsNullOrWhiteSpace(txtMKmoi.Text))
            {
                MessageBox.Show("Chưa nhập mật khẩu mới!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMKmoi.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNhapLaiMK.Text))
            {
                MessageBox.Show("Chưa nhập lại mật khẩu mới!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNhapLaiMK.Focus();
                return;
            }
            DialogResult result = MessageBox.Show("Xác nhận đổi mật khẩu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
                
                //kiểm tra mật khẩu mới và nhập lại mật khẩu
                if (txtMKmoi.Text != txtNhapLaiMK.Text)
                {
                    MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //cập nhật mật khẩu mới
                string commandString = $"UPDATE NHANVIEN SET MATKHAU = '{txtMKmoi.Text}' WHERE IDNV = '{nv.IDNV}'";
                DataConnect dtBase = new DataConnect();
                dtBase.UpdateData(commandString);
                nv.MatKhau = txtMKmoi.Text;
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
