using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace AnnualReports.Charting
{
    public class PieChart
    {
        public int ChartSize { get; set; }

        public int ChartPadding { get; set; }

        public int LegendWidth { get; set; }

        public int LegendPadding { get; set; }

        public int LeftPadding { get; set; }

        public int Height { get { return ChartSize + titleHeight; } }

        public int Width
        {
            get { return (int)(LeftPadding + ChartSize + LegendWidth); }
        }

        public int ChartRadius { get { return (ChartSize - ChartPadding) / 2; } }

        private int titleHeight = 60;

        public string Title { get; set; }

        List<Pie> pies = new List<Pie>();

        List<Color> colors = new List<Color>();

        static List<Color> possibleColors = new List<Color>() {
            Color.FromArgb(255,141,211,199),
            Color.FromArgb(255, 190,186,218),
            Color.FromArgb(255,251,128,114),
            Color.FromArgb(255,128,177,211),
            Color.FromArgb(255, 253,180,98),
            Color.FromArgb(255, 179,222,105),
            Color.FromArgb(255,252,205,229),
            Color.FromArgb(255, 217,217,217),
            Color.FromArgb(255, 188,128,189),
            Color.FromArgb(255, 204,235,197),
            Color.FromArgb(255, 255,255,179),
            Color.Beige,
            Color.LightSteelBlue,
            Color.Orange,
            Color.Teal,
            Color.SlateBlue,
            Color.LightSteelBlue,
        };

        public PieChart(List<Tuple<string, decimal>> entries)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            CalculateColors(entries.Count);
            CalculatePies(entries);

            ChartSize = 450;
            ChartPadding = 50;

            LeftPadding = 0;

            LegendWidth = 500;
            LegendPadding = 50;
        }

        public Bitmap DrawChart()
        {
            Bitmap image = new Bitmap(ChartSize, ChartSize);
            Graphics graph = Graphics.FromImage(image);

            Rectangle rect = new Rectangle(ChartPadding, ChartPadding, ChartSize - 2 * ChartPadding, ChartSize - 2 * ChartPadding);

            using (var font = FontManager.GetBoldFont(12))
            {

                var labelBrush = new SolidBrush(Color.White);
                var labelOutsideBrush = new SolidBrush(Color.DimGray);
                var format = new StringFormat()
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };

                for (int i = 0; i < pies.Count; i++)
                {
                    var pie = pies[i];

                    SolidBrush SliceBrush = new SolidBrush(pies[i].Color);
                    var pen = new Pen(Color.White, 1);

                    graph.FillPie(SliceBrush, rect, (float)pie.StartingAngle, (float)pie.Angle);
                    graph.DrawPie(pen, rect, (float)pie.StartingAngle, (float)pie.Angle);
                    graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                    if (pie.Angle > 10)
                    {
                        var labelPoint = GetLabelPoint((int)(ChartRadius * 0.7), pie.StartingAngle, pie.Angle);
                        graph.DrawString((pie.Angle / 3.6).ToString("N2") + "%", font, labelBrush, labelPoint, format);
                    }
                    else if (pie.Angle > 0)
                    {
                        var labelPoint = GetLabelPoint((int)(ChartRadius * 1.05), pie.StartingAngle, pie.Angle);
                        graph.DrawString((pie.Angle / 3.6).ToString("N2") + "%", font, labelOutsideBrush, labelPoint, format);
                    }
                }
            }

            return image;
        }

        private Point GetLabelPoint(int radius, double startAngle, double arcAngle)
        {
            double centerAngle = (startAngle + arcAngle / 2) / 180 * Math.PI;

            //Calculate out the center point of the string region, also the center of the pie.
            Point center = new Point()
            {
                X = (int)(ChartSize / 2 + radius * Math.Cos(centerAngle)),
                Y = (int)(ChartSize / 2 + radius * Math.Sin(centerAngle))
            };
            return center;
        }

        public Bitmap DrawLegend()
        {
            Bitmap image = new Bitmap(LegendWidth, ChartSize);
            Graphics graphics = Graphics.FromImage(image);

            var padding = LegendPadding;

            using (var font = FontManager.GetFont(24))
            {
                var textBrush = new SolidBrush(Color.Black);

                foreach (var pie in pies)
                {
                    var brush = new SolidBrush(pie.Color);

                    var colorRectangle = new Rectangle(20, padding + 5, 10, 10);

                    var textRectangle = new Rectangle(40, padding, LegendWidth - 40, 40);

                    graphics.FillRectangle(brush, colorRectangle);

                    graphics.DrawString(pie.Label, font, textBrush, textRectangle);

                    padding += 40;
                }
            }

            return image;
        }

        public Bitmap DrawTitle()
        {
            var image = new Bitmap(Width, titleHeight);
            Graphics graphics = Graphics.FromImage(image);
            Rectangle rectangle = new Rectangle(0, 20, Width, titleHeight-20);
            StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            using (var font = FontManager.GetFont(28))
            {
                //var title = "e kratkoročne obveze";
                //byte[] bytes = Encoding.Default.GetBytes(title);
                //var titleUTF8 = Encoding.UTF8.GetString(bytes);

                //Console.WriteLine("GDI char set {0}", (int)font.GdiCharSet);

                graphics.DrawString(Title, font, new SolidBrush(Color.Black), rectangle, format);

            }

            return image;
        }

        public void DrawParallel(String path)
        {
            var bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);


            Task<Bitmap> taskDrawChart = Task.Run(() => DrawChart());
            Task<Bitmap> taskTitleChart = Task.Run(() => DrawTitle());
            Task<Bitmap> taskDrawLegend = Task.Run(() => DrawLegend());

            Task.WaitAll(taskDrawChart, taskDrawLegend, taskTitleChart);


            graphics.DrawImage(taskTitleChart.Result, new Rectangle(0, 0, Width, titleHeight));
            graphics.DrawImage(taskDrawChart.Result, new Rectangle(0, titleHeight, ChartSize, ChartSize));
            graphics.DrawImage(taskDrawLegend.Result, new Rectangle(ChartSize, titleHeight, ChartSize, ChartSize));

            bitmap.Save(path);
        }

        public Bitmap Draw()
        {
            var bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            graphics.DrawImage(DrawTitle(), new Rectangle(0, 0, Width, titleHeight));
            graphics.DrawImage(DrawChart(), new Rectangle(LeftPadding, titleHeight, ChartSize, ChartSize));
            graphics.DrawImage(DrawLegend(), new Rectangle(LeftPadding + ChartSize, titleHeight, ChartSize, ChartSize));

            return bitmap;
            //bitmap.Save(path);
        }

        private void CalculateColors(int n)
        {
            colors = possibleColors.GetRange(0, n);
        }

        private void CalculatePies(List<Tuple<string, decimal>> entries)
        {

            decimal total = 0;

            entries.ForEach(entry =>
            {
                total += entry.Item2;
            });

            pies = new List<Pie>();


            double startingAngle = 0;
            int i = 0;

            entries.ForEach(entry =>
            {

                double angle = (double)(entry.Item2 / total) * 360;

                var pie = new Pie { Angle = angle, StartingAngle = startingAngle, Color = colors[i], Label = entry.Item1 };

                pies.Add(pie);
                startingAngle += angle;
                i++;
            });
        }
    }
}