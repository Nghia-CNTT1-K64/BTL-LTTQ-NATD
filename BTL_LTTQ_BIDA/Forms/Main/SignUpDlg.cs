using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Utils;
namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class SignUpDlg : Form
    {
        private Data.DataConnect dtBase = new Data.DataConnect();
        public SignUpDlg()
        {
            InitializeComponent();
        }
        private void SignUpDlg_Load(object sender, EventArgs e)
        {
            UIStyler.ApplyFormStyle(this);
            txtIDNV.ReadOnly = true;
            string sql = "SELECT TOP 1 IDNV FROM NHANVIEN ORDER BY IDNV DESC";
            DataTable table = dtBase.ReadData(sql);

            string newID = "NV001";
            if (table.Rows.Count > 0)
            {
                string lastID = table.Rows[0]["IDNV"].ToString(); // VD: NV05
                int numberPart = 1;
                if (lastID.Length > 2 && int.TryParse(lastID.Substring(2), out int tmp))
                    numberPart = tmp + 1;

                newID = "NV" + numberPart.ToString("000"); // → DV06
            }

            txtIDNV.Text = newID;

            //ân mk
            txtMK.PasswordChar = '●';
            txtNhapLaiMK.PasswordChar = '●';

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            LoginDlg fLogin = new LoginDlg();
            this.Hide();
            fLogin.FormClosed += (s, args) => this.Close();//tắt signup khi login hiện
            fLogin.Show();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            //ktra rỗng
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Chưa nhập họ tên!");
                txtHoTen.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Chưa nhập số điện thoại!");
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Chưa nhập CCCD!");
                txtCCCD.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(txtTenDN.Text))
            {
                MessageBox.Show("Chưa nhập tên đăng nhập!");
                txtTenDN.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMK.Text))
            {
                MessageBox.Show("Chưa nhập mật khẩu!");
                txtMK.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNhapLaiMK.Text))
            {
                MessageBox.Show("Chưa nhập lại mật khẩu!");
                txtNhapLaiMK.Focus();
                return;
            }

            //check username tồn tại
            Data.DataConnect dtConnect = new Data.DataConnect();
            string sqlCheckUser = $"SELECT * FROM NHANVIEN WHERE TENDANGNHAP = '{txtTenDN.Text}'";
            DataTable dt = dtConnect.ReadData(sqlCheckUser);
            if(dt.Rows.Count > 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng chọn tên khác.");
                txtTenDN.Focus();
                return;
            }

            //CHECK CCCD TỒN TẠI
            string sqlCheckCCCD = $"SELECT * FROM NHANVIEN WHERE CCCD = '{txtCCCD.Text}'";
            dt = dtConnect.ReadData(sqlCheckCCCD);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("CCCD đã tồn tại! Vui lòng kiểm tra lại.");
                txtCCCD.Focus();
                return;
            }
            // Kiểm tra định dạng CCCD (12 số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtCCCD.Text, @"^\d{12}$"))
            {
                MessageBox.Show("CCCD phải gồm đúng 12 số!");
                txtCCCD.Focus();
                return;
            }


            //check sdt tồn tại
            string sqlCheckSDT = $"SELECT * FROM NHANVIEN WHERE SoDT = '{txtSDT.Text}'";
            dt = dtConnect.ReadData(sqlCheckSDT);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Số điện thoại đã tồn tại! Vui lòng kiểm tra lại.");
                txtSDT.Focus();
                return;
            }
            // Kiểm tra định dạng SĐT Việt Nam
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text, @"^(03|05|07|08|09)\d{8}$"))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng của Việt Nam!");
                txtSDT.Focus();
                return;
            }
            //ktra mk khớp
            if (txtMK.Text != txtNhapLaiMK.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi");
                txtNhapLaiMK.Focus();
                return;
            }
            //ktra ngày sinh
            if (dtpNgaySinh.Value > DateTime.Now.AddYears(-17))
            {
                MessageBox.Show("Nhân viên phải ít nhất 17 tuổi!");
                return;
            }

            int quyen = 0; //quyền nhân viên
            int hienthi = 0; //ktra admin duyệt chưa

            //thêm nhân viên
            string sqlInsert = $"INSERT INTO NHANVIEN(IDNV,SODT, HOTENNV, NGAYSINH, CCCD, TENDANGNHAP, MATKHAU, QUYENADMIN, HIENTHI) " +
                $"VALUES(N'{txtIDNV.Text}', '{txtSDT.Text}', N'{txtHoTen.Text}', '{dtpNgaySinh.Value.ToString("yyyy-MM-dd")}', '{txtCCCD.Text}', '{txtTenDN.Text}', '{txtMK.Text}', {quyen}, {hienthi})";

            //gọi update
            dtConnect.UpdateData(sqlInsert);
            MessageBox.Show("Đăng ký thành công! Vui lòng chờ Admin duyệt tài khoản.", "Thông báo");

        }

        private void txtCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (txtCCCD.Text.Length >= 12 && !char.IsControl(e.KeyChar)) 
            {
                e.Handled = true;
            }
        }
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (txtSDT.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkBoxHienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHienMK.Checked)
            {
                
                // Hiện mật khẩu
                txtMK.PasswordChar = '\0'; // bỏ mask
                txtNhapLaiMK.PasswordChar = '\0'; // bỏ mask
            }
            else
            {
                
                // Ẩn mật khẩu
                txtMK.PasswordChar = '●'; // đặt lại mask
                txtNhapLaiMK.PasswordChar = '●';
            }
        }

        
    }
}
