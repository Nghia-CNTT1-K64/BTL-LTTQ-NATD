﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BTL_LTTQ_BIDA.Data
{
    internal class Function
    {
        public void FillCombox(ComboBox cb, DataTable dt, string display, string value)
        {
            cb.DataSource = dt;
            cb.DisplayMember = display;
            cb.ValueMember = value;
        }
    }
}
