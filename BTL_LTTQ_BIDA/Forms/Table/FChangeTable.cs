using BTL_LTTQ_BIDA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_LTTQ_BIDA.Forms.Table
{
    public partial class FChangeTable : Form
    {
        private string idHD;
        private string idBanCu;
        DataConnect dtbase = new DataConnect();
        Function ft = new Function();

        public FChangeTable(string idhd, string idbanCu)
        {
            InitializeComponent();
            idHD = idhd;
            idBanCu = idbanCu;
        }

        private void FChangeTable_Load(object sender, EventArgs e)
        {
            try
            {
                txtBanCu.Text = idBanCu;
                txtBanCu.ReadOnly = true;

                string sqlBanMoi = "SELECT IDBAN FROM BAN WHERE TRANGTHAI = 0";
                DataTable dtBanMoi = dtbase.ReadData(sqlBanMoi);

                ft.FillComboBox(cboBanMoi, dtBanMoi, "IDBAN", "IDBAN");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu đổi bàn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoiBan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboBanMoi.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn bàn mới!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idBanMoi = cboBanMoi.SelectedValue.ToString();
                if (idBanMoi == idBanCu)
                {
                    MessageBox.Show("Bàn mới không được trùng với bàn cũ!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 1. Lấy thông tin phiên chơi hiện tại
                string sqlPhien = $@"
                    SELECT p.GIOBATDAU, b.GIATIEN
                    FROM HOADON h
                    JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
                    JOIN BAN b ON p.IDBAN = b.IDBAN
                    WHERE h.IDHD = '{idHD}'";
                DataTable dtPhien = dtbase.ReadData(sqlPhien);

                if (dtPhien.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiên chơi hiện tại!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime gioBD = Convert.ToDateTime(dtPhien.Rows[0]["GIOBATDAU"]);
                double giaBanCu = Convert.ToDouble(dtPhien.Rows[0]["GIATIEN"]);
                DateTime gioHienTai = DateTime.Now;

                // 🔹 2. Tính tiền đã chơi bàn cũ
                double gioChoi = (gioHienTai - gioBD).TotalHours;
                double soBlock = Math.Ceiling(gioChoi / 0.5) * 0.5;
                double tienThem = giaBanCu * soBlock;

                // 🔹 Xác nhận với người dùng
                DialogResult confirm = MessageBox.Show(
                    $"Khách đã chơi ở bàn {idBanCu} trong {soBlock:F1} giờ (~{tienThem:N0} VNĐ).\nBạn có chắc muốn đổi sang bàn {idBanMoi} không?",
                    "Xác nhận đổi bàn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                // 🔹 3. Cộng tiền bàn cũ vào hóa đơn
                dtbase.UpdateData($@"
                    UPDATE HOADON 
                    SET TONGTIEN = ISNULL(TONGTIEN, 0) + {tienThem}
                    WHERE IDHD = '{idHD}'");

                // 🔹 4. Cập nhật lại phiên chơi: đổi bàn + reset giờ bắt đầu
                dtbase.UpdateData($@"
                    UPDATE PHIENCHOI
                    SET IDBAN = '{idBanMoi}', GIOBATDAU = GETDATE()
                    WHERE IDPHIEN = (SELECT IDPHIEN FROM HOADON WHERE IDHD = '{idHD}')");

                // 🔹 5. Cập nhật trạng thái bàn
                dtbase.UpdateData($"UPDATE BAN SET TRANGTHAI = 0 WHERE IDBAN = '{idBanCu}'");
                dtbase.UpdateData($"UPDATE BAN SET TRANGTHAI = 1 WHERE IDBAN = '{idBanMoi}'");

                MessageBox.Show($"Đổi bàn thành công!\nĐã cộng {tienThem:N0} VNĐ vào hóa đơn.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔁 Cập nhật lại giao diện form chính
                foreach (Form f in Application.OpenForms)
                {
                    if (f is FMain mainForm)
                    {
                        mainForm.LoadTable();
                        mainForm.ReloadBillsTree();
                        break;
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đổi bàn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
