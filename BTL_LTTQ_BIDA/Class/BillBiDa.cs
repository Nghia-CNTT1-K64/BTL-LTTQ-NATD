using BTL_LTTQ_BIDA.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ_BIDA.Class
{
    public class BillBiDa
    {
        DataConnect dtbase = new DataConnect();
        public BillBiDa() { }

        //Version 1
        //public Bill GetUnCheckBill(int idHD)
        //{
        //    string commandText = $"SELECT * FROM HOADON WHERE IDHD = '{idHD}'";
        //    DataTable data = dtbase.ReadData(commandText);
        //    Bill bill = new Bill((string)data.Rows[0][0], (string)data.Rows[0][2]);
        //    return bill;
        //}

        //public  List<Bill> GetListUnCheckBillID()
        //{
        //    List<Bill> billlist = new List<Bill>();
        //    string commandText = "SELECT * FROM HOADON WHERE TRANGTHAI = 0";
        //    DataTable data = dtbase.ReadData(commandText);
        //    foreach (DataRow row in data.Rows)
        //    {
        //        string idhd = row["idhd"].ToString();
        //        string idkh = row["idkh"].ToString();

        //        Bill bill = new Bill(idhd, idkh);
        //        billlist.Add(bill);
        //    }
        //    return billlist;
        //}

        //public  List<Bill> GetListCheckedBill()
        //{
        //    List<Bill> billlist = new List<Bill>();
        //    string commandText = "SELECT * FROM HOADON WHERE TRANGTHAI = 1";
        //    DataTable data = dtbase.ReadData(commandText);
        //    foreach (DataRow row in data.Rows)
        //    {
        //        string idhd = row["idhd"].ToString();
        //        string makh = row["idkh"].ToString();
        //        Bill bill = new Bill(idhd, makh);
        //        billlist.Add(bill);
        //    }
        //    return billlist;
        //}


        //Version 2
        public Bill GetUnCheckBill(string idHD)
        {
            string sql = $"SELECT h.IDHD, h.IDKH, p.IDBAN " +
                         $"FROM HOADON h " +
                         $"JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN " +
                         $"WHERE h.IDHD = '{idHD}'";
            DataTable data = dtbase.ReadData(sql);
            if (data.Rows.Count == 0) return null;

            string idhd = data.Rows[0]["IDHD"].ToString();
            string idkh = data.Rows[0]["IDKH"].ToString();
            string idban = data.Rows[0]["IDBAN"] == DBNull.Value ? null : data.Rows[0]["IDBAN"].ToString();

            return new Bill(idhd, idkh, idban);
        }

        public List<Bill> GetListUnCheckBillID()
        {
            List<Bill> billlist = new List<Bill>();
            string sql = "SELECT h.IDHD, h.IDKH, p.IDBAN " +
                         "FROM HOADON h " +
                         "JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN " +
                         "WHERE h.TRANGTHAI = 0";
            DataTable data = dtbase.ReadData(sql);

            foreach (DataRow row in data.Rows)
            {
                string idhd = row["IDHD"].ToString();
                string idkh = row["IDKH"].ToString();
                string idban = row["IDBAN"] == DBNull.Value ? "Chưa chọn" : row["IDBAN"].ToString();

                billlist.Add(new Bill(idhd, idkh, idban));
            }

            return billlist;
        }

        public List<Bill> GetListCheckedBill()
        {
            List<Bill> billlist = new List<Bill>();
            string sql = "SELECT h.IDHD, h.IDKH, p.IDBAN " +
                         "FROM HOADON h " +
                         "JOIN PHIENCHOI p ON h.IDPHIEN = p.IDPHIEN " +
                         "WHERE h.TRANGTHAI = 1";
            DataTable data = dtbase.ReadData(sql);

            foreach (DataRow row in data.Rows)
            {
                string idhd = row["IDHD"].ToString();
                string idkh = row["IDKH"].ToString();
                string idban = row["IDBAN"] == DBNull.Value ? "Chưa chọn" : row["IDBAN"].ToString();

                billlist.Add(new Bill(idhd, idkh, idban));
            }

            return billlist;
        }
    }
}
