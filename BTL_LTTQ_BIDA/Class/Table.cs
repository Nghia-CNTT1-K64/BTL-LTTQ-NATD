using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ_BIDA.Class
{
    public class Table
    {
        public Table(string idban, double giatien, int trangthai)
        {
            Idban = idban;
            Giatien = giatien;
            Trangthai = trangthai;
            idhdCurrent = -1;
        }
        public Table()
        {
            IdhdCurrent = -1;  // có thể giữ giá trị mặc định này
        }


        private string idban;
        private double giatien;
        private int trangthai;
        private int idhdCurrent;

        public string Idban { get => idban; set => idban = value; }

        public double Giatien { get => giatien; set => giatien = value; }

        public int Trangthai { get => trangthai; set => trangthai = value; }

        public int IdhdCurrent { get => idhdCurrent; set => idhdCurrent = value; }
    }
}
