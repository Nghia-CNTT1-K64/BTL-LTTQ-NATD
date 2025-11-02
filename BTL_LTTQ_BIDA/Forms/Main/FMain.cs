using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; // <<< TÔI ĐÃ THÊM THƯ VIỆN NÀY
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//dung de ket noi sql
using System.Data.SqlClient;

namespace BTL_LTTQ_BIDA
{
    public partial class FMain : Form
    {
        public static string ConnectionString = @"Data Source=DESKTOP-7KGQ86Q\SQLEXPRESS01;Initial Catalog=QLQuanBilliard;Integrated Security=True;";
        public FMain()
        {
            InitializeComponent();
        }

        // <<< PHẦN 2: THÊM HÀM GET SQL DATA >>>
        /// <summary>
        /// Chạy một câu lệnh SELECT và trả về một DataTable
        /// </summary>
        /// <param name="query">Câu lệnh SQL (SELECT)</param>
        /// <returns>DataTable chứa kết quả</returns>
        public static DataTable GetSqlData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                // Dùng 'using' để đảm bảo kết nối được đóng ngay cả khi có lỗi
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open(); // Mở kết nối
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        adapter.Fill(dt); // Đổ dữ liệu vào DataTable
                    }
                } // Kết nối tự động đóng ở đây
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu không lấy được dữ liệu
                MessageBox.Show("Lỗi khi thực thi GetSqlData: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
        // <<< KẾT THÚC PHẦN 2 >>>


        private void btnThemHD_Click(object sender, EventArgs e)
        {

        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnThemHD_Click_1(object sender, EventArgs e)
        {

        }
    }
}