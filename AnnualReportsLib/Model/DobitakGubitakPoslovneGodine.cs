using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{

	public class DobitGubitakPoslovneGodine
	{
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }
		public Stavka DobitPoslovneGodine { get; set; }
		public Stavka GubitakPoslovneGodine { get; set; }
		public Stavka Ukupno { get; set; }

		public List<Entry> GetNotes()
		{
            var paragraph = new Paragraph();
            paragraph.Title = "Dobit ili gubitak poslovne godine";
            paragraph.Subtitle = "Bilješka br. 26";

            if (DobitPoslovneGodine.TekucaGodina > 0 )
            {
                paragraph.Text = String.Format("Društvo je u poslovnoj godini {0}. ostvarilo neto dobit u iznosu od {1:N2} kn.", Godina, DobitPoslovneGodine.TekucaGodina);
            }
            else if (GubitakPoslovneGodine.TekucaGodina >0 )
            {
                paragraph.Text = String.Format("Društvo je u poslovnoj godini {0}. ostvarilo gubitak u iznosu od {1:N2} kn.", Godina, GubitakPoslovneGodine.TekucaGodina);
            }
            else {
                paragraph.Text = String.Format("Društvo u poslovnoj godini {0}. nije ostvarilo dobit niti je poslovalo s gubitkom.", Godina);
            }

			return new List<Entry>() {
                paragraph
			};
		}
	}
}