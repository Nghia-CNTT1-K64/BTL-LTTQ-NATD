using System;
using System.Data;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Data;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class AddTableDlg : Form
    {
        private readonly DataConnect dtBase = new DataConnect();

        public frmThemBanMoi()
        {
            InitializeComponent();
        }

        // =====================================================
        // 🔹 HÀM TẠO MÃ BÀN TỰ ĐỘNG (VD: B001 → B002 → B003 ...)
        // =====================================================
        private void LoadNextTableID()
        {
            string nextId = "B001"; // Giá trị mặc định nếu bảng trống

            try
            {
                string query = "SELECT TOP 1 IDBAN FROM BAN ORDER BY IDBAN DESC";
                DataTable dt = dtBase.ReadData(query);

                if (dt.Rows.Count > 0)
                {
                    string maxId = dt.Rows[0]["IDBAN"].ToString(); // ví dụ "B011"
                    string prefix = "B";
                    string numberPart = maxId.Substring(prefix.Length);

                    if (int.TryParse(numberPart, out int lastNumber))
                    {
                        nextId = prefix + (lastNumber + 1).ToString("D3"); // "B012"
                    }
                }

                txtMaBan.Text = nextId;
                txtMaBan.Enabled = false; // Không cho người dùng sửa
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể lấy mã bàn tự động:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaBan.Text = nextId;
            }
        }

        // =====================================================
        // 🔹 SỰ KIỆN LOAD FORM → TỰ TẠO MÃ BÀN TIẾP THEO
        // =====================================================
        private void frmThemBanMoi_Load(object sender, EventArgs e)
        {
            LoadNextTableID();
        }

        // =====================================================
        // 🔹 NÚT LƯU: THÊM BÀN MỚI VÀ TỰ ĐỘNG LOAD MÃ TIẾP THEO
        // =====================================================
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                string idBan = txtMaBan.Text.Trim();
                string strGia = txtGiaTien.Text.Trim();

                if (string.IsNullOrEmpty(idBan))
                {
                    MessageBox.Show("Mã bàn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(strGia, out decimal giaTien))
                {
                    MessageBox.Show("Giá tiền phải là một số hợp lệ.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTien.Focus();
                    return;
                }

                string sql = "INSERT INTO BAN (IDBAN, GIATIEN, TrangThai) VALUES (@p0, @p1, 0)";
                object[] param = { idBan, giaTien };

                int rows = dtBase.ExecuteNonQuery(sql, param);

                if (rows > 0)
                {
                    MessageBox.Show("✅ Thêm bàn mới thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset form
                    txtGiaTien.Clear();
                    LoadNextTableID();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được thêm.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PRIMARY KEY") || ex.Message.Contains("trùng"))
                    MessageBox.Show("❌ Mã bàn đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show($"Lỗi khi thêm bàn mới:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // 🔹 NÚT HỦY: ĐÓNG FORM
        // =====================================================
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // =====================================================
        // 🔹 CÁC SỰ KIỆN KHÔNG DÙNG
        // =====================================================
        private void txtGiaTien_TextChanged(object sender, EventArgs e) { }
        private void lblMaBan_Click(object sender, EventArgs e) { }
        private void lblGiaTien_Click(object sender, EventArgs e) { }
    }
}
