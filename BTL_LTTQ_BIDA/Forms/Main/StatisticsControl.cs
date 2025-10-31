using BTL_LTTQ_BIDA.Data;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class StatisticsControl : UserControl
    {
        private readonly DataConnect dtBase = new DataConnect();
        private readonly BarChart<double> barChart;

        private DateTime startDate, endDate;
        private DataTable currentTable;

        private readonly float[] zoomRatioArr = { 0.25f, 0.5f, 0.75f, 1.0f, 1.5f, 2.0f };
        private int currentZoomRatioIndex = 3;

        public StatisticsControl()
        {
            InitializeComponent();

            barChart = new BarChart<double>
            {
                BarColor = Color.Cyan,
                VerticalText = "Doanh thu (VNĐ)",
                AutoCalculateVerticalValueList = true
            };

            // enable double buffering on the control to reduce flicker
            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, barChart, new object[] { true });
        }

        private void StatisticsControl_Load(object sender, EventArgs e)
        {
            InitImageLists();
            InitDefaultSettings();

            panel_Chart.Controls.Add(barChart);
            barChart.Dock = DockStyle.Fill;

            button_BarColor.BackColor = barChart.BarColor;
            button_BarBorderColor.BackColor = barChart.BarBorderColor;
            button_AxisColor.BackColor = barChart.AxisColor;
        }

        private void InitImageLists()
        {
            var basePath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Images"));

            imageListSmall.Images.Clear();
            imageListSmall.ImageSize = new Size(16, 16);
            imageListSmall.ColorDepth = ColorDepth.Depth32Bit;

            imageList32x32.Images.Clear();
            imageList32x32.ImageSize = new Size(32, 32);
            imageList32x32.ColorDepth = ColorDepth.Depth32Bit;

            try
            {
                imageListSmall.Images.Add("play", Image.FromFile(Path.Combine(basePath, "play-button-green-icon.png")));
                buttonShow.ImageList = imageListSmall;
                buttonShow.ImageKey = "play";
                buttonShow.TextImageRelation = TextImageRelation.TextBeforeImage;

                imageList32x32.Images.Add("ZoomIn", Image.FromFile(Path.Combine(basePath, "ZoomIn.png")));
                imageList32x32.Images.Add("ZoomOut", Image.FromFile(Path.Combine(basePath, "ZoomOut.png")));

                button_ZoomIn.ImageList = imageList32x32;
                button_ZoomIn.ImageKey = "ZoomIn";
                button_ZoomOut.ImageList = imageList32x32;
                button_ZoomOut.ImageKey = "ZoomOut";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Không thể tải ảnh từ thư mục Images.\n" + ex.Message,
                    "Lỗi nạp ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitDefaultSettings()
        {
            comboBox_GroupBy.SelectedIndex = 0;
            startDate = DateTime.Now.Date.AddDays(-7);
            endDate = DateTime.Now.Date;
            dateTimePicker_StartDate.Value = startDate;
            dateTimePicker_EndDate.Value = endDate;
        }

        private void CheckBox_ValueLabel_CheckedChanged(object sender, EventArgs e)
        {
            barChart.IsValueLabelShowed = checkBox_ValueLabel.Checked;
        }

        private void ComboBox_TableType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox_ID.Items.Clear();
            comboBox_Name.Items.Clear();

            var index = comboBox_TableType.SelectedIndex;
            string sql;
            switch (index)
            {
                case 0: sql = "SELECT IDHD FROM HOADON"; break;
                case 1: sql = "SELECT IDDV, TENDV FROM DICHVU"; break;
                case 2: sql = "SELECT IDKH, HOTEN FROM KHACHHANG"; break;
                case 3: sql = "SELECT IDNV, HOTENNV FROM NHANVIEN"; break;
                default: sql = "SELECT IDBAN FROM BAN"; break;
            }

            var table = dtBase.ReadData(sql);
            foreach (DataRow row in table.Rows)
            {
                comboBox_ID.Items.Add(row[0].ToString());
                comboBox_Name.Items.Add(table.Columns.Count > 1 ? row[1].ToString() : row[0].ToString());
            }
        }

        private void ButtonShow_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                startDate = dateTimePicker_StartDate.Value.Date;
                endDate = dateTimePicker_EndDate.Value.Date;

                if (comboBox_TableType.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn loại đối tượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBox_GroupBy.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn cách hiển thị.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var sql =
                    "SELECT " + GetSelect() +
                    ", DOANHTHU = SUM(" + GetSumExpression() + ") " +
                    "FROM " + GetTableName() + " " +
                    GetCondition() +
                    " GROUP BY " + GetGroupBy();

                currentTable = dtBase.ReadData(sql);

                if (currentTable.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu phù hợp!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DisplaySummary(currentTable);
                DrawChart(currentTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void button_ZoomIn_Click(object sender, EventArgs e) => AdjustZoom(1);
        private void button_ZoomOut_Click(object sender, EventArgs e) => AdjustZoom(-1);

        private void DisplaySummary(DataTable table)
        {
            var rows = table.Rows.Cast<DataRow>();
            textBox_Total.Text = rows.Sum(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
            textBox_AverageValue.Text = rows.Average(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
            textBox_MaxValue.Text = rows.Max(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
            textBox_MinValue.Text = rows.Min(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
        }

        private void DrawChart(DataTable table)
        {
            barChart.ChartItems.Clear();
            foreach (DataRow row in table.Rows)
            {
                var name = row[0].ToString();
                var value = Convert.ToDouble(row["DOANHTHU"]);
                barChart.ChartItems.Add(new ChartItem<double>(name, value));
            }

            barChart.HorizontalText = comboBox_TableType.Text;
            barChart.VerticalText = "Doanh thu (VNĐ)";

            var numBars = table.Rows.Count;
            var totalWidth = Math.Max(panel_Chart.Width, numBars * 120);

            barChart.Width = totalWidth;
            barChart.Height = panel_Chart.Height - 10;

            panel_Chart.AutoScroll = true;
            panel_Chart.AutoScrollMinSize = new Size(barChart.Width, barChart.Height);
            barChart.Refresh();
        }

        private void AdjustZoom(int direction)
        {
            var newIndex = currentZoomRatioIndex + direction;
            if (newIndex < 0 || newIndex >= zoomRatioArr.Length) return;

            currentZoomRatioIndex = newIndex;
            var zoom = zoomRatioArr[currentZoomRatioIndex];

            barChart.ZoomRatio = zoom;
            barChart.Width = (int)(barChart.Width * zoom);
            barChart.Height = (int)(barChart.Height * zoom);

            button_ZoomOut.Enabled = currentZoomRatioIndex > 0;
            button_ZoomIn.Enabled = currentZoomRatioIndex < zoomRatioArr.Length - 1;

            barChart.Refresh();
            barChart.Parent?.PerformLayout();
        }

        private string GetTableName()
        {
            switch (comboBox_TableType.SelectedIndex)
            {
                case 0: return "HOADON h";
                case 1: return "HOADONDV hdv JOIN DICHVU dv ON hdv.IDDV = dv.IDDV JOIN HOADON h ON hdv.IDHD = h.IDHD";
                case 2: return "HOADON h JOIN KHACHHANG k ON h.IDKH = k.IDKH";
                case 3: return "HOADON h JOIN NHANVIEN n ON h.IDNV = n.IDNV";
                default: return "PHIENCHOI p JOIN BAN b ON p.IDBAN = b.IDBAN JOIN HOADON h ON h.IDPHIEN = p.IDPHIEN";
            }
        }

        private string GetSumExpression()
        {
            switch (comboBox_TableType.SelectedIndex)
            {
                case 1: return "hdv.SOLUONG * dv.GIATIEN";
                case 4: return "(DATEDIFF(MINUTE, p.GIOBATDAU, p.GIOKETTHUC) / 60.0 * b.GIATIEN)";
                default: return "h.TONGTIEN";
            }
        }

        private string GetSelect()
        {
            var g = comboBox_GroupBy.SelectedIndex;
            var t = comboBox_TableType.SelectedIndex;

            if (g == 0)
            {
                switch (t)
                {
                    case 0: return "NAME = N'Hóa đơn ' + h.IDHD";
                    case 1: return "NAME = dv.TENDV + ' (' + dv.IDDV + ')'";
                    case 2: return "NAME = k.HOTEN + ' (' + h.IDKH + ')'";
                    case 3: return "NAME = n.HOTENNV + ' (' + h.IDNV + ')'";
                    default: return "NAME = N'Bàn ' + b.IDBAN";
                }
            }

            return g == 1 ? "NGHD = CAST(h.NGAYLAP AS DATE)" : "NAM = YEAR(h.NGAYLAP), THANG = MONTH(h.NGAYLAP)";
        }

        private string GetGroupBy()
        {
            var g = comboBox_GroupBy.SelectedIndex;
            var t = comboBox_TableType.SelectedIndex;

            if (g == 0)
            {
                switch (t)
                {
                    case 0: return "h.IDHD";
                    case 1: return "dv.IDDV, dv.TENDV";
                    case 2: return "h.IDKH, k.HOTEN";
                    case 3: return "h.IDNV, n.HOTENNV";
                    default: return "b.IDBAN";
                }
            }

            return g == 1 ? "CAST(h.NGAYLAP AS DATE)" : "YEAR(h.NGAYLAP), MONTH(h.NGAYLAP)";
        }

        private string GetCondition()
        {
            var t = comboBox_TableType.SelectedIndex;
            var where = "WHERE ";

            if (comboBox_ID.SelectedIndex != -1)
            {
                var id = comboBox_ID.SelectedItem.ToString();
                var col = t == 0 ? "h.IDHD" :
                          t == 1 ? "dv.IDDV" :
                          t == 2 ? "h.IDKH" :
                          t == 3 ? "h.IDNV" : "b.IDBAN";
                where += col + " = N'" + id + "' AND ";
            }

            if (comboBox_GroupBy.SelectedIndex <= 1)
            {
                var s = startDate.ToString("yyyy-MM-dd");
                var e = endDate.ToString("yyyy-MM-dd");
                where += $"CAST(h.NGAYLAP AS DATE) BETWEEN '{s}' AND '{e}'";
            }
            else
            {
                var s = startDate.ToString("yyyy-MM-dd");
                var e = endDate.ToString("yyyy-MM-dd");
                where += "YEAR(h.NGAYLAP) * 12 + MONTH(h.NGAYLAP) BETWEEN " +
                         $"YEAR('{s}') * 12 + MONTH('{s}') AND " +
                         $"YEAR('{e}') * 12 + MONTH('{e}')";
            }

            return where;
        }
    }
}
