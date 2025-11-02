using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Classes;
using BTL_LTTQ_BIDA.Forms.Account;
using BTL_LTTQ_BIDA.Forms.Main;

namespace BTL_LTTQ_BIDA
{
    public partial class FMain : Form
    {
        private NhanVien currentUser;
        public FMain(NhanVien nv)
        {
            InitializeComponent();
            this.currentUser = nv;
        }
        private void FMain_Load(object sender, EventArgs e)
        {
            adminToolStripMenuItem.Enabled = currentUser.QuyenAdmin;
        }

        private void btnThemHD_Click(object sender, EventArgs e)
        {

        }

        //menu quản trị viên - Nghĩa
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminMenu fAdminMenu = new AdminMenu(currentUser);
            fAdminMenu.ShowDialog();
        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountInfoDlg accountInfoDlg = new AccountInfoDlg(currentUser);
            accountInfoDlg.ShowDialog();
        }
    }
}
