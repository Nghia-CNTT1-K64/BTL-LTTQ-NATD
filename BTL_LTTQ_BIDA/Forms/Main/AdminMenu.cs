using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using BTL_LTTQ_BIDA.Data;
using BTL_LTTQ_BIDA.Classes;
using BTL_LTTQ_BIDA.Forms.Account;
using BTL_LTTQ_BIDA.Forms.Table;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class AdminMenu : Form
    {
        private readonly DataConnect dtBase = new DataConnect();
        private string oldIDDV = string.Empty;
        private StatisticsControl ucThongKe;
        private const string CurrencySuffix = " VNĐ";

        private NhanVien currentUser;

        public AdminMenu()
        {
            InitializeComponent();
        }

        public AdminMenu(NhanVien nv) : this()
        {
            currentUser = nv;
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
        public void UpdateDichVu()
        {
            try
            {
                var table = dtBase.ExecuteQuery("SELECT * FROM DICHVU ORDER BY IDDV");
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

                    dgvDichVu.Rows.Add(id, tenDV, string.Format(CultureInfo.CurrentCulture, "{0:N0}{1}", giaTien, CurrencySuffix), hienThi ? "Có" : "Không", soLuong);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách dịch vụ:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Close();
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

                var giaText = txtGiaDV.Text.Replace(CurrencySuffix, "").Trim();
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
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SHOW / HIDE SERVICE
        private void btnBoHienThi_Click(object sender, EventArgs e) => SetServiceVisibility(false);
        private void btnHienThiLai_Click(object sender, EventArgs e) => SetServiceVisibility(true);

        private void SetServiceVisibility(bool isVisible)
        {
            if (dgvDichVu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần thay đổi hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = Convert.ToString(dgvDichVu.SelectedRows[0].Cells["IDDV"].Value) ?? string.Empty;

            var sql = "UPDATE DICHVU SET HIENTHI = @p0 WHERE IDDV = @p1";
            dtBase.ExecuteNonQuery(sql, new object[] { isVisible ? 1 : 0, id });

            MessageBox.Show(isVisible ? "Đã hiển thị lại dịch vụ." : "Đã ẩn dịch vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            UpdateDichVu();
        }

        // ADD SERVICE
        private void btnThemDV_Click(object sender, EventArgs e)
        {
            var frm = new FAddService();
            frm.FormClosed += (s, args) => UpdateDichVu();
            frm.Show();
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
                string.Format("Bạn có chắc muốn xóa dịch vụ '{0}' (Mã: {1}) không?", tenDV, id),
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
                MessageBox.Show("Lỗi khi xóa dịch vụ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQLNhanVien_Click_1(object sender, EventArgs e) => ShowPanel(pAdminNhanVien);
        private void btnQLDichVu_Click_1(object sender, EventArgs e) => ShowPanel(pAdminDichVu);
        private void btnQLBan_Click_1(object sender, EventArgs e) => ShowPanel(pAdminBan);

        private void AdminMenu_Load(object sender, EventArgs e)
        {
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // staff grid
            if (dgvNhanVien.Columns.Count == 0)
            {
                dgvNhanVien.Columns.Add("IDNV", "Mã Nhân Viên");
                dgvNhanVien.Columns.Add("HOTENNV", "Họ Tên");
                dgvNhanVien.Columns.Add("SODT", "Số Điện Thoại");
                dgvNhanVien.Columns.Add("NGAYSINH", "Ngày Sinh");
                dgvNhanVien.Columns.Add("CCCD", "CCCD");
                dgvNhanVien.Columns.Add("QUYENADMIN", "Quyền Admin");
                dgvNhanVien.Columns.Add("TENDANGNHAP", "Tên Đăng Nhập");
                dgvNhanVien.Columns.Add("NGHIVIEC", "Nghỉ việc");
                dgvNhanVien.Columns.Add("HIENTHI", "Trạng Thái");
            }

            UpdateNhanVien();
            dgvNhanVien.RowPrePaint += dgvNhanVien_RowPrePaint;
            btnTuyenDungLai.Enabled = false;
            btnChinhSua.Enabled = false;
            btnChoNghiViec.Enabled = false;
            btnTuyenDung.Enabled = false;

            // tables
            if (dgvBan.Columns.Count == 0)
            {
                dgvBan.Columns.Add("IDBAN", "Mã Bàn");
                dgvBan.Columns.Add("GIATIEN", "Giá tiền");
                dgvBan.Columns.Add("TRANGTHAI", "Trạng Thái");
            }
            updateBan();
            btnChinhSuaBan.Enabled = false;
            txtMaBan.ReadOnly = true;

            InitDgvDichVu();
            UpdateDichVu();

            txtMaDV.ReadOnly = true;
            txtMaDV.TabStop = false;

            InitKhachHang();
            UpdateKhachHang();
        }

        // quản lí nhân viên
        private void UpdateNhanVien()
        {
            dgvNhanVien.Rows.Clear();

            var table = dtBase.ExecuteQuery("SELECT * FROM NHANVIEN WHERE QUYENADMIN IS NOT NULL");
            if (table == null)
                return;

            foreach (DataRow row in table.Rows)
            {
                string id = Convert.ToString(row["IDNV"]);
                string sdt = Convert.ToString(row["SODT"]);
                string tenNV = Convert.ToString(row["HOTENNV"]);

                string ngaySinhStr = string.Empty;
                if (row["NGAYSINH"] != DBNull.Value)
                {
                    DateTime ngaySinh = (DateTime)row["NGAYSINH"];
                    ngaySinhStr = ngaySinh.ToString("dd/MM/yyyy");
                }

                string cccd = Convert.ToString(row["CCCD"]);

                bool isAdmin = row["QUYENADMIN"] != DBNull.Value ? Convert.ToBoolean(row["QUYENADMIN"]) : false;
                string quyenAdmin = isAdmin ? "Có" : "Không";

                string tenDangNhap = Convert.ToString(row["TENDANGNHAP"]);

                bool daDuyet = row["HIENTHI"] != DBNull.Value ? Convert.ToBoolean(row["HIENTHI"]) : false;
                string duyet = daDuyet ? "Đã duyệt" : "Chờ duyệt";

                bool nghiViec = row["NGHIVIEC"] != DBNull.Value ? Convert.ToBoolean(row["NGHIVIEC"]) : false;
                string trangThaiNV = nghiViec ? "Đang làm" : "Nghỉ việc";

                dgvNhanVien.Rows.Add(id, tenNV, sdt, ngaySinhStr, cccd, quyenAdmin, tenDangNhap, trangThaiNV, duyet);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            using (var dlg = new AccountToApproveDlg())
            {
                dlg.ShowDialog();
            }
            Show();
        }

        // lấy ttin ra textbox
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnChinhSua.Enabled = true;
                btnChoNghiViec.Enabled = true;
                btnTuyenDung.Enabled = true;

                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtID.Text = Convert.ToString(row.Cells["IDNV"].Value);
                txtTenNV.Text = Convert.ToString(row.Cells["HOTENNV"].Value);
                txtSDT.Text = Convert.ToString(row.Cells["SODT"].Value);
                txtCCCD.Text = Convert.ToString(row.Cells["CCCD"].Value);
                txtTenDN.Text = Convert.ToString(row.Cells["TENDANGNHAP"].Value);
                cbQQTV.Checked = Convert.ToString(row.Cells["QUYENADMIN"].Value) == "Có";

                DateTime parsed;
                if (DateTime.TryParseExact(Convert.ToString(row.Cells["NGAYSINH"].Value), "dd/MM/yyyy", null, DateTimeStyles.None, out parsed))
                    dtpNgaySinh.Value = parsed;

                string trangThaiNV = Convert.ToString(row.Cells["NGHIVIEC"].Value);
                if (trangThaiNV == "Nghỉ việc")
                {
                    btnTuyenDungLai.Enabled = true;
                    btnChoNghiViec.Enabled = false;
                }
                else
                {
                    btnTuyenDungLai.Enabled = false;
                }
            }
        }

        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn chỉnh sửa thông tin nhân viên?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!KtraRong() || !KtraTrungThongTinNVKhacVoiCotDaChon() || !KtraDinhDang())
                return;

            string idNV = txtID.Text;
            string tenNV = txtTenNV.Text;
            string sdt = txtSDT.Text;
            string cccd = txtCCCD.Text;
            string tenDN = txtTenDN.Text;
            bool isAdmin = cbQQTV.Checked;
            string ngaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");

            string sql = "UPDATE NHANVIEN SET HOTENNV = @p0, SODT = @p1, CCCD = @p2, TENDANGNHAP = @p3, QUYENADMIN = @p4, NGAYSINH = @p5 WHERE IDNV = @p6";
            dtBase.ExecuteNonQuery(sql, new object[] { tenNV, sdt, cccd, tenDN, isAdmin ? 1 : 0, ngaySinh, idNV });

            MessageBox.Show("Đã chỉnh sửa thông tin nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVien();
        }

        private void btnChoNghiViec_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn cho nhân viên này nghỉ việc?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (dgvNhanVien.SelectedRows.Count == 0)
                return;

            string idNV = dgvNhanVien.SelectedRows[0].Cells["IDNV"].Value.ToString().Trim();
            string sql = "UPDATE NHANVIEN SET NGHIVIEC = 0 WHERE IDNV = @p0";
            dtBase.ExecuteNonQuery(sql, new object[] { idNV });

            MessageBox.Show("Đã cho nhân viên nghỉ việc thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVien();
        }

        private void dgvNhanVien_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvNhanVien.Rows[e.RowIndex];

            if (row.Cells["NGHIVIEC"] == null || row.Cells["NGHIVIEC"].Value == null)
                return;

            string trangThaiNV = Convert.ToString(row.Cells["NGHIVIEC"].Value);

            if (trangThaiNV == "Nghỉ việc")
            {
                row.DefaultCellStyle.BackColor = Color.LightGray;
                row.DefaultCellStyle.ForeColor = Color.DarkGray;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void btnTuyenDungLai_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn tuyển dụng lại nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (dgvNhanVien.SelectedRows.Count == 0)
                return;

            string idNV = dgvNhanVien.SelectedRows[0].Cells["IDNV"].Value.ToString().Trim();
            string sql = "UPDATE NHANVIEN SET NGHIVIEC = 1 WHERE IDNV = @p0";
            dtBase.ExecuteNonQuery(sql, new object[] { idNV });

            MessageBox.Show("Đã tuyển dụng lại nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVien();
            btnTuyenDungLai.Enabled = false;
            btnChoNghiViec.Enabled = true;
        }

        private void btnThemNV_Click(object sender, EventArgs e)
        {
            string sql = "SELECT TOP 1 IDNV FROM NHANVIEN ORDER BY IDNV DESC";
            DataTable table = dtBase.ExecuteQuery(sql);

            string newID = "NV001";
            if (table != null && table.Rows.Count > 0)
            {
                string lastID = table.Rows[0]["IDNV"].ToString();
                int numberPart = 1;
                if (lastID.Length > 2 && int.TryParse(lastID.Substring(2), out int tmp))
                    numberPart = tmp + 1;

                newID = "NV" + numberPart.ToString("000");
            }

            txtID.Text = newID;
            btnTuyenDung.Enabled = true;

            txtCCCD.Clear();
            txtSDT.Clear();
            txtTenDN.Clear();
            txtTenNV.Clear();
            cbQQTV.Checked = false;
        }

        private void btnTuyenDung_Click_1(object sender, EventArgs e)
        {
            if (!KtraRong() || !KtraTrungThongTinNVKhacVoiCotDaChon() || !KtraDinhDang())
                return;

            int quyen = cbQQTV.Checked ? 1 : 0;
            int hienthi = 1;
            string defaultPassword = "123456789";
            string sql = "INSERT INTO NHANVIEN(IDNV,SODT, HOTENNV, NGAYSINH, CCCD, TENDANGNHAP, MATKHAU, QUYENADMIN, HIENTHI) VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)";
            dtBase.ExecuteNonQuery(sql, new object[]
            {
                txtID.Text, txtSDT.Text, txtTenNV.Text, dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
                txtCCCD.Text, txtTenDN.Text, defaultPassword, quyen, hienthi
            });

            MessageBox.Show("Tuyển dụng nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVien();
        }

        // kiểm tra định dạng
        private bool KtraDinhDang()
        {
            if (!Regex.IsMatch(txtSDT.Text, @"^(03|05|07|08|09)\d{8}$"))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng Việt Nam!");
                txtSDT.Focus();
                return false;
            }
            if (!Regex.IsMatch(txtCCCD.Text, @"^\d{12}$"))
            {
                MessageBox.Show("CCCD phải gồm đúng 12 số!");
                txtCCCD.Focus();
                return false;
            }
            if (dtpNgaySinh.Value > DateTime.Now.AddYears(-17))
            {
                MessageBox.Show("Nhân viên phải ít nhất 17 tuổi!");
                return false;
            }
            if (dtpNgaySinh.Value < DateTime.Now.AddYears(-75))
            {
                MessageBox.Show("Ngày sinh không hợp lý!");
                return false;
            }
            return true;
        }

        // kiểm tra rỗng
        private bool KtraRong()
        {
            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                MessageBox.Show("Chưa nhập họ tên!");
                txtTenNV.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Chưa nhập số điện thoại!");
                txtSDT.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Chưa nhập CCCD!");
                txtCCCD.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenDN.Text))
            {
                MessageBox.Show("Chưa nhập tên đăng nhập!");
                txtTenDN.Focus();
                return false;
            }
            return true;
        }

        private bool KtraTrungThongTinNVKhacVoiCotDaChon()
        {
            string idNV = txtID.Text;

            string checkQuery = "SELECT * FROM NHANVIEN WHERE TENDANGNHAP = @p0 AND IDNV <> @p1";
            DataTable dtCheck = dtBase.ExecuteQuery(checkQuery, new object[] { txtTenDN.Text, idNV });
            if (dtCheck.Rows.Count > 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng chọn tên đăng nhập khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDN.Focus();
                return false;
            }

            string checkCCCDQuery = "SELECT * FROM NHANVIEN WHERE CCCD = @p0 AND IDNV <> @p1";
            DataTable dtCheckCCCD = dtBase.ExecuteQuery(checkCCCDQuery, new object[] { txtCCCD.Text, idNV });
            if (dtCheckCCCD.Rows.Count > 0)
            {
                MessageBox.Show("CCCD đã tồn tại! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCCCD.Focus();
                return false;
            }

            string checkSDTQuery = "SELECT * FROM NHANVIEN WHERE SODT = @p0 AND IDNV <> @p1";
            DataTable dtCheckSDT = dtBase.ExecuteQuery(checkSDTQuery, new object[] { txtSDT.Text, idNV });
            if (dtCheckSDT.Rows.Count > 0)
            {
                MessageBox.Show("Số điện thoại đã tồn tại! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return false;
            }

            return true;
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            if (txtSDT.Text.Length >= 10 && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            if (txtCCCD.Text.Length >= 12 && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnXuatDSNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT IDNV, HOTENNV, SODT, NGAYSINH, CCCD, QUYENADMIN, TENDANGNHAP, NGHIVIEC, HIENTHI FROM NHANVIEN";
                DataTable dt = dtBase.ExecuteQuery(sql);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Excel.Application excelApp = new Excel.Application { Visible = false };

                Excel.Workbook wb = excelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet ws = (Excel._Worksheet)wb.ActiveSheet;
                ws.Name = "DanhSachNhanVien";

                ws.Cells[1, 1] = "DANH SÁCH NHÂN VIÊN";
                Excel.Range title = ws.Range["A1", "I1"];
                title.Merge();
                title.Font.Bold = true;
                title.Font.Size = 16;
                title.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                string[] headers = { "Mã NV", "Họ Tên", "Số ĐT", "Ngày Sinh", "CCCD", "Quyền Admin", "Tên Đăng Nhập", "Nghỉ việc", "Trạng Thái" };
                for (int i = 0; i < headers.Length; i++)
                    ws.Cells[3, i + 1] = headers[i];

                Excel.Range headerRange = ws.Range["A3", "I3"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Color.LightGray;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                int row = 4;
                foreach (DataRow r in dt.Rows)
                {
                    ws.Cells[row, 1] = r["IDNV"].ToString();
                    ws.Cells[row, 2] = r["HOTENNV"].ToString();
                    ws.Cells[row, 3] = r["SODT"].ToString();

                    if (DateTime.TryParse(Convert.ToString(r["NGAYSINH"]), out DateTime ngaySinh))
                        ws.Cells[row, 4] = ngaySinh.ToString("dd/MM/yyyy");

                    ws.Cells[row, 5] = r["CCCD"].ToString();
                    ws.Cells[row, 6] = Convert.ToBoolean(r["QUYENADMIN"]) ? "Có" : "Không";
                    ws.Cells[row, 7] = r["TENDANGNHAP"].ToString();
                    ws.Cells[row, 8] = Convert.ToBoolean(r["NGHIVIEC"]) ? "Đang làm" : "Nghỉ việc";
                    ws.Cells[row, 9] = Convert.ToBoolean(r["HIENTHI"]) ? "Đã duyệt" : "Chờ duyệt";
                    row++;
                }

                Excel.Range usedRange = ws.Range["A3", string.Format("I{0}", row - 1)];
                usedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                ws.Columns.AutoFit();

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "DanhSachNhanVien.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                wb.Close();
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // quản lí bàn
        private void updateBan()
        {
            dgvBan.Rows.Clear();
            DataTable table = dtBase.ExecuteQuery("SELECT * FROM BAN");
            if (table == null) return;
            foreach (DataRow row in table.Rows)
            {
                string idBan = Convert.ToString(row["IDBAN"]);
                string giaTien = Convert.ToString(row["GIATIEN"]);
                string trangThai = Convert.ToString(row["TRANGTHAI"]);
                dgvBan.Rows.Add(idBan, giaTien, trangThai);
            }
        }

        private void dgvBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnChinhSuaBan.Enabled = true;
                DataGridViewRow row = dgvBan.Rows[e.RowIndex];
                txtMaBan.Text = Convert.ToString(row.Cells["IDBAN"].Value);
                txtGiaTien.Text = Convert.ToString(row.Cells["GIATIEN"].Value);
                cboTrangThai.Text = Convert.ToString(row.Cells["TRANGTHAI"].Value);
                btnChinhSuaBan.Enabled = true;
            }
        }

        private void btnChinhSuaBan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn chỉnh sửa thông tin bàn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string idBan = txtMaBan.Text;
            string giaTien = txtGiaTien.Text;
            string trangThai = cboTrangThai.Text;
            string sql = "UPDATE BAN SET GIATIEN = @p0, TRANGTHAI = @p1 WHERE IDBAN = @p2";
            dtBase.ExecuteNonQuery(sql, new object[] { giaTien, trangThai, idBan });

            MessageBox.Show("Đã chỉnh sửa thông tin bàn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            updateBan();
        }

        private void btnThemBan_Click(object sender, EventArgs e)
        {
            using (var addTableDlg = new AddTableDlg())
            {
                addTableDlg.ShowDialog();
            }
            updateBan();
        }

        /// <summary>
        /// / Quản lí khách hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQLKhachHang_Click(object sender, EventArgs e) => ShowPanel(pAdminKhachHang);

        private void InitKhachHang()
        {
            dgvKhachHang.Columns.Clear();
            dgvKhachHang.RowHeadersVisible = false;
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.ReadOnly = true;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhachHang.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (dgvKhachHang.Columns.Count == 0)
            {
                dgvKhachHang.Columns.Add("IDKH", "Mã Khách Hàng");
                dgvKhachHang.Columns.Add("HOTEN", "Họ Tên");
                dgvKhachHang.Columns.Add("SODT", "Số Điện Thoại");
                dgvKhachHang.Columns.Add("DCHI", "Địa Chỉ");
            }
        }

        private void UpdateKhachHang()
        {
            dgvKhachHang.Rows.Clear();

            DataTable table = dtBase.ExecuteQuery("SELECT * FROM KHACHHANG ORDER BY IDKH");
            if (table == null) return;

            foreach (DataRow row in table.Rows)
            {
                string id = Convert.ToString(row["IDKH"]);
                string tenKH = Convert.ToString(row["HOTEN"]);
                string sdt = Convert.ToString(row["SODT"]);
                string diaChi = Convert.ToString(row["DCHI"]);

                dgvKhachHang.Rows.Add(id, tenKH, sdt, diaChi);
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIDKH.Enabled = false;
            if (e.RowIndex >= 0)
            {
                btnChinhSuaKH.Enabled = true;

                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
                txtIDKH.Text = Convert.ToString(row.Cells["IDKH"].Value);
                txtHoTenKH.Text = Convert.ToString(row.Cells["HOTEN"].Value);
                txtSDTKH.Text = Convert.ToString(row.Cells["SODT"].Value);
                txtDiaChiKH.Text = Convert.ToString(row.Cells["DCHI"].Value);
            }
        }

        private bool isAddingCustomer = false;

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            if (!isAddingCustomer)
            {
                btnChinhSuaKH.Enabled = false;
                isAddingCustomer = true;
                btnThemKH.Text = "Lưu khách hàng";

                txtHoTenKH.Clear();
                txtDiaChiKH.Clear();
                txtSDTKH.Clear();

                txtIDKH.Text = GenerateNextCustomerID();
                txtIDKH.Enabled = false;
                txtHoTenKH.Focus();

                dgvKhachHang.ClearSelection();
                return;
            }

            string maKH = txtIDKH.Text.Trim();
            string tenKH = txtHoTenKH.Text.Trim();
            string diaChi = txtDiaChiKH.Text.Trim();
            string sdt = txtSDTKH.Text.Trim();
            btnChinhSuaKH.Enabled = true;

            if (string.IsNullOrEmpty(tenKH))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTenKH.Focus();
                return;
            }

            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDTKH.Focus();
                return;
            }

            if (!Regex.IsMatch(sdt, @"^(03|05|07|08|09)\d{8}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Phải gồm 10 chữ số và bắt đầu bằng 03, 05, 07, 08 hoặc 09.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDTKH.Focus();
                return;
            }

            // 🟩 Kiểm tra trùng số điện thoại
            string sqlCheckSDT = "SELECT COUNT(*) FROM KHACHHANG WHERE SODT = @p0";
            int countSDT = Convert.ToInt32(dtBase.ExecuteScalar(sqlCheckSDT, new object[] { sdt }));
            if (countSDT > 0)
            {
                MessageBox.Show("⚠️ Số điện thoại này đã tồn tại! Vui lòng kiểm tra lại.",
                                "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDTKH.Focus();
                return;
            }

            // 🟢 Xác nhận trước khi thêm
            var confirm = MessageBox.Show($"Bạn có chắc muốn thêm khách hàng '{tenKH}' không?",
                                          "Xác nhận thêm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            string sqlInsert = "INSERT INTO KHACHHANG (IDKH, HOTEN, DCHI, SODT) VALUES (@p0, @p1, @p2, @p3)";
            dtBase.ExecuteNonQuery(sqlInsert, new object[] { maKH, tenKH, diaChi, sdt });

            MessageBox.Show($"✅ Đã thêm khách hàng {tenKH} (Mã {maKH}) thành công!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            UpdateKhachHang();

            txtIDKH.Clear();
            txtHoTenKH.Clear();
            txtDiaChiKH.Clear();
            txtSDTKH.Clear();
            txtIDKH.Enabled = true;
            btnThemKH.Text = "Thêm khách hàng";
            isAddingCustomer = false;
        }



        private string GenerateNextCustomerID()
        {
            string sql = "SELECT TOP 1 IDKH FROM KHACHHANG ORDER BY IDKH DESC";
            DataTable dt = dtBase.ExecuteQuery(sql);

            if (dt == null || dt.Rows.Count == 0)
                return "KH001";

            string lastID = Convert.ToString(dt.Rows[0]["IDKH"]);
            string numberPart = lastID.Length > 2 ? lastID.Substring(2) : "0";
            int nextNum = 1;
            int.TryParse(numberPart, out nextNum);
            nextNum++;
            return "KH" + nextNum.ToString("D3");
        }

        private void btnXuatDSKH_Click(object sender, EventArgs e)
        {
            // 🟦 Hỏi xác nhận trước khi xuất
            var confirm = MessageBox.Show("Bạn có muốn xuất danh sách khách hàng ra Excel không?",
                                          "Xác nhận xuất dữ liệu",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                string sql = "SELECT IDKH, HOTEN, DCHI, SODT FROM KHACHHANG ORDER BY IDKH";
                DataTable dt = dtBase.ExecuteQuery(sql);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var excelApp = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                var wb = excelApp.Workbooks.Add(Type.Missing);
                var ws = (Microsoft.Office.Interop.Excel._Worksheet)wb.ActiveSheet;
                ws.Name = "DanhSachKhachHang";

                ws.Cells[1, 1] = "DANH SÁCH KHÁCH HÀNG";
                var titleRange = ws.Range["A1", "D1"];
                titleRange.Merge();
                titleRange.Font.Size = 16;
                titleRange.Font.Bold = true;
                titleRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                string[] headers = { "Mã KH", "Họ Tên", "Địa Chỉ", "Số Điện Thoại" };
                for (int i = 0; i < headers.Length; i++)
                    ws.Cells[3, i + 1] = headers[i];

                var headerRange = ws.Range["A3", "D3"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Color.LightGray;
                headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                int row = 4;
                foreach (DataRow dr in dt.Rows)
                {
                    ws.Cells[row, 1] = dr["IDKH"].ToString();
                    ws.Cells[row, 2] = dr["HOTEN"].ToString();
                    ws.Cells[row, 3] = dr["DCHI"].ToString();
                    ws.Cells[row, 4] = dr["SODT"].ToString();
                    row++;
                }

                ws.Range["A3", $"D{row - 1}"].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                ws.Columns.AutoFit();

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "DanhSachKhachHang.xlsx"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(sfd.FileName);
                    MessageBox.Show("✅ Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                wb.Close();
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file Excel:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool isEditingCustomer = false;

        private void btnChinhSuaKH_Click(object sender, EventArgs e)
        {
            if (!isEditingCustomer)
            {
                if (string.IsNullOrWhiteSpace(txtIDKH.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                isEditingCustomer = true;
                btnChinhSuaKH.Text = "Lưu";
                btnThemKH.Enabled = false;

                txtHoTenKH.ReadOnly = txtDiaChiKH.ReadOnly = txtSDTKH.ReadOnly = false;
                txtHoTenKH.BackColor = txtDiaChiKH.BackColor = txtSDTKH.BackColor = Color.LightYellow;

                txtHoTenKH.Focus();
                return;
            }

            string id = txtIDKH.Text.Trim();
            string ten = txtHoTenKH.Text.Trim();
            string diaChi = txtDiaChiKH.Text.Trim();
            string sdt = txtSDTKH.Text.Trim();

            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Tên khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTenKH.Focus();
                return;
            }
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDTKH.Focus();
                return;
            }
            if (!Regex.IsMatch(sdt, @"^(03|05|07|08|09)\d{8}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDTKH.Focus();
                return;
            }

            // 🟨 Kiểm tra trùng SDT (ngoại trừ khách hàng hiện tại)
            string sqlCheckSDT = "SELECT COUNT(*) FROM KHACHHANG WHERE SODT = @p0 AND IDKH <> @p1";
            int countSDT = Convert.ToInt32(dtBase.ExecuteScalar(sqlCheckSDT, new object[] { sdt, id }));
            if (countSDT > 0)
            {
                MessageBox.Show("⚠️ Số điện thoại này đã được sử dụng bởi khách hàng khác!",
                                "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDTKH.Focus();
                return;
            }

            // 🟡 Xác nhận trước khi lưu
            var confirm = MessageBox.Show($"Bạn có chắc muốn lưu thay đổi cho khách hàng '{ten}' không?",
                                          "Xác nhận chỉnh sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            string sql = "UPDATE KHACHHANG SET HOTEN = @p0, DCHI = @p1, SODT = @p2 WHERE IDKH = @p3";
            dtBase.ExecuteNonQuery(sql, new object[] { ten, diaChi, sdt, id });

            MessageBox.Show("✅ Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            UpdateKhachHang();
            btnChinhSuaKH.Text = "Chỉnh sửa";
            isEditingCustomer = false;
            btnThemKH.Enabled = true;

            txtHoTenKH.BackColor = txtDiaChiKH.BackColor = txtSDTKH.BackColor = Color.White;
        }



        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvKhachHang.SelectedRows[0].Cells["IDKH"].Value.ToString();
            string ten = dgvKhachHang.SelectedRows[0].Cells["HOTEN"].Value.ToString();

            // 🟥 Xác nhận trước khi xóa
            var confirm = MessageBox.Show($"⚠️ Bạn có chắc chắn muốn xóa khách hàng '{ten}' (Mã {id}) không?",
                                          "Xác nhận xóa khách hàng",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                string sqlCheck = "SELECT COUNT(*) FROM HOADON WHERE IDKH = @p0";
                int count = Convert.ToInt32(dtBase.ExecuteScalar(sqlCheck, new object[] { id }));

                if (count > 0)
                {
                    MessageBox.Show("❌ Khách hàng này đã có hóa đơn, không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sqlDel = "DELETE FROM KHACHHANG WHERE IDKH = @p0";
                dtBase.ExecuteNonQuery(sqlDel, new object[] { id });

                MessageBox.Show("✅ Đã xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateKhachHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa khách hàng:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}