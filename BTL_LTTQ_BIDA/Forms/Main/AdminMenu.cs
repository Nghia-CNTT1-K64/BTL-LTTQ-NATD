using BTL_LTTQ_BIDA.Data;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BTL_LTTQ_BIDA.Classes;
using BTL_LTTQ_BIDA.Data;
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

        public AdminMenu()
        {
            InitializeComponent();
        }

        // ===========================================================
        // 🎨 UI BEAUTIFY – tạo hiệu ứng đẹp, giống CSS
        // ===========================================================
        //private void ApplyCustomUI()
        //{
        //    // 🎯 Nền tổng thể
        //    this.BackColor = Color.FromArgb(245, 248, 255); // màu nền nhẹ

        //    // 🎯 Style cho các panel
        //    foreach (Panel pnl in new[] { pAdminNhanVien, pAdminBan, pAdminDichVu, pAdminThongKe })
        //    {
        //        pnl.BackColor = Color.White;
        //        pnl.BorderStyle = BorderStyle.FixedSingle;
        //    }

        //    // 🎯 Style DataGridView
        //    dgvDichVu.BackgroundColor = Color.White;
        //    dgvDichVu.BorderStyle = BorderStyle.None;
        //    dgvDichVu.EnableHeadersVisualStyles = false;
        //    dgvDichVu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 144, 255);
        //    dgvDichVu.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        //    dgvDichVu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        //    dgvDichVu.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        //    dgvDichVu.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 250, 255);
        //    dgvDichVu.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 224, 138);
        //    dgvDichVu.DefaultCellStyle.SelectionForeColor = Color.Black;
        //    dgvDichVu.GridColor = Color.FromArgb(200, 200, 200);

        //    // 🎯 Style các button
        //    foreach (var btn in this.Controls.OfType<Button>())
        //        StyleButton(btn);

        //    foreach (var pnl in this.Controls.OfType<Panel>())
        //        foreach (var btn in pnl.Controls.OfType<Button>())
        //            StyleButton(btn);

        //    // 🎯 TextBox bo góc & viền sáng
        //    foreach (TextBox tb in this.Controls.OfType<TextBox>())
        //        StyleTextbox(tb);

        //    foreach (Panel pnl in this.Controls.OfType<Panel>())
        //        foreach (TextBox tb in pnl.Controls.OfType<TextBox>())
        //            StyleTextbox(tb);
        //}

        //private void StyleButton(Button btn)
        //{
        //    btn.FlatStyle = FlatStyle.Flat;
        //    btn.FlatAppearance.BorderSize = 0;
        //    btn.BackColor = Color.FromArgb(30, 144, 255);
        //    btn.ForeColor = Color.White;
        //    btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        //    btn.Cursor = Cursors.Hand;

        //    btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 120, 215);
        //    btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(30, 144, 255);
        //}

        //private void StyleTextbox(TextBox tb)
        //{
        //    tb.BorderStyle = BorderStyle.FixedSingle;
        //    tb.Font = new Font("Segoe UI", 9);
        //    tb.BackColor = Color.FromArgb(252, 252, 255);
        //}


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
            this.Close();
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

        private NhanVien currentUser;
        public AdminMenu(NhanVien nv)
        {
            InitializeComponent();
        }

        private void btnQLNhanVien_Click_1(object sender, EventArgs e) => ShowPanel(pAdminNhanVien);


        private void btnQLDichVu_Click_1(object sender, EventArgs e) => ShowPanel(pAdminDichVu);


        private void btnQLBan_Click_1(object sender, EventArgs e) => ShowPanel(pAdminBan);


        private void AdminMenu_Load(object sender, EventArgs e)
        {
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //quản lí nhân viên - Nghĩa
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
            btnTuyenDung.Enabled = false;//
            //quản lí bàn -Nghĩa
            if (dgvBan.Columns.Count == 0)
            {
                dgvBan.Columns.Add("IDBAN", "Mã Bàn");
                dgvBan.Columns.Add("GIATIEN", "Giá tiền");
                dgvBan.Columns.Add("TRANGTHAI", "Trạng Thái");
            }
            updateBan();
            btnChinhSuaBan.Enabled = false;
            txtMaBan.ReadOnly = true;
            //quản lí bàn -Nghĩa
            InitDgvDichVu();
            UpdateDichVu();

            txtMaDV.ReadOnly = true;
            txtMaDV.TabStop = false;

        }

        
        

        //quản lí nhân viên - Nghĩa
        private void UpdateNhanVien()
        {
            dgvNhanVien.Rows.Clear(); // Xoá data cũ

            string commandString = "SELECT * FROM NHANVIEN WHERE QUYENADMIN IS NOT NULL";
            DataConnect conn = new DataConnect();
            DataTable table = conn.ReadData(commandString);

            foreach (DataRow row in table.Rows)
            {
                string id = row["IDNV"].ToString(); 
                string sdt = row["SODT"].ToString();
                string tenNV = row["HOTENNV"].ToString();

                DateTime ngaySinh = DateTime.MinValue;
                if (row["NGAYSINH"] != DBNull.Value)
                {
                    ngaySinh = (DateTime)row["NGAYSINH"];
                }

                string cccd = row["CCCD"].ToString();

                bool isAdmin = row["QUYENADMIN"] != DBNull.Value ? (bool)row["QUYENADMIN"] : false;
                string quyenAdmin = isAdmin ? "Có" : "Không";

                string tenDangNhap = row["TENDANGNHAP"].ToString();

                bool daDuyet = row["HIENTHI"] != DBNull.Value ? (bool)row["HIENTHI"] : false;
                string duyet = daDuyet ? "Đã duyệt" : "Chờ duyệt";

                bool nghiViec = row["NGHIVIEC"] != DBNull.Value ? (bool)row["NGHIVIEC"] : false;
                string trangThaiNV = nghiViec ? "Đang làm" : "Nghỉ việc";

                dgvNhanVien.Rows.Add(id, tenNV, sdt, ngaySinh.ToString("dd/MM/yyyy"), cccd, quyenAdmin, tenDangNhap,trangThaiNV, duyet);
            }
        }


        //private void btnTroVe_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            AccountToApproveDlg dlg = new AccountToApproveDlg();
            dlg.ShowDialog();
            this.Show();

        }

        //lấy ttin ra textbox
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >=0)
            {
                btnChinhSua.Enabled = true;
                btnChoNghiViec.Enabled = true;
                btnTuyenDung.Enabled = true;

                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtID.Text = row.Cells["IDNV"].Value.ToString();
                txtTenNV.Text = row.Cells["HOTENNV"].Value.ToString();
                txtSDT.Text = row.Cells["SODT"].Value.ToString();
                txtCCCD.Text = row.Cells["CCCD"].Value.ToString();
                txtTenDN.Text = row.Cells["TENDANGNHAP"].Value.ToString();
                cbQQTV.Checked = row.Cells["QUYENADMIN"].Value.ToString() == "Có" ;
                dtpNgaySinh.Value = DateTime.ParseExact(row.Cells["NGAYSINH"].Value.ToString(), "dd/MM/yyyy", null);
                string trangThaiNV = row.Cells["NGHIVIEC"].Value.ToString();
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
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn chỉnh sửa thông tin nhân viên?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dr == DialogResult.Yes) {
                //ktra rỗng
                if(!KtraRong())
                {
                    return;
                }
                //ktra trùng 
                if (!KtraTrungThongTinNVKhacVoiCotDaChon())
                {
                    return;
                }
                //ktra định dạng
                if (!KtraDinhDang())
                {
                    return;
                }

                string idNV = txtID.Text;
                string tenNV = txtTenNV.Text;
                string sdt = txtSDT.Text;
                string cccd = txtCCCD.Text;
                string tenDN = txtTenDN.Text;
                bool isAdmin = cbQQTV.Checked;
                string ngaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
                string commandString = $"UPDATE NHANVIEN SET HOTENNV = N'{tenNV}', SODT = '{sdt}', CCCD = '{cccd}', TENDANGNHAP = '{tenDN}', QUYENADMIN = {(isAdmin ? 1 : 0)}, NGAYSINH = '{ngaySinh}' WHERE IDNV = '{idNV}'";
                DataConnect conn = new DataConnect();
                conn.UpdateData(commandString);
                MessageBox.Show("Đã chỉnh sửa thông tin nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateNhanVien();
            }
        }

        //
        private void btnChoNghiViec_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn cho nhân viên này nghỉ việc?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (dgvNhanVien.SelectedRows.Count == 0)
                    return;
                string idNV = dgvNhanVien.SelectedRows[0].Cells["IDNV"].Value.ToString().Trim();
                string commandString = $"UPDATE NHANVIEN SET NGHIVIEC = 0 WHERE IDNV = '{idNV}' ";
                DataConnect conn = new DataConnect();
                conn.UpdateData(commandString);
                MessageBox.Show("Đã cho nhân viên nghỉ việc thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateNhanVien();
            }
             
        }
        private void dgvNhanVien_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvNhanVien.Rows[e.RowIndex];

            // ⚙️ Kiểm tra null để tránh lỗi
            if (row.Cells["NGHIVIEC"] == null || row.Cells["NGHIVIEC"].Value == null)
                return;

            string trangThaiNV = row.Cells["NGHIVIEC"].Value.ToString();

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
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn tuyển dụng lại nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes) {
                if (dgvNhanVien.SelectedRows.Count == 0)
                    return;
                string idNV = dgvNhanVien.SelectedRows[0].Cells["IDNV"].Value.ToString().Trim();
                string commandString = $"UPDATE NHANVIEN SET NGHIVIEC = 1 WHERE IDNV = '{idNV}' ";
                DataConnect conn = new DataConnect();
                conn.UpdateData(commandString);
                MessageBox.Show("Đã tuyển dụng lại nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateNhanVien();
                btnTuyenDungLai.Enabled = false;
                btnChoNghiViec.Enabled = true;
            }
        }

        //
        private void btnThemNV_Click(object sender, EventArgs e)
        {
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

            txtID.Text = newID;
            btnTuyenDung.Enabled = true;

            txtCCCD.Clear();
            txtSDT.Clear();
            txtTenDN.Clear();
            txtTenNV.Clear();
            cbQQTV.Checked = false;
        }

        //
        private void btnTuyenDung_Click_1(object sender, EventArgs e)
        {

            //ktra rỗng
            if(!KtraRong())
            {
                return;
            }

            if (!KtraTrungThongTinNVKhacVoiCotDaChon())
            {
                return;
            }
            if (!KtraDinhDang())
            {
                return;
            }


            //thêm nhân viên
            int quyen = cbQQTV.Checked ? 1 : 0; //quyền admin
            int hienthi = 1; //ktra admin duyệt rồi
            string defaultPassword = "123456789"; //mật khẩu mặc định
            string sqlInsert = $"INSERT INTO NHANVIEN(IDNV,SODT, HOTENNV, NGAYSINH, CCCD, TENDANGNHAP, MATKHAU, QUYENADMIN, HIENTHI) " +
                $"VALUES(N'{txtID.Text}', '{txtSDT.Text} ', N'{txtTenNV.Text}', '{dtpNgaySinh.Value.ToString("yyyy-MM-dd")}', '{txtCCCD.Text}', '{txtTenDN.Text}', '{defaultPassword}', {quyen}, {hienthi})";
            //gọi update
            dtBase.UpdateData(sqlInsert);
            MessageBox.Show("Tuyển dụng nhân viên thành công!", "Thông báo");
            UpdateNhanVien();

        }
        //kiểm tra định dạng
        private bool KtraDinhDang()
        {
            // This method is intentionally left empty.
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text, @"^(03|05|07|08|09)\d{8}$"))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng Việt Nam!");
                txtSDT.Focus();
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtCCCD.Text, @"^\d{12}$"))
            {
                MessageBox.Show("CCCD phải gồm đúng 12 số!");
                txtCCCD.Focus();
                return false;
            }
            // Nhân viên >= 17 tuổi
            if (dtpNgaySinh.Value > DateTime.Now.AddYears(-17))
            {
                MessageBox.Show("Nhân viên phải ít nhất 17 tuổi!");
                return false;
            }

            // Không quá già (phòng trường hợp nhập nhầm)
            if (dtpNgaySinh.Value < DateTime.Now.AddYears(-75))
            {
                MessageBox.Show("Ngày sinh không hợp lý!");
                return false;
            }
            return true;
        }
        //kiểm tra rỗng
        private bool KtraRong()
        {
            // This method is intentionally left empty.
            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                MessageBox.Show("Chưa nhập họ tên!");
                txtTenNV.Focus();
                return false ;
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
            //ktra trùng tên đăng nhập
            string checkQuery = $"SELECT * FROM NHANVIEN WHERE TENDANGNHAP = '{txtTenDN.Text}' AND IDNV <> '{idNV}'";
            DataTable dtCheck = dtBase.ReadData(checkQuery);
            if (dtCheck.Rows.Count > 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng chọn tên đăng nhập khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDN.Focus();
                return false;
            }
            //ktra trung cccd
            string checkCCCDQuery = $"SELECT * FROM NHANVIEN WHERE CCCD = '{txtCCCD.Text}' AND IDNV <> '{idNV}'";
            DataTable dtCheckCCCD = dtBase.ReadData(checkCCCDQuery);
            if (dtCheckCCCD.Rows.Count > 0)
            {
                MessageBox.Show("CCCD đã tồn tại! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCCCD.Focus();
                return false;
            }
            //ktra sdt
            string checkSDTQuery = $"SELECT * FROM NHANVIEN WHERE SODT = '{txtSDT.Text}' AND IDNV <> '{idNV}'";
            DataTable dtCheckSDT = dtBase.ReadData(checkSDTQuery);
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
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (txtSDT.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
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
        private void btnXuatDSNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu nhân viên từ database
                string sql = "SELECT IDNV, HOTENNV, SODT, NGAYSINH, CCCD, QUYENADMIN, TENDANGNHAP, NGHIVIEC, HIENTHI FROM NHANVIEN";
                DataTable dt = dtBase.ReadData(sql);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Khởi tạo ứng dụng Excel
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;

                Excel.Workbook wb = excelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet ws = (Excel._Worksheet)wb.ActiveSheet;
                ws.Name = "DanhSachNhanVien";

                // Tiêu đề
                ws.Cells[1, 1] = "DANH SÁCH NHÂN VIÊN";
                Excel.Range title = ws.Range["A1", "I1"];
                title.Merge();
                title.Font.Bold = true;
                title.Font.Size = 16;
                title.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Header
                string[] headers = { "Mã NV", "Họ Tên", "Số ĐT", "Ngày Sinh", "CCCD", "Quyền Admin", "Tên Đăng Nhập", "Nghỉ việc", "Trạng Thái" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[3, i + 1] = headers[i];
                }

                Excel.Range headerRange = ws.Range["A3", "I3"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Color.LightGray;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Ghi dữ liệu
                int row = 4;
                foreach (DataRow r in dt.Rows)
                {
                    ws.Cells[row, 1] = r["IDNV"].ToString();
                    ws.Cells[row, 2] = r["HOTENNV"].ToString();
                    ws.Cells[row, 3] = r["SODT"].ToString();

                    if (DateTime.TryParse(r["NGAYSINH"].ToString(), out DateTime ngaySinh))
                        ws.Cells[row, 4] = ngaySinh.ToString("dd/MM/yyyy");

                    ws.Cells[row, 5] = r["CCCD"].ToString();
                    ws.Cells[row, 6] = ((bool)r["QUYENADMIN"]) ? "Có" : "Không";
                    ws.Cells[row, 7] = r["TENDANGNHAP"].ToString();
                    ws.Cells[row, 8] = ((bool)r["NGHIVIEC"]) ? "Đang làm" : "Nghỉ việc";
                    ws.Cells[row, 9] = ((bool)r["HIENTHI"]) ? "Đã duyệt" : "Chờ duyệt";
                    row++;
                }

                // Căn chỉnh & format
                Excel.Range usedRange = ws.Range["A3", $"I{row - 1}"];
                usedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                ws.Columns.AutoFit();

                // Hộp thoại lưu file
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
        //quan lí nhân viên - Nghĩa

        //quản lí bàn -Nghĩa
        private void updateBan()
        {
            dgvBan.Rows.Clear();
            string commandString = "SELECT * FROM BAN";
            DataConnect conn = new DataConnect();
            DataTable table = conn.ReadData(commandString);
            foreach (DataRow row in table.Rows)
            {
                string idBan = row["IDBAN"].ToString();
                string giaTien = row["GIATIEN"].ToString();
                string trangThai = row["TRANGTHAI"].ToString();
                dgvBan.Rows.Add(idBan, giaTien, trangThai);
            }
        }

        private void dgvBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >=0)
            {
                btnChinhSuaBan.Enabled = true;
                DataGridViewRow row = dgvBan.Rows[e.RowIndex];
                txtMaBan.Text = row.Cells["IDBAN"].Value.ToString();
                txtGiaTien.Text = row.Cells["GIATIEN"].Value.ToString();
                cboTrangThai.Text = row.Cells["TRANGTHAI"].Value.ToString();
                btnChinhSuaBan.Enabled = true;
            }
            
        }

        private void btnChinhSuaBan_Click(object sender, EventArgs e)
        {
            
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn chỉnh sửa thông tin bàn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if(dr == DialogResult.Yes) {
                
                string idBan = txtMaBan.Text;
                string giaTien = txtGiaTien.Text;
                string trangThai = cboTrangThai.Text;
                string commandString = $"UPDATE BAN SET GIATIEN = '{giaTien}', TRANGTHAI = N'{trangThai}' WHERE IDBAN = '{idBan}'";
                DataConnect conn = new DataConnect();
                conn.UpdateData(commandString);
                MessageBox.Show("Đã chỉnh sửa thông tin bàn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateBan();
            }
        }

        private void btnThemBan_Click(object sender, EventArgs e)
        {
            AddTableDlg addTableDlg = new AddTableDlg();
            addTableDlg.ShowDialog();
            updateBan();
        }









        //quản lí bàn -Nghĩa



    }
}
