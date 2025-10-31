using System;
using System.Data;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Data;
using BTL_LTTQ_BIDA.Forms.Main;
using System.Data.SqlClient;

namespace BTL_LTTQ_BIDA.Forms.Account
{

    public partial class AccountInfoDlg : Form
    {

        private readonly DataConnect dtBase = new DataConnect();
        private readonly Function fn = new Function();

        public AccountInfoDlg()
        {
            InitializeComponent();
        }

        // 🔹 Khi nhấn Hủy → quay lại FMain
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
            // Kiểm tra FMain đang mở hay chưa
            FMain mainForm = Application.OpenForms["FMain"] as FMain;
            if (mainForm != null)
                mainForm.Show();
            else
            {
                mainForm = new FMain();
                mainForm.Show();
            }
        }

        // 🔹 Khi form load → đọc dữ liệu nhân viên hiện tại
        private void AccountInfoDlg_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = $"SELECT * FROM NHANVIEN WHERE IDNV = '{FMain.IDNV_Current}'";
                DataTable dt = dtBase.ReadData(sql);

                //if (dt.Rows.Count == 0)
                //{
                //    MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Close();
                //    return;
                //}

                DataRow row = dt.Rows[0];
                txtID.Text = FMain.IDNV_Current.ToString();
                txtTenNhanVien.Text = row["HOTENNV"].ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row["NGAYSINH"]);
                txtCCCD.Text = row["CCCD"].ToString();
                txtTenDangNhap.Text = row["TENDANGNHAP"].ToString();
                txtMatKhau.Text = row["MATKHAU"].ToString();
                cbQQTV.Checked = Convert.ToBoolean(row["QUYENADMIN"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin tài khoản: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 Hiện / Ẩn mật khẩu
        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !cbHienMK.Checked;
        }

        // 🔹 Lưu thay đổi
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!CheckValid()) return;

            try
            {
                string sql = $@"
                    UPDATE NHANVIEN SET
                        HOTENNV = N'{txtTenNhanVien.Text}',
                        NGAYSINH = '{dtpNgaySinh.Value:yyyy-MM-dd}',
                        CCCD = '{txtCCCD.Text}',
                        MATKHAU = '{txtMatKhau.Text}',
                        QUYENADMIN = {(cbQQTV.Checked ? 1 : 0)}
                    WHERE IDNV = '{FMain.IDNV_Current}'";

                dtBase.UpdateData(sql);

                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 Kiểm tra hợp lệ dữ liệu nhập
        private bool CheckValid()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
                    throw new Exception("Mục \"Họ tên\" không được để trống.");
                if (txtTenNhanVien.Text.Length > 50)
                    throw new Exception("Độ dài họ tên tối đa chỉ 50 kí tự.");

                if (string.IsNullOrWhiteSpace(txtCCCD.Text))
                    throw new Exception("Mục \"Số CCCD/CMND\" không được để trống.");
                if (txtCCCD.Text.Length > 15)
                    throw new Exception("Độ dài số CCCD/CMND tối đa chỉ 15 kí tự.");

                if (txtMatKhau.Text.Length < 8)
                    throw new Exception("Độ dài mật khẩu phải tối thiểu 8 kí tự.");
                if (txtMatKhau.Text.Length > 32)
                    throw new Exception("Độ dài mật khẩu tối đa chỉ 32 kí tự.");

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lưu không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
