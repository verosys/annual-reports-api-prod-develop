using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class VrijednosnaUskladjenja
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka DugotrajneImovineOsimFinImovine { get; set; }
        public Stavka KratkotrajneImovineOsimFinImovine { get; set; }
        public Stavka Ukupno { get; set; }

		public List<Entry> GetNotes()
		{
			return new List<Entry>()
			{
				new Paragraph (Note, Ukupno.TekucaGodina)
			};
		}

		public static Note Note = new Note()
		{
			Title = "Vrijednosna usklađenja",
			Subtitle = "Bilješka br. 5f",
			Value = "U poslovnoj godini vrijednosna usklađenja iznosila su {0:N2} kn",
			Zero = "Društvo u poslovnoj godini nije imalo vrijednosna usklađenja nefinancijske imovine."
		};
    }
}