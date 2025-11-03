using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ_BIDA.Class
{
    public class KhachHang
    {
        public KhachHang(string idkh, string hoten, string dchi, string sodt)
        {
            Idkh = idkh;
            Hoten = hoten;
            Dchi = dchi;
            Sodt = sodt;
        }

        public string Idkh { get; set; }
        public string Hoten { get; set; }
        public string Dchi { get; set; }
        public string Sodt { get; set; }
    }
}
