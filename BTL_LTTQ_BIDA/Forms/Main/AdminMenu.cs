using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class AdminMenu : Form
    {

        private DataConnect dtBase = new DataConnect();
        private NhanVien currentUser;
        public AdminMenu(NhanVien nv)
        {
            InitializeComponent();
        }
        private void ShowPanel(Panel targetPanel)
        {
            pAdminNhanVien.Visible = pAdminBan.Visible = pAdminDichVu.Visible = pAdminThongKe.Visible = false;
            targetPanel.Visible = true;
        }
        private void btnQLNhanVien_Click_1(object sender, EventArgs e) => ShowPanel(pAdminNhanVien);


        private void btnQLDichVu_Click_1(object sender, EventArgs e) => ShowPanel(pAdminDichVu);


        private void btnQLBan_Click_1(object sender, EventArgs e) => ShowPanel(pAdminBan);


        private void AdminMenu_Load(object sender, EventArgs e)
        {

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


        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            string trangThaiNV = row.Cells["NGHIVIEC"].Value.ToString();

            if (trangThaiNV == "Nghỉ việc")
            {
                // Nhân viên nghỉ (False = 0)
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
