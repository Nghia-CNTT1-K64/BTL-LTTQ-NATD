using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ_BIDA.Forms;
using BTL_LTTQ_BIDA.Forms.Account;
using BTL_LTTQ_BIDA.Forms.Main;

namespace BTL_LTTQ_BIDA
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FMain());
        }
    }
}
