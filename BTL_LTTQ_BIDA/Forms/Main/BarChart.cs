using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    public partial class BarChart<T> : Control where T : struct
    {
        private Color barColor = Color.SteelBlue;
        private Color barBorderColor = Color.Black;
        private Color axisColor = Color.DimGray;
        private string horizontalText = string.Empty;
        private string verticalText = string.Empty;
        private bool isValueLabelShow;
        private bool autoCalculateVerticalValueList;
        private float zoomRatio = 1.0f;
        private float labelRotationAngle = 45f;

        private readonly List<ChartItem<T>> chartItems = new List<ChartItem<T>>();
        private int hoveredIndex = -1;
        private readonly ToolTip tooltip = new ToolTip();

        public BarChart()
        {
            BackColor = Color.White;
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
        }

        public BarChart(IContainer container) : this()
        {
            if (container != null)
                container.Add(this);
        }

        //===================== DRAW ======================
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (chartItems.Count == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Layout constants
            const int leftMargin = 60, topMargin = 15, rightMargin = 20, bottomMargin = 50;
            float chartHeight = Math.Max(Height - topMargin - bottomMargin, 10);
            float chartWidth = Math.Max(Width - leftMargin - rightMargin, 10);
            float zoom = zoomRatio;

            // bar size
            float barWidth = Math.Max((chartWidth / Math.Max(chartItems.Count, 1)) * 0.6f * zoom, 6f);
            float gap = barWidth * 0.4f;

            float maxValue = chartItems.Max(c => Convert.ToSingle(c.Value));
            if (maxValue <= 0f) maxValue = 1f;

            // Grid lines
            using (Pen gridPen = new Pen(Color.LightGray, 1f) { DashStyle = DashStyle.Dash })
            {
                for (int i = 0; i <= 5; i++)
                {
                    float y = topMargin + chartHeight - (i * chartHeight / 5f);
                    g.DrawLine(gridPen, leftMargin, y, Width - rightMargin, y);
                }
            }

            // Axes
            using (Pen axisPen = new Pen(axisColor, 2f))
            {
                g.DrawLine(axisPen, leftMargin, topMargin, leftMargin, topMargin + chartHeight);
                g.DrawLine(axisPen, leftMargin, topMargin + chartHeight, Width - rightMargin, topMargin + chartHeight);
            }

            // Center bars
            float totalBarsWidth = chartItems.Count * (barWidth + gap);
            float startX = leftMargin + Math.Max((chartWidth - totalBarsWidth) / 2f, 0f);

            // Rotation angle
            labelRotationAngle = chartItems.Count > 15 ? 60f : 45f;

            using (Font valueFont = new Font("Segoe UI", 8f * zoom))
            using (Font labelFont = new Font("Segoe UI", 8f * zoom))
            using (Font axisFont = new Font("Segoe UI", 9f, FontStyle.Bold))
            {
                for (int i = 0; i < chartItems.Count; i++)
                {
                    var item = chartItems[i];
                    float value = Convert.ToSingle(item.Value);
                    float barHeight = (value / maxValue) * (chartHeight - 25f);
                    float x = startX + i * (barWidth + gap);
                    float y = topMargin + chartHeight - barHeight;

                    RectangleF rect = new RectangleF(x, y, barWidth, barHeight);

                    // shadow
                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                        g.FillRectangle(shadowBrush, rect.X + 3, rect.Y + 3, rect.Width, rect.Height);

                    // gradient fill
                    Color mainColor = (i == hoveredIndex)
                        ? ControlPaint.Light(barColor, 0.3f)
                        : barColor;
                    using (LinearGradientBrush gradient = new LinearGradientBrush(rect,
                        ControlPaint.Light(mainColor, 0.2f),
                        ControlPaint.Dark(mainColor, 0.2f),
                        LinearGradientMode.Vertical))
                    {
                        // rounded top corners
                        GraphicsPath path = RoundedRect(rect, 6f);
                        g.FillPath(gradient, path);
                        using (Pen borderPen = new Pen(barBorderColor, 1f))
                            g.DrawPath(borderPen, path);
                    }

                    // value label
                    if (isValueLabelShow)
                    {
                        string val = FormatValue(value);
                        SizeF valSize = g.MeasureString(val, valueFont);
                        g.DrawString(val, valueFont, Brushes.Black,
                            x + (barWidth - valSize.Width) / 2f,
                            y - valSize.Height - 2f);
                    }

                    // x label
                    string label = item.Name ?? string.Empty;
                    SizeF lblSize = g.MeasureString(label, labelFont);
                    GraphicsState state = g.Save();
                    g.TranslateTransform(x + barWidth / 2f, topMargin + chartHeight + 5f);
                    g.RotateTransform(-labelRotationAngle);
                    g.DrawString(label, labelFont, Brushes.Black, -lblSize.Width / 2f, 0f);
                    g.Restore(state);
                }

                // Vertical title
                GraphicsState stateV = g.Save();
                g.TranslateTransform(15f, topMargin + chartHeight / 2f);
                g.RotateTransform(-90f);
                g.DrawString(verticalText, axisFont, Brushes.Black, 0, 0);
                g.Restore(stateV);

                // Horizontal title
                SizeF hSize = g.MeasureString(horizontalText, axisFont);
                g.DrawString(horizontalText, axisFont, Brushes.Black,
                    leftMargin + chartWidth / 2f - hSize.Width / 2f,
                    topMargin + chartHeight + 25f);

                // Border around chart
                using (Pen borderPen = new Pen(Color.Gray, 1))
                    g.DrawRectangle(borderPen, leftMargin - 5, topMargin - 5, chartWidth + 10, chartHeight + 10);
            }
        }

        //===================== HELPER ======================
        private GraphicsPath RoundedRect(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float r2 = radius * 2f;
            path.AddArc(rect.X, rect.Y, r2, r2, 180, 90);
            path.AddArc(rect.Right - r2, rect.Y, r2, r2, 270, 90);
            path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            path.CloseFigure();
            return path;
        }

        private string FormatValue(double value)
        {
            if (value >= 1_000_000_000) return (value / 1_000_000_000D).ToString("0.#") + "B";
            if (value >= 1_000_000) return (value / 1_000_000D).ToString("0.#") + "M";
            if (value >= 1_000) return (value / 1_000D).ToString("0.#") + "K";
            return value.ToString("N0");
        }

        //===================== INTERACTION ======================
        protected override void OnMouseMove(MouseEventArgs e)
        {
            for (int i = 0; i < chartItems.Count; i++)
            {
                RectangleF rect = GetBarRectangle(i);
                if (rect.Contains(e.Location))
                {
                    if (hoveredIndex != i)
                    {
                        hoveredIndex = i;
                        Invalidate();
                    }
                    tooltip.SetToolTip(this, chartItems[i].Name + ": " + chartItems[i].Value.ToString());
                    return;
                }
            }

            if (hoveredIndex != -1)
            {
                hoveredIndex = -1;
                tooltip.Hide(this);
                Invalidate();
            }
        }

        private RectangleF GetBarRectangle(int index)
        {
            const int leftMargin = 60, topMargin = 15, rightMargin = 20, bottomMargin = 50;
            float chartHeight = Math.Max(Height - topMargin - bottomMargin, 10);
            float chartWidth = Math.Max(Width - leftMargin - rightMargin, 10);
            float barWidth = Math.Max((chartWidth / Math.Max(chartItems.Count, 1)) * 0.6f * zoomRatio, 6f);
            float gap = barWidth * 0.4f;
            float totalBarsWidth = chartItems.Count * (barWidth + gap);
            float startX = leftMargin + Math.Max((chartWidth - totalBarsWidth) / 2f, 0f);

            float maxValue = chartItems.Max(c => Convert.ToSingle(c.Value));
            if (maxValue <= 0f) maxValue = 1f;

            float value = Convert.ToSingle(chartItems[index].Value);
            float barHeight = (value / maxValue) * (chartHeight - 25f);
            float x = startX + index * (barWidth + gap);
            float y = topMargin + chartHeight - barHeight;
            return new RectangleF(x, y, barWidth, barHeight);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        //===================== PROPERTIES ======================
        public Color BarColor { get => barColor; set { barColor = value; Invalidate(); } }
        public Color BarBorderColor { get => barBorderColor; set { barBorderColor = value; Invalidate(); } }
        public Color AxisColor { get => axisColor; set { axisColor = value; Invalidate(); } }
        public string HorizontalText { get => horizontalText; set { horizontalText = value ?? ""; Invalidate(); } }
        public string VerticalText { get => verticalText; set { verticalText = value ?? ""; Invalidate(); } }
        public bool IsValueLabelShowed { get => isValueLabelShow; set { isValueLabelShow = value; Invalidate(); } }
        public List<ChartItem<T>> ChartItems => chartItems;
        public float ZoomRatio { get => zoomRatio; set { zoomRatio = Math.Max(0.25f, Math.Min(3f, value)); Invalidate(); } }
        public float LabelRotationAngle { get => labelRotationAngle; set { labelRotationAngle = value; Invalidate(); } }
        public bool AutoCalculateVerticalValueList { get => autoCalculateVerticalValueList; set { autoCalculateVerticalValueList = value; Invalidate(); } }
    }

    //===================== DATA ITEM ======================
    public class ChartItem<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }

        public ChartItem(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => Name;
    }
}
