using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ_BIDA.Data
{
    internal class DataConnect
    {
        SqlConnection sqlconnect = null;

        // ✅ CHUỖI KẾT NỐI ĐÃ SỬA ĐÚNG
        private readonly string connectStr = @"Data Source=tcp:DESKTOP-7KGQ86Q\SQLEXPRESS01;Initial Catalog=QLQuanBilliard;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;";

        // mở kết nối
        public void OpenConnect()
        {
            sqlconnect = new SqlConnection(connectStr);
            if (sqlconnect.State != ConnectionState.Open)
            {
                sqlconnect.Open();
            }
        }

        // đóng kết nối
        public void CloseConnect()
        {
            if (sqlconnect != null && sqlconnect.State != ConnectionState.Closed)
            {
                sqlconnect.Close();
                sqlconnect.Dispose();
            }
        }

        // thực hiện SELECT SQL và trả về DataTable
        public DataTable ReadData(string sqlstr)
        {
            DataTable dt = new DataTable();
            OpenConnect();

            SqlDataAdapter da = new SqlDataAdapter(sqlstr, sqlconnect);
            da.Fill(dt);

            CloseConnect();
            da.Dispose();

            return dt;
        }

        // thực hiện lệnh INSERT, UPDATE, DELETE
        public void UpdateData(string sqlstr)
        {
            OpenConnect();

            SqlCommand cmd = new SqlCommand(sqlstr, sqlconnect);
            cmd.ExecuteNonQuery();

            CloseConnect();
            cmd.Dispose();
        }
    }
}
