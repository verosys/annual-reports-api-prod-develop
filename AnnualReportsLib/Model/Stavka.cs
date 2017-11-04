using System;
using AnnualReports.Charting;

namespace AnnualReports.Model
{

    public class Stavka
    {
        public string Label { get; set; }
        public string AOP { get; set; }
        public string RedniBroj { get; set; }
        public decimal TekucaGodina { get; set; }
        public decimal ProslaGodina { get; set; }

        public decimal Index {
            get 
            {
                return (ProslaGodina == 0) ? 0 : TekucaGodina / ProslaGodina;
            }
        }

        public Stavka() { }

        public Stavka(string label, string aop, decimal prethodnaGodina, decimal tekucaGodina, string redniBroj = "")
        {
            this.Label = label;
            this.AOP = aop;
            this.ProslaGodina = prethodnaGodina;
            this.TekucaGodina = tekucaGodina;
            this.RedniBroj = redniBroj;
        }

        public string[] ToRow ()
        {
            var label = Label.Trim().TrimStart(new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }).TrimStart('.').Trim();

            return new string[] { label, ProslaGodina.ToString("N2") + " kn", TekucaGodina.ToString("N2") + " kn", Index.ToString("N2") };
        }

        public string[] ToRow(string label)
		{
			return new string[] { label, ProslaGodina.ToString("N2") + " kn", TekucaGodina.ToString("N2") + " kn", Index.ToString("N2") };
		}

		public Stavka Duplicate (String newLabel)
        {
            return new Stavka(newLabel, AOP, ProslaGodina, TekucaGodina, RedniBroj);
        }

        public Tuple<string, decimal> ToChartEntry ()
        {
            return new Tuple<string, decimal>(Label, TekucaGodina);
        }

        public Tuple<string, decimal> ToChartEntry(string label)
		{
            return new Tuple<string, decimal>(label, TekucaGodina);
		}

        public BarEntry ToBarEntry ()
        {
            return new BarEntry()
            {
                Label = Label,
                ValueI = ProslaGodina,
                ValueII = TekucaGodina
            };
        }

        public BarEntry ToBarEntry(string label)
		{
			return new BarEntry()
			{
                Label = label,
				ValueI = ProslaGodina,
				ValueII = TekucaGodina
			};
		}
    }
}