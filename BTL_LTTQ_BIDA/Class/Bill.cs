using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ_BIDA.Class
{
    public class Bill
    {
        public Bill(string idhd, string idkh, string idban = null)
        {
            Idhd = idhd;
            Idkh = idkh;
            Idban = idban;
            Trangthai = 0;
        }

        public string Idhd { get; set; }
        public string Idkh { get; set; }
        public string Idban { get; set; } // 🆕 Thêm thuộc tính mã bàn
        public DateTime Giolaphoadon { get; set; }
        public double Thanhtien { get; set; }
        public int Trangthai { get; set; }
    }
}
