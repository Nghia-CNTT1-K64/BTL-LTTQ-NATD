using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ_BIDA.Utils
{
    /// <summary>
    /// Lớp cung cấp hàm "CSS cho WinForms" – dùng để làm đẹp giao diện.
    /// Dùng chung cho toàn bộ các Form / UserControl.
    /// </summary>
    public static class UIStyler
    {
        // 🎨 Áp dụng style tổng thể cho form
        public static void ApplyFormStyle(Form form)
        {
            if (form == null) return;

            form.BackColor = Color.FromArgb(245, 248, 255); // nền sáng nhẹ
            form.Font = new Font("Segoe UI", 9);

            foreach (Control c in form.Controls)
                ApplyControlStyle(c);
        }

        // 🎨 Duyệt đệ quy toàn bộ control trong form
        private static void ApplyControlStyle(Control ctrl)
        {
            if (ctrl is Panel pnl)
            {
                pnl.BackColor = Color.White;
                pnl.BorderStyle = BorderStyle.FixedSingle;

                foreach (Control c in pnl.Controls)
                    ApplyControlStyle(c);
            }
            else if (ctrl is Button btn)
            {
                StyleButton(btn);
            }
            else if (ctrl is DataGridView dgv)
            {
                StyleDataGridView(dgv);
            }
            else if (ctrl is TextBox tb)
            {
                StyleTextbox(tb);
            }
            else if (ctrl.HasChildren)
            {
                foreach (Control c in ctrl.Controls)
                    ApplyControlStyle(c);
            }
        }

        // ================== COMPONENT STYLES ==================

        public static void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(30, 144, 255);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 120, 215);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(30, 144, 255);
        }

        public static void StyleTextbox(TextBox tb)
        {
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.Font = new Font("Segoe UI", 9);
            tb.BackColor = Color.FromArgb(252, 252, 255);
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 144, 255);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 250, 255);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 224, 138);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.GridColor = Color.FromArgb(200, 200, 200);
        }
    }
}
