using System;
using System.Drawing;

namespace AnnualReports.Charting
{
    public struct Pie
    {
        public Color Color { get; set; }

        public double Angle { get; set; }

        public double StartingAngle { get; set; }

        public string Label { get; set; }
    }
}
