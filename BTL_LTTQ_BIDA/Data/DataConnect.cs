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
        string connectStr = "Data Source=localhost;Initial Catalog=QLQuanBilliard;Integrated Security=True;";

        //open connect 
        public void OpenConnect()
        {
            sqlconnect = new SqlConnection(connectStr);
            if (sqlconnect.State != ConnectionState.Open)
            {
                sqlconnect.Open();
            }
        }

        public void CloseConnect()
        {
            if (sqlconnect.State != ConnectionState.Closed)
            {
                sqlconnect.Close();
                sqlconnect.Dispose();
            }
        }

        //thực hiện select SQL và trả về 1 dataTable
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

        public void UpdateData(string sqlstr)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlstr;
            cmd.Connection = sqlconnect;
            cmd.ExecuteNonQuery();

            CloseConnect();
            cmd.Dispose();

        }
    }
}
