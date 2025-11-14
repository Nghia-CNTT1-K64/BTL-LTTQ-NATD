using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Classes;
using BTL_LTTQ_BIDA.Data;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class LoginDlg : Form
    {
        public LoginDlg()
        {
            InitializeComponent();
        }

        private void LoginDlg_Load(object sender, EventArgs e)
        {
           
        }

        private void ptcLogoQuan_Click(object sender, EventArgs e)
        {

        }

        private void btnDK_Click(object sender, EventArgs e)
        {
            SignUpDlg fSignUp = new SignUpDlg();
            this.Hide();
            fSignUp.FormClosed += (s, args) => this.Show();//tắt login khi signup hiện
            fSignUp.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void checkBoxHienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHienMK.Checked)
            {
                // Hiện mật khẩu
                txtMatKhau.PasswordChar = '\0'; // bỏ mask
            }
            else
            {
                // Ẩn mật khẩu
                txtMatKhau.PasswordChar = '●'; // đặt lại mask
            }
        }

        private void thucHienDangNhap()
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtTenDN.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDN.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            //Kiểm tra đăng nhập
            DataConnect dc = new DataConnect();

            string query = $"SELECT * FROM NHANVIEN WHERE TENDANGNHAP = '{txtTenDN.Text}' AND MATKHAU = '{txtMatKhau.Text}'";

            DataTable dt = dc.ReadData(query);

            // Không tìm thấy -> sai
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Đăng nhập thất bại",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //ktra tkhoan đc duyêt chưa
            bool active = (bool)dt.Rows[0]["HIENTHI"];
            if (!active)
            {
                MessageBox.Show("Tài khoản của bạn chưa được duyệt! Vui lòng liên hệ Admin.",
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //ktra nhân viên đã nghỉ việc
            bool nghiViec = dt.Rows[0]["NGHIVIEC"] != DBNull.Value && !(bool)dt.Rows[0]["NGHIVIEC"];
            if (nghiViec)
            {
                MessageBox.Show("Nhân viên này đã nghỉ việc, không thể đăng nhập", "đăng nhập thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;


            }

            //ktra quyền admin
            bool isAdmin = (bool)dt.Rows[0]["QUYENADMIN"];
            if (isAdmin)
            {
                MessageBox.Show("Đăng nhập thành công với quyền Quản trị viên.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Đăng nhập thành công với quyền Nhân viên.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Tạo object nhân viên
            NhanVien nv = new NhanVien()
            {
                IDNV = dt.Rows[0]["IDNV"].ToString(),
                HoTenNV = dt.Rows[0]["HOTENNV"].ToString(),
                NgaySinh = (DateTime)dt.Rows[0]["NGAYSINH"],
                SoDT = dt.Rows[0]["SODT"].ToString(),
                CCCD = dt.Rows[0]["CCCD"].ToString(),
                TenDangNhap = dt.Rows[0]["TENDANGNHAP"].ToString(),
                MatKhau = dt.Rows[0]["MATKHAU"].ToString(),
                QuyenAdmin = (bool)dt.Rows[0]["QUYENADMIN"],
                HienThi = (bool)dt.Rows[0]["HIENTHI"]
            };

            FMain fMain = new FMain(nv);
            //FMain fMain = new FMain(isAdmin);
            this.Hide();
            fMain.FormClosed += (s, args) => this.Close();//tắt login khi main hiện
            fMain.Show();
        }
       

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                thucHienDangNhap();
            }
        }

        private void btnDN_Click_1(object sender, EventArgs e)
        {
            thucHienDangNhap();
        }
    }
}
