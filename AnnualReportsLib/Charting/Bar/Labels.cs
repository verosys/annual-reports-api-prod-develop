using System;

namespace AnnualReports.Charting
{
    public class Labels
    {
        public static string[] GetValueLabels(decimal maxValue, int numberOfLabels)
        {
            decimal step = maxValue / 4;

            var labels = new string[numberOfLabels];

            for (int i = 0; i < numberOfLabels; i++)
            {
                labels[i] = (i * step).ToString("N2") + "kn";
            }

            return labels;
        }


		public static decimal GetChartMax(decimal maxValue)
		{
			var range = GetRange(maxValue);
			int n = GetN(maxValue, (int)range);

			//Console.WriteLine("n: {0}", n);

			if (range > 0)
			{
				int chartMax = (int)(n * Math.Pow(10, range - 1));
				if (chartMax % 4 == 0)
					return chartMax;
				else
					return (int)((n + 1) * Math.Pow(10, range - 1));
			}
			else
			{
				int chartMax = (int)(n * Math.Pow(10, -range));
				if (chartMax % 4 == 0)
					return (decimal)(n * Math.Pow(10, range - 1));
				else
					return (decimal)((n + 1) * Math.Pow(10, range - 1));
			}
		}

		private static int GetN(decimal number, int range)
		{
            //Console.WriteLine("Number {0}, range {1}", number, range);
			return (int)Math.Ceiling(number / (decimal)Math.Pow(10, range - 1));
		}

        private static int GetRange(decimal maxValue)
		{
			return (int)Math.Ceiling(Math.Log10((double)maxValue));
		}
    }
}
