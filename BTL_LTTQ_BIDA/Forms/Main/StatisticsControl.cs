﻿using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using BTL_LTTQ_BIDA.Data;

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

        private int currentPageNum = 1;
        private int maxPageNum = 1;
        private int beginIndex = 0;
        private int endIndex = 0;

        // ==============================================
        // CONSTRUCTOR
        // ==============================================
        public StatisticsControl()
        {
            InitializeComponent();

            barChart = new BarChart<double>
            {
                BarColor = Color.Cyan,
                VerticalText = "Doanh thu (VNĐ)",
                AutoCalculateVerticalValueList = true
            };

            // Enable double buffering to reduce flicker
            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, barChart, new object[] { true });
        }

        // ==============================================
        // FORM LOAD
        // ==============================================
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

        // ==============================================
        // INITIALIZATION
        // ==============================================
        private void InitImageLists()
        {
            var basePath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Images"));
            imageListSmall.Images.Clear();
            imageList32x32.Images.Clear();

            imageListSmall.ImageSize = new Size(16, 16);
            imageListSmall.ColorDepth = ColorDepth.Depth32Bit;
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

        // ==============================================
        // EVENTS
        // ==============================================
        private void CheckBox_ValueLabel_CheckedChanged(object sender, EventArgs e)
        {
            barChart.IsValueLabelShowed = checkBox_ValueLabel.Checked;
        }

        private void ComboBox_TableType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox_ID.Items.Clear();
            comboBox_Name.Items.Clear();

            string sql;
            switch (comboBox_TableType.SelectedIndex)
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

                var sql = "SELECT " + GetSelect() +
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

        private void label_Page_TextChanged(object sender, EventArgs e)
        {
            button_PageDecrease.Enabled = currentPageNum > 1;
            button_PageIncrease.Enabled = currentPageNum < maxPageNum;
        }

        // ==============================================
        // DRAW CHART
        // ==============================================
        private void DrawChart(DataTable table)
        {
            barChart.HorizontalText = comboBox_TableType.Text;
            barChart.VerticalText = "Doanh thu (VNĐ)";

            panel_Chart.AutoScroll = false;

            beginIndex = 0;
            endIndex = Math.Min(15, table.Rows.Count);
            currentPageNum = 1;
            maxPageNum = (int)Math.Ceiling(table.Rows.Count / 15.0);

            label_Page.Text = $"Trang {currentPageNum}/{maxPageNum}";

            barChart.Dock = DockStyle.Fill;
            barChart.Width = panel_Chart.Width - 5;
            barChart.Height = panel_Chart.Height - 5;

            UpdateChartPage();
            barChart.Refresh();
        }

        private void UpdateChartPage()
        {
            barChart.ChartItems.Clear();

            for (int i = beginIndex; i < endIndex; i++)
            {
                string name = currentTable.Rows[i][0].ToString();
                double value = Convert.ToDouble(currentTable.Rows[i]["DOANHTHU"]);
                barChart.ChartItems.Add(new ChartItem<double>(name, value));
            }

            barChart.Refresh();
        }

        // ==============================================
        // PAGINATION
        // ==============================================
        private void button_PageDecrease_Click(object sender, EventArgs e)
        {
            if (currentTable == null || currentTable.Rows.Count == 0 || currentPageNum <= 1) return;

            currentPageNum--;
            label_Page.Text = $"Trang {currentPageNum}/{maxPageNum}";

            endIndex = beginIndex;
            beginIndex = Math.Max(0, beginIndex - 15);

            UpdateChartPage();
        }

        private void button_PageIncrease_Click(object sender, EventArgs e)
        {
            if (currentTable == null || currentTable.Rows.Count == 0 || currentPageNum >= maxPageNum) return;

            currentPageNum++;
            label_Page.Text = $"Trang {currentPageNum}/{maxPageNum}";

            beginIndex = endIndex;
            endIndex = Math.Min(currentTable.Rows.Count, endIndex + 15);

            UpdateChartPage();
        }

        // ==============================================
        // OTHER METHODS
        // ==============================================
        private void AdjustZoom(int direction)
        {
            int newIndex = currentZoomRatioIndex + direction;
            if (newIndex < 0 || newIndex >= zoomRatioArr.Length) return;

            currentZoomRatioIndex = newIndex;
            float zoom = zoomRatioArr[currentZoomRatioIndex];

            barChart.ZoomRatio = zoom;
            barChart.Refresh();

            button_ZoomOut.Enabled = currentZoomRatioIndex > 0;
            button_ZoomIn.Enabled = currentZoomRatioIndex < zoomRatioArr.Length - 1;
        }

        private void DisplaySummary(DataTable table)
        {
            var rows = table.Rows.Cast<DataRow>();
            textBox_Total.Text = rows.Sum(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
            textBox_AverageValue.Text = rows.Average(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
            textBox_MaxValue.Text = rows.Max(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
            textBox_MinValue.Text = rows.Min(r => Convert.ToDecimal(r["DOANHTHU"])).ToString("C0");
        }

        // ==============================================
        // SQL HELPERS
        // ==============================================
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
            int g = comboBox_GroupBy.SelectedIndex;
            int t = comboBox_TableType.SelectedIndex;

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

            return g == 1 ? "NGHD = CAST(h.NGAYLAP AS DATE)" :
                            "NAM = YEAR(h.NGAYLAP), THANG = MONTH(h.NGAYLAP)";
        }

        private string GetGroupBy()
        {
            int g = comboBox_GroupBy.SelectedIndex;
            int t = comboBox_TableType.SelectedIndex;

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

            return g == 1 ? "CAST(h.NGAYLAP AS DATE)" :
                            "YEAR(h.NGAYLAP), MONTH(h.NGAYLAP)";
        }

        private string GetCondition()
        {
            int t = comboBox_TableType.SelectedIndex;
            string where = "WHERE ";

            if (comboBox_ID.SelectedIndex != -1)
            {
                string id = comboBox_ID.SelectedItem.ToString();
                string col = t == 0 ? "h.IDHD" :
                             t == 1 ? "dv.IDDV" :
                             t == 2 ? "h.IDKH" :
                             t == 3 ? "h.IDNV" : "b.IDBAN";
                where += $"{col} = N'{id}' AND ";
            }

            string s = startDate.ToString("yyyy-MM-dd");
            string e = endDate.ToString("yyyy-MM-dd");

            if (comboBox_GroupBy.SelectedIndex <= 1)
                where += $"CAST(h.NGAYLAP AS DATE) BETWEEN '{s}' AND '{e}'";
            else
                where += "YEAR(h.NGAYLAP) * 12 + MONTH(h.NGAYLAP) BETWEEN " +
                         $"YEAR('{s}') * 12 + MONTH('{s}') AND YEAR('{e}') * 12 + MONTH('{e}')";

            return where;
        }

        private void button_ExportExcel_Click(object sender, EventArgs e)
        {
            if (currentTable == null || currentTable.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var sfd = new SaveFileDialog
                {
                    Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                    FileName = $"ThongKe_{comboBox_TableType.Text}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                })
                {
                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    // Create Excel application
                    Excel.Application app = null;
                    Excel.Workbook workbook = null;
                    Excel.Worksheet sheet = null;
                    Excel.Chart chart = null;
                    Excel.ChartObject chartObject = null;

                    try
                    {
                        app = new Excel.Application();
                        workbook = app.Workbooks.Add(Type.Missing);
                        sheet = (Excel.Worksheet)workbook.Sheets[1];
                        sheet.Name = "ThongKe";

                        // Title
                        sheet.Cells[1, 1] = "THỐNG KÊ DOANH THU " + comboBox_TableType.Text.ToUpper();
                        Excel.Range titleRange = sheet.Range["A1", "D1"];
                        titleRange.Merge();
                        titleRange.Font.Size = 14;
                        titleRange.Font.Bold = true;
                        titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // Column headers
                        for (int i = 0; i < currentTable.Columns.Count; i++)
                        {
                            sheet.Cells[3, i + 1] = currentTable.Columns[i].ColumnName;
                            Excel.Range cell = (Excel.Range)sheet.Cells[3, i + 1];
                            cell.Font.Bold = true;
                            cell.Interior.Color = ColorTranslator.ToOle(Color.LightCyan);
                            cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }

                        // Data
                        for (int i = 0; i < currentTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < currentTable.Columns.Count; j++)
                            {
                                sheet.Cells[i + 4, j + 1] = currentTable.Rows[i][j].ToString();
                                ((Excel.Range)sheet.Cells[i + 4, j + 1]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            }
                        }

                        // Auto fit and chart
                        sheet.Columns.AutoFit();

                        var charts = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
                        chartObject = charts.Add(350, 60, 500, 300);
                        chart = chartObject.Chart;

                        Excel.Range chartRange = sheet.Range[
                            sheet.Cells[3, 1],
                            sheet.Cells[currentTable.Rows.Count + 3, currentTable.Columns.Count]
                        ];

                        chart.SetSourceData(chartRange);
                        chart.ChartType = Excel.XlChartType.xlColumnClustered;
                        chart.ChartTitle.Text = "Biểu đồ doanh thu " + comboBox_TableType.Text;
                        chart.HasLegend = true;
                        chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionBottom;
                        chart.Axes(Excel.XlAxisType.xlCategory).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlCategory).AxisTitle.Text = comboBox_TableType.Text;
                        chart.Axes(Excel.XlAxisType.xlValue).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlValue).AxisTitle.Text = "Doanh thu (VNĐ)";

                        // Save
                        workbook.SaveAs(sfd.FileName);
                    }
                    finally
                    {
                        // Close and release COM objects
                        try { workbook?.Close(); } catch { }
                        try { app?.Quit(); } catch { }

                        if (chart != null) Marshal.ReleaseComObject(chart);
                        if (chartObject != null) Marshal.ReleaseComObject(chartObject);
                        if (sheet != null) Marshal.ReleaseComObject(sheet);
                        if (workbook != null) Marshal.ReleaseComObject(workbook);
                        if (app != null) Marshal.ReleaseComObject(app);
                    }

                    MessageBox.Show($"✅ Xuất file Excel thành công!\nĐường dẫn:\n{sfd.FileName}",
                        "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi xuất Excel: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
