using BTL_LTTQ_BIDA.Class;
using BTL_LTTQ_BIDA.Data;
using BTL_LTTQ_BIDA.Forms.Table;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Classes;


namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class FHoaDon : Form
    {
        Function func = new Function();
        DataConnect dtbase = new DataConnect();

        DichVuBiDa dichVuBiDa = new DichVuBiDa();
        BillBiDa billBiDa = new BillBiDa();
        KhachHangBiDa khachhangBiDa = new KhachHangBiDa();

        DateTime gioBatDau; //thời gian bắt đầu chơi
        private string idhd;
        private string idkh;
        private NhanVien currentUser;

        public static int Trangthai { get; set; } //được lấy từ hàm TvHD_NodeMouseDoubleClick ở FMain

        public string IDHD //được lấy từ hàm TvHD_NodeMouseDoubleClick ở FMain
        {
            get { return idhd; }
            set { idhd = value; }
        }
        public string IDKH
        {
            get { return idkh; }
            set { idkh = value; }
        }


        public FHoaDon(string id_hd, NhanVien nv)
        {
            InitializeComponent();
            IDHD = id_hd;
            currentUser = nv;
        }
        

        private void FHoaDon_Load(object sender, EventArgs e)
        {

            try
            {
                string sql = $@"
                    SELECT h.IDHD, h.IDKH, h.IDNV, h.NGAYLAP, nv.HOTENNV 
                    FROM HOADON h
                    JOIN NHANVIEN nv ON h.IDNV = nv.IDNV
                    WHERE h.IDHD = '{IDHD}'";

                DataTable dt = dtbase.ReadData(sql);

                string idKH = "";
                if (dt.Rows.Count > 0)
                {
                    txtMaHD.Text = dt.Rows[0]["IDHD"].ToString();
                    idKH = dt.Rows[0]["IDKH"].ToString();
                    txtMaNV.Text = dt.Rows[0]["IDNV"].ToString();
                    txtTenNV.Text = dt.Rows[0]["HOTENNV"].ToString();

                    if (dt.Rows[0]["NGAYLAP"] != DBNull.Value)
                        dtpNgay.Value = Convert.ToDateTime(dt.Rows[0]["NGAYLAP"]);
                }

                // 🔹 Load danh sách khách hàng trước
                LoadKhachHang();

                // 🔹 Sau đó mới set giá trị đã chọn
                if (!string.IsNullOrEmpty(idKH))
                {
                    cboMaKH.SelectedValue = idKH;
                    IDKH = idKH; // để LoadKH() biết khách hàng nào
                    LoadKH();
                }

                LoadDichVu();
                LoadBan();
                LoadDichVuDaThem();
                TinhTongTienDV();
                SetupTrangThaiHoaDon();

                KhoaTextBox(true);
                txtDiaChi.Enabled = false;
                txtSDT.Enabled = false;
                txtTenKH.Enabled = false;
                KiemTraQuyenNut();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTrangThaiHoaDon()
        {
            try
            {
                if (Trangthai == 0)
                {
                    // ✅ Hóa đơn đang hoạt động
                    btnKetThucHD.Visible = true;
                    flpDichVu.Enabled = true;
                    btnThemDV.Enabled = true;
                    dgvHDDV.Enabled = true;
                    btnDoiBan.Visible = true;

                    // 🔹 Lấy giờ bắt đầu
                    string sqlGioBD = $@"
                        SELECT p.GIOBATDAU 
                        FROM HOADON h
                        JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
                        WHERE h.IDHD = '{IDHD}'";

                    DataTable dtGio = dtbase.ReadData(sqlGioBD);
                    if (dtGio.Rows.Count > 0 && dtGio.Rows[0]["GIOBATDAU"] != DBNull.Value)
                    {
                        gioBatDau = Convert.ToDateTime(dtGio.Rows[0]["GIOBATDAU"]);
                        timerChoi.Start();
                        lblThoiGianChoi.Text = "Đang tính giờ...";
                    }
                    else
                    {
                        lblThoiGianChoi.Text = "Chưa bắt đầu tính giờ";
                    }
                }
                else if (Trangthai == 1)
                {
                    // ❌ Hóa đơn đã kết thúc
                    btnKetThucHD.Visible = false;
                    timerChoi.Stop();
                    flpDichVu.Enabled = false;
                    btnThemDV.Enabled = false;
                    dgvHDDV.Enabled = false;
                    btnDoiBan.Visible = false;
                    btnXuatHD.Visible = true;

                    // 🔹 Lấy giờ bắt đầu và kết thúc để hiển thị thời gian chơi
                    string sqlTime = $@"
                        SELECT p.GIOBATDAU, p.GIOKETTHUC
                        FROM HOADON h
                        JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
                        WHERE h.IDHD = '{IDHD}'";

                    DataTable dtTime = dtbase.ReadData(sqlTime);
                    if (dtTime.Rows.Count > 0)
                    {
                        DateTime gioBD = Convert.ToDateTime(dtTime.Rows[0]["GIOBATDAU"]);
                        DateTime gioKT = Convert.ToDateTime(dtTime.Rows[0]["GIOKETTHUC"]);

                        TimeSpan thoiGianChoi = gioKT - gioBD;

                        // Tính giờ và phút rõ ràng
                        int gio = (int)thoiGianChoi.TotalHours;
                        int phut = thoiGianChoi.Minutes;

                        lblThoiGianChoi.Text = $"Thời gian chơi: {gio} giờ {phut} phút";
                    }
                    else
                    {
                        lblThoiGianChoi.Text = "Không có dữ liệu thời gian";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thiết lập trạng thái hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void LoadDichVu()
        {
            List<DichVu> dichVuList = dichVuBiDa.LoadDichVuList();
            foreach (DichVu item in dichVuList)
            {
                Button btn = new Button()
                {
                    Width = 100,
                    Height = 40,
                    Text = item.TenDV,
                    BackColor = Color.Green,
                    Tag = item.IDdv
                };
                flpDichVu.Controls.Add(btn);
                btn.Click += Btn_Click;
                btn.Cursor = Cursors.Hand;
            }
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button?.BackColor == Color.Green)
            {
                button.BackColor = Color.Red;
            }
            else if (button?.BackColor == Color.Red)
            {
                button.BackColor = Color.Green;
            }


        }

        //void LoadUnCheckHD()
        //{
        //    List<Bill> listbill = billBiDa.GetListUnCheckBillID();
        //    foreach (Bill item in listbill)
        //    {
        //        if (item.Idhd == idhd)
        //        {

        //            txtMaHD.Text = item.Idhd;
        //            txtMaKH.Text = item.Idkh;
        //        }
        //    }
        //}
        //void LoadCheckedHD()
        //{
        //    List<Bill> listbill = billBiDa.GetListCheckedBill();
        //    foreach (Bill item in listbill)
        //    {
        //        if (item.Idhd == idhd)
        //        {

        //            txtMaHD.Text = item.Idhd;
        //            txtMaKH.Text = item.Idkh;
        //        }
        //    }

        //}


        void LoadUnCheckHD()
        {
            string sql = $"SELECT IDHD, IDKH, NGAYLAP FROM HOADON WHERE IDHD = '{IDHD}'";
            DataTable dt = new DataConnect().ReadData(sql);
            if (dt.Rows.Count > 0)
            {
                txtMaHD.Text = dt.Rows[0]["IDHD"].ToString();
                cboMaKH.Text = dt.Rows[0]["IDKH"].ToString();

                // 🔹 Gán ngày hóa đơn
                if (dt.Rows[0]["NGAYLAP"] != DBNull.Value)
                    dtpNgay.Value = Convert.ToDateTime(dt.Rows[0]["NGAYLAP"]);
                else
                {
                    // Nếu ngày trống, tự động lấy từ mã hóa đơn
                    dtpNgay.Value = GetDateFromBillID(txtMaHD.Text);
                }
            }
        }
        void LoadCheckedHD()
        {
            string sql = $"SELECT IDHD, IDKH, NGAYLAP FROM HOADON WHERE IDHD = '{IDHD}'";
            DataTable dt = new DataConnect().ReadData(sql);
            if (dt.Rows.Count > 0)
            {
                txtMaHD.Text = dt.Rows[0]["IDHD"].ToString();
                cboMaKH.Text = dt.Rows[0]["IDKH"].ToString();

                if (dt.Rows[0]["NGAYLAP"] != DBNull.Value)
                    dtpNgay.Value = Convert.ToDateTime(dt.Rows[0]["NGAYLAP"]);
                else
                    dtpNgay.Value = GetDateFromBillID(txtMaHD.Text);
            }
        }

        private DateTime GetDateFromBillID(string idhd)
        {
            try
            {
                // VD: HD301020258 → lấy "30102025" (từ vị trí 2, độ dài 8)
                string datePart = idhd.Substring(2, 8);

                int day = int.Parse(datePart.Substring(0, 2));
                int month = int.Parse(datePart.Substring(2, 2));
                int year = int.Parse(datePart.Substring(4, 4));

                return new DateTime(year, month, day);
            }
            catch
            {
                // Nếu lỗi parse, trả về ngày hiện tại
                return DateTime.Now;
            }
        }


        void LoadKH()
        {
            //Version 1
            //KhachHang khachhang = khachhangBiDa.GetKhachhang(idkh);
            //txtTenKH.Text = khachhang.Hoten == null ? "" : khachhang.Hoten.ToString();
            //txtSDT.Text = khachhang.Sodt == null ? "" : khachhang.Sodt.ToString();
            //txtDiaChi.Text = khachhang.Dchi == null ? "" : khachhang.Dchi.ToString();


            //Version 2
            if (string.IsNullOrEmpty(IDKH))
                return; // Không có khách hàng -> thoát sớm

            KhachHang khachhang = khachhangBiDa.GetKhachhang(IDKH);
            if (khachhang == null) return;

            txtTenKH.Text = khachhang.Hoten ?? "";
            txtSDT.Text = khachhang.Sodt ?? "";
            txtDiaChi.Text = khachhang.Dchi ?? "";

        }

        private void LoadKhachHang()
        {
            string sql = "SELECT IDKH, HOTEN FROM KHACHHANG";
            DataTable dtKH = dtbase.ReadData(sql);
            func.FillComboBox(cboMaKH, dtKH, "IDKH", "IDKH"); // hiển thị mã KH
            cboMaKH.SelectedIndex = -1;
        }


        //hiển thị thông tin bàn ở dgvBan
        void LoadBan()
        {
            string sql = $@"
                SELECT 
                b.IDBAN AS [Mã bàn], 
                b.GIATIEN AS [Giá tiền], 
                p.GIOBATDAU AS [Giờ bắt đầu], 
                p.GIOKETTHUC AS [Giờ kết thúc]
                FROM PHIENCHOI p
                JOIN BAN b ON p.IDBAN = b.IDBAN
                JOIN HOADON h ON h.IDPHIEN = p.IDPHIEN
                WHERE h.IDHD = '{IDHD}'";

            DataTable dtBan = new DataConnect().ReadData(sql);
            dgvBan.DataSource = dtBan;

            // 🔹 Tăng độ rộng cho các cột thời gian để hiển thị đầy đủ ngày + giờ
            if (dgvBan.Columns.Contains("Giờ bắt đầu"))
            {
                dgvBan.Columns["Giờ bắt đầu"].Width = 180;  // hoặc 200 nếu bạn muốn
            }
            if (dgvBan.Columns.Contains("Giờ kết thúc"))
            {
                dgvBan.Columns["Giờ kết thúc"].Width = 180;
            }
        }


        //hiển thị thông tin các dịch vụ ở dgvHDDV
        void LoadDichVuDaThem()
        {
            string sql = $@"
                SELECT dv.IDDV, dv.TENDV, dv.GIATIEN, hddv.SOLUONG,
               (dv.GIATIEN * hddv.SOLUONG) AS THANHTIEN
                FROM HOADONDV hddv
                JOIN DICHVU dv ON hddv.IDDV = dv.IDDV
                WHERE hddv.IDHD = '{IDHD}'";

            DataTable dtDV = dtbase.ReadData(sql);
            dgvHDDV.DataSource = dtDV;
        }

        private void btnThemDV_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có chọn số lượng hay không
                int sl = (int)nud.Value;
                if (sl <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool daThemDichVu = false;

                // Duyệt qua các nút dịch vụ
                foreach (Control ct in flpDichVu.Controls)
                {
                    if (ct is Button btn && btn.BackColor == Color.Red)
                    {
                        if (btn.Tag == null) continue;
                        string iddv = btn.Tag.ToString();

                        // 🔹 Kiểm tra tồn kho
                        string sqlTon = $"SELECT SOLUONG FROM DICHVU WHERE IDDV = '{iddv}'";
                        DataTable dtTon = new DataConnect().ReadData(sqlTon);

                        if (dtTon.Rows.Count == 0)
                        {
                            MessageBox.Show($"Không tìm thấy dịch vụ {btn.Text} trong hệ thống!");
                            continue;
                        }

                        int ton = Convert.ToInt32(dtTon.Rows[0]["SOLUONG"]);
                        if (ton < sl)
                        {
                            MessageBox.Show($"Không đủ số lượng tồn kho cho {btn.Text}! (Hiện còn {ton})", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        // 🔹 Kiểm tra dịch vụ này đã có trong hóa đơn chưa
                        string sqlCheck = $"SELECT * FROM HOADONDV WHERE IDHD = '{IDHD}' AND IDDV = '{iddv}'";
                        DataTable dtCheck = new DataConnect().ReadData(sqlCheck);

                        if (dtCheck.Rows.Count > 0)
                        {
                            // ✅ Nếu đã có thì cập nhật thêm số lượng
                            string sqlUpdate = $"UPDATE HOADONDV SET SOLUONG = SOLUONG + {sl} WHERE IDHD = '{IDHD}' AND IDDV = '{iddv}'";
                            new DataConnect().UpdateData(sqlUpdate);
                        }
                        else
                        {
                            // ✅ Nếu chưa có thì thêm mới
                            string sqlInsert = $"INSERT INTO HOADONDV (IDHD, IDDV, SOLUONG) VALUES ('{IDHD}', '{iddv}', {sl})";
                            new DataConnect().UpdateData(sqlInsert);
                        }

                        // 🔹 Trừ tồn kho
                        string sqlTru = $"UPDATE DICHVU SET SOLUONG = SOLUONG - {sl} WHERE IDDV = '{iddv}'";
                        new DataConnect().UpdateData(sqlTru);

                        daThemDichVu = true; //đánh dấu đã thêm ít nhất 1 dịch vụ
                    }
                }

                // 🔹 Chỉ thông báo nếu có thêm dịch vụ thật sự
                if (daThemDichVu)
                {
                    LoadDichVuDaThem();
                    TinhTongTienDV();
                    MessageBox.Show("Đã thêm hoặc cập nhật dịch vụ vào hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dịch vụ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuyHoaDon_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn hủy hóa đơn này không?\n" +
                    "Hành động này sẽ hoàn lại số lượng dịch vụ và xóa dữ liệu hóa đơn!",
                    "Xác nhận hủy hóa đơn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                // 🔹 1. Lấy thông tin phiên chơi & trạng thái hóa đơn
                string sqlGetPhien = $"SELECT IDPHIEN, TRANGTHAI FROM HOADON WHERE IDHD = '{IDHD}'";
                DataTable dtPhien = dtbase.ReadData(sqlGetPhien);
                if (dtPhien.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn cần hủy!",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string idPhien = dtPhien.Rows[0]["IDPHIEN"].ToString();
                int trangThaiHD = Convert.ToInt32(dtPhien.Rows[0]["TRANGTHAI"]);

                // 🔹 2. Lấy ID bàn
                string sqlGetBan = $"SELECT IDBAN FROM PHIENCHOI WHERE IDPHIEN = '{idPhien}'";
                DataTable dtBan = dtbase.ReadData(sqlGetBan);
                string idBan = dtBan.Rows.Count > 0 ? dtBan.Rows[0]["IDBAN"].ToString() : null;

                // 🔹 3. Hoàn lại số lượng dịch vụ
                string sqlGetDV = $"SELECT IDDV, SOLUONG FROM HOADONDV WHERE IDHD = '{IDHD}'";
                DataTable dtDV = dtbase.ReadData(sqlGetDV);

                foreach (DataRow r in dtDV.Rows)
                {
                    string iddv = r["IDDV"].ToString();
                    int sl = Convert.ToInt32(r["SOLUONG"]);
                    string sqlHoan = $"UPDATE DICHVU SET SOLUONG = SOLUONG + {sl} WHERE IDDV = '{iddv}'";
                    dtbase.UpdateData(sqlHoan);
                }

                // 🔹 4. Xóa dữ liệu hóa đơn, phiên chơi, chi tiết dịch vụ
                dtbase.UpdateData($"DELETE FROM HOADONDV WHERE IDHD = '{IDHD}'");
                dtbase.UpdateData($"DELETE FROM HOADON WHERE IDHD = '{IDHD}'");
                dtbase.UpdateData($"DELETE FROM PHIENCHOI WHERE IDPHIEN = '{idPhien}'");

                // 🔹 5. Nếu hóa đơn đang xử lý thì cập nhật bàn về trống
                if (trangThaiHD == 0 && !string.IsNullOrEmpty(idBan))
                {
                    dtbase.UpdateData($"UPDATE BAN SET TRANGTHAI = 0 WHERE IDBAN = '{idBan}'");
                }

                // ✅ Nếu là hóa đơn đã kết thúc thì KHÔNG cập nhật bàn

                // 🔹 6. Cập nhật lại giao diện
                MessageBox.Show(
                    trangThaiHD == 0
                        ? "Đã hủy hóa đơn đang xử lý, bàn đã được trả về trạng thái trống!"
                        : "Đã hủy hóa đơn đã kết thúc (bàn giữ nguyên trạng thái).",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // 🔁 Cập nhật lại danh sách hóa đơn và bàn ở FMain
                foreach (Form f in Application.OpenForms)
                {
                    if (f is FMain mainForm)
                    {
                        mainForm.ReloadBillsTree();
                        mainForm.LoadTable();
                        break;
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy hóa đơn: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaKH.SelectedIndex == -1) return;

            string maKH = cboMaKH.SelectedValue.ToString();
            string sql = $"SELECT * FROM KHACHHANG WHERE IDKH = '{maKH}'";
            DataTable dt = new DataConnect().ReadData(sql);

            if (dt.Rows.Count > 0)
            {
                txtTenKH.Text = dt.Rows[0]["HOTEN"].ToString();
                txtSDT.Text = dt.Rows[0]["SODT"].ToString();
                txtDiaChi.Text = dt.Rows[0]["DCHI"].ToString();
            }
        }

        //sinh mã kh tự động
        private string GenerateNextCustomerID()
        {
            string sql = "SELECT TOP 1 IDKH FROM KHACHHANG ORDER BY IDKH DESC";
            DataTable dt = new DataConnect().ReadData(sql);

            if (dt.Rows.Count == 0)
                return "KH001";

            string lastID = dt.Rows[0]["IDKH"].ToString();
            int number = int.Parse(lastID.Substring(2)) + 1;
            return "KH" + number.ToString("D3");
        }

        private void txtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sdt = txtSDT.Text.Trim();
                if (string.IsNullOrEmpty(sdt))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = $"SELECT * FROM KHACHHANG WHERE SODT = '{sdt}'";
                DataTable dt = new DataConnect().ReadData(sql);

                if (dt.Rows.Count > 0)
                {
                    // ✅ Có khách hàng
                    cboMaKH.SelectedValue = dt.Rows[0]["IDKH"].ToString();
                    txtTenKH.Text = dt.Rows[0]["HOTEN"].ToString();
                    txtDiaChi.Text = dt.Rows[0]["DCHI"].ToString();

                    MessageBox.Show("Đã tìm thấy khách hàng trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnTaoKH.Visible = false;
                }
                else
                {
                    // ❌ Chưa có khách hàng
                    DialogResult result = MessageBox.Show(
                        "Khách hàng chưa tồn tại. Bạn có muốn tạo mới không?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        btnTaoKH.Visible = true;
                        txtTenKH.Focus();
                    }
                    else
                    {
                        btnTaoKH.Visible = false;
                    }
                }
            }
        }

        private void btnTaoKH_Click(object sender, EventArgs e)
        {
            string maKH = cboMaKH.Text.Trim();
            string tenKH = txtTenKH.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string sdt = txtSDT.Text.Trim();

            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrEmpty(tenKH))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus();
                return;
            }
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // Nếu người dùng không nhập mã KH → tự sinh
            if (string.IsNullOrEmpty(maKH))
            {
                maKH = GenerateNextCustomerID();
            }
            else
            {
                // Kiểm tra trùng mã KH nếu nhập tay
                string sqlCheck = $"SELECT * FROM KHACHHANG WHERE IDKH = '{maKH}'";
                DataTable dtCheck = new DataConnect().ReadData(sqlCheck);
                if (dtCheck.Rows.Count > 0)
                {
                    MessageBox.Show("Mã khách hàng đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Thêm mới khách hàng
            string sqlInsert = $"INSERT INTO KHACHHANG VALUES ('{maKH}', N'{tenKH}', N'{diaChi}', '{sdt}')";
            new DataConnect().UpdateData(sqlInsert);

            MessageBox.Show($"Thêm khách hàng thành công! (Mã: {maKH})", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnTaoKH.Visible = false;
            

            LoadKhachHang();
            cboMaKH.SelectedValue = maKH;
        }

        private void dgvHDDV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 🛡️ Không xử lý nếu click header hoặc ngoài dòng dữ liệu
                if (e.RowIndex < 0) return;

                // ✅ Lấy thông tin dịch vụ từ dòng được double-click
                string iddv = dgvHDDV.Rows[e.RowIndex].Cells["IDDV"].Value.ToString();
                string tenDV = dgvHDDV.Rows[e.RowIndex].Cells["TENDV"].Value.ToString();
                int sl = Convert.ToInt32(dgvHDDV.Rows[e.RowIndex].Cells["SOLUONG"].Value);

                // ✅ Hỏi xác nhận
                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn hủy dịch vụ '{tenDV}' (SL: {sl}) khỏi hóa đơn này không?",
                    "Xác nhận hủy dịch vụ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm != DialogResult.Yes)
                    return;

                // ✅ Hoàn lại số lượng tồn
                string sqlHoan = $"UPDATE DICHVU SET SOLUONG = SOLUONG + {sl} WHERE IDDV = '{iddv}'";
                dtbase.UpdateData(sqlHoan);

                // ✅ Xóa dịch vụ khỏi hóa đơn
                string sqlDelete = $"DELETE FROM HOADONDV WHERE IDHD = '{IDHD}' AND IDDV = '{iddv}'";
                dtbase.UpdateData(sqlDelete);

                // ✅ Load lại danh sách dịch vụ
                LoadDichVuDaThem();
                TinhTongTienDV();

                MessageBox.Show($"Đã hủy dịch vụ '{tenDV}' và hoàn lại {sl} vào tồn kho!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy dịch vụ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerChoi_Tick(object sender, EventArgs e)
        {
            TimeSpan thoiGianChoi = DateTime.Now - gioBatDau;
            lblThoiGianChoi.Text = "Thời gian chơi: " +
                $"{(int)thoiGianChoi.TotalHours:00}:{thoiGianChoi.Minutes:00}:{thoiGianChoi.Seconds:00}";

            // 🔹 Cập nhật tổng tiền theo thời gian thực
            TinhTongTienHD();
        }


        private void TinhTongTienDV()
        {
            string sql = $@"
                SELECT SUM(dv.GIATIEN * hddv.SOLUONG) AS TongTienDV
                FROM HOADONDV hddv
                JOIN DICHVU dv ON dv.IDDV = hddv.IDDV
                WHERE hddv.IDHD = '{IDHD}'";

            DataTable dt = dtbase.ReadData(sql);
            if (dt.Rows.Count > 0 && dt.Rows[0]["TongTienDV"] != DBNull.Value)
            {
                double tongTienDV = Convert.ToDouble(dt.Rows[0]["TongTienDV"]);
                txtTongTienDV.Text = tongTienDV.ToString(); 
            }
            else
            {
                txtTongTienDV.Text = "0";
            }

            // Sau khi cập nhật tiền DV → cập nhật lại tổng HD
            TinhTongTienHD();
        }

        //Version 1
        //private void TinhTongTienHD()
        //{
        //    try
        //    {
        //        string sql = $@"
        //            SELECT b.GIATIEN, p.GIOBATDAU, p.GIOKETTHUC, h.TRANGTHAI
        //            FROM HOADON h
        //            JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
        //            JOIN BAN b ON p.IDBAN = b.IDBAN
        //            WHERE h.IDHD = '{IDHD}'";

        //        DataTable dt = dtbase.ReadData(sql);
        //        if (dt.Rows.Count == 0) return;

        //        double giaBan = Convert.ToDouble(dt.Rows[0]["GIATIEN"]);
        //        DateTime gioBD = Convert.ToDateTime(dt.Rows[0]["GIOBATDAU"]);
        //        DateTime gioKT;

        //        // 🔹 Nếu hóa đơn đã kết thúc → lấy giờ kết thúc từ DB
        //        // 🔹 Nếu chưa kết thúc → lấy thời gian hiện tại
        //        if (Convert.ToInt32(dt.Rows[0]["TRANGTHAI"]) == 1 && dt.Rows[0]["GIOKETTHUC"] != DBNull.Value)
        //            gioKT = Convert.ToDateTime(dt.Rows[0]["GIOKETTHUC"]);
        //        else
        //            gioKT = DateTime.Now;

        //        // 🔹 Tính thời gian chơi (giờ)
        //        double gioChoi = (gioKT - gioBD).TotalHours;

        //        // 🔹 Làm tròn block 0.5 giờ
        //        double soBlock = Math.Ceiling(gioChoi / 0.5) * 0.5;

        //        double tienBan = giaBan * soBlock;
        //        double tienDV = string.IsNullOrEmpty(txtTongTienDV.Text) ? 0 : Convert.ToDouble(txtTongTienDV.Text);

        //        double tongTienHD = tienBan + tienDV;

        //        txtTienBan.Text = tienBan.ToString();
        //        txtTongTienHD.Text = tongTienHD.ToString();
        //    }
        //    catch
        //    {
        //        txtTienBan.Text = "0";
        //        txtTongTienHD.Text = txtTongTienDV.Text;
        //    }
        //}


        private void TinhTongTienHD()
        {
            try
            {
                // 🔹 Lấy thông tin bàn, giá, giờ bắt đầu/kết thúc
                string sql = $@"
                    SELECT b.GIATIEN, p.GIOBATDAU, p.GIOKETTHUC, h.TRANGTHAI, h.TONGTIEN
                    FROM HOADON h
                    JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
                    JOIN BAN b ON p.IDBAN = b.IDBAN
                    WHERE h.IDHD = '{IDHD}'";

                DataTable dt = dtbase.ReadData(sql);
                if (dt.Rows.Count == 0) return;

                double giaBan = Convert.ToDouble(dt.Rows[0]["GIATIEN"]);
                DateTime gioBD = Convert.ToDateTime(dt.Rows[0]["GIOBATDAU"]);
                DateTime gioKT;

                int trangThaiHD = Convert.ToInt32(dt.Rows[0]["TRANGTHAI"]);
                double tongTienDaLuu = Convert.ToDouble(dt.Rows[0]["TONGTIEN"]);

                // 🔹 Nếu hóa đơn đã kết thúc thì dùng giờ kết thúc
                if (trangThaiHD == 1 && dt.Rows[0]["GIOKETTHUC"] != DBNull.Value)
                    gioKT = Convert.ToDateTime(dt.Rows[0]["GIOKETTHUC"]);
                else
                    gioKT = DateTime.Now;

                // 🔹 Tính số giờ chơi hiện tại (chưa kết thúc)
                double gioChoi = (gioKT - gioBD).TotalHours;
                double soBlock = Math.Ceiling(gioChoi / 0.5) * 0.5;
                double tienBanHienTai = giaBan * soBlock;

                // 🔹 Tiền dịch vụ hiện tại
                double tienDV = string.IsNullOrEmpty(txtTongTienDV.Text) ? 0 : Convert.ToDouble(txtTongTienDV.Text);

                // 🔹 Tổng tiền = tiền hóa đơn đã có + tiền bàn hiện tại + tiền dịch vụ
                double tongTienHD = tongTienDaLuu + tienBanHienTai + tienDV;

                // 🔹 Cập nhật textbox
                txtTienBan.Text = tienBanHienTai.ToString("N0");
                txtTongTienHD.Text = tongTienHD.ToString("N0");
            }
            catch (Exception ex)
            {
                txtTienBan.Text = "0";
                txtTongTienHD.Text = "0";
                MessageBox.Show("Lỗi tính tổng tiền: " + ex.Message);
            }
        }



        private void btnKetThucHD_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn kết thúc hóa đơn này không?",
                    "Xác nhận kết thúc hóa đơn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                // 🔹 Lấy thông tin phiên chơi để lưu giờ kết thúc
                string sqlPhien = $@"
                    SELECT h.IDPHIEN, p.IDBAN
                    FROM HOADON h
                    JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
                    WHERE h.IDHD = '{IDHD}'";
                DataTable dt = dtbase.ReadData(sqlPhien);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiên chơi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string idPhien = dt.Rows[0]["IDPHIEN"].ToString();
                string idBan = dt.Rows[0]["IDBAN"].ToString();

                // 🔹 Ghi giờ kết thúc
                DateTime gioKetThuc = DateTime.Now;
                dtbase.UpdateData($"UPDATE PHIENCHOI SET GIOKETTHUC = '{gioKetThuc:yyyy-MM-dd HH:mm:ss}' WHERE IDPHIEN = '{idPhien}'");

                // 🔹 Lấy tổng tiền đã hiển thị
                double tongTienHD = 0;
                double.TryParse(txtTongTienHD.Text.Replace(",", ""), out tongTienHD);

                // 🔹 Cập nhật hóa đơn: lưu tổng tiền + đổi trạng thái
                string sqlUpdateHD = $@"
                    UPDATE HOADON 
                    SET TONGTIEN = {tongTienHD}, TRANGTHAI = 1 
                    WHERE IDHD = '{IDHD}'";
                dtbase.UpdateData(sqlUpdateHD);

                // 🔹 Cập nhật bàn về trạng thái trống
                dtbase.UpdateData($"UPDATE BAN SET TRANGTHAI = 0 WHERE IDBAN = '{idBan}'");

                // 🔹 Ngừng timer + thông báo
                timerChoi.Stop();
                MessageBox.Show($"Hóa đơn đã được kết thúc!\nTổng tiền: {tongTienHD:N0} VNĐ",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔹 Reload lại danh sách hóa đơn ở FMain
                foreach (Form f in Application.OpenForms)
                {
                    if (f is FMain mainForm)
                    {
                        mainForm.ReloadBillsTree();
                        mainForm.LoadTable(); // 🔁 cập nhật lại danh sách bàn
                        break;
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết thúc hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            try
            {
                // 🧾 1. Tạo ứng dụng Excel
                Excel.Application exApp = new Excel.Application();
                exApp.Visible = true;

                // 🧾 2. Tạo workbook và worksheet
                Excel.Workbook exBook = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
                exSheet.Name = "HoaDon_" + IDHD;

                // 🧾 3. Tiêu đề hóa đơn
                Excel.Range title = exSheet.Range["A1", "E1"];
                title.Merge();
                title.Value = "HÓA ĐƠN BILLIARD";
                title.Font.Size = 20;
                title.Font.Bold = true;
                title.Font.Color = Color.Blue;
                title.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // 🧾 4. Thông tin chung
                exSheet.Cells[3, 1] = "Mã hóa đơn:";
                exSheet.Cells[3, 2] = IDHD;
                exSheet.Cells[4, 1] = "Khách hàng:";
                exSheet.Cells[4, 2] = txtTenKH.Text;
                exSheet.Cells[5, 1] = "Số điện thoại:";
                exSheet.Cells[5, 2] = txtSDT.Text;
                exSheet.Cells[6, 1] = "Địa chỉ:";
                exSheet.Cells[6, 2] = txtDiaChi.Text;
                exSheet.Cells[7, 1] = "Ngày lập:";
                exSheet.Cells[7, 2] = dtpNgay.Value.ToString("dd/MM/yyyy");

                // 🧾 5. Thông tin bàn
                exSheet.Cells[9, 1] = "THÔNG TIN BÀN";
                exSheet.Cells[9, 1].Font.Bold = true;

                string sqlBan = $@"
                    SELECT b.IDBAN, b.GIATIEN, p.GIOBATDAU, p.GIOKETTHUC
                    FROM HOADON h
                    JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
                    JOIN BAN b ON p.IDBAN = b.IDBAN
                    WHERE h.IDHD = '{IDHD}'";

                DataTable dtBan = dtbase.ReadData(sqlBan);
                if (dtBan.Rows.Count > 0)
                {
                    exSheet.Cells[10, 1] = "Mã bàn";
                    exSheet.Cells[10, 2] = "Giá tiền";
                    exSheet.Cells[10, 3] = "Giờ bắt đầu";
                    exSheet.Cells[10, 4] = "Giờ kết thúc";
                    exSheet.Range["A10", "D10"].Font.Bold = true;

                    for (int i = 0; i < dtBan.Rows.Count; i++)
                    {
                        exSheet.Cells[i + 11, 1] = dtBan.Rows[i]["IDBAN"].ToString();
                        exSheet.Cells[i + 11, 2] = Convert.ToDouble(dtBan.Rows[i]["GIATIEN"]);
                        exSheet.Cells[i + 11, 3] = Convert.ToDateTime(dtBan.Rows[i]["GIOBATDAU"]).ToString("dd/MM/yyyy HH:mm");
                        exSheet.Cells[i + 11, 4] = Convert.ToDateTime(dtBan.Rows[i]["GIOKETTHUC"]).ToString("dd/MM/yyyy HH:mm");
                    }

                    // ✅ Định dạng tiền tệ cho cột giá bàn
                    exSheet.Range["B11", "B" + (10 + dtBan.Rows.Count)].NumberFormat = "#,##0.00";
                }

                // 🧾 6. Thông tin dịch vụ
                int startDVTitle = 13 + dtBan.Rows.Count;
                exSheet.Cells[startDVTitle, 1] = "THÔNG TIN DỊCH VỤ";
                exSheet.Cells[startDVTitle, 1].Font.Bold = true;

                string sqlDV = $@"
                    SELECT dv.IDDV, dv.TENDV, dv.GIATIEN, hddv.SOLUONG, (dv.GIATIEN * hddv.SOLUONG) AS THANHTIEN
                    FROM HOADONDV hddv
                    JOIN DICHVU dv ON hddv.IDDV = dv.IDDV
                    WHERE hddv.IDHD = '{IDHD}'";

                DataTable dtDV = dtbase.ReadData(sqlDV);
                int startRow = startDVTitle + 1;

                exSheet.Cells[startRow, 1] = "Mã DV";
                exSheet.Cells[startRow, 2] = "Tên dịch vụ";
                exSheet.Cells[startRow, 3] = "Giá";
                exSheet.Cells[startRow, 4] = "Số lượng";
                exSheet.Cells[startRow, 5] = "Thành tiền";
                exSheet.Range["A" + startRow, "E" + startRow].Font.Bold = true;

                for (int i = 0; i < dtDV.Rows.Count; i++)
                {
                    exSheet.Cells[startRow + 1 + i, 1] = dtDV.Rows[i]["IDDV"].ToString();
                    exSheet.Cells[startRow + 1 + i, 2] = dtDV.Rows[i]["TENDV"].ToString();
                    exSheet.Cells[startRow + 1 + i, 3] = Convert.ToDouble(dtDV.Rows[i]["GIATIEN"]);
                    exSheet.Cells[startRow + 1 + i, 4] = Convert.ToInt32(dtDV.Rows[i]["SOLUONG"]);
                    exSheet.Cells[startRow + 1 + i, 5] = Convert.ToDouble(dtDV.Rows[i]["THANHTIEN"]);
                }

                // ✅ Định dạng cột tiền dịch vụ
                if (dtDV.Rows.Count > 0)
                {
                    int endRow = startRow + dtDV.Rows.Count;
                    exSheet.Range["C" + (startRow + 1), "C" + endRow].NumberFormat = "#,##0.00";
                    exSheet.Range["E" + (startRow + 1), "E" + endRow].NumberFormat = "#,##0.00";
                }

                // 🧾 7. Tổng kết: Tiền bàn, Tiền DV, Tổng HD
                int lastRow = startRow + dtDV.Rows.Count + 3;

                exSheet.Cells[lastRow, 4] = "Tiền bàn:";
                exSheet.Cells[lastRow, 5] = Convert.ToDouble(txtTienBan.Text);

                exSheet.Cells[lastRow + 1, 4] = "Tiền dịch vụ:";
                exSheet.Cells[lastRow + 1, 5] = Convert.ToDouble(txtTongTienDV.Text);

                exSheet.Cells[lastRow + 2, 4] = "Tổng tiền hóa đơn:";
                exSheet.Cells[lastRow + 2, 5] = Convert.ToDouble(txtTongTienHD.Text);

                // ✅ Format tiền tệ
                exSheet.Range["E" + lastRow, "E" + (lastRow + 2)].NumberFormat = "#,##0.00";
                exSheet.Range["D" + lastRow, "D" + (lastRow + 2)].Font.Bold = true;
                exSheet.Range["E" + lastRow, "E" + (lastRow + 2)].Font.Bold = true;

                // 🧾 8. Tự động căn chỉnh
                exSheet.Columns.AutoFit();

                // ✅ Thông báo
                MessageBox.Show("Xuất hóa đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoiBan_Click(object sender, EventArgs e)
        {
            try
            {
                if (Trangthai != 0)
                {
                    MessageBox.Show("Chỉ có thể đổi bàn khi hóa đơn đang xử lý!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy ID bàn hiện tại của hóa đơn
                string sql = $@"
                    SELECT p.IDBAN 
                    FROM HOADON h
                    JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
                    WHERE h.IDHD = '{IDHD}'";
                DataTable dt = dtbase.ReadData(sql);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy bàn hiện tại!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string idBanCu = dt.Rows[0]["IDBAN"].ToString();

                // Mở form đổi bàn
                FChangeTable fChange = new FChangeTable(IDHD, idBanCu);
                fChange.ShowDialog();

                // Sau khi đổi bàn xong → cập nhật lại giao diện
                LoadBan();
                TinhTongTienHD();

                foreach (Form f in Application.OpenForms)
                {
                    if (f is FMain mainForm)
                    {
                        mainForm.LoadTable();
                        mainForm.ReloadBillsTree();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form đổi bàn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //nghĩa
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // 🟢 1. Kiểm tra rỗng
                if (string.IsNullOrWhiteSpace(txtTenKH.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenKH.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiaChi.Focus();
                    return;
                }

                // 🟢 2. Lấy mã khách hàng hiện tại
                if (cboMaKH.SelectedValue == null)
                {
                    MessageBox.Show("Chưa chọn khách hàng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idKH = cboMaKH.SelectedValue.ToString();

                // 🟢 3. Cập nhật lại thông tin khách hàng trong DB
                //string sqlUpdate = $@"
                //    UPDATE KHACHHANG 
                //    SET HOTEN = N'{txtTenKH.Text.Replace("'", "''")}', 
                //    DCHI = N'{txtDiaChi.Text.Replace("'", "''")}', 
                //    SODT = '{txtSDT.Text.Replace("'", "''")}'
                //    WHERE IDKH = '{idKH}'";

                //dtbase.UpdateData(sqlUpdate);

                string sqlUpdateHD = $@"
                    UPDATE HOADON 
                    SET IDKH = '{idKH}'
                    WHERE IDHD = '{IDHD}'";
                dtbase.UpdateData(sqlUpdateHD);

                MessageBox.Show("Đã lưu thông tin khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🟢 4. Khóa lại giao diện sau khi lưu
                KhoaTextBox(true);
                btnLuu.Visible = false;
                btnHuySua.Visible = false;
                btnSuaHoaDon.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnHuySua_Click(object sender, EventArgs e)
        {
            KhoaTextBox(true);
            btnLuu.Visible = false;
            btnHuySua.Visible = false;
            btnSuaHoaDon.Enabled = true;


            // Tải lại dữ liệu gốc
            FHoaDon_Load(sender, e);
        }
        private void btnSuaHoaDon_Click(object sender, EventArgs e)
        {
            KhoaTextBox(false);
            btnLuu.Visible = true;
            btnHuySua.Visible = true;
            btnSuaHoaDon.Enabled = false;
        }
        private void KhoaTextBox(bool khoa)
        {          
            cboMaKH.Enabled = !khoa;

        }
        private void KiemTraQuyenNut()
        {
            try
            {
                //Lấy thông tin giờ kết thúc
                string sql = $@"
            SELECT p.GIOKETTHUC
            FROM HOADON h
            JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN
            WHERE h.IDHD = '{IDHD}'";
                DataTable dt = dtbase.ReadData(sql);

                bool coGioKetThuc = (dt.Rows.Count > 0 && dt.Rows[0]["GIOKETTHUC"] != DBNull.Value);

                // 🟢 Nút Sửa: chỉ bật khi hóa đơn đã có giờ kết thúc
                btnSuaHoaDon.Enabled = (currentUser.QuyenAdmin && coGioKetThuc);

                // 🟢 Nút Hủy hóa đơn:
                // Nếu đã kết thúc -> chỉ Admin được hủy
                // Nếu chưa kết thúc -> ai cũng có thể hủy
                if (coGioKetThuc)
                    btnHuyHoaDon.Enabled = currentUser.QuyenAdmin;
                else
                    btnHuyHoaDon.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra quyền nút: " + ex.Message);
            }
        }
    }
}
