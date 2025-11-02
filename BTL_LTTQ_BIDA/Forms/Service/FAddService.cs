using BTL_LTTQ_BIDA.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Utils;
namespace BTL_LTTQ_BIDA.Forms
{
    public partial class FAddService : Form
    {
        private readonly DataConnect dtBase = new DataConnect();

        public FAddService()
        {
            InitializeComponent();
        }

        // ===========================================================
        // 🔹 Khi form load
        // ===========================================================
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GenerateNewServiceID();
            UIStyler.ApplyFormStyle(this);
        }

        // ===========================================================
        // 🧩 Sinh mã dịch vụ mới (VD: DV001, DV002, ...)
        // ===========================================================
        private void GenerateNewServiceID()
        {
            try
            {
                string sql = "SELECT TOP 1 IDDV FROM DICHVU ORDER BY IDDV DESC";
                DataTable table = dtBase.ReadData(sql);

                string newID = "DV001";
                if (table.Rows.Count > 0)
                {
                    string lastID = table.Rows[0]["IDDV"].ToString();
                    if (lastID.Length >= 3 && int.TryParse(lastID.Substring(2), out int number))
                        newID = "DV" + (number + 1).ToString("000");
                }

                txtMaDichVu.Text = newID;
                txtGiaTien.Text = "0";
                txtSoLuong.Text = "0";
                txtTenDichVu.Clear();

                txtMaDichVu.ReadOnly = true;
                txtMaDichVu.TabStop = false;
                txtTenDichVu.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo mã dịch vụ:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===========================================================
        // 🔹 Nút Hủy → Yes = đóng form, No = không làm gì
        // ===========================================================
        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close(); // ✅ đóng form
            }
            // ❌ chọn No → không làm gì, ở lại form
        }

        // ===========================================================
        // 🔹 Nút Lưu → Yes = lưu DB, No = clear textbox (trừ mã)
        // ===========================================================
        private void btnLuu_Click(object sender, EventArgs e)
        {   
            try
            {
                ValidateInput(out string id, out string ten, out decimal gia, out int soLuong);

                DialogResult confirm = MessageBox.Show(
                    "Bạn có muốn lưu dịch vụ này không?",
                    "Xác nhận lưu",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.No)
                {
                    // ❌ Nếu chọn "No" → chỉ xóa trắng dữ liệu (trừ mã)
                    txtTenDichVu.Clear();
                    txtGiaTien.Text = "0";
                    txtSoLuong.Text = "0";
                    txtTenDichVu.Focus();
                    return;
                }

                // ✅ Nếu chọn "Yes" → tiến hành lưu
                string checkSql = "SELECT COUNT(*) FROM DICHVU WHERE IDDV = @p0";
                int count = Convert.ToInt32(dtBase.ExecuteScalar(checkSql, new object[] { id }));

                if (count > 0)
                {
                    MessageBox.Show("❌ Mã dịch vụ đã tồn tại. Vui lòng nhập mã khác.",
                        "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    GenerateNewServiceID();
                    return;
                }

                string insertSql = @"
                    INSERT INTO DICHVU (IDDV, TENDV, GIATIEN, HIENTHI, SOLUONG)
                    VALUES (@p0, @p1, @p2, 1, @p3)";
                dtBase.ExecuteNonQuery(insertSql, new object[] { id, ten, gia, soLuong });

                MessageBox.Show($"✅ Thêm dịch vụ '{ten}' thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Tạo mã mới cho lần thêm kế tiếp
                GenerateNewServiceID();
            }
            catch (SqlException ex)
            {
                string message;
                switch (ex.Number)
                {
                    case 207: message = "Giá trị bạn nhập không đúng định dạng."; break;
                    case 2627: message = "Mã dịch vụ bị trùng. Vui lòng nhập mã khác."; break;
                    case 2628: message = "Độ dài chuỗi vượt quá giới hạn cho phép."; break;
                    default: message = "Lỗi SQL: " + ex.Message; break;
                }

                MessageBox.Show("❌ " + message, "Thêm thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ " + ex.Message, "Thêm thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===========================================================
        // 🧩 Kiểm tra dữ liệu nhập vào
        // ===========================================================
        private void ValidateInput(out string id, out string ten, out decimal gia, out int soLuong)
        {
            id = txtMaDichVu.Text.Trim();
            ten = txtTenDichVu.Text.Trim();
            string giaStr = txtGiaTien.Text.Trim();
            string slStr = txtSoLuong.Text.Trim();

            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("Mã dịch vụ không được để trống.");
            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên dịch vụ không được để trống.");
            if (!decimal.TryParse(giaStr, out gia) || gia < 0)
                throw new Exception("Giá tiền không hợp lệ.");
            if (!int.TryParse(slStr, out soLuong) || soLuong < 0)
                throw new Exception("Số lượng không hợp lệ.");
        }
    }
}
