﻿using BTL_LTTQ_BIDA.Forms.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_LTTQ_BIDA
{
    public partial class FMain : Form
    {
        public static int IDKH { get; set; }
        public static int IDHD { get; set; }
        public static int IDNV_Current { get; set; }
        public static bool IsAdminState { get; set; }
        public FMain()
        {
            InitializeComponent();
        }

        private void FMain_Load(object sender, EventArgs e)
        {

        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountInfoDlg form = new AccountInfoDlg();
            form.ShowDialog();
        }
    }
}
