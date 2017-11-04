using System;
using System.Collections.Generic;
using System.Drawing;
using AnnualReports.Charting;

namespace AnnualReports.Document
{
    public class PieChartEntry :Entry
    {
		public string Title { get; set; }
		public List<Tuple<string, decimal>> Entries { get; set; }

        public PieChartEntry()
        {
            Entries = new List<Tuple<string, decimal>>();
        }

        public Bitmap GetChart ()
        {
            var chart = new PieChart(Entries) { Title = this.Title };
            return chart.Draw();
        }
    }
}
