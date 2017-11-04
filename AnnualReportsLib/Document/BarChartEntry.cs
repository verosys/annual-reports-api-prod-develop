using System.Collections.Generic;
using System.Drawing;
using AnnualReports.Charting;

namespace AnnualReports.Document
{
    public class BarChartEntry: Entry
    {
        public string Title { get; set; }
        public string LegendI { get; set; }
        public string LegendII { get; set; }

        public List<BarEntry> Entries { get; set; }

        public BarChartEntry(string legendI, string legendII)
        {
            LegendI = legendI;
            LegendII = legendII;
        }

		public Bitmap GetChart()
		{
            var chart = new BarChart(Entries)
            {
                Title = "Grafikon usporedbe s prethodnom godinom",
                LegendTitleI = LegendI,
                LegendTitleII = LegendII
            };

			return chart.Draw();
		}
    }
}