using BTL_LTTQ_BIDA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ_BIDA.Class
{
    public class KhachHangBiDa
    {
        DataConnect dtbase = new DataConnect();
        public KhachHangBiDa() { }

        public KhachHang GetKhachhang(string idkh)
        {
            string commandText = $"SELECT * FROM KHACHHANG WHERE IDKH = '{idkh}'";
            var data = dtbase.ReadData(commandText);
            if (data.Rows.Count > 0)
            {
                string hoten = data.Rows[0]["HOTEN"].ToString();
                string dchi = data.Rows[0]["DCHI"].ToString();
                string sodt = data.Rows[0]["SODT"].ToString();
                KhachHang khachhang = new KhachHang(idkh, hoten, dchi, sodt);
                return khachhang;
            }
            return null;
        }
    }
}
