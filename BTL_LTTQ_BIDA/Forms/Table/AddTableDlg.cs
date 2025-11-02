using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class frmThemBanMoi : Form
    {
        public frmThemBanMoi()
        {
            InitializeComponent();
        }

        // ---
        // SỬA 1: TÁCH LOGIC TẠO MÃ BÀN RA HÀM RIÊNG
        // ---
        private void LoadNextTableID()
        {
            string nextId = "B001"; // Giá trị mặc định nếu bảng trống
            try
            {
                // 1. Lấy IDBAN lớn nhất hiện tại
                string query = "SELECT TOP 1 IDBAN FROM BAN ORDER BY IDBAN DESC";
                DataTable dt = FMain.GetSqlData(query); // Giả sử hàm này tồn tại

                // 2. Kiểm tra xem bảng có dữ liệu không
                if (dt.Rows.Count > 0)
                {
                    string maxId = dt.Rows[0][0].ToString(); // Lấy "B011"
                    string prefix = "B";
                    string numberPart = maxId.Substring(prefix.Length);

                    if (int.TryParse(numberPart, out int lastNumber))
                    {
                        // 4. Cộng 1 vào phần số (11 + 1 = 12)
                        int nextNumber = lastNumber + 1;

                        // 5. Định dạng lại (12 -> "012")
                        nextId = prefix + nextNumber.ToString("D3"); // Kết quả: "B012"
                    }
                }

                // 6. Gán ID mới vào textbox
                txtMaBan.Text = nextId;
                txtMaBan.Enabled = false; // Không cho sửa mã bàn
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy mã bàn tự động: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaBan.Text = nextId; // Gán giá trị dự phòng
            }
        }

        // ---
        // SỬA 2: GỌI HÀM TẠO MÃ BÀN KHI LOAD FORM
        // ---
        private void frmThemBanMoi_Load(object sender, EventArgs e)
        {
            LoadNextTableID(); // Tải mã bàn đầu tiên (ví dụ: B011)
        }

        // ---
        // SỬA 3: SỬA LẠI NÚT LƯU (KHÔNG TẮT FORM)
        // ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Tên cột (IDBAN, GIATIEN, TrangThai) phải khớp CSDL
            string commandText = "INSERT INTO BAN(IDBAN, GIATIEN, TrangThai) VALUES(@idBan, @giaTien, 0)";
            string connectionString = FMain.ConnectionString;

            try
            {
                // Bước kiểm tra định dạng
                string idBan = txtMaBan.Text;
                decimal giaTien;

                if (string.IsNullOrEmpty(idBan))
                {
                    MessageBox.Show("Mã bàn không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(txtGiaTien.Text, out giaTien))
                {
                    MessageBox.Show("Giá tiền phải là một con số.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtGiaTien.Focus();
                    return;
                }

                // Gửi lệnh an toàn
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(commandText, conn))
                    {
                        cmd.Parameters.AddWithValue("@idBan", idBan);
                        cmd.Parameters.AddWithValue("@giaTien", giaTien);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm bàn mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // === THAY ĐỔI THEO YÊU CẦU ===
                // Xóa 2 dòng this.Close() và this.DialogResult

                // 1. Xóa ô giá tiền
                txtGiaTien.Text = "";

                // 2. Tải mã bàn tiếp theo (ví dụ: B012)
                LoadNextTableID();
                // ==============================
            }
            catch (SqlException ex)
            {
                // Xử lý lỗi trùng mã
                foreach (SqlError error in ex.Errors)
                {
                    string message;
                    switch (error.Number)
                    {
                        case 2627:
                            message = "Mã bàn trùng lặp.";
                            break;
                        default:
                            message = $"Lỗi SQL: {error.Message}";
                            break;
                    }
                    MessageBox.Show(message, "Thêm thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi chung: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---
        // SỬA 4: ĐẢM BẢO NÚT HỦY HOẠT ĐỘNG (ĐÓNG FORM)
        // ---
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close(); // Dòng này sẽ đóng form "frmThemBanMoi"
        }

        // --- Các sự kiện trống (không cần dùng) ---
        private void txtGiaTien_TextChanged(object sender, EventArgs e) { }
        private void lblMaBan_Click(object sender, EventArgs e) { }
        private void txtMaBan_TextChanged(object sender, EventArgs e) { }
        private void lblGiaTien_Click(object sender, EventArgs e) { }
        private void txtMaBan_TextChanged_1(object sender, EventArgs e) { }
        private void lblMaBan_Click_1(object sender, EventArgs e) { }
    }
}