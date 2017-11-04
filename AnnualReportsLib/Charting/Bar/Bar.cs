using System;
using System.Drawing;
using System.Collections.Generic;

namespace AnnualReports.Charting
{
    public class Bar
    {
        public string Label { get; set; }
        public int StartingPosition { get; set; }
        public int Width { get; set; }
        public int HeightI { get; set; }
        public int HeightII { get; set; }
        public Color ColorI { get; set; }
        public Color ColorII { get; set; }

        public static List<Bar> GetBars(int frameWidth, int frameHeight, List<BarEntry> entries, decimal maxValue)
        {
            List<Bar> bars = new List<Bar>();

            int n = entries.Count;

            int barWidth = (int)(frameWidth / (2 * n + 1));

            int maxHeight = frameHeight;

            decimal scaleFactor = maxHeight / maxValue;

            int startingPosition = barWidth;

            foreach (var entry in entries)
            {
                
                Bar bar = new Bar()
                {
                    Width = barWidth,
                    HeightI = (int)(entry.ValueI * scaleFactor),
                    HeightII = (int)(entry.ValueII * scaleFactor),
                    StartingPosition = startingPosition,
                    Label = entry.Label,
                    ColorI = Color.LightSteelBlue,
                    ColorII = Color.SteelBlue
                };

                bars.Add(bar);

                startingPosition += 2 * barWidth; //one for 1st bar, one for the 2nd bar, one for the empty space
            }

            return bars;
        }
    }
}