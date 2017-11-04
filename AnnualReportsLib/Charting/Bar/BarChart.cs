using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace AnnualReports.Charting
{
    public class BarChart : IDisposable
    {
        public string Title { get; set; }
        public string LegendTitleI { get; set; }
        public string LegendTitleII { get; set; }

        List<Bar> bars = new List<Bar>();

        public int Width { get; }
        public int Height { get; }
        public int Padding { get; }
        //public int LabelsWidth { get; set; }
        public int TitleHeight { get; }

        public int LabelsHeight { get; }
        public int ValuesWidth { get; }

        decimal maxChartValue;
        decimal scaleFactor;

        string[] labels;

        readonly Color xColor = Color.Black;
        readonly Color yColor = Color.Black;
        readonly Color lineColor = Color.FromArgb(230, 230, 240);
        readonly Color axisColor = Color.FromArgb(230, 230, 240);

        private int estimatedValueHeight = 22;

        private int upperYAxis; //zbog cjelobrojnog dijeljenja dodje do malog odstupanja

        private int legendWidth = 80;

        private int textPadding = 2;

        private readonly Font labelFont;
        private readonly Font titleFont;
        private readonly Font valueLabelFont;

        public BarChart(List<BarEntry> entries, int width = 800, int height = 500)
        {
            Width = width;
            Height = height;
            Padding = height / 15;
            ValuesWidth = width / 8;
            LabelsHeight = height / 12;
            TitleHeight = height / 6;

            labelFont = FontManager.GetFont(15);
            titleFont = FontManager.GetFont(20);
            valueLabelFont = FontManager.GetFont(12);

            decimal maxEntryValue = 0;
            entries.ForEach(x =>
            {
                if (x.ValueI > maxEntryValue)
                    maxEntryValue = x.ValueI;
                if (x.ValueII > maxEntryValue)
                    maxEntryValue = x.ValueII;
            });

            maxChartValue = Labels.GetChartMax(maxEntryValue);

            labels = Labels.GetValueLabels(maxChartValue, 5);

            bars = Bar.GetBars(width - 2 * Padding - ValuesWidth, height - 2 * Padding - LabelsHeight - TitleHeight, entries, maxChartValue);

            scaleFactor = height / maxChartValue;
        }

        public Bitmap Draw()
        {
            Bitmap bitmap = new Bitmap(Width, Height);

            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);

            var pen = new Pen(axisColor, 1);

            DrawTitle(graphics);

            DrawLines(graphics, 4);

            DrawBarsWithLabels(graphics);

            DrawAxes(graphics);

            DrawLegend(graphics);

            return bitmap;
        }

        private void DrawAxes(Graphics graphics)
        {
            var pen = new Pen(axisColor, 1);

            graphics.DrawLine(pen, new Point(ValuesWidth, Height - Padding - LabelsHeight), new Point(Width - Padding, Height - Padding - LabelsHeight));

            graphics.DrawLine(pen, new Point(ValuesWidth, Height - Padding - LabelsHeight), new Point(ValuesWidth, upperYAxis));
        }

        private void DrawLines(Graphics graphics, int numberOfLines)
        {
            var startX = ValuesWidth;
            var endX = Width - Padding;
            var startY = Padding + TitleHeight;
            var endY = Height - Padding - LabelsHeight;

            var pen = new Pen(lineColor, 1);
            var brush = new SolidBrush(yColor);

            int step = (endY - startY) / numberOfLines + 1;

            int currentY = endY;

            StringFormat stringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Far,
                Alignment = StringAlignment.Far
            };

            for (int i = 0; i <= numberOfLines; i++)
            {
                var label = labels[i];

                var startPoint = new Point(startX, currentY);
                var endPoint = new Point(endX, currentY);
                graphics.DrawLine(pen, startPoint, endPoint);

                var layoutRectangle = new Rectangle(0, currentY - estimatedValueHeight / 2, ValuesWidth - textPadding, estimatedValueHeight);
                graphics.DrawString(label, valueLabelFont, brush, layoutRectangle, stringFormat);

                upperYAxis = currentY;
                currentY -= step;
            }
        }

        private void DrawBarsWithLabels(Graphics graphics)
        {
            int size = bars.Count;

            var brush = new SolidBrush(xColor);

            StringFormat stringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            var barPadding = 0;
            if (bars != null && bars.Count >0)
            {
                 barPadding = (int)(bars[0].Width * 0.05);
            }

            for (int i = 0; i < size; i++)
            {
                var bar = bars[i];

                var barBrushI = new SolidBrush(bar.ColorI);

                Point barStart1 = new Point()
                {
                    X = ValuesWidth + bar.StartingPosition,
                    Y = Height - (Padding + LabelsHeight) - bar.HeightI
                };
                graphics.FillRectangle(barBrushI, new Rectangle(barStart1.X + barPadding, barStart1.Y, bar.Width / 2 - (2*barPadding), bar.HeightI));

                var barBrushII = new SolidBrush(bar.ColorII);

                Point barStart2 = new Point()
                {
                    X = ValuesWidth + bar.StartingPosition + bar.Width / 2,
                    Y = Height - (Padding + LabelsHeight) - bar.HeightII
                };
                graphics.FillRectangle(barBrushII, new Rectangle(barStart2.X + barPadding, barStart2.Y, bar.Width / 2 - (2 * barPadding), bar.HeightII));

                var labelY = 0;
                if (i%2 ==1)
                {
                    labelY = Height - Padding - (LabelsHeight / 2);
                }
                else
                {
                    labelY = Height - Padding - (LabelsHeight);
                }

                var layoutRectangle = new Rectangle(ValuesWidth + bar.StartingPosition - (int) (1.0 * bar.Width), labelY, 3 * bar.Width, LabelsHeight);
                graphics.DrawString(bar.Label, labelFont, brush, layoutRectangle, stringFormat);
            }
        }

        private void DrawTitle(Graphics graphics)
        {
            var brush = new SolidBrush(Color.Black);
            var layoutRectangle = new Rectangle(0, Padding, Width - (Padding), TitleHeight * 1 / 2);

            StringFormat stringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            graphics.DrawString(Title, titleFont, brush, layoutRectangle, stringFormat);
        }

        private void DrawLegend(Graphics graphics)
        {
            var textBrush = new SolidBrush(Color.DimGray);
            var brushI = new SolidBrush(Color.LightSteelBlue);
            var brushII = new SolidBrush(Color.SteelBlue);

            int y = Padding + TitleHeight * 1 / 2;
            int x = Width / 2 - legendWidth / 2 - 30;

            var rectangleI = new Rectangle(x, y + 1, 14, 14);
            var rectangleII = new Rectangle(x + 100, y + 1, 14, 14);

            graphics.FillRectangle(brushI, rectangleI);
            graphics.FillRectangle(brushII, rectangleII);

            graphics.DrawString(LegendTitleI, labelFont, textBrush, new Point(x + 20, y));
            graphics.DrawString(LegendTitleII, labelFont, textBrush, new Point(x + 120, y));
        }

        public void Dispose()
        {
            labelFont.Dispose();
            titleFont.Dispose();
        }
    }
}