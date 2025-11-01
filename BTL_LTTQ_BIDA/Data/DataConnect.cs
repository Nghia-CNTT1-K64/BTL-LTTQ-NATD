using System;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ_BIDA.Data
{
    internal class DataConnect
    {
        private SqlConnection sqlconnect = null;
        private readonly string connectStr = "Data Source=localhost;Initial Catalog=QLQuanBilliard4;Integrated Security=True;";

        // ==========================
        // 🔹 MỞ / ĐÓNG KẾT NỐI
        // ==========================
        public void OpenConnect()
        {
            if (sqlconnect == null)
                sqlconnect = new SqlConnection(connectStr);

            if (sqlconnect.State != ConnectionState.Open)
                sqlconnect.Open();
        }

        public void CloseConnect()
        {
            if (sqlconnect != null && sqlconnect.State != ConnectionState.Closed)
            {
                sqlconnect.Close();
                sqlconnect.Dispose();
                sqlconnect = null;
            }
        }

        // ==========================
        // 🔹 ĐỌC DỮ LIỆU (SELECT)
        // ==========================
        public DataTable ReadData(string sqlstr)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnect();
                SqlDataAdapter da = new SqlDataAdapter(sqlstr, sqlconnect);
                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đọc dữ liệu: {ex.Message}");
            }
            finally
            {
                CloseConnect();
            }
            return dt;
        }

        // ==========================
        // 🔹 CẬP NHẬT (INSERT / UPDATE / DELETE) CỔ ĐIỂN
        // ==========================
        public void UpdateData(string sqlstr)
        {
            try
            {
                OpenConnect();
                SqlCommand cmd = new SqlCommand(sqlstr, sqlconnect);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
            }
            finally
            {
                CloseConnect();
            }
        }

        // =====================================================================
        // 🔹 HÀM MỚI: THỰC THI CÂU LỆNH VỚI THAM SỐ AN TOÀN (PHÒNG SQL INJECTION)
        // =====================================================================

        /// <summary>
        /// Thực thi câu lệnh INSERT / UPDATE / DELETE với tham số hóa.
        /// </summary>
        public int ExecuteNonQuery(string sql, object[] parameters = null)
        {
            int rowsAffected = 0;
            try
            {
                OpenConnect();
                using (SqlCommand cmd = new SqlCommand(sql, sqlconnect))
                {
                    if (parameters != null)
                    {
                        // Duyệt và thêm tham số tự động dạng @p0, @p1, ...
                        for (int i = 0; i < parameters.Length; i++)
                            cmd.Parameters.AddWithValue($"@p{i}", parameters[i] ?? DBNull.Value);
                    }

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thực thi lệnh SQL: {ex.Message}");
            }
            finally
            {
                CloseConnect();
            }
            return rowsAffected;
        }

        /// <summary>
        /// Trả về giá trị đầu tiên của truy vấn (SELECT COUNT(*), SELECT MAX(...), ...).
        /// </summary>
        public object ExecuteScalar(string sql, object[] parameters = null)
        {
            object result = null;
            try
            {
                OpenConnect();
                using (SqlCommand cmd = new SqlCommand(sql, sqlconnect))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Length; i++)
                            cmd.Parameters.AddWithValue($"@p{i}", parameters[i] ?? DBNull.Value);
                    }
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu Scalar: {ex.Message}");
            }
            finally
            {
                CloseConnect();
            }
            return result ?? 0;
        }

        /// <summary>
        /// Trả về DataTable từ truy vấn SELECT có tham số.
        /// </summary>
        public DataTable ExecuteQuery(string sql, object[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnect();
                using (SqlCommand cmd = new SqlCommand(sql, sqlconnect))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Length; i++)
                            cmd.Parameters.AddWithValue($"@p{i}", parameters[i] ?? DBNull.Value);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đọc dữ liệu có tham số: {ex.Message}");
            }
            finally
            {
                CloseConnect();
            }
            return dt;
        }
    }
}
