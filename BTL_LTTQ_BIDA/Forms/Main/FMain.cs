using BTL_LTTQ_BIDA.Class;
using BTL_LTTQ_BIDA.Data;
using BTL_LTTQ_BIDA.Forms.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using BTL_LTTQ_BIDA.Utils;

namespace BTL_LTTQ_BIDA
{
    public partial class FMain : Form
    {
        DataConnect dtbase = new DataConnect();

        TableBiDa tableBiDa = new TableBiDa();
        BillBiDa billBiDa = new BillBiDa();

        public static string IDKH { get; set; }
        public static string IDHD { get; set; }
        public static string IDNV_Current { get; set; } 
        public static bool IsAdminState { get; set; }

        private readonly TreeNode root_XuLi = new TreeNode();
        private readonly TreeNode root_KetThuc = new TreeNode();

        public FMain()
        {
            InitializeComponent();
        }


        private void FMain_Load(object sender, EventArgs e)
        {

            IDNV_Current = "NV001"; // 👈 gán tạm để test

            // ✅ BẬT DOUBLE BUFFERING CHO CÁC CONTROL LỚN
            SetDoubleBuffered(flpTable);
            SetDoubleBuffered(tvHD);

            LoadTable();
            LoadBill_Uncheck();
            LoadBill_Checked();
            tvHD.Nodes.Add(root_XuLi);
            tvHD.Nodes.Add(root_KetThuc);
            root_XuLi.Text = "Hóa đơn đang xử lý";
            root_KetThuc.Text = "Hóa đơn đã kết thúc";
            tvHD.NodeMouseDoubleClick += TvHD_NodeMouseDoubleClick;
        }

        public void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tablelist = tableBiDa.LoadTableList();
            foreach (Table item in tablelist)
            {
                Button btn = new Button() { Width = 50, Height = 50 };
                if (item.Trangthai == 0)
                {
                    btn.Text = "Bàn " + item.Idban + Environment.NewLine + "trống";
                    btn.BackColor = Color.Green;
                }
                else
                {
                    btn.Text = "Bàn " + item.Idban + Environment.NewLine + "có người";
                    btn.BackColor = Color.Red;
                }
                btn.Tag = item.Idban;
                flpTable.Controls.Add(btn);
                //btn.Click += Btn_Click;
                btn.Cursor = Cursors.Default;

            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            string idban = (sender as Button).Tag.ToString();
            List<Table> tablelist = tableBiDa.LoadTableList();
            foreach (Table item in tablelist)
            {
                if ( item.Idban == idban)
                {
                    if (item.Trangthai == 0)
                    {
                        tableBiDa.UpdateDataTable(item, 1);
                        item.Trangthai = 1;
                    }
                    else if (item.Trangthai == 1)
                    {
                        tableBiDa.UpdateDataTable(item, 0);
                        item.Trangthai = 0;
                    }
                }
            }
            LoadTable();
        }

        private void LoadBill_Uncheck()
        {
            root_XuLi.Nodes.Clear(); // 🔹 Xóa toàn bộ node cũ trước khi load lại

            List<Bill> billist = billBiDa.GetListUnCheckBillID();
            foreach (Bill item in billist)
            {
                TreeNode nodebill = new TreeNode()
                {
                    //Version 1
                    //Text = item.Idhd.ToString()


                    //Version 2
                    Text = $"{item.Idhd} - {item.Idban}"
                };
                root_XuLi.Nodes.Add(nodebill);

            }
            root_XuLi.Expand(); // 🔹 Mở rộng node cho dễ nhìn
        }

        private void LoadBill_Checked()
        {
            root_KetThuc.Nodes.Clear(); // 🔹 Xóa toàn bộ node cũ trước khi load lại
            List<Bill> billist = billBiDa.GetListCheckedBill();
            foreach (Bill item in billist)
            {
                //Version 1
                //TreeNode nodebill = new TreeNode();
                //nodebill.Text = item.Idhd.ToString();
                //root_KetThuc.Nodes.Add(nodebill);


                //Version 2
                TreeNode nodebill = new TreeNode()
                {
                    Text = $"{item.Idhd} - {item.Idban}"
                };
                root_KetThuc.Nodes.Add(nodebill);
            }
            root_KetThuc.Expand(); // 🔹 Mở rộng node cho dễ nhìn    
        }

        private void btnThemHD_Click_1(object sender, EventArgs e)
        {
            //Sinh mã hóa đơn mới trước khi mở form
            //IDHD = GenerateNextID(IDHD);

            ////Mở form thêm hóa đơn (form sẽ dùng được FMain.IDHD)
            //FAddHD dlg = new FAddHD();
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    TreeNode nodex = new TreeNode()
            //    {
            //        Text = IDKH.ToString()
            //    };

            //    root_XuLi.Nodes.Add(nodex);

            //}
            ////cập nhật lại giao diện sao khi thêm hóa đơn
            //LoadTable();


            try
            {
                // 1️ Sinh mã hóa đơn mới theo ngày
                IDHD = GenerateNextID();

                // 2️ Tạo mã phiên tương ứng (P301020251)
                string idPhien = "P" + DateTime.Now.ToString("ddMMyyyyHHmmssfff");

                // 3️ Tạo phiên chơi tạm (chưa có bàn)
                string insertPhien = $"INSERT INTO PHIENCHOI (IDPHIEN, IDBAN, GIOBATDAU, GIOKETTHUC) VALUES ('{idPhien}', NULL, NULL, NULL)";
                dtbase.UpdateData(insertPhien);

                // 4️ Tạo hóa đơn tạm (chưa thanh toán)
                string insertHD = $"INSERT INTO HOADON (IDHD, IDPHIEN, IDNV, NGAYLAP, TONGTIEN, TRANGTHAI) " +
                                  $"VALUES ('{IDHD}', '{idPhien}', '{IDNV_Current}', GETDATE(), 0, 0)";
                dtbase.UpdateData(insertHD);

                // 5️ Mở form thêm hóa đơn
                FAddHD form = new FAddHD();
                form.ShowDialog();

                // 6️ Sau khi đóng, load lại danh sách hóa đơn đang xử lý
                LoadBill_Uncheck();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hóa đơn mới: " + ex.Message);
            }


        }


