using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ_BIDA.Class
{
    public class DichVu
    {
        public DichVu(string iddv, string tenDV, double giatien)
        {
            IDdv = iddv;
            TenDV = tenDV;
            Giatien = giatien;
        }

        public string IDdv { get; set; }
        public string TenDV { get; set; }
        public double Giatien { get; set; }
    }
}
