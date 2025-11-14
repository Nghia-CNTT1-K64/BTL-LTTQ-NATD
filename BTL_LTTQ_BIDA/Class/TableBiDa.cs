using BTL_LTTQ_BIDA.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ_BIDA.Class
{
    public class TableBiDa
    {
        DataConnect dtbase = new DataConnect();

        private static TableBiDa instance;
        public static TableBiDa Instance
        {
            get
            {
                if (instance == null) instance = new TableBiDa();
                return instance;
            }
            private set { instance = value; }
        }
        public TableBiDa() { }

        public List<Table> LoadTableList()
        {
            List<Table> tablelist = new List<Table>();
            string commandText = "SELECT * FROM BAN WHERE TRANGTHAI is not null";
            var data = dtbase.ReadData(commandText);

            foreach (DataRow item in data.Rows)
            {
                string idban = item["idban"].ToString();
                double money = Convert.ToDouble(item["giatien"]);
                int trangthai = Convert.ToInt32(item["trangthai"]);
                Table table = new Table(idban, money, trangthai);
                tablelist.Add(table);
            }

            return tablelist;

        }
        public void UpdateDataTable(Table table, int i)
        {
            string commandText = $"update Ban set trangthai={i} where idban= '{table.Idban}'";
            dtbase.UpdateData(commandText);
        }

    }
}
