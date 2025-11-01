using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Data;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class AdminMenu : Form
    {
        private readonly DataConnect dtBase = new DataConnect();
        private string oldIDDV = string.Empty;
        private StatisticsControl ucThongKe;
        private const string CurrencySuffix = " VNĐ";

        public AdminMenu()
        {
            InitializeComponent();
        }

        // FORM LOAD
        private void AdminMenu_Load(object sender, EventArgs e)
        {
            ShowPanel(pAdminNhanVien);
            InitDgvDichVu();
            UpdateDichVu();

            // Ensure ID textbox is never editable by the user
            txtMaDV.ReadOnly = true;
            txtMaDV.TabStop = false;
        }

        // DGV SETUP
        private void InitDgvDichVu()
        {
            dgvDichVu.Columns.Clear();
            dgvDichVu.RowHeadersVisible = false;
            dgvDichVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDichVu.AllowUserToAddRows = false;
            dgvDichVu.ReadOnly = true;
            dgvDichVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDichVu.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDichVu.Columns.Add("IDDV", "Mã DV");
            dgvDichVu.Columns["IDDV"].Width = 80;
            dgvDichVu.Columns["IDDV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDichVu.Columns.Add("TENDV", "Tên dịch vụ");
            dgvDichVu.Columns["TENDV"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvDichVu.Columns.Add("GIATIEN", "Giá tiền");
            dgvDichVu.Columns["GIATIEN"].Width = 120;
            dgvDichVu.Columns["GIATIEN"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvDichVu.Columns.Add("HIENTHI", "Hiển thị");
            dgvDichVu.Columns["HIENTHI"].Width = 100;
            dgvDichVu.Columns["HIENTHI"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDichVu.Columns.Add("SOLUONG", "Số lượng");
            dgvDichVu.Columns["SOLUONG"].Width = 100;
            dgvDichVu.Columns["SOLUONG"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // LOAD DATA
        private void UpdateDichVu()
        {
            try
            {
                var table = dtBase.ReadData("SELECT * FROM DICHVU ORDER BY IDDV");
                dgvDichVu.Rows.Clear();

                if (table == null || table.Rows.Count == 0)
                    return;

                foreach (DataRow row in table.Rows)
                {
                    var id = Convert.ToString(row["IDDV"]) ?? string.Empty;
                    var tenDV = Convert.ToString(row["TENDV"]) ?? string.Empty;

                    decimal.TryParse(Convert.ToString(row["GIATIEN"]), NumberStyles.Any,
                        CultureInfo.CurrentCulture, out decimal giaTien);

                    var hienThiVal = Convert.ToString(row["HIENTHI"]) ?? string.Empty;
                    bool hienThi = string.Equals(hienThiVal, "True", StringComparison.OrdinalIgnoreCase) || hienThiVal == "1";

                    int.TryParse(Convert.ToString(row["SOLUONG"]), out int soLuong);

                    dgvDichVu.Rows.Add(id, tenDV, $"{giaTien:N0}{CurrencySuffix}", hienThi ? "Có" : "Không", soLuong);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách dịch vụ:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // PANEL SWITCH
        private void ShowPanel(Panel targetPanel)
        {
            pAdminNhanVien.Visible = pAdminBan.Visible = pAdminDichVu.Visible = pAdminThongKe.Visible = false;
            targetPanel.Visible = true;

            if (targetPanel == pAdminDichVu)
                UpdateDichVu();
        }

        private void btnQLNhanVien_Click(object sender, EventArgs e) => ShowPanel(pAdminNhanVien);
        private void btnQLDichVu_Click(object sender, EventArgs e) => ShowPanel(pAdminDichVu);
        private void btnQLBan_Click(object sender, EventArgs e) => ShowPanel(pAdminBan);

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            pAdminThongKe.Controls.Clear();

            if (ucThongKe == null)
                ucThongKe = new StatisticsControl();

            ucThongKe.Dock = DockStyle.Fill;
            pAdminThongKe.Controls.Add(ucThongKe);

            ShowPanel(pAdminThongKe);
        }

        // NAVIGATION BACK TO MAIN
        private void btnTroVe_Click(object sender, EventArgs e)
        {
            Hide();
            var mainForm = Application.OpenForms["FMain"] as FMain;
            if (mainForm == null)
            {
                mainForm = new FMain();
                mainForm.FormClosed += (s, args) => Close();
            }
            mainForm.Show();
        }

        // DGV ROW SELECTED
        private void dgvDichVu_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDichVu.Rows.Count)
                return;

            var row = dgvDichVu.Rows[e.RowIndex];

            txtMaDV.Text = Convert.ToString(row.Cells["IDDV"].Value) ?? string.Empty;
            txtTenDV.Text = Convert.ToString(row.Cells["TENDV"].Value) ?? string.Empty;
            txtGiaDV.Text = (Convert.ToString(row.Cells["GIATIEN"].Value) ?? string.Empty).Replace(CurrencySuffix, "").Trim();
            txtSoLuong.Text = Convert.ToString(row.Cells["SOLUONG"].Value) ?? string.Empty;
            oldIDDV = txtMaDV.Text.Trim();

            var hienThiValue = (Convert.ToString(row.Cells["HIENTHI"].Value) ?? string.Empty).Trim();
            btnBoHienThi.Enabled = string.Equals(hienThiValue, "Có", StringComparison.OrdinalIgnoreCase);
            btnHienThiLai.Enabled = string.Equals(hienThiValue, "Không", StringComparison.OrdinalIgnoreCase);
        }

        // EDIT SERVICE
        private void btnChinhSuaDV_Click(object sender, EventArgs e)
        {
            bool isEditing = btnChinhSuaDV.Text == "Chỉnh sửa";

            // Allow editing only for name, price and quantity. ID must stay read-only.
            txtTenDV.ReadOnly = !isEditing;
            txtGiaDV.ReadOnly = !isEditing;
            txtSoLuong.ReadOnly = !isEditing;
            txtMaDV.ReadOnly = true; // never editable

            btnChinhSuaDV.Text = isEditing ? "Lưu" : "Chỉnh sửa";

            if (!isEditing)
                SaveServiceChanges();
        }

        private void SaveServiceChanges()
        {
            try
            {
                var newID = txtMaDV.Text.Trim();
                var ten = txtTenDV.Text.Trim();

                if (string.IsNullOrWhiteSpace(newID))
                    throw new Exception("Mã dịch vụ không được để trống!");
                if (string.IsNullOrWhiteSpace(ten))
                    throw new Exception("Tên dịch vụ không được để trống!");

                var giaText = txtGiaDV.Text.Replace("VNĐ", "").Trim();
                if (!decimal.TryParse(giaText, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal gia))
                    throw new Exception("Giá tiền không hợp lệ!");

                if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong))
                    throw new Exception("Số lượng không hợp lệ!");

                var sql = "UPDATE DICHVU SET IDDV = @p0, TENDV = @p1, GIATIEN = @p2, SOLUONG = @p3 WHERE IDDV = @p4";
                dtBase.ExecuteNonQuery(sql, new object[] { newID, ten, gia, soLuong, oldIDDV });

                MessageBox.Show("Đã lưu thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateDichVu();
                oldIDDV = newID;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SHOW / HIDE SERVICE
        private void btnBoHienThi_Click(object sender, EventArgs e) => SetServiceVisibility(false);
        private void btnHienThiLai_Click(object sender, EventArgs e) => SetServiceVisibility(true);

        private void SetServiceVisibility(bool isVisible)
        {
            if (dgvDichVu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần thay đổi hiển thị.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = Convert.ToString(dgvDichVu.SelectedRows[0].Cells["IDDV"].Value) ?? string.Empty;

            var sql = "UPDATE DICHVU SET HIENTHI = @p0 WHERE IDDV = @p1";
            dtBase.ExecuteNonQuery(sql, new object[] { isVisible ? 1 : 0, id });

            MessageBox.Show(isVisible ? "Đã hiển thị lại dịch vụ." : "Đã ẩn dịch vụ.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            UpdateDichVu();
        }

        // ADD SERVICE
        private void btnThemDV_Click(object sender, EventArgs e)
        {
            // Mở form FAddService ở dạng non-modal (song song, không chặn)
            var frm = new FAddService();
            frm.FormClosed += (s, args) =>
            {
                // Cập nhật danh sách dịch vụ sau khi form đóng
                UpdateDichVu();
            };
            frm.Show(); // ✅ KHÔNG DÙNG ShowDialog()
        }


        // DELETE SERVICE (double-click)
        private void dgvDichVu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDichVu.Rows.Count)
                return;

            var id = Convert.ToString(dgvDichVu.Rows[e.RowIndex].Cells["IDDV"].Value) ?? string.Empty;
            var tenDV = Convert.ToString(dgvDichVu.Rows[e.RowIndex].Cells["TENDV"].Value) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(id))
                return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa dịch vụ '{tenDV}' (Mã: {id}) không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var checkSql = "SELECT COUNT(*) FROM HOADONDV WHERE IDDV = @p0";
                int count = Convert.ToInt32(dtBase.ExecuteScalar(checkSql, new object[] { id }));

                if (count > 0)
                {
                    MessageBox.Show(
                        "Dịch vụ này đang được sử dụng trong hóa đơn, không thể xóa.\nHệ thống sẽ ẩn dịch vụ này thay thế.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var hideSql = "UPDATE DICHVU SET HIENTHI = 0 WHERE IDDV = @p0";
                    dtBase.ExecuteNonQuery(hideSql, new object[] { id });
                }
                else
                {
                    var deleteSql = "DELETE FROM DICHVU WHERE IDDV = @p0";
                    dtBase.ExecuteNonQuery(deleteSql, new object[] { id });

                    MessageBox.Show("Đã xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                UpdateDichVu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dịch vụ: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
