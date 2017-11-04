using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
	//AKTIVA/DUGOTRAJNA IMOVINA/
	public class DugotrajnaImovina
    {
		public int Godina { get; set; }
		
        public int ProslaGodina { get { return Godina - 1; } }

        public NematerijalnaImovina NematerijalnaImovina { get; set; }

        public MaterijalnaImovina MaterijalnaImovina { get; set; }

        public FinancijskaImovina FinancijskaImovina { get; set; }

        public PotrazivanjaDugotrajna Potrazivanja { get; set; }

        public Stavka OdgodjenaPoreznaImovina { get; set; }

        public Stavka Ukupno { get; set; }

        public DugotrajnaImovina()
        {
            NematerijalnaImovina = new NematerijalnaImovina();
            MaterijalnaImovina = new MaterijalnaImovina();
            FinancijskaImovina = new FinancijskaImovina();
            Potrazivanja = new PotrazivanjaDugotrajna();
        }

        public List<Entry> GetNotes()
        {
            List<Entry> entries = new List<Entry>();

            entries.Add(new Paragraph(Note, Ukupno.TekucaGodina));
            entries.AddRange(NematerijalnaImovina.GetNotes());
            entries.AddRange(MaterijalnaImovina.GetNotes());
            entries.AddRange(FinancijskaImovina.GetNotes());
            entries.AddRange(Potrazivanja.GetNotes());

            return entries;
        }

		public static Note Note = new Note()
		{
			Title = "DUGOTRAJNA IMOVINA",
			Subtitle = "Bilješka br. 8",
			Info = "Dugotrajna imovina društva sastoji se od nematerijalne imovine, dugotrajne materijalne imovine, dugotrajne financijske imovine, dugoročnih potraživanja i odgođene porezne imovine.",
            Value = "Knjigovodstvena vrijednost materijalne imovine na dan izvještavanja iznosila je {0:N2} kn."
		};
	}
}