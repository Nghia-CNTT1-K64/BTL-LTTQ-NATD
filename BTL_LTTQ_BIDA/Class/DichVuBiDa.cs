using BTL_LTTQ_BIDA.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_LTTQ_BIDA.Class
{
    internal class DichVuBiDa
    {
        DataConnect dtbase = new DataConnect();
        public List<DichVu> LoadDichVuList()
        {
            List<DichVu> dichvulist = new List<DichVu>();
            string commandText = "SELECT * FROM DICHVU WHERE HIENTHI = 1";
            DataTable data = dtbase.ReadData(commandText);
            DataGridView dataGridView = new DataGridView()
            {
                DataSource = data
            };
            dataGridView.Show();

            foreach (DataRow item in data.Rows)
            {
                try
                {
                    string iddv = item["iddv"].ToString();
                    string tenDV = item["tenDV"].ToString();
                    double money = Convert.ToDouble(item["giatien"]);
                    DichVu dichvu = new DichVu(iddv, tenDV, money);
                    dichvulist.Add(dichvu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return dichvulist;
        }
    }
}
