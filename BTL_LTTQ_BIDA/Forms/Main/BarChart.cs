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

        public BarChart()
        {
            BackColor = Color.White;
            DoubleBuffered = true;
        }

        public BarChart(IContainer container) : this()
        {
            container?.Add(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (chartItems.Count == 0)
                return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Layout constants
            const int leftMargin = 60;
            const int topMargin = 10;
            const int rightMargin = 20;
            const int bottomMargin = 45;
            const float axisThickness = 2f;

            float chartHeight = Math.Max(Height - topMargin - bottomMargin, 10);
            float chartWidth = Math.Max(Width - leftMargin - rightMargin - 20, 10);

            float zoom = zoomRatio;
            float barWidth = Math.Max((chartWidth / Math.Max(chartItems.Count, 1)) * 0.6f * zoom, 6f);
            float gap = barWidth * 0.5f;

            // Safe maximum value
            float maxValue = chartItems.Max(c => Convert.ToSingle(c.Value));
            if (maxValue <= 0f) maxValue = 1f;

            // Draw axes
            using (var axisPen = new Pen(axisColor, axisThickness))
            {
                g.DrawLine(axisPen, leftMargin, topMargin, leftMargin, topMargin + chartHeight);
                g.DrawLine(axisPen, leftMargin, topMargin + chartHeight, Width - rightMargin, topMargin + chartHeight);
            }

            // Center bars when there are few
            float totalBarsWidth = chartItems.Count * (barWidth + gap);
            float startX = leftMargin + Math.Max((chartWidth - totalBarsWidth) / 2f, 0f);

            // Rotate labels when many bars
            labelRotationAngle = chartItems.Count > 15 ? 60f : 45f;

            using (var barBrush = new SolidBrush(barColor))
            using (var borderPen = new Pen(barBorderColor, 1f))
            using (var valueFont = new Font(Font.FontFamily, Math.Max(7f * zoom, 7f)))
            using (var labelFont = new Font(Font.FontFamily, Math.Max(8f * zoom, 8f)))
            using (var axisFont = new Font(Font.FontFamily, 9f, FontStyle.Bold))
            {
                for (int i = 0; i < chartItems.Count; i++)
                {
                    var item = chartItems[i];
                    float value = Convert.ToSingle(item.Value);
                    float barHeight = (value / maxValue) * (chartHeight - 30f) * zoom;
                    float x = startX + i * (barWidth + gap);
                    float y = topMargin + chartHeight - barHeight;

                    var rect = new RectangleF(x, y, barWidth, barHeight);

                    // Draw bar and border
                    g.FillRectangle(barBrush, rect);
                    g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width, rect.Height);

                    // Value label
                    if (isValueLabelShow)
                    {
                        string val = value.ToString("N0");
                        var valSize = g.MeasureString(val, valueFont);
                        g.DrawString(val, valueFont, Brushes.Black,
                                     x + (barWidth - valSize.Width) / 2f,
                                     y - valSize.Height - 2f);
                    }

                    // Rotated X label (save/restore Graphics state)
                    string label = item.Name ?? string.Empty;
                    var lblSize = g.MeasureString(label, labelFont);
                    var gs = g.Save();
                    g.TranslateTransform(x + barWidth / 2f, topMargin + chartHeight + 5f);
                    g.RotateTransform(-labelRotationAngle);
                    g.DrawString(label, labelFont, Brushes.Black, -lblSize.Width / 2f, 0f);
                    g.Restore(gs);
                }

                // Vertical axis title
                var gs2 = g.Save();
                g.TranslateTransform(10f, topMargin + chartHeight / 2f);
                g.RotateTransform(-90f);
                g.DrawString(verticalText, axisFont, Brushes.Black, 0f, 0f);
                g.Restore(gs2);

                // Horizontal axis title (centered)
                var hTextSize = g.MeasureString(horizontalText, axisFont);
                g.DrawString(horizontalText, axisFont, Brushes.Black,
                    leftMargin + chartWidth / 2f - hTextSize.Width / 2f,
                    topMargin + chartHeight + 20f);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        // Properties
        public Color BarColor
        {
            get => barColor;
            set { barColor = value; Invalidate(); }
        }

        public Color BarBorderColor
        {
            get => barBorderColor;
            set { barBorderColor = value; Invalidate(); }
        }

        public Color AxisColor
        {
            get => axisColor;
            set { axisColor = value; Invalidate(); }
        }

        public string HorizontalText
        {
            get => horizontalText;
            set { horizontalText = value ?? string.Empty; Invalidate(); }
        }

        public string VerticalText
        {
            get => verticalText;
            set { verticalText = value ?? string.Empty; Invalidate(); }
        }

        public bool IsValueLabelShowed
        {
            get => isValueLabelShow;
            set { isValueLabelShow = value; Invalidate(); }
        }

        public List<ChartItem<T>> ChartItems => chartItems;

        public float ZoomRatio
        {
            get => zoomRatio;
            set
            {
                zoomRatio = Math.Max(0.25f, Math.Min(3f, value));
                Invalidate();
            }
        }

        public float LabelRotationAngle
        {
            get => labelRotationAngle;
            set { labelRotationAngle = value; Invalidate(); }
        }

        public bool AutoCalculateVerticalValueList
        {
            get => autoCalculateVerticalValueList;
            set { autoCalculateVerticalValueList = value; Invalidate(); }
        }
    }

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
