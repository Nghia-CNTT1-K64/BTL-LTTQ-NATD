using BTL_LTTQ_BIDA.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

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

            try
            {
                // Sinh mã dịch vụ mới: DV001, DV002, ...
                string sql = "SELECT TOP 1 IDDV FROM DICHVU ORDER BY IDDV DESC";
                DataTable table = dtBase.ReadData(sql);

                string newID = "DV001";
                if (table.Rows.Count > 0)
                {
                    string lastID = table.Rows[0]["IDDV"].ToString(); // VD: DV010
                    if (lastID.Length >= 3 && int.TryParse(lastID.Substring(2), out int number))
                        newID = "DV" + (number + 1).ToString("000");
                }

                txtMaDichVu.Text = newID;
                txtGiaTien.Text = "0";
                txtSoLuong.Text = "0"; // ✅ Thêm dòng này để nhập số lượng mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo mã dịch vụ: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===========================================================
        // 🔹 Nút Hủy
        // ===========================================================
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ===========================================================
        // 🔹 Nút Lưu (Thêm dịch vụ)
        // ===========================================================
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // 🧩 Kiểm tra dữ liệu hợp lệ
                if (string.IsNullOrWhiteSpace(txtMaDichVu.Text))
                    throw new Exception("Mã dịch vụ không được để trống.");
                if (string.IsNullOrWhiteSpace(txtTenDichVu.Text))
                    throw new Exception("Tên dịch vụ không được để trống.");
                if (!decimal.TryParse(txtGiaTien.Text, out decimal giaTien) || giaTien < 0)
                    throw new Exception("Giá tiền không hợp lệ.");
                if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong < 0)
                    throw new Exception("Số lượng không hợp lệ.");

                // 🧠 Kiểm tra mã DV đã tồn tại chưa
                string checkSql = $"SELECT COUNT(*) FROM DICHVU WHERE IDDV = N'{txtMaDichVu.Text.Trim()}'";
                int count = Convert.ToInt32(dtBase.ReadData(checkSql).Rows[0][0]);
                if (count > 0)
                    throw new Exception("Mã dịch vụ đã tồn tại. Vui lòng nhập mã khác.");

                // ✅ Thực hiện thêm mới
                string sql = $@"
                    INSERT INTO DICHVU (IDDV, TENDV, GIATIEN, HIENTHI, SOLUONG)
                    VALUES (N'{txtMaDichVu.Text}', N'{txtTenDichVu.Text}', {giaTien}, 1, {soLuong})";

                dtBase.UpdateData(sql);

                MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SqlException ex)
            {
                string message;
                switch (ex.Number)
                {
                    case 207:
                        message = "Giá trị bạn nhập không đúng định dạng.";
                        break;
                    case 2627:
                        message = "Mã dịch vụ bị trùng. Vui lòng nhập mã khác.";
                        break;
                    case 2628:
                        message = "Độ dài chuỗi vượt quá giới hạn cho phép.";
                        break;
                    default:
                        message = "Lỗi SQL: " + ex.Message;
                        break;
                }


                MessageBox.Show(message, "Thêm thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thêm thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