        //hàm tạo mã hóa đơn kế tiếp tự động

        //Version 1
        //public  string GenerateNextID(string currentID)
        //{
        //    string today = DateTime.Now.ToString("ddMMyyyy");

        //    // Lấy hóa đơn mới nhất trong ngày hôm nay
        //    string sql = $"SELECT TOP 1 IDHD FROM HOADON WHERE IDHD LIKE 'HD{today}%' ORDER BY IDHD DESC";
        //    DataTable dt = dtbase.ReadData(sql);

        //    if (dt.Rows.Count == 0)
        //    {
        //        // Chưa có hóa đơn nào hôm nay → bắt đầu từ 1
        //        return $"HD{today}1";
        //    }
        //    else
        //    {
        //        // Tách phần số cuối cùng để +1
        //        string lastID = dt.Rows[0]["IDHD"].ToString();
        //        string numberPart = lastID.Substring(10);
        //        int nextNum = 1;
        //        int.TryParse(numberPart, out nextNum);
        //        nextNum++;
        //        return $"HD{today}{nextNum}";
        //    }
        //}

        //Version 2
        public string GenerateNextID()
        {
            string today = DateTime.Now.ToString("ddMMyyyy");

            // 🔹 Lấy mã hóa đơn mới nhất của ngày hôm nay
            string sql = $@"
                SELECT TOP 1 IDHD 
                FROM HOADON 
                WHERE IDHD LIKE 'HD{today}%'
                ORDER BY CAST(SUBSTRING(IDHD, 11, LEN(IDHD)) AS INT) DESC";

            DataTable dt = dtbase.ReadData(sql);

            // 🔹 Nếu chưa có hóa đơn nào trong ngày → bắt đầu từ 1
            if (dt.Rows.Count == 0)
            {
                return $"HD{today}1";
            }
            else
            {
                // 🔹 Lấy mã hóa đơn cuối cùng và tách phần số thứ tự ra
                string lastID = dt.Rows[0]["IDHD"].ToString();
                int lastNumber = 0;

                // Lấy phần số thứ tự (bắt đầu từ vị trí 11 vì HDddMMyyyy có 10 ký tự đầu)
                string numberPart = lastID.Substring(10);
                int.TryParse(numberPart, out lastNumber);

                // 🔹 Sinh mã kế tiếp
                int nextNumber = lastNumber + 1;
                return $"HD{today}{nextNumber}";
            }
        }




        private void TvHD_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Version 1
            //try
            //{
            //    // Lấy ID HD từ node đang chọn (chuỗi)
            //    string idhd = tvHD.SelectedNode.Text;

            //    // Tạo form hóa đơn, truyền mã khách hàng (string)
            //    FHoaDon fHoadon = new FHoaDon(idhd);
            //    if (tvHD.SelectedNode.Parent.Text == "Hóa đơn đang xử lý")
            //    {
            //        FHoaDon.Trangthai = 0;
            //    }
            //    else if (tvHD.SelectedNode.Parent.Text == "Hóa đơn đã kết thúc")
            //    {
            //        FHoaDon.Trangthai = 1;
            //    }
            //    fHoadon.ShowDialog();

            //}
            //catch (Exception) { }


            //Version 2
            try
            {
                // Lấy chuỗi hiển thị: "HD301020251 - B01"
                string fullText = tvHD.SelectedNode.Text;

                // Tách chỉ phần mã hóa đơn
                string idhd = fullText.Split('-')[0].Trim(); // Lấy phần "HD301020251"

                // Mở form hóa đơn
                FHoaDon fHoadon = new FHoaDon(idhd);

                // Xác định trạng thái
                if (tvHD.SelectedNode.Parent.Text == "Hóa đơn đang xử lý")
                {
                    FHoaDon.Trangthai = 0;
                }
                else if (tvHD.SelectedNode.Parent.Text == "Hóa đơn đã kết thúc")
                {
                    FHoaDon.Trangthai = 1;
                }

                fHoadon.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        //hàm này giúp cho mỗi lần load form sẽ không bị nhấp nháy
        public static void SetDoubleBuffered(Control control)
        {
            PropertyInfo propInfo = typeof(Control).GetProperty(
                                                    "DoubleBuffered",
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Instance);
            propInfo?.SetValue(control, true, null);
            foreach (Control ctrl in control.Controls)
                SetDoubleBuffered(ctrl);
        }


        //để cập nhật lại tvHD mỗi khi xóa hd hoặc thêm hd mới
        public void ReloadBillsTree()
        {
            root_XuLi.Nodes.Clear();
            root_KetThuc.Nodes.Clear();

            LoadBill_Uncheck();
            LoadBill_Checked();

            root_XuLi.Text = "Hóa đơn đang xử lý";
            root_KetThuc.Text = "Hóa đơn đã kết thúc";
            tvHD.Nodes.Clear();
            tvHD.Nodes.Add(root_XuLi);
            tvHD.Nodes.Add(root_KetThuc);

            tvHD.ExpandAll();
        }


    }
}
