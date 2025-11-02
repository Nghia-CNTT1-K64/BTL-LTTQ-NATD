using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Data;
using BTL_LTTQ_BIDA.Utils;

namespace BTL_LTTQ_BIDA.Forms.Account
{
    public partial class AccountToApproveDlg : Form
    {
        public AccountToApproveDlg()
        {
            InitializeComponent();
        }

        private void AccountToApproveDlg_Load(object sender, EventArgs e)
        {
            UIStyler.ApplyFormStyle(this);
            if (dgvNhanVien.Columns.Count == 0)
            {
                dgvNhanVien.Columns.Add("IDNV", "Mã Nhân Viên");
                
                dgvNhanVien.Columns.Add("HOTENNV", "Họ Tên");
                dgvNhanVien.Columns.Add("SODT", "Số Điện Thoại");
                dgvNhanVien.Columns.Add("NGAYSINH", "Ngày Sinh");
                dgvNhanVien.Columns.Add("CCCD", "CCCD");
                dgvNhanVien.Columns.Add("QUYENADMIN", "Quyền Admin");
                dgvNhanVien.Columns.Add("TENDANGNHAP", "Tên Đăng Nhập");
                dgvNhanVien.Columns.Add("HIENTHI", "Trạng Thái");
                dgvNhanVien.Columns.Add("NGHIVIEC", "Nghỉ việc");
            }
            UpdateNhanVienChuaDuocDuyet();
        }
        private void UpdateNhanVienChuaDuocDuyet()
        {
            dgvNhanVien.Rows.Clear(); // Xoá data cũ

            string commandString = "SELECT * FROM NHANVIEN WHERE HIENTHI =0";
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

                dgvNhanVien.Rows.Add(id, tenNV, sdt, ngaySinh.ToString("dd/MM/yyyy"), cccd, quyenAdmin, tenDangNhap, duyet);
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            btnDuyet.Enabled = true;
            btnXoa.Enabled = true;
        }
         

        private void btnDuyetTatCa_Click(object sender, EventArgs e)
        {
            string commandString = "UPDATE NHANVIEN SET HIENTHI = 1 WHERE HIENTHI = 0";
            DataConnect conn = new DataConnect();
            conn.UpdateData(commandString);
            MessageBox.Show("Đã duyệt tất cả tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVienChuaDuocDuyet();
        }

        private void btnXoaTatCa_Click(object sender, EventArgs e)
        {
            string commandString = "DELETE FROM NHANVIEN WHERE HIENTHI = 0";
            DataConnect conn = new DataConnect();
            conn.UpdateData(commandString);
            MessageBox.Show("Đã xóa tất cả tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVienChuaDuocDuyet();
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count == 0)
                return;
            string idNV = dgvNhanVien.SelectedRows[0].Cells["IDNV"].Value.ToString();
            string commandString = $"DELETE FROM NHANVIEN WHERE IDNV = {idNV}";
            DataConnect conn = new DataConnect();
            conn.UpdateData(commandString);
            MessageBox.Show("Đã xóa tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVienChuaDuocDuyet();
            btnDuyet.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnDuyet_Click_1(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count == 0)
                return;
            string idNV = dgvNhanVien.SelectedRows[0].Cells["IDNV"].Value.ToString();
            string commandString = $"UPDATE NHANVIEN SET HIENTHI = 1 WHERE IDNV = {idNV}";
            DataConnect conn = new DataConnect();
            conn.UpdateData(commandString);
            MessageBox.Show("Đã duyệt tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateNhanVienChuaDuocDuyet();
            btnDuyet.Enabled = false;
            btnXoa.Enabled = false;
        }
    }
}