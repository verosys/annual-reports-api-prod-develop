using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{

	//AKTIVA/KRATKOTRAJNA IMOVINA
	public class KratkotrajnaImovina
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Zalihe Zalihe { get; set; }

        public KratkotrajnaPotrazivanja Potrazivanja { get; set; }

        public KratkotrajnaFinancijskaImovina FinancijskaImovina { get; set; }

        public Stavka NovacUBanciIBlagajni { get; set; }

        public Stavka Ukupno { get; set; }

        public KratkotrajnaImovina()
        {
            Zalihe = new Zalihe();
            Potrazivanja = new KratkotrajnaPotrazivanja();
            FinancijskaImovina = new KratkotrajnaFinancijskaImovina();
        }

		public List<Entry> GetNotes()
		{
			var entries = new List<Entry>();
            entries.AddRange(Potrazivanja.GetNotes());
            entries.AddRange(FinancijskaImovina.GetNotes());
            entries.Add(new Paragraph(Note, NovacUBanciIBlagajni.TekucaGodina));
			return entries;
		}

		public static Note Note = new Note()
		{
			Title = "Novac u banci i blagajni",
			Subtitle = "Bilješka br. 17",
            Value = "Na dan izvještavanja iznos novca u banci kojeg čine salda na kunskim i/ili deviznim računima te novac u blagajni iznosio je {0:N2} kn"
		};
    }
}