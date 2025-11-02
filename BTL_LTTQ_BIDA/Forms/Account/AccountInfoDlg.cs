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

namespace BTL_LTTQ_BIDA.Forms.Account
{
    public partial class AccountInfoDlg : Form
    {
        private NhanVien nv;
        public AccountInfoDlg(NhanVien nv)
        {
            InitializeComponent();
            this.nv = nv;
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            using (ChangePassword form = new ChangePassword(nv))
            {
                form.ShowDialog(this);
                txtMatKhau.Text = nv.MatKhau;
            }
        }

        private void AccountInfoDlg_Load(object sender, EventArgs e)
        {
            cbHienMK.Checked = false;
            txtMatKhau.PasswordChar = '●';
            txtCCCD.Text = nv.CCCD;
            txtTenNhanVien.Text = nv.HoTenNV;
            txtID.Text = nv.IDNV;
            txtMatKhau.Text = nv.MatKhau;
            cbQQTV.Checked = nv.QuyenAdmin;
            txtTenDangNhap.Text = nv.TenDangNhap;


        }

        private void cbHienMK_CheckedChanged(object sender, EventArgs e)
        {
            if(cbHienMK.Checked)
            {
                txtMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtMatKhau.PasswordChar = '●';
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
          
        }
    }
}