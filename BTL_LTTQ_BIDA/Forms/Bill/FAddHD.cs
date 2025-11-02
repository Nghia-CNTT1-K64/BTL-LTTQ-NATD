using BTL_LTTQ_BIDA.Class;
using BTL_LTTQ_BIDA.Data;
using BTL_LTTQ_BIDA.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Utils;




namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class FAddHD : Form
    {
        Function func = new Function();

        TableBiDa tableBiDa = new TableBiDa();
        DichVuBiDa dichVuBiDa = new DichVuBiDa();


        DataConnect dtbase = new DataConnect();

        private string selectedTableId = null; // Lưu ID bàn đang chọn

        public FAddHD()
        {
            InitializeComponent();
        }

        private void FAddHD_Load(object sender, EventArgs e)
        {
            UIStyler.ApplyFormStyle(this);
            //load mã hóa đơn từ FMain khi ấn tạo hóa đơn mới
            txtMaHD.Text = FMain.IDHD;

            //load khách hàng
            LoadKhachHang();


            LoadDataDV();
            LoadTable();
            LoadDichVu();

            // 🔹 Lấy mã NV từ nhân viên đang đăng nhập
            txtMaNV.Text = FMain.IDNV_Current;
            txtMaNV.ReadOnly = true;

            // 🔹 Lấy tên nhân viên tương ứng
            string sql = $"SELECT HOTENNV FROM NHANVIEN WHERE IDNV = '{FMain.IDNV_Current}'";
            DataTable dtNV = new DataConnect().ReadData(sql);
            if (dtNV.Rows.Count > 0)
            {
                txtTenNV.Text = dtNV.Rows[0]["HOTENNV"].ToString();
                txtTenNV.ReadOnly = true;
            }

        }

        

        void LoadDataDV()
        {
            //string commandText = $"SELECT TENDV [Tên dịch vụ], GIATIEN [Giá tiền], SOLUONG [Số lượng] " +
            //                    $"FROM DICHVU, HOADONDV " +
            //                    $"WHERE DICHVU.IDDV = HOADONDV.IDDV " +
            //                    $"and HOADONDV.IDHD = '{FMain.IDHD}'";
            string cmtext = $"select dv.IDDV, TENDV, GIATIEN, hddv.SOLUONG" +
                            $" from DICHVU dv" +
                            $" join HOADONDV hddv on hddv.IDDV = dv.IDDV" +
                            $" where hddv.IDHD = '{FMain.IDHD}'";
            dgvHDDV.DataSource = dtbase.ReadData(cmtext);
        }

        void LoadTable()
        {
            flpBan.Controls.Clear();
            List<BTL_LTTQ_BIDA.Class.Table> tableList = tableBiDa.LoadTableList();
            foreach (BTL_LTTQ_BIDA.Class.Table item in tableList)
            {
                Button btn = new Button() { Width = 100, Height = 50 };
                if (item.Trangthai == 0)
                {
                    btn.Text = $"Bàn {item.Idban}{Environment.NewLine}trống";
                    btn.BackColor = Color.Green;
                    btn.Tag = item.Idban;
                    flpBan.Controls.Add(btn);
                }

                btn.Tag = item.Idban;
                btn.Click += BtnBan_Click;
                btn.Cursor = Cursors.Hand;

            }
        }


        private void BtnBan_Click(object sender, EventArgs e)
        {
            //Button button = sender as Button;
            //List<BTL_LTTQ_BIDA.Class.Table> tablelist = tableBiDa.LoadTableList();
            //foreach (BTL_LTTQ_BIDA.Class.Table item in tablelist)
            //{

            //    if (button?.Text == "Bàn " + item.Idban + Environment.NewLine + "trống")
            //    {
            //        MessageBox.Show(item.Idban.ToString());
            //        if (item.Trangthai == 0)
            //        {
            //            if (button.BackColor == Color.Green) button.BackColor = Color.Red;
            //            tableBiDa.UpdateDataTable(item, 1);
            //            button.BackColor = Color.Red;
            //            button.Text = "Bàn " + item.Idban + Environment.NewLine + "đang chọn";
            //            item.Trangthai = 1;
            //            break;
            //        }
            //    }
            //    else if (button?.Text == "Bàn " + item.Idban + Environment.NewLine + "đang chọn")
            //    {
            //        MessageBox.Show("hmm");
            //        tableBiDa.UpdateDataTable(item, 0);
            //        if (button.BackColor == Color.Red) button.BackColor = Color.Green;


            //        button.Text = "Bàn " + item.Idban + Environment.NewLine + "trống";
            //        item.Trangthai = 0;
            //        break;
            //    }

            //}

            Button button = sender as Button;
            if (button == null) return;

            string idBan = button.Tag.ToString();

            // Nếu bàn này đang được chọn → bỏ chọn
            if (selectedTableId == idBan)
            {
                button.BackColor = Color.Green;
                button.Text = $"Bàn {idBan}{Environment.NewLine}trống";
                selectedTableId = null;

                // Cập nhật trạng thái bàn về trống
                tableBiDa.UpdateDataTable(new BTL_LTTQ_BIDA.Class.Table { Idban = idBan }, 0);
                return;
            }

            // Bỏ chọn bàn cũ nếu có
            foreach (Control ctrl in flpBan.Controls)
            {
                if (ctrl is Button oldBtn && oldBtn.BackColor == Color.Red)
                {
                    string oldId = oldBtn.Tag.ToString();
                    oldBtn.BackColor = Color.Green;
                    oldBtn.Text = $"Bàn {oldId}{Environment.NewLine}trống";

                    // Cập nhật trạng thái bàn cũ về trống
                    tableBiDa.UpdateDataTable(new BTL_LTTQ_BIDA.Class.Table { Idban = oldId }, 0);
                }
            }

            // Chọn bàn mới
            button.BackColor = Color.Red;
            button.Text = $"Bàn {idBan}{Environment.NewLine}đang chọn";
            selectedTableId = idBan;

            // Cập nhật trạng thái bàn đang sử dụng
            tableBiDa.UpdateDataTable(new BTL_LTTQ_BIDA.Class.Table { Idban = idBan }, 1);
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
                flpDV.Controls.Add(btn);
                btn.Click += BtnDV_Click;
                btn.Cursor = Cursors.Hand;
            }
        }

        private void BtnDV_Click(object sender, EventArgs e)
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








        private void btnTao_Click(object sender, EventArgs e) //buttonAddHD
        {

            //Version 2: Đã tạo hóa đơn tạm từ FMain khi ấn tạo hóa đơn mới

            if (string.IsNullOrEmpty(selectedTableId))
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi tạo hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cboMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            


            // 🔹 Lấy ID phiên hiện tại của hóa đơn
            string sqlGetPhien = $"SELECT IDPHIEN FROM HOADON WHERE IDHD = '{FMain.IDHD}'";
            DataTable dtPhien = dtbase.ReadData(sqlGetPhien);
            if (dtPhien.Rows.Count == 0) return;
            string idPhien = dtPhien.Rows[0]["IDPHIEN"].ToString();

            // 🔹 Gán bàn cho phiên chơi + bắt đầu tính giờ
            string updatePhien = $"UPDATE PHIENCHOI SET IDBAN = '{selectedTableId}', GIOBATDAU = GETDATE() WHERE IDPHIEN = '{idPhien}'";
            dtbase.UpdateData(updatePhien);

            // 🔹 Cập nhật khách hàng cho hóa đơn
            string updateHD = $"UPDATE HOADON SET IDKH = '{cboMaKH.Text}' WHERE IDHD = '{FMain.IDHD}'";
            dtbase.UpdateData(updateHD);

            // 🔹 Cập nhật bàn sang trạng thái đang sử dụng
            tableBiDa.UpdateDataTable(new BTL_LTTQ_BIDA.Class.Table { Idban = selectedTableId }, 1);

            MessageBox.Show("Hóa đơn đã được khởi tạo và bắt đầu tính giờ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnTao.Enabled = false; // 🚫 KHÔNG cho ấn “Tạo hóa đơn” nữa
            btnThoat.Enabled = true; // ✅ Cho phép thoát

            // 🔁 Cập nhật lại danh sách bàn ở FMain
            foreach (Form f in Application.OpenForms)
            {
                if (f is FMain mainForm)
                {
                    mainForm.LoadTable();
                    break;
                }
                //if(f is AdminMenu adminForm)
                //{
                //    adminForm.UpdateDichVu();
                //    break;
                //}
            }
            

        }

        private void btnThemDV_Click(object sender, EventArgs e) //btnAddDV
        {
            //Version 1
            //List<DichVu> dichvulist = dichVuBiDa.LoadDichVuList();
            //foreach (Control ct in flpDV.Controls)
            //{
            //    foreach (DichVu item in dichvulist)
            //    {
            //        if (ct.Text == item.TenDV)
            //        {
            //            if (ct.BackColor == Color.Red)
            //            {
            //                string commandText = $"INSERT INTO HOADONDV VALUES ('{FMain.IDHD}', '{item.IDdv}', '{nud.Value}')";
            //                dtbase.UpdateData(commandText);
            //                break;
            //            }
            //        }
            //    }

            //}
            //LoadDataDV();


            //Version 2
            // Kiểm tra có chọn số lượng hay không
            int sl = (int)nud.Value;
            if (sl <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool daThemDichVu = false;

            foreach (Control ct in flpDV.Controls)
            {
                if (ct is Button btn && btn.BackColor == Color.Red)
                {
                    if (btn.Tag == null) continue;
                    string iddv = btn.Tag.ToString();
                    

                    // 🔹 Kiểm tra tồn kho
                    DataTable dtCheck = dtbase.ReadData($"SELECT SOLUONG FROM DICHVU WHERE IDDV = '{iddv}'");
                    int ton = Convert.ToInt32(dtCheck.Rows[0]["SOLUONG"]);
                    if (ton < sl)
                    {
                        MessageBox.Show($"Không đủ số lượng tồn kho cho {btn.Text}! (Hiện còn {ton})", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    // 🔹 Kiểm tra xem dịch vụ này đã có trong hóa đơn chưa
                    string sqlCheck = $"SELECT * FROM HOADONDV WHERE IDHD = '{FMain.IDHD}' AND IDDV = '{iddv}'";
                    DataTable dtExist = dtbase.ReadData(sqlCheck);

                    if (dtExist.Rows.Count > 0)
                    {
                        // ✅ Đã có → cập nhật số lượng
                        string update = $"UPDATE HOADONDV SET SOLUONG = SOLUONG + {sl} WHERE IDHD = '{FMain.IDHD}' AND IDDV = '{iddv}'";
                        dtbase.UpdateData(update);
                    }
                    else
                    {
                        // ✅ Chưa có → thêm mới
                        string insert = $"INSERT INTO HOADONDV (IDHD, IDDV, SOLUONG) VALUES ('{FMain.IDHD}', '{iddv}', {sl})";
                        dtbase.UpdateData(insert);
                    }

                    // 🔹 Trừ tồn kho
                    dtbase.UpdateData($"UPDATE DICHVU SET SOLUONG = SOLUONG - {sl} WHERE IDDV = '{iddv}'");

                    daThemDichVu = true;
                }
            }

            
            if (daThemDichVu)
            {
                LoadDataDV();
                MessageBox.Show("Đã thêm/cập nhật dịch vụ vào hóa đơn!");
            }
        }


        //load mã kh
        private void LoadKhachHang()
        {
            string sql = "SELECT IDKH, HOTEN FROM KHACHHANG";
            DataTable dtKH = dtbase.ReadData(sql);
            func.FillComboBox(cboMaKH, dtKH, "IDKH", "IDKH"); // Display = tên KH, Value = mã KH
            cboMaKH.SelectedIndex = -1;
        }

        private void cboMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaKH.SelectedIndex == -1) return;

            string maKH = cboMaKH.SelectedValue.ToString();
            string sql = $"SELECT * FROM KHACHHANG WHERE IDKH = '{maKH}'";
            DataTable dt = dtbase.ReadData(sql);

            if (dt.Rows.Count > 0)
            {
                txtTenKH.Text = dt.Rows[0]["HOTEN"].ToString();
                txtSDT.Text = dt.Rows[0]["SODT"].ToString();
                txtDiaChi.Text = dt.Rows[0]["DCHI"].ToString();
            }
        }













        //nếu ấn thoát thì hóa đơn đang cbi tạo sẽ quay về trạng thái chưa tạo
        private void btnThoat_Click(object sender, EventArgs e)
        {
            FMain.IDHD = null;
            this.Close();
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
                DataTable dt = dtbase.ReadData(sql);

                if (dt.Rows.Count > 0)
                {
                    // ✅ Có khách hàng rồi
                    cboMaKH.SelectedValue = dt.Rows[0]["IDKH"].ToString();
                    txtTenKH.Text = dt.Rows[0]["HOTEN"].ToString();
                    txtDiaChi.Text = dt.Rows[0]["DCHI"].ToString();

                    MessageBox.Show("Đã tìm thấy khách hàng trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnTaoKH.Visible = false; // Ẩn nút tạo khách hàng nếu đã có
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
                        btnTaoKH.Visible = true; // Hiển thị nút tạo khách hàng
                        btnTao.Enabled = false;   // 🚫 KHÔNG cho ấn “Tạo hóa đơn”
                        cboMaKH.Enabled = false; // 🚫 KHÔNG cho chọn mã KH từ combobox

                        txtTenKH.Enabled = true;
                        txtDiaChi.Enabled = true;

                        btnThoat.Enabled = false; // 🚫 KHÔNG cho thoát khi đang tạo KH mới

                        //xóa các thông tin khách hàng hiện tại để nhập mới
                        cboMaKH.SelectedIndex = -1;
                        txtTenKH.Clear();
                        txtDiaChi.Clear();

                        txtTenKH.Focus();
                    }
                    else
                    {
                        btnTaoKH.Visible = false;
                        btnTao.Enabled = true; //nếu k tạo kh thì mới được tạo hóa đơn
                    }
                }
            }
        }

        private void btnTaoKH_Click(object sender, EventArgs e)
        {
            //string maKH = cboMaKH.Text.Trim();
            //string tenKH = txtTenKH.Text.Trim();
            //string diaChi = txtDiaChi.Text.Trim();
            //string sdt = txtSDT.Text.Trim();

            //// 1️⃣ Kiểm tra dữ liệu nhập
            //if (string.IsNullOrEmpty(tenKH))
            //{
            //    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtTenKH.Focus();
            //    return;
            //}

            //if (string.IsNullOrEmpty(sdt))
            //{
            //    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtSDT.Focus();
            //    return;
            //}

            //// 2️⃣ Nếu không nhập mã KH → tự động sinh mã mới
            //if (string.IsNullOrEmpty(maKH))
            //{
            //    maKH = GenerateNextCustomerID(); 
            //}
            //else
            //{
            //    // 3️⃣ Nếu có nhập → kiểm tra trùng
            //    string sqlCheck = $"SELECT * FROM KHACHHANG WHERE IDKH = '{maKH}'";
            //    DataTable dtCheck = dtbase.ReadData(sqlCheck);
            //    if (dtCheck.Rows.Count > 0)
            //    {
            //        MessageBox.Show("Mã khách hàng đã tồn tại! Vui lòng nhập mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}

            //// 4️⃣ Thêm khách hàng mới vào CSDL
            //string sqlInsert = $"INSERT INTO KHACHHANG VALUES ('{maKH}', N'{tenKH}', N'{diaChi}', '{sdt}')";
            //dtbase.UpdateData(sqlInsert);

            //MessageBox.Show($"Thêm khách hàng mới thành công!\nMã KH: {maKH}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //btnTaoKH.Visible = false;
            //btnTao.Enabled = true; //cho phép tạo hóa đơn sau khi tạo khách hàng xong

            //// 5️⃣ Cập nhật lại combobox khách hàng
            //LoadKhachHang();
            //cboMaKH.SelectedValue = maKH;


            //Version 2
            string maKH = cboMaKH.Text.Trim();
            string tenKH = txtTenKH.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string sdt = txtSDT.Text.Trim();

            // 1️⃣ Kiểm tra dữ liệu nhập cơ bản
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

            // 2️⃣ Kiểm tra định dạng số điện thoại Việt Nam (bắt đầu bằng 0, có 10 chữ số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập đúng 10 chữ số và bắt đầu bằng 0.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 3️⃣ Nếu không nhập mã KH → tự động sinh mã mới
            if (string.IsNullOrEmpty(maKH))
            {
                maKH = GenerateNextCustomerID();
            }
            else
            {
                // 4️⃣ Nếu có nhập → kiểm tra trùng mã KH
                string sqlCheck = $"SELECT * FROM KHACHHANG WHERE IDKH = '{maKH}'";
                DataTable dtCheck = dtbase.ReadData(sqlCheck);
                if (dtCheck.Rows.Count > 0)
                {
                    MessageBox.Show("Mã khách hàng đã tồn tại! Vui lòng nhập mã khác.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 5️⃣ Kiểm tra trùng số điện thoại (tránh tạo KH trùng SDT)
            string sqlCheckPhone = $"SELECT * FROM KHACHHANG WHERE SODT = '{sdt}'";
            DataTable dtPhone = dtbase.ReadData(sqlCheckPhone);
            if (dtPhone.Rows.Count > 0)
            {
                MessageBox.Show("Số điện thoại này đã tồn tại trong hệ thống! Vui lòng kiểm tra lại.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 6️⃣ Thêm khách hàng mới vào CSDL
            string sqlInsert = $"INSERT INTO KHACHHANG VALUES ('{maKH}', N'{tenKH}', N'{diaChi}', '{sdt}')";
            dtbase.UpdateData(sqlInsert);

            MessageBox.Show($"Thêm khách hàng mới thành công!\nMã KH: {maKH}",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 7️⃣ Cập nhật lại giao diện
            btnTaoKH.Visible = false;
            btnTao.Enabled = true; // cho phép tạo hóa đơn sau khi thêm KH

            LoadKhachHang();
            cboMaKH.SelectedValue = maKH;
        }

        //sinh mã khách hàng tự động
        private string GenerateNextCustomerID()
        {
            string sql = "SELECT TOP 1 IDKH FROM KHACHHANG ORDER BY IDKH DESC";
            DataTable dt = dtbase.ReadData(sql);

            if (dt.Rows.Count == 0)
            {
                return "KH001"; // nếu chưa có khách hàng nào
            }
            else
            {
                string lastID = dt.Rows[0]["IDKH"].ToString(); // VD: "KH012"
                string numberPart = lastID.Substring(2);        // "012"
                int nextNum = 1;
                int.TryParse(numberPart, out nextNum);
                nextNum++;
                return "KH" + nextNum.ToString("D3");          // tạo lại "KH013"
            }
        }

        private void FAddHD_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string sqlCheck = $"SELECT IDPHIEN, TRANGTHAI FROM HOADON WHERE IDHD = '{FMain.IDHD}'";
                DataTable dt = dtbase.ReadData(sqlCheck);
                if (dt.Rows.Count == 0) return;

                string idPhien = dt.Rows[0]["IDPHIEN"].ToString();
                int trangThai = Convert.ToInt32(dt.Rows[0]["TRANGTHAI"]);

                if (trangThai == 0) // chỉ rollback nếu hóa đơn chưa thanh toán
                {
                    // 1️⃣ Hoàn lại dịch vụ
                    string sqlDV = $"SELECT IDDV, SOLUONG FROM HOADONDV WHERE IDHD = '{FMain.IDHD}'";
                    DataTable dv = dtbase.ReadData(sqlDV);
                    foreach (DataRow r in dv.Rows)
                    {
                        string iddv = r["IDDV"].ToString();
                        int sl = Convert.ToInt32(r["SOLUONG"]);
                        dtbase.UpdateData($"UPDATE DICHVU SET SOLUONG = SOLUONG + {sl} WHERE IDDV = '{iddv}'");
                    }

                    // 2️⃣ Xóa dịch vụ + hóa đơn + phiên chơi
                    dtbase.UpdateData($"DELETE FROM HOADONDV WHERE IDHD = '{FMain.IDHD}'");
                    dtbase.UpdateData($"DELETE FROM HOADON WHERE IDHD = '{FMain.IDHD}'");
                    dtbase.UpdateData($"DELETE FROM PHIENCHOI WHERE IDPHIEN = '{idPhien}'");

                    // 3️⃣ Cập nhật bàn về trạng thái trống
                    if (!string.IsNullOrEmpty(selectedTableId))
                        tableBiDa.UpdateDataTable(new BTL_LTTQ_BIDA.Class.Table { Idban = selectedTableId }, 0);
                }

                FMain.IDHD = null;

                // 🔁 Cập nhật lại danh sách bàn ở FMain
                foreach (Form f in Application.OpenForms)
                {
                    if (f is FMain mainForm)
                    {
                        mainForm.LoadTable();
                        break;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi rollback hóa đơn: " + ex.Message);
            }
        }

        private void dgvHDDV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 🛡️ Bảo vệ: Không xử lý nếu click header hoặc ngoài dòng dữ liệu
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
                string sqlDelete = $"DELETE FROM HOADONDV WHERE IDHD = '{FMain.IDHD}' AND IDDV = '{iddv}'";
                dtbase.UpdateData(sqlDelete);

                // ✅ Load lại danh sách dịch vụ
                LoadDataDV();

                MessageBox.Show($"Đã hủy dịch vụ '{tenDV}' và hoàn lại {sl} vào tồn kho!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy dịch vụ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
